using System;
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

        [TestMethod]
        public void GetDynamics_SampleSnapshots()
        {
            var startSnapshot = new InventorySnapshot
            {
                Source = "Default",
                Timestamp = DateTime.Parse("2018-08-07T07:28:17.7350053Z"),
                Assets = new[]
                {
                    new AssetBalanceInventory
                    {
                        AssetId = "LKK",
                        AssetDisplayId = "LKK",
                        Inventories = new[]
                        {
                            new AssetInventory
                            {
                                Exchange = "lykke",
                                Volume = 4.0m,
                                SellVolume = 8.0m,
                                BuyVolume = 2.0m,
                                VolumeInUsd = 12.0m
                            },
                            new AssetInventory
                            {
                                Exchange = "bitstamp",
                                Volume = -6.0m,
                                SellVolume = 8.0m,
                                BuyVolume = 2.0m,
                                VolumeInUsd = -24.0m
                            }
                        },
                        Balances = new[]
                        {
                            new AssetBalance
                            {
                                Exchange = "lykke",
                                Amount = 89.0m,
                                AmountInUsd = 40m
                            },
                            new AssetBalance
                            {
                                Exchange = "bitstamp",
                                Amount = 80.0m,
                                AmountInUsd = 49.0m
                            }
                        }
                    },
                    new AssetBalanceInventory
                    {
                        AssetId = "BTC",
                        AssetDisplayId = "BTC",
                        Inventories = new[]
                        {
                            new AssetInventory
                            {
                                Exchange = "lykke",
                                Volume = 12.0m,
                                SellVolume = 4.0m,
                                BuyVolume = 6.0m,
                                VolumeInUsd = 48.0m
                            },
                            new AssetInventory
                            {
                                Exchange = "bitstamp",
                                Volume = 16.0m,
                                SellVolume = 5.0m,
                                BuyVolume = 8.0m,
                                VolumeInUsd = 32.0m
                            }
                        },
                        Balances = new[]
                        {
                            new AssetBalance
                            {
                                Exchange = "lykke",
                                Amount = 50.0m,
                                AmountInUsd = 80m
                            },
                            new AssetBalance
                            {
                                Exchange = "bitstamp",
                                Amount = 200.0m,
                                AmountInUsd = 150.0m
                            }
                        }
                    }
                },
                AssetPairInventories = new[]
                {
                    new AssetPairInventory
                    {
                        AssetPair = "BTCUSD",
                        TotalSellBaseVolume = 1.8m,
                        TotalBuyBaseVolume = 0.09m,
                        TotalSellQuoteVolume = 500m,
                        TotalBuyQuoteVolume = 1000m,
                        CountSellTrades = 3,
                        CountBuyTrades = 5
                    },
                    new AssetPairInventory
                    {
                        AssetPair = "BTCGBP",
                        TotalSellBaseVolume = 1.0m,
                        TotalBuyBaseVolume = 9.0m,
                        TotalSellQuoteVolume = 100.0m,
                        TotalBuyQuoteVolume = 450.0m,
                        CountSellTrades = 8,
                        CountBuyTrades = 6
                    },
                }
            };

            var endSnapshot = new InventorySnapshot
            {
                Source = "Default",
                Timestamp = DateTime.Parse("2018-08-08T07:28:17.7350053Z"),
                Assets = new[]
                {
                    new AssetBalanceInventory
                    {
                        AssetId = "LKK",
                        AssetDisplayId = "LKK",
                        Inventories = new[]
                        {
                            new AssetInventory
                            {
                                Exchange = "lykke",
                                Volume = 12.2m,
                                SellVolume = 4.1m,
                                BuyVolume = 12.3m,
                                VolumeInUsd = 24.4m
                            },
                            new AssetInventory
                            {
                                Exchange = "external",
                                Volume = 24.0m,
                                SellVolume = 13.6m,
                                BuyVolume = 92.1m,
                                VolumeInUsd = 48.0m
                            }
                        },
                        Balances = new[]
                        {
                            new AssetBalance
                            {
                                Exchange = "lykke",
                                Amount = 92.6m,
                                AmountInUsd = 40.1m
                            },
                            new AssetBalance
                            {
                                Exchange = "external",
                                Amount = 44.2m,
                                AmountInUsd = 22.2m
                            }
                        }
                    },
                    new AssetBalanceInventory
                    {
                        AssetId = "ZAJK",
                        AssetDisplayId = "ZAJK",
                        Inventories = new[]
                        {
                            new AssetInventory
                            {
                                Exchange = "lykke",
                                Volume = 12.2m,
                                SellVolume = 24.2m,
                                BuyVolume = 8.6m,
                                VolumeInUsd = 24.4m
                            },
                            new AssetInventory
                            {
                                Exchange = "external",
                                Volume = 13.0m,
                                SellVolume = 31.0m,
                                BuyVolume = 240.0m,
                                VolumeInUsd = 26.0m
                            }
                        },
                        Balances = new[]
                        {
                            new AssetBalance
                            {
                                Exchange = "lykke",
                                Amount = 12.6m,
                                AmountInUsd = 50.0m
                            },
                            new AssetBalance
                            {
                                Exchange = "external",
                                Amount = 14.2m,
                                AmountInUsd = 32.2m
                            }
                        }
                    }
                },
                AssetPairInventories = new[]
                {
                    new AssetPairInventory
                    {
                        AssetPair = "BTCUSD",
                        TotalSellBaseVolume = 18.8m,
                        TotalBuyBaseVolume = 10.19m,
                        TotalSellQuoteVolume = 480m,
                        TotalBuyQuoteVolume = 9000.00m,
                        CountSellTrades = 14,
                        CountBuyTrades = 7
                    },
                    new AssetPairInventory
                    {
                        AssetPair = "BTCJPY",
                        TotalSellBaseVolume = 2.0m,
                        TotalBuyBaseVolume = 11.0m,
                        TotalSellQuoteVolume = 52.0m,
                        TotalBuyQuoteVolume = 350.0m,
                        CountSellTrades = 15,
                        CountBuyTrades = 8
                    },
                }
            };

            InventorySnapshotDynamics dynamics = new InventoryDynamicsCalculator().GetDynamics(startSnapshot, endSnapshot);

            string expectedDynamics =
                "{"
                + "  \"Source\": \"Default\","
                + "  \"Timestamp\": \"2018-08-08T14:28:17.7350053+07:00\","
                + "  \"Assets\": ["
                + "    {"
                + "      \"Asset\": \"LKK\","
                + "      \"AssetDisplayId\": \"LKK\","
                + "      \"Inventories\": ["
                + "        {"
                + "          \"Exchange\": \"lykke\","
                + "          \"Volume\": 8.2,"
                + "          \"SellVolume\": -3.9,"
                + "          \"BuyVolume\": 10.3,"
                + "          \"VolumeInUsd\": 16.4"
                + "        },"
                + "        {"
                + "          \"Exchange\": \"bitstamp\","
                + "          \"Volume\": 6.0,"
                + "          \"SellVolume\": -8.0,"
                + "          \"BuyVolume\": -2.0,"
                + "          \"VolumeInUsd\": 24.0"
                + "        },"
                + "        {"
                + "          \"Exchange\": \"external\","
                + "          \"Volume\": 24.0,"
                + "          \"SellVolume\": 13.6,"
                + "          \"BuyVolume\": 92.1,"
                + "          \"VolumeInUsd\": 48.0"
                + "        }"
                + "      ],"
                + "      \"Balances\": ["
                + "        {"
                + "          \"Exchange\": \"lykke\","
                + "          \"Amount\": 92.6,"
                + "          \"AmountInUsd\": 40.1"
                + "        },"
                + "        {"
                + "          \"Exchange\": \"bitstamp\","
                + "          \"Amount\": 0.0,"
                + "          \"AmountInUsd\": 0.0"
                + "        },"
                + "        {"
                + "          \"Exchange\": \"external\","
                + "          \"Amount\": 44.2,"
                + "          \"AmountInUsd\": 22.2"
                + "        }"
                + "      ],"
                + "      \"TotalBalance\": 136.8,"
                + "      \"TotalBalanceInUsd\": 62.3,"
                + "      \"PriceByBalanceInUsd\": 0.4554093567251461988304093567"
                + "    },"
                + "    {"
                + "      \"Asset\": \"BTC\","
                + "      \"AssetDisplayId\": \"BTC\","
                + "      \"Inventories\": ["
                + "        {"
                + "          \"Exchange\": \"lykke\","
                + "          \"Volume\": -12.0,"
                + "          \"SellVolume\": -4.0,"
                + "          \"BuyVolume\": -6.0,"
                + "          \"VolumeInUsd\": -48.0"
                + "        },"
                + "        {"
                + "          \"Exchange\": \"bitstamp\","
                + "          \"Volume\": -16.0,"
                + "          \"SellVolume\": -5.0,"
                + "          \"BuyVolume\": -8.0,"
                + "          \"VolumeInUsd\": -32.0"
                + "        }"
                + "      ],"
                + "      \"Balances\": ["
                + "        {"
                + "          \"Exchange\": \"lykke\","
                + "          \"Amount\": 0.0,"
                + "          \"AmountInUsd\": 0.0"
                + "        },"
                + "        {"
                + "          \"Exchange\": \"bitstamp\","
                + "          \"Amount\": 0.0,"
                + "          \"AmountInUsd\": 0.0"
                + "        }"
                + "      ],"
                + "      \"TotalBalance\": 0.0,"
                + "      \"TotalBalanceInUsd\": 0.0,"
                + "      \"PriceByBalanceInUsd\": 0.0"
                + "    },"
                + "    {"
                + "      \"Asset\": \"ZAJK\","
                + "      \"AssetDisplayId\": \"ZAJK\","
                + "      \"Inventories\": ["
                + "        {"
                + "          \"Exchange\": \"lykke\","
                + "          \"Volume\": 12.2,"
                + "          \"SellVolume\": 24.2,"
                + "          \"BuyVolume\": 8.6,"
                + "          \"VolumeInUsd\": 24.4"
                + "        },"
                + "        {"
                + "          \"Exchange\": \"external\","
                + "          \"Volume\": 13.0,"
                + "          \"SellVolume\": 31.0,"
                + "          \"BuyVolume\": 240.0,"
                + "          \"VolumeInUsd\": 26.0"
                + "        }"
                + "      ],"
                + "      \"Balances\": ["
                + "        {"
                + "          \"Exchange\": \"lykke\","
                + "          \"Amount\": 12.6,"
                + "          \"AmountInUsd\": 50.0"
                + "        },"
                + "        {"
                + "          \"Exchange\": \"external\","
                + "          \"Amount\": 14.2,"
                + "          \"AmountInUsd\": 32.2"
                + "        }"
                + "      ],"
                + "      \"TotalBalance\": 26.8,"
                + "      \"TotalBalanceInUsd\": 82.2,"
                + "      \"PriceByBalanceInUsd\": 3.0671641791044776119402985075"
                + "    }"
                + "  ],"
                + "  \"AssetPairInventories\": ["
                + "    {"
                + "      \"AssetPair\": \"BTCUSD\","
                + "      \"TotalSellBaseVolume\": 17.0,"
                + "      \"TotalBuyBaseVolume\": 10.10,"
                + "      \"TotalSellQuoteVolume\": -20.0,"
                + "      \"TotalBuyQuoteVolume\": 8000.00,"
                + "      \"CountSellTrades\": 11,"
                + "      \"CountBuyTrades\": 2"
                + "    },"
                + "    {"
                + "      \"AssetPair\": \"BTCGBP\","
                + "      \"TotalSellBaseVolume\": -1.0,"
                + "      \"TotalBuyBaseVolume\": -9.0,"
                + "      \"TotalSellQuoteVolume\": -100.0,"
                + "      \"TotalBuyQuoteVolume\": -450.0,"
                + "      \"CountSellTrades\": -8,"
                + "      \"CountBuyTrades\": -6"
                + "    },"
                + "    {"
                + "      \"AssetPair\": \"BTCJPY\","
                + "      \"TotalSellBaseVolume\": 2.0,"
                + "      \"TotalBuyBaseVolume\": 11.0,"
                + "      \"TotalSellQuoteVolume\": 52.0,"
                + "      \"TotalBuyQuoteVolume\": 350.0,"
                + "      \"CountSellTrades\": 15,"
                + "      \"CountBuyTrades\": 8"
                + "    }"
                + "  ]"
                + "}";



            Assert.AreEqual(expectedDynamics.Replace(" ", ""), dynamics.ToJson());
        }
    }
}
