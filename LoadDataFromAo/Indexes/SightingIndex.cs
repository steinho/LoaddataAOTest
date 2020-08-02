using System;
using System.Collections.Generic;
using System.Text;

namespace LoadDataFromAo.Indexes
{
    public class SightingIndex
    {
        public string Id { get; set; }
        public int TaxonId { get; set; }
        public DateTime StartDate { get; set; }
        public List<int> ReporterId { get; set; }
        public List<int> ObserverIds { get; set; }
        public List<int> AreaIds { get; set; }
    }
}
