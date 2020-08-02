using System;
using System.Collections.Generic;
using System.Text;
using LoadDataFromAo.Indexes;

namespace RavenIndexTests
{
    public static class AOStuff
    {
        public const int Hoem = 1;
        public const int Pedersen = 2;

        public const int Spurv = 1;
        public const int Spurver = 2;
        public const int Fugl = 3;
        public const int Dyr = 4;
        public const int KornSpurv = 5;



        public const int Norge = 1;
        public const int Trondelag = 2;
        public const int Malvik = 3;
        public const int Stjordal = 4;

        public static LoadDataFromAo.Indexes.SightingIndex ObsHoemMalvik(int id)
        {
            return new SightingIndex()
            {
                Id = id.ToString(),
                AreaIds = new List<int>(){ Norge, Trondelag, Malvik},
                ObserverIds = new List<int>() { Hoem},
                ReporterId = new List<int>() { Hoem },
                StartDate = new DateTime(2020,1,1),
                TaxonId = 1
            };
        }
        public static LoadDataFromAo.Indexes.SightingIndex ObsPedersenStjordal(int id)
        {
            return new SightingIndex()
            {
                Id = id.ToString(),
                AreaIds = new List<int>() { Norge, Trondelag, Stjordal },
                ObserverIds = new List<int>() { Pedersen },
                ReporterId = new List<int>() { Pedersen },
                StartDate = new DateTime(2020, 1, 1),
                TaxonId = 1
            };
        }

        internal static SightingIndex GetObsMalvik(int id, int brukerid, int art, DateTime dateTime)
        {
            return new SightingIndex()
            {
                Id = id.ToString(),
                AreaIds = new List<int>() { Norge, Trondelag, Malvik },
                ObserverIds = new List<int>() { brukerid },
                ReporterId = new List<int>() { brukerid },
                StartDate = dateTime,
                TaxonId = art
            };
        }
    }
}
