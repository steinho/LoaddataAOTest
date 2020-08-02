using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
//using System.Linq;
using System.Text;
using LoadDataFromAo.Indexes;
using Raven.Client.Documents.Indexes;

//using Raven.Client.Documents.Indexes;

namespace LoadDataFromAo.RavenIndexes
{
    public class RapporteringsvolumIndex : AbstractIndexCreationTask<SightingIndex, RapporteringsvolumIndex.Result>
    {
        public class Result
        {
            public int BrukerId { get; set; }

            public int Count { get; set; }
        }
        public RapporteringsvolumIndex()
        {
            Map = sightings => from sighting in sightings
                //let categoryName = LoadDocument<Category>(product.Category).Name
                select new Result
                {
                    BrukerId = sighting.ReporterId.FirstOrDefault(),
                    Count = 1
                };

            Reduce = results => from result in results
                group result by result.BrukerId into g
                select new
                {
                    BrukerId = g.Key,
                    Count = g.Sum(x => x.Count)
                };

        }
    }

    public class ObservatorAreaIndex : AbstractIndexCreationTask<SightingIndex, ObservatorAreaIndex.Result>
    {
        public class Result
        {
            public int BrukerId { get; set; }
            public int AreaId { get; set; }
            public int Count { get; set; }
            public SpeciesList[] SpeciesLists { get; set; }
        }

        public class SpeciesList
        {
            public int TaxonId { get; set; }
            public DateTime Date { get; set; }
        }
        public ObservatorAreaIndex()
        {
            Map = sightings => from sighting in sightings
                from observer in sighting.ObserverIds
                from areas in sighting.AreaIds
                select new Result
                {
                    BrukerId = observer,
                    AreaId = areas,
                    SpeciesLists = new SpeciesList[]
                        {new SpeciesList() {Date = sighting.StartDate, TaxonId = sighting.TaxonId}},
                    Count = 0
                };

            Reduce = results => from result in results
                group result by new {result.BrukerId, result.AreaId} into g
                let rows = g.SelectMany(x => x.SpeciesLists).GroupBy(y => y.TaxonId).Select(z => new SpeciesList() { TaxonId = z.Key, Date = z.Min(a => a.Date) }).ToArray() // .OrderByDescending(b=>b.Date)

                select new Result
                {
                    BrukerId = g.Key.BrukerId,
                    AreaId = g.Key.AreaId,
                    SpeciesLists = rows,
                    Count = rows.Length
                };

            Index(x => x.SpeciesLists, FieldIndexing.No);
            Stores.Add(x => x.SpeciesLists, FieldStorage.Yes);
            //Stores.Add(x => x.SpeciesLists, FieldStorage.Yes);

        }


    }

    public class ObservatorFirstIndex : AbstractIndexCreationTask<SightingIndex, ObservatorFirstIndex.Result>
    {
        public class Result
        {
            public int BrukerId { get; set; }
            //public int TaxonId { get; set; }
            //public DateTime Date { get; set; }
            public int Count { get; set; }

            public SpeciesList[] SpeciesLists { get; set; }
        }

        public class SpeciesList
        {
            public int TaxonId { get; set; }
            public DateTime Date { get; set; }
        }
        public ObservatorFirstIndex()
        {
            Map = sightings => from sighting in sightings
                from observer in sighting.ObserverIds
                select new Result
                {
                    BrukerId = observer,
                    SpeciesLists = new SpeciesList[]{ new SpeciesList() { Date = sighting.StartDate, TaxonId = sighting.TaxonId}},
                    Count = 0
                };

            Reduce = results => from result in results
                group result by result.BrukerId into g
                let rows = g.SelectMany(x=>x.SpeciesLists).GroupBy(y=>y.TaxonId).Select(z=> new SpeciesList(){ TaxonId = z.Key, Date = z.Min(a=>a.Date)}).ToArray() // .OrderByDescending(b=>b.Date)

                                select new Result
                {
                    BrukerId = g.Key,
                    SpeciesLists = rows,
                    Count = rows.Length
                };

            Index(x=>x.SpeciesLists, FieldIndexing.No);
            Stores.Add(x=>x.SpeciesLists, FieldStorage.Yes);
        }


    }
    public class ObservatorFirstIndex2 : AbstractIndexCreationTask<SightingIndex, ObservatorFirstIndex2.Result>
    {
        public class Result
        {
            public int BrukerId { get; set; }
            public int TaxonId { get; set; }
            public DateTime Date { get; set; }
            public int Count { get; set; }

            //public SpeciesList[] SpeciesLists { get; set; }
        }

        public class SpeciesList
        {
            public int TaxonId { get; set; }
            public DateTime Date { get; set; }
        }
        public ObservatorFirstIndex2()
        {
            Map = sightings => from sighting in sightings
                from observer in sighting.ObserverIds
                select new Result
                {
                    BrukerId = observer,
                    Date = sighting.StartDate,
                    TaxonId = sighting.TaxonId
                    ,
                    Count = 1
                };

            Reduce = results => from result in results
                group result by new { result.BrukerId, result.TaxonId }
                into g
                    //let rows = g.SelectMany(x=>x.SpeciesLists).GroupBy(y=>y.TaxonId).Select(z=> new SpeciesList(){ TaxonId = z.Key, Date = z.Min(a=>a.Date)}).ToArray() // .OrderByDescending(b=>b.Date)

                select new Result
                {
                    BrukerId = g.Key.BrukerId,
                    TaxonId = g.Key.TaxonId,
                    Date = g.Max(x => x.Date),
                    Count = g.Sum(x => x.Count)
                };

            //Index(x=>x.SpeciesLists, FieldIndexing.No);
            //Stores.Add(x=>x.SpeciesLists, FieldStorage.Yes);
        }


    }
    public class ObservatorAreaIndex2 : AbstractIndexCreationTask<SightingIndex, ObservatorAreaIndex2.Result>
    {
        public class Result
        {
            public int BrukerId { get; set; }
            public int AreaId { get; set; }
            public int Count { get; set; }
            public int TaxonId { get; set; }
            public DateTime Date { get; set; }
            //public SpeciesList[] SpeciesLists { get; set; }
        }

        //public class SpeciesList
        //{
        //    public int TaxonId { get; set; }
        //    public DateTime Date { get; set; }
        //}
        public ObservatorAreaIndex2()
        {
            Map = sightings => from sighting in sightings
                from observer in sighting.ObserverIds
                from areas in sighting.AreaIds
                select new Result
                {
                    BrukerId = observer,
                    AreaId = areas, 
                    Date = sighting.StartDate, 
                    TaxonId = sighting.TaxonId,
                    Count = 1
                };

            Reduce = results => from result in results
                group result by new { result.BrukerId, result.AreaId, result.TaxonId } into g
                //let rows = g.SelectMany(x => x.SpeciesLists).GroupBy(y => y.TaxonId).Select(z => new SpeciesList() { TaxonId = z.Key, Date = z.Min(a => a.Date) }).ToArray() // .OrderByDescending(b=>b.Date)

                select new Result
                {
                    BrukerId = g.Key.BrukerId,
                    AreaId = g.Key.AreaId,
                    TaxonId = g.Key.TaxonId,
                    Date = g.Max(x=>x.Date),
                    Count = g.Sum(x=>x.Count)
                };

            //Index(x => x.SpeciesLists, FieldIndexing.No);
            //Stores.Add(x => x.SpeciesLists, FieldStorage.Yes);
            //Stores.Add(x => x.SpeciesLists, FieldStorage.Yes);

        }


    }
}
