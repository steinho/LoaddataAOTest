using System;
using System.Collections.Generic;

namespace LoadDataFromAo.Models
{
    public partial class Area
    {
        public Area()
        {
            Site = new HashSet<Site>();
            SiteAreas = new HashSet<SiteAreas>();
        }

        public int Id { get; set; }
        public int AreaDatasetId { get; set; }
        public string FeatureId { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Bbox { get; set; }
        public int? AreaDatasetSubTypeId { get; set; }
        public string AttributesXml { get; set; }
        public int? ParentId { get; set; }
        public string Abbrevation { get; set; }
        public int SortOrder { get; set; }

        public virtual AreaDataset AreaDataset { get; set; }
        public virtual ICollection<Site> Site { get; set; }
        public virtual ICollection<SiteAreas> SiteAreas { get; set; }
    }
}
