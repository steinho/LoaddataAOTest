using System;
using System.Collections.Generic;

namespace LoadDataFromAo.Models
{
    public partial class User
    {
        public User()
        {
            Sighting = new HashSet<Sighting>();
            SightingRelation = new HashSet<SightingRelation>();
            SiteControlingUser = new HashSet<Site>();
            SiteUser = new HashSet<Site>();
        }

        public int Id { get; set; }
        public string UserAlias { get; set; }
        public string AlarmCode { get; set; }
        public int CurrentCultureGlobalizationCultureId { get; set; }
        public int SpeciesNamesLanguageId { get; set; }
        public int CoordinateSystemId { get; set; }
        public int CoordinateSystemNotationId { get; set; }
        public int? TaxonId { get; set; }
        public int? SpeciesGroupId { get; set; }
        public int PersonId { get; set; }
        public bool AcceptedAgreement { get; set; }
        public bool PublicUserGallery { get; set; }
        public bool DisableUserInLists { get; set; }
        public bool? DiaryShowWeather { get; set; }
        public bool? DiaryShowMap { get; set; }
        public bool? DiaryShowStatistics { get; set; }
        public bool? DiaryPrintCalendar { get; set; }
        public bool? DiaryPrintImages { get; set; }
        public bool TaxonPickerAutoSelect { get; set; }
        public bool DisableFormInstructions { get; set; }
        public bool AddCreatedSitesAsFavorites { get; set; }
        public int AcceptedAgreementVersion { get; set; }
        public bool AcceptedRulesForImageComments { get; set; }
        public int LastGlobalSystemInformationMessage { get; set; }
        public int? LicenseId { get; set; }
        public DateTime? LastTopListUpdate { get; set; }
        public int? DiarySortOrderId { get; set; }
        public int? DefaultStoredSearchCriteriasId { get; set; }
        public bool IsUnreliableReporter { get; set; }
        public DateTime? AcceptedAgreementDate { get; set; }

        public virtual ICollection<Sighting> Sighting { get; set; }
        public virtual ICollection<SightingRelation> SightingRelation { get; set; }
        public virtual ICollection<Site> SiteControlingUser { get; set; }
        public virtual ICollection<Site> SiteUser { get; set; }
    }
}
