using System;
using System.Linq;
using LoadDataFromAo.Indexes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.TestDriver;

namespace RavenIndexTests
{
    [TestClass]
    public class UnitTest1 : RavenTestDriver
    {
        private static bool initialized = false;
        //This allows us to modify the conventions of the store we get from 'GetDocumentStore'
        protected override void PreInitialize(IDocumentStore documentStore)
        {
            documentStore.Conventions.MaxNumberOfRequestsPerSession = 50;
        }

        [TestMethod]
        public void TestRapporteringsvolum()
        {
            using (var store = GetDocumentStore())
            {
                store.ExecuteIndex(new LoadDataFromAo.RavenIndexes.RapporteringsvolumIndex());
                using (var session = store.OpenSession())
                {
                    session.Store(AOStuff.GetObsMalvik(1, AOStuff.Hoem, AOStuff.KornSpurv, new DateTime(2020, 2, 2)));

                    session.Store(AOStuff.GetObsMalvik(2, AOStuff.Hoem,  AOStuff.KornSpurv, new DateTime(2020, 2, 2)));

                    session.Store(AOStuff.ObsPedersenStjordal(3));

                    session.SaveChanges();
                }

                WaitForIndexing(
                    store); //If we want to query documents sometime we need to wait for the indexes to catch up
                using (var session = store.OpenSession())
                {
                    var query = session
                        .Query<LoadDataFromAo.RavenIndexes.RapporteringsvolumIndex.Result,
                            LoadDataFromAo.RavenIndexes.RapporteringsvolumIndex>().Where(x=>x.Year == 0).OrderByDescending(x => x.Count).ToArray();
                    Assert.AreEqual(AOStuff.Hoem, query.First().BrukerId, "hoem vinner");
                    Assert.AreEqual(2, query.First().Count, "hoems to arter");
                    Assert.AreEqual(AOStuff.Pedersen, query.Last().BrukerId);
                    Assert.AreEqual(2, query.Count(), "kun to brukerkontoer - kun 2 p� topplista");


                }
            }
        }

        [TestInitialize]
        public void Setup()
        {
            if (initialized == false)
            {
                ConfigureServer(new TestServerOptions
                {
                    DataDirectory = "C:\\RavenDBTestDir",

                });
                initialized = true;
            }

        }
        [TestMethod]
        public void TestRapporteringsvolumSiste�r()
        {
            
            using (var store = GetDocumentStore())
            {
                store.ExecuteIndex(new LoadDataFromAo.RavenIndexes.RapporteringsvolumIndex());
                using (var session = store.OpenSession())
                {
                    session.Store(AOStuff.GetObsMalvik(1, AOStuff.Hoem, AOStuff.KornSpurv, new DateTime(2019, 2, 2)));

                    session.Store(AOStuff.GetObsMalvik(2, AOStuff.Hoem, AOStuff.KornSpurv, new DateTime(2010, 2, 2)));

                    session.Store(AOStuff.ObsPedersenStjordal(3));

                    session.SaveChanges();
                }

                WaitForIndexing(
                    store); //If we want to query documents sometime we need to wait for the indexes to catch up
                using (var session = store.OpenSession())
                {
                    var query = session
                        .Query<LoadDataFromAo.RavenIndexes.RapporteringsvolumIndex.Result,
                            LoadDataFromAo.RavenIndexes.RapporteringsvolumIndex>().Where(x=>x.Year == 2020).OrderByDescending(x => x.Count).ToArray();
                    Assert.AreEqual(AOStuff.Pedersen, query.First().BrukerId, "pedersen vinner 2020");
                    Assert.AreEqual(1, query.First().Count, "pedersens ene art");
                    Assert.AreEqual(1, query.Count(), "kun en brukerkonto i 2020 p� topplista");


                }
            }
        }
    }

}
