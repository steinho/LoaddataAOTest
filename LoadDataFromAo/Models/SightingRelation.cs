using System;
using System.Collections.Generic;

namespace LoadDataFromAo.Models
{
    public partial class SightingRelation
    {
        public int Id { get; set; }
        public int SightingId { get; set; }
        public int UserId { get; set; }
        public int SightingRelationTypeId { get; set; }
        public int Sort { get; set; }
        public bool Discover { get; set; }
        public bool IsPublic { get; set; }
        public int? DeterminationYear { get; set; }
        public DateTime? RegisterDate { get; set; }
        public int? ChangedByUserId { get; set; }
        public DateTime? EditDate { get; set; }
        public int? OldUserId { get; set; }

        public virtual Sighting Sighting { get; set; }
        public virtual User User { get; set; }
    }
}
