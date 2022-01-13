using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portfolio.Models
{
    public class CryptoMarketDataModel
    {
        public string Id { get; set; }
        [JsonProperty("market_data")]
        public MarketPropertiesModel MarketData { get; set; }
    }

    public class MarketPropertiesModel
    {
        [JsonProperty("market_cap")]
        public MarketCap MarketCap { get; set; }
        [JsonProperty("market_cap_rank")]
        public int MarketCapRank { get; set; }
        [JsonProperty("total_volume")]
        public TotalVolume TotalVolume { get; set; }
        [JsonProperty("total_supply")]
        public float TotalSupply { get; set; }
        [JsonProperty("circulating_supply")]
        public float CirculatingSupply { get; set; }
        [JsonProperty("last_updated")]
        public DateTime LastUpdated { get; set; }
    }

    public class MarketCap
    {
        public long Usd { get; set; }
    }

    public class TotalVolume
    {
        public long Usd { get; set; }
    }

}