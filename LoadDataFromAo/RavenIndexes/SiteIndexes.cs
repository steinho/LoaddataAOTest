using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LoadDataFromAo.Indexes;
using Raven.Client.Documents.Indexes;

namespace LoadDataFromAo.RavenIndexes
{
    public class SiteIndexes
    {
        public class SimpleSiteIndex : AbstractIndexCreationTask<TestModels.Site>
        {
            public SimpleSiteIndex()
            {
                Map = sites => from site in sites
                    select new
                    {
                        site.Name,
                        site.UserId
                    };
            }
        }

        public class SimpleSiteIndex2 : AbstractIndexCreationTask<TestModels.Site>
        {
            public class Result
            {
                public string Name { get; set; }
                public int UserId { get; set; }
                public string ParentName { get; set; }
                //public string ParentSiteId { get; set; }
            }

            public SimpleSiteIndex2()
            {
                Map = sites => from site in sites
                    let loadDocument = LoadDocument<TestModels.Site>(site.ParentSiteId)
                    select new Result(){ 
                        Name = site.Name,
                        UserId = site.UserId,
                        ParentName = loadDocument != null ? loadDocument.Name : "none",
                        //ParentSiteId = site.ParentSiteId
                    };
            }
        }
    }
}
