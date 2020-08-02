using System;
using System.Collections.Generic;

namespace LoadDataFromAo.Models
{
    public partial class Sighting
    {
        public Sighting()
        {
            SightingRelation = new HashSet<SightingRelation>();
            SightingState = new HashSet<SightingState>();
        }

        public int Id { get; set; }
        public int SightingTypeId { get; set; }
        public bool ProtectedBySystem { get; set; }
        public DateTime? RegisterDate { get; set; }
        public DateTime? EditDate { get; set; }
        public int? ChangedByUserId { get; set; }
        public DateTime? StartDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public DateTime? EndDate { get; set; }
        public TimeSpan? EndTime { get; set; }
        public int? Quantity { get; set; }
        public int? TaxonId { get; set; }
        public int? SiteId { get; set; }
        public bool UnsureDetermination { get; set; }
        public int? StageId { get; set; }
        public int? GenderId { get; set; }
        public int? ActivityId { get; set; }
        public int? DeterminationMethodId { get; set; }
        public DateTime? HiddenByProvider { get; set; }
        public bool NotPresent { get; set; }
        public bool NotRecovered { get; set; }
        public bool Unspontaneous { get; set; }
        public bool NoteOfInterest { get; set; }
        public int? UnitId { get; set; }
        public int? QuantityOfSubstrate { get; set; }
        public int? DiscoveryMethodId { get; set; }
        public int? Length { get; set; }
        public int? Weight { get; set; }
        public int? SightingSummaryId { get; set; }
        public int? PublicationId { get; set; }
        public int? BiotopeId { get; set; }
        public int? SightingBiotopeDescriptionId { get; set; }
        public int? SubstrateId { get; set; }
        public int? SightingSubstrateDescriptionId { get; set; }
        public int? SubstrateSpeciesId { get; set; }
        public int? SightingSubstrateSpeciesDescriptionId { get; set; }
        public int? ControlingOrganisationId { get; set; }
        public int? ControlingUserId { get; set; }
        public int ValidationStatusId { get; set; }
        public bool HasImages { get; set; }
        public bool HasTriggeredValidationRules { get; set; }
        public bool HasAnyTriggeredValidationRuleWithWarning { get; set; }
        public int? MinDepth { get; set; }
        public int? MaxDepth { get; set; }
        public int? MinHeight { get; set; }
        public int? MaxHeight { get; set; }
        public Guid Guid { get; set; }
        public int? MovedFromTaxonId { get; set; }
        public int? CurrentSightingStateTypeId { get; set; }
        public int? SightingTypeSearchGroupId { get; set; }
        public DateTime? EditDateForExport { get; set; }
        public int? StartDateDay { get; set; }
        public int? EndDateDay { get; set; }
        public int? TriggeredValidationLevel { get; set; }
        public int? RegionalSightingState { get; set; }
        public DateTime? PublishDate { get; set; }
        public int? SpeciesGroupId { get; set; }
        //public int? BiotopeNiN2id { get; set; }
        //public DateTime? QueryDate { get; set; }
        //public bool HasComments { get; set; }

        public virtual User ControlingUser { get; set; }
        public virtual Site Site { get; set; }
        public virtual ICollection<SightingRelation> SightingRelation { get; set; }
        public virtual ICollection<SightingState> SightingState { get; set; }
    }
}
