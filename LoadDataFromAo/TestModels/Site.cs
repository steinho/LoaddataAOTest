using System;
using System.Collections.Generic;
using System.Text;

namespace LoadDataFromAo.TestModels
{
    public class Site
    {
        public string Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string ParentSiteId { get; set; }
        public string PresentationName { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Accuracy { get; set; }
        public DateTime Date { get; set; }
    }
}
