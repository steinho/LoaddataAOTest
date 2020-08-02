using System;
using System.Collections.Generic;

namespace LoadDataFromAo.Models
{
    public partial class SiteAreas
    {
        public int SiteId { get; set; }
        public int AreasId { get; set; }

        public virtual Area Areas { get; set; }
    }
}
