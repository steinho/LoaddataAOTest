using System;
using System.Collections.Generic;

namespace LoadDataFromAo.Models
{
    public partial class AreaDataset
    {
        public AreaDataset()
        {
            Area = new HashSet<Area>();
        }

        public int Id { get; set; }
        public int CountryIsoCode { get; set; }
        public string Name { get; set; }
        public bool IsRequired { get; set; }
        public bool IsIndirect { get; set; }
        public bool AllowOverrideIndirectType { get; set; }
        public int AreaDatasetCategoryId { get; set; }
        public bool IsValidationArea { get; set; }
        public short SortOrder { get; set; }
        public string AttributesToHtmlXslt { get; set; }
        public int AreaLevel { get; set; }
        public bool HasStatistics { get; set; }
        public int? ParentId { get; set; }
        public int? MaxProtectionLevel { get; set; }

        public virtual ICollection<Area> Area { get; set; }
    }
}
