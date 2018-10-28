using JetBrains.Annotations;

namespace Lykke.Service.MarketMakerReports.Client.Models.PnL
{
    /// <summary>
    /// Model for by asset PnL results
    /// </summary>
    [PublicAPI]
    public class AssetPnLModel
    {
        /// <summary>
        /// Asset DisplayId
        /// </summary>
        public string Asset { get; set; }
        
        /// <summary>
        /// Trading part of the PnL
        /// </summary>
        public decimal Trading { get; set; }
        
        /// <summary>
        /// Directional part of the PnL
        /// </summary>
        public decimal Directional { get; set; }
        
        /// <summary>
        /// Total PnL as sum of its parts
        /// </summary>
        public decimal Total { get; set; }
     
        /// <summary>
        /// Balance in the currency of asset on the start date of PnL calculation period
        /// </summary>
        public decimal StartBalance { get; set; }
        
        /// <summary>
        /// Converted to USD balance on the start date of PnL calculation period
        /// </summary>
        public decimal StartBalanceInUsd { get; set;  }
        
        /// <summary>
        /// Price of the asset (amount of USD per one asset) on the start date
        /// </summary>
        public decimal StartPrice { get; set; }
        
        /// <summary>
        /// Final balance in the currency of asset at the end of PnL calculation period
        /// </summary>
        public decimal EndBalance { get; set; }
        
        /// <summary>
        /// Final balance (converted to USD) in the currency of asset at the end of PnL calculation period 
        /// </summary>
        public decimal EndBalanceInUsd { get; set; }
        
        /// <summary>
        /// Price of the asset (amount of USD per one asset) at the end of the PnL calculation period
        /// </summary>
        public decimal EndPrice { get; set; }
    }
}
