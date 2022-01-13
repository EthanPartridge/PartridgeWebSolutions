using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Portfolio.Hubs;
using Portfolio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Portfolio.BackgroundTasks
{
    //Background Service to request data from CoinGecko API
    public class CryptoHttpListener : BackgroundService
    {
        private readonly ILogger _logger;
        private HttpClient client;
        private readonly Uri baseAddress = new Uri("https://api.coingecko.com/api/v3");
        private readonly IHubContext<CryptoPriceHub, ICryptoPriceClient> _hubContext;
        private CryptoPriceChartDataModel cryptoPriceChartData;
        private CryptoMarketDataModel cryptoMarketData;

        //Dependency Injection
        public CryptoHttpListener(ILogger<CryptoHttpListener> logger, IHubContext<CryptoPriceHub, ICryptoPriceClient> hubContext)
        {
            _logger = logger;
            _hubContext = hubContext;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            //Initalizes model when background service starts
            cryptoPriceChartData = new CryptoPriceChartDataModel();
            cryptoMarketData = new CryptoMarketDataModel();
            //Initializes client when background service starts
            client = new HttpClient();
            client.BaseAddress = baseAddress;
            //call base StartAsync to make sure the method does whatever it is supposed to do just in case as a backup
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            //Closes HTTP client connection
            client.Dispose();
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                HttpResponseMessage priceResponse = client.GetAsync(client.BaseAddress + "/coins/bitcoin/market_chart?vs_currency=usd&days=1&interval=minutely").Result;
                HttpResponseMessage marketDataResponse = client.GetAsync(client.BaseAddress + "/coins/bitcoin?localization=false&market_data=true").Result;
                if (priceResponse.IsSuccessStatusCode && marketDataResponse.IsSuccessStatusCode)
                {
                    string priceData = priceResponse.Content.ReadAsStringAsync().Result;
                    string marketData = marketDataResponse.Content.ReadAsStringAsync().Result;
                    cryptoPriceChartData = JsonConvert.DeserializeObject<CryptoPriceChartDataModel>(priceData);
                    cryptoMarketData = JsonConvert.DeserializeObject<CryptoMarketDataModel>(marketData);
                    await SendMessage(cryptoPriceChartData, cryptoMarketData);
                }
                else
                {
                    _logger.LogError("API call failed");
                }

                await Task.Delay(1 * 60 * 1000, stoppingToken);
            }
        }

        public async Task SendMessage(CryptoPriceChartDataModel cryptoPriceChartData, CryptoMarketDataModel cryptoMarketData)
        {
            (long[] unixTimeArray, decimal[] priceArray) = CreateArraysFromData(cryptoPriceChartData);
            string[] cryptoMarketDataArray = CombineDataIntoStringArray(cryptoMarketData);
            await _hubContext.Clients.All.CryptoPriceUpdated(unixTimeArray, priceArray, cryptoMarketDataArray);
        }

        public (long[] unixTimeArray, decimal[] priceArray) CreateArraysFromData(CryptoPriceChartDataModel cryptoPriceChartData)
        {
            List<long> unixTimeArray = new List<long>();
            List<decimal> priceArray = new List<decimal>();
            for (int i = 0; i < cryptoPriceChartData.Prices.GetLength(0); i++)
            {
                unixTimeArray.Add(Convert.ToInt64(cryptoPriceChartData.Prices[i, 0]));
                priceArray.Add(Convert.ToDecimal(cryptoPriceChartData.Prices[i, 1]));
            }
            return (unixTimeArray.ToArray(), priceArray.ToArray());
        }

        public string[] CombineDataIntoStringArray(CryptoMarketDataModel cryptoMarketData)
        {
            List<string> cryptoMarketDataArray = new List<string>();
            cryptoMarketDataArray.Add("marketCapRank:" + cryptoMarketData.MarketData.MarketCapRank);
            cryptoMarketDataArray.Add("marketCap:" + cryptoMarketData.MarketData.MarketCap.Usd);
            cryptoMarketDataArray.Add("totalVolume:" + cryptoMarketData.MarketData.TotalVolume.Usd);
            cryptoMarketDataArray.Add("circulatingSupply:" + cryptoMarketData.MarketData.CirculatingSupply);
            cryptoMarketDataArray.Add("totalSupply:" + cryptoMarketData.MarketData.TotalSupply);
            return cryptoMarketDataArray.ToArray();
        }
    }
}
