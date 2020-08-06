using System;
using System.Collections.Generic;
using System.Linq;
using LoadDataFromAo.Indexes;
using LoadDataFromAo.Models;
using LoadDataFromAo.RavenIndexes;
using Microsoft.EntityFrameworkCore;
using Nest;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Session;

namespace LoadDataFromAo
{
    class Program
    {
        static void Main(string[] args)
        {
            first();
        }

        private static void first()
        {
            var siteAreas = new Dictionary<int, List<int>>();

            //Elasticsearch setup
            //var settings = new ConnectionSettings(new Uri("http://localhost:9200"))
            //.DefaultIndex("sightings");

            //var client = new ElasticClient(settings);

            //ravendb setup
            var theStore = DocumentStoreHolder.Store;
            var batchSize = 100;
            theStore.ExecuteIndexes(new AbstractIndexCreationTask[]
                {new RapporteringsvolumIndex(),
                    new ObservationvolumIndex(),
                    new AreaIndex(),
                    new ObservatorFirstIndex2(),
                    new ObservatorAreaIndex2()});
            for (int i = 10297801; i < 30000000; i += batchSize)
            {
                using (var context = new AoContext())
                {
                    context.ChangeTracker.AutoDetectChangesEnabled = false;
                    context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                    context.Database.SetCommandTimeout(180);


                    // Read from db
                    var sightings = context.Sighting.AsNoTracking()
                        //.Include(x => x.SightingState)
                        .Include(x => x.SightingRelation)
                        .Where(x =>
                            x.SightingTypeId == 0
                            && x.SightingState.Any(y => y.IsActive && y.SightingStateTypeId == 30)
                            && x.SiteId != null && x.Id > i)
                        .OrderBy(x => x.Id)
                        //.Skip(i)
                        .Take(batchSize)
                        .ToList();

                    var siteNewIds = sightings
                        .Select(x => x.SiteId).Distinct()
                        .Where(x => !siteAreas.ContainsKey(x.Value));

                    var newSiteAreas = context.SiteAreas
                        .Where(x => siteNewIds.Contains(x.SiteId))
                        .ToList()
                        .GroupBy(x => x.SiteId)
                        .ToDictionary(x => x.Key, y => y.Select(z => z.AreasId).ToList());

                    foreach (var newSiteArea in newSiteAreas)
                    {
                        siteAreas.Add(newSiteArea.Key, newSiteArea.Value);
                    }

                    //Map
                    var sightingIndexes = sightings.Select(x => new SightingIndex
                    {
                        Id = x.Id.ToString(),
                        TaxonId = x.TaxonId ?? 0,
                        StartDate = x.StartDate ?? new DateTime(0, 0, 0),
                        ReporterId = x.SightingRelation.Where(y => y.SightingRelationTypeId == 1)
                            .Select(y => y.UserId).ToList(),
                        ObserverIds = x.SightingRelation.Where(y => y.SightingRelationTypeId == 2)
                            .Select(y => y.UserId).ToList(),
                        AreaIds = siteAreas[x.SiteId.Value]
                    }).ToList();

                    // Write to Elasticsearch
                    //var response = client.IndexMany(sightingIndexes);

                    // Write to RavenDB
                    using (var session = theStore.OpenSession())
                    {
                        foreach (var index in sightingIndexes)
                        {
                            session.Store(index);
                        }

                        session.SaveChanges();
                    }

                    Console.WriteLine($"{DateTime.Now:hh:mm:ss} {i}: sightingCount: {sightings.Count}");
                }
            }
        }
    }
}
