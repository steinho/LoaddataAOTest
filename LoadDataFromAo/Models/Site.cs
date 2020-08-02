using System;
using System.Collections.Generic;

namespace LoadDataFromAo.Models
{
    public partial class Site
    {
        public Site()
        {
            InverseParent = new HashSet<Site>();
            Sighting = new HashSet<Sighting>();
        }

        public int Id { get; set; }
        public int? ParentId { get; set; }
        public int? IncludedBySiteId { get; set; }
        public string Name { get; set; }
        public string PresentationName { get; set; }
        public string InputString { get; set; }
        public int Accuracy { get; set; }
        public int Xcoord { get; set; }
        public int Ycoord { get; set; }
        public int? UserId { get; set; }
        public bool? IsPrivate { get; set; }
        public string Comment { get; set; }
        public int? ControlingOrganisationId { get; set; }
        public int? ControlingUserId { get; set; }
        public bool? IsWithinAllowedArea { get; set; }
        public int? SpeciesGroupId { get; set; }
        public DateTime RegisterDate { get; set; }
        public DateTime EditDate { get; set; }
        public int? ChangedByUserId { get; set; }
        public int? ProjectId { get; set; }
        public string ExternalId { get; set; }
        public int? AreaId { get; set; }
        public int? ApiId { get; set; }
        public string Region { get; set; }
        public string Municipality { get; set; }
        public int? TaxonId { get; set; }
        public int? SiteGroupId { get; set; }

        public virtual Area Area { get; set; }
        public virtual User ControlingUser { get; set; }
        public virtual Site Parent { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Site> InverseParent { get; set; }
        public virtual ICollection<Sighting> Sighting { get; set; }
    }
}
