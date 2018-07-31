using JetBrains.Annotations;
using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.MarketMakerReports.Client 
{
    /// <summary>
    /// MarketMakerReports client settings.
    /// </summary>
    [PublicAPI]
    public class MarketMakerReportsServiceClientSettings 
    {
        /// <summary>Service url.</summary>
        [HttpCheck("api/isalive")]
        public string ServiceUrl {get; set;}
    }
}
