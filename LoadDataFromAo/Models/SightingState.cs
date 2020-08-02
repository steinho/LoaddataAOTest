using System;
using System.Collections.Generic;

namespace LoadDataFromAo.Models
{
    public partial class SightingState
    {
        public int Id { get; set; }
        public int SightingId { get; set; }
        public int SightingStateTypeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
        public string SerializedState { get; set; }

        public virtual Sighting Sighting { get; set; }
    }
}
