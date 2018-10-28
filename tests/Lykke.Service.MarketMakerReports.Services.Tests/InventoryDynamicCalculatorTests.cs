using System;
using System.Globalization;
using Common;
using Lykke.Service.MarketMakerReports.Core.Domain.InventorySnapshots;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lykke.Service.MarketMakerReports.Services.Tests
{
    [TestClass]
    public class InventoryDynamicCalculatorTests
    {
        [TestMethod]
        public void GetDynamics_NullSnapshots_Throws()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                new InventoryDynamicsCalculator().GetDynamics(null, null);
            });

            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                new InventoryDynamicsCalculator().GetDynamics(new InventorySnapshot(), null);
            });

            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                new InventoryDynamicsCalculator().GetDynamics(null, new InventorySnapshot());
            });
        }
    }
}
