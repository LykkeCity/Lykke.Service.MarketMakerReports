using System.Collections.Generic;
using System.Linq;
using Lykke.Service.MarketMakerReports.Core.Domain.InventorySnapshots;
using Lykke.Service.MarketMakerReports.Core.Repositories;
using Lykke.Service.RateCalculator.Client;
using Lykke.Service.RateCalculator.Client.AutorestClient.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using Lykke.Common.Log;

namespace Lykke.Service.MarketMakerReports.Services.Tests
{
    [TestClass]
    public class InventorySnapshotServiceTests
    {
        private string _snapshot = "{" +
"   \"Source\": \"Default\"," +
"   \"Timestamp\": \"2018-08-07T07:28:17.7350053Z\"," +
"   \"Assets\": [" +
"     {" +
"       \"Asset\": \"AUD\"," +
"       \"Inventories\": []," +
"       \"Balances\": [" +
"         {" +
"           \"Exchange\": \"lykke\"," +
"           \"Amount\": 477.0" +
"         }" +
"       ]" +
"     }," +
"     {" +
"       \"Asset\": \"BTC\"," +
"       \"Inventories\": [" +
"         {" +
"           \"Exchange\": \"fakeExchange\"," +
"           \"Volume\": -0.15," +
"           \"SellVolume\": 0.15," +
"           \"BuyVolume\": 0.0" +
"         }," +
"         {" +
"           \"Exchange\": \"lykke\"," +
"           \"Volume\": 0.212," +
"           \"SellVolume\": 0.005," +
"           \"BuyVolume\": 0.217" +
"         }" +
"       ]," +
"       \"Balances\": [" +
"         {" +
"           \"Exchange\": \"lykke\"," +
"           \"Amount\": 297.65804925" +
"         }," +
"         {" +
"           \"Exchange\": \"bitstamp\"," +
"           \"Amount\": 0.07348523" +
"         }" +
"       ]" +
"     }," +
"     {" +
"       \"Asset\": \"CHF\"," +
"       \"Inventories\": []," +
"       \"Balances\": [" +
"         {" +
"           \"Exchange\": \"lykke\"," +
"           \"Amount\": 1000000.0" +
"         }" +
"       ]" +
"     }," +
"     {" +
"       \"Asset\": \"EUR\"," +
"       \"Inventories\": []," +
"       \"Balances\": [" +
"         {" +
"           \"Exchange\": \"lykke\"," +
"           \"Amount\": 319.34" +
"         }," +
"         {" +
"           \"Exchange\": \"bitstamp\"," +
"           \"Amount\": 402.48" +
"         }" +
"       ]" +
"     }," +
"     {" +
"       \"Asset\": \"GBP\"," +
"       \"Inventories\": []," +
"       \"Balances\": [" +
"         {" +
"           \"Exchange\": \"lykke\"," +
"           \"Amount\": 999938.48" +
"         }" +
"       ]" +
"     }," +
"     {" +
"       \"Asset\": \"JPY\"," +
"       \"Inventories\": []," +
"       \"Balances\": [" +
"         {" +
"           \"Exchange\": \"lykke\"," +
"           \"Amount\": 200.0" +
"         }" +
"       ]" +
"     }," +
"     {" +
"       \"Asset\": \"LKK\"," +
"       \"Inventories\": []," +
"       \"Balances\": [" +
"         {" +
"           \"Exchange\": \"lykke\"," +
"           \"Amount\": 1000000.0" +
"         }" +
"       ]" +
"     }," +
"     {" +
"       \"Asset\": \"LKK4972_AutoTest\"," +
"       \"Inventories\": []," +
"       \"Balances\": [" +
"         {" +
"           \"Exchange\": \"lykke\"," +
"           \"Amount\": 1100.0" +
"         }" +
"       ]" +
"     }," +
"     {" +
"       \"Asset\": \"USD\"," +
"       \"Inventories\": [" +
"         {" +
"           \"Exchange\": \"fakeExchange\"," +
"           \"Volume\": 1252.000," +
"           \"SellVolume\": 0.0," +
"           \"BuyVolume\": 1252.000" +
"         }," +
"         {" +
"           \"Exchange\": \"lykke\"," +
"           \"Volume\": -1370.7850," +
"           \"SellVolume\": 1403.4475," +
"           \"BuyVolume\": 32.6625" +
"         }" +
"       ]," +
"       \"Balances\": [" +
"         {" +
"           \"Exchange\": \"lykke\"," +
"           \"Amount\": 1017208.72" +
"         }," +
"         {" +
"           \"Exchange\": \"bitstamp\"," +
"           \"Amount\": 253.85" +
"         }" +
"       ]" +
"     }," +
"     {" +
"       \"Asset\": \"e9a8347b-6ef0-419a-9be3-f708ceb3a0f0\"," +
"       \"Inventories\": []," +
"       \"Balances\": [" +
"         {" +
"           \"Exchange\": \"lykke\"," +
"           \"Amount\": 1000000.0" +
"         }" +
"       ]" +
"     }," +
"     {" +
"       \"Asset\": \"ETH\"," +
"       \"Inventories\": []," +
"       \"Balances\": [" +
"         {" +
"           \"Exchange\": \"bitstamp\"," +
"           \"Amount\": 0.01500000" +
"         }" +
"       ]" +
"     }" +
"   ]" +
" }";
        
        [TestMethod]
        public void Test()
        {
            var repositoryMock = new Mock<IInventorySnapshotRepository>();
            var rateCalculatorClientMock = new Mock<IRateCalculatorClient>();
            rateCalculatorClientMock.Setup(x => x.GetAmountInBaseAsync(It.IsAny<IEnumerable<BalanceRecord>>(), It.IsAny<string>()))
                .ReturnsAsync((IEnumerable<BalanceRecord> balances, string currency) => 
                    balances.Select(x => new BalanceRecord(x.Balance, currency)));
            
            var service = new InventorySnapshotService(
                Mock.Of<ILogFactory>(),
                repositoryMock.Object, 
                rateCalculatorClientMock.Object);

            var snapshot = (InventorySnapshot)JsonConvert.DeserializeObject(_snapshot, typeof(InventorySnapshot));
            
            service.HandleAsync(snapshot).GetAwaiter().GetResult();
        }
    }
}
