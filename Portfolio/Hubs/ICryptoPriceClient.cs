using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Portfolio.Hubs
{
    public interface ICryptoPriceClient
    {
        Task CryptoPriceUpdated(long[] unixTimeArray, decimal[] priceArray, string[] cryptoMarketDataArray );
    }
}