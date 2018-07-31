using AutoMapper;
using Lykke.Service.MarketMakerReports.AzureRepositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lykke.Service.MarketMakerReports.Tests
{
    [TestClass]
    public class TestAssembly
    {
        [AssemblyInitialize]
        public static void Initialize(TestContext testContext)
        {
            Mapper.Initialize(cfg => cfg.AddProfile<AutoMapperProfile>());
            Mapper.AssertConfigurationIsValid();
        }

        [TestMethod]
        public void AutoMapper_OK()
        {
            Assert.IsTrue(true);
        }
    }
}
