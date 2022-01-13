using Portfolio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portfolio.ViewModels
{
    public class IndexViewModel
    {
        public FormModel Form { get; set; }
        public CryptoMarketDataModel CryptoMarketData { get; set; }
        public CryptoPriceChartDataModel CryptoPriceChartData { get; set; }
    }
}
