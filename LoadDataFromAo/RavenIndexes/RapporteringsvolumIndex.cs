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
            public int Year { get; set; }
            public int[] Taxons { get; set; }
            public TaxonCount[] TaxonCounts  { get; set; }

        public class TaxonCount
            {
                public int TaxonId { get; set; }
                public int Count { get; set; }
            }
        }
        public RapporteringsvolumIndex()
        {
            Map = sightings => from sighting in sightings
                from year in new int[] {sighting.StartDate.Year, 0}
                //let categoryName = LoadDocument<Category>(product.Category).Name
                select new Result
                {
                    BrukerId = sighting.ReporterId.FirstOrDefault(),
                    Year = year,
                    Count = 1,
                    Taxons = new []{ sighting.TaxonId },
                    TaxonCounts = new Result.TaxonCount[] {new Result.TaxonCount() { TaxonId = sighting.TaxonId, Count = 1} }
                };

            Reduce = results => from result in results
                group result by new {result.BrukerId,result.Year } into g
                select new Result
                {
                    BrukerId = g.Key.BrukerId,
                    Year = g.Key.Year,
                    Count = g.Sum(x => x.Count),
                    Taxons = g.SelectMany(x => x.Taxons).Distinct().ToArray(),
                    TaxonCounts = g.SelectMany(x => x.TaxonCounts).GroupBy(x => x.TaxonId).Select(y => new Result.TaxonCount() { TaxonId = y.Key, Count = y.Sum(z => z.Count) }).ToArray()
                };
            Index(x => x.TaxonCounts, FieldIndexing.No);
            Store(x=>x.TaxonCounts, FieldStorage.Yes);
        }
    }
    public class ObservationvolumIndex : AbstractIndexCreationTask<SightingIndex, ObservationvolumIndex.Result>
    {
        public class Result
        {
            public int BrukerId { get; set; }

            public int Count { get; set; }
            public int TaxonCount { get; set; }
            public int Year { get; set; }
            public int[] Taxons { get; set; }
            public TaxonStat[] TaxonStats{ get; set; }

            public class TaxonStat
            {
                public int TaxonId { get; set; }
                public int Count { get; set; }
                public DateTime Date { get; set; }
            }
        }
        public ObservationvolumIndex()
        {
            Map = sightings => from sighting in sightings
                from year in new int[] { sighting.StartDate.Year, 0 }
                from obervator in sighting.ObserverIds
                //let categoryName = LoadDocument<Category>(product.Category).Name
                select new Result
                {
                    BrukerId = obervator,
                    Year = year,
                    Count = 1,
                    TaxonCount = 0,
                    Taxons = new[] { sighting.TaxonId },
                    TaxonStats = new Result.TaxonStat[] { new Result.TaxonStat() { TaxonId = sighting.TaxonId, Count = 1, Date = sighting.StartDate} }
                };

            Reduce = results => from result in results
                group result by new { result.BrukerId, result.Year } into g
                let rows = g.SelectMany(x => x.TaxonStats).GroupBy(x => x.TaxonId).Select(y => new Result.TaxonStat() { TaxonId = y.Key, Count = y.Sum(z => z.Count), Date = y.Min(i => i.Date) }).ToArray()
                                select new Result
                {
                    BrukerId = g.Key.BrukerId,
                    Year = g.Key.Year,
                    Count = g.Sum(x => x.Count),
                    TaxonCount = rows.Length,
                    Taxons = g.SelectMany(x => x.Taxons).Distinct().ToArray(),
                    TaxonStats = rows
                };
            Index(x => x.TaxonStats, FieldIndexing.No);
            Store(x => x.TaxonStats, FieldStorage.Yes);
        }
    }
    public class AreaIndex : AbstractIndexCreationTask<SightingIndex, AreaIndex.Result>
    {
        public class Result
        {
            public int AreaId { get; set; }
            public int Count { get; set; }
            public int Year { get; set; }
            public int[] Taxons { get; set; }
            public TaxonCount[] TaxonCounts { get; set; }

        public class TaxonCount
        {
            public int TaxonId { get; set; }
            public DateTime Date { get; set; }
            public int Count { get; set; }
        }
        }


        //public class SpeciesList
        //{
        //    public int TaxonId { get; set; }
        //    public DateTime Date { get; set; }
        //}
        public AreaIndex()
        {
            Map = sightings => from sighting in sightings
                from year in new int[] { sighting.StartDate.Year, 0 }
                               from areas in sighting.AreaIds
                               select new Result
                               {
                                   AreaId = areas,
                                   Year = year,
                                   Taxons = new[] { sighting.TaxonId },
                                   TaxonCounts = new Result.TaxonCount[] { new Result.TaxonCount() { TaxonId = sighting.TaxonId, Count = 1, Date = sighting.StartDate} },
                                   Count = 1
                               };

            Reduce = results => from result in results
                                group result by new { result.AreaId, result.Year} into g
                                let rows = g.SelectMany(x => x.TaxonCounts).GroupBy(x => x.TaxonId).Select(y => new Result.TaxonCount() { TaxonId = y.Key, Count = y.Sum(z => z.Count), Date = y.Min(i=>i.Date)}).ToArray()

                                select new Result
                                {
                                    AreaId = g.Key.AreaId,
                                    Year = g.Key.Year,
                                    Count = rows.Length,
                                    Taxons = g.SelectMany(x => x.Taxons).Distinct().ToArray(),
                                    TaxonCounts = g.SelectMany(x => x.TaxonCounts).GroupBy(x => x.TaxonId).Select(y => new Result.TaxonCount() { TaxonId = y.Key, Count = y.Sum(z => z.Count), Date = y.Min(i => i.Date) }).ToArray()
                                };
            Index(x => x.TaxonCounts, FieldIndexing.No);
            Store(x => x.TaxonCounts, FieldStorage.Yes);
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
            // trolig trenger vi ikke indeksere brukerid - skal vel ikke spørre på denne
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
