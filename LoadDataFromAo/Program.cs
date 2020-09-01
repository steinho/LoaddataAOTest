using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LoadDataFromAo.Indexes;
using LoadDataFromAo.RavenIndexes;
using Microsoft.EntityFrameworkCore;
using Raven.Client.Documents.Indexes;

namespace LoadDataFromAo
{
    class Program
    {
        static void Main(string[] args)
        {
            //LastSite();
            //TestSimpleSiteIndexes();
            //TestSimpleSiteIndexes2();
            //TestAggregeringSite();
            LastFraAO();
        }

        private static void TestAggregeringSite()
        {
            var documentStore = DocumentStoreHolder.Store;
            documentStore.ExecuteIndexes(new[] { new SiteIndexes.SimpleSiteIndex2() });//, });

            // test spørring
            var session = documentStore.OpenSession();
            var resultat = (
                    from s in session.Query<TestModels.Site>()
                    group s by s.ParentSiteId into g
                    select new
                    {
                        Sum = g.Count(),
                        ParentId = g.Key
                    }
                )
                .ToList();
            Console.WriteLine(resultat.First().Sum);

            var resultat2 = (
                    from s in session.Query<TestModels.Site, SiteIndexes.SimpleSiteIndex>()
                    group s by s.ParentSiteId into g
                    select new
                    {
                        Sum = g.Count(),
                        ParentId = g.Key
                    }
                )
                .ToList();
            Console.WriteLine(resultat2.First().Sum);
        }

        private static void TestSimpleSiteIndexes2()
        {
            var documentStore = DocumentStoreHolder.Store;
            documentStore.ExecuteIndexes(new[] { new SiteIndexes.SimpleSiteIndex2() });//, });

            // test spørring
            var session = documentStore.OpenSession();
            var resultat = session
                .Query<RavenIndexes.SiteIndexes.SimpleSiteIndex2.Result, RavenIndexes.SiteIndexes.SimpleSiteIndex2>()
                .Where(x => x.ParentName == "abelvær")
                .OfType<TestModels.Site>().ToList();
            // feil type
            // Type Result - gir bare mening på map reduce indexer 
            Console.WriteLine(resultat.First().Name);


            // aggregering


        }

        private static void TestSimpleSiteIndexes()
        {
            var documentStore = DocumentStoreHolder.Store;
            documentStore.ExecuteIndexes(new []{ new SiteIndexes.SimpleSiteIndex() });//, });

            // test spørring
            var session = documentStore.OpenSession();
            //var resultat = session.Query<TestModels.Site, RavenIndexes.SiteIndexes.SimpleSiteIndex>()
            //    .Where(x => x.Name == "  brettingvegen 2-1641").ToList();
            //// får hele dokumentet - men kan bare spørre på indekserte felt
            //Console.WriteLine(resultat.First().PresentationName);

            
            // test uten index
            var resultat2 = session.Query<TestModels.Site>()
                .Where(x => x.PresentationName.StartsWith("  Bretting")).ToList();
                            //&& x.UserId == 0).ToList();
            // && startswith
            Console.WriteLine(resultat2.First().PresentationName);


        }

        private static void LastSite()
        {
            var documentStore = DocumentStoreHolder.Store;
            var batchSize = 10000;
            var position = 0;


            var optionsBuilder = new DbContextOptionsBuilder<AoContext>();
            optionsBuilder.UseSqlServer(
                "Server=.;Database=.;User=*;Password=*;");

            while (true)
            {
                using var context = new AoContext(optionsBuilder.Options);
                var users = context.Site.Skip(position).Take(batchSize).Select(x => new TestModels.Site()
                {
                    Id = "Site/" + x.Id,
                    UserId = x.UserId ?? 0,
                    Name = x.Name,
                    ParentSiteId = "Site/" + x.ParentId,
                    PresentationName = x.PresentationName,
                    X = x.Xcoord,
                    Y = x.Ycoord,
                    Accuracy = x.Accuracy,
                    Date = x.EditDate
                }).ToArray();

                if (users.Length == 0)
                {
                    break;
                }

                position += batchSize;
                using var ravenSession = documentStore.OpenSession();
                foreach (var site in users)
                {
                    ravenSession.Store(site);
                }

                ravenSession.SaveChanges();
            }
        }

        private static void LastFraAO()
        {
            var siteAreas = new Dictionary<int, List<int>>();
            
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
