using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Portfolio.Models;
using System;
using System.Net.Http;
using Newtonsoft.Json;
using Portfolio.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Portfolio.Utilities;

namespace Portfolio.Controllers
{
    public class HomeController : Controller
    {
        private readonly Uri baseAddress = new Uri("https://api.coingecko.com/api/v3");
        private readonly HttpClient client;
        private readonly ILogger _logger;
        private CryptoPriceChartDataModel cryptoPriceChartData;
        private CryptoMarketDataModel cryptoMarketData;
        private IndexViewModel indexViewModel;
        private readonly IMailSender _mailSender;

        //Dependency injection
        public HomeController(ILogger<HomeController> logger, IMailSender mailSender)
        {
            _logger = logger;
            client = new HttpClient();
            client.BaseAddress = baseAddress;
            cryptoPriceChartData = new CryptoPriceChartDataModel();
            cryptoMarketData = new CryptoMarketDataModel();
            indexViewModel = new IndexViewModel();
            _mailSender = mailSender;
        }

        [HttpGet]
        public IActionResult Index()
        {
            //Get API data and format them to be properly displayed
            HttpResponseMessage priceResponse = client.GetAsync(client.BaseAddress + "/coins/bitcoin/market_chart?vs_currency=usd&days=1&interval=minutely").Result;
            HttpResponseMessage marketDataResponse = client.GetAsync(client.BaseAddress + "/coins/bitcoin?localization=false&market_data=true").Result;
            if (priceResponse.IsSuccessStatusCode && marketDataResponse.IsSuccessStatusCode)
            {
                string priceData = priceResponse.Content.ReadAsStringAsync().Result;
                string marketData = marketDataResponse.Content.ReadAsStringAsync().Result;
                cryptoPriceChartData = JsonConvert.DeserializeObject<CryptoPriceChartDataModel>(priceData);
                cryptoMarketData = JsonConvert.DeserializeObject<CryptoMarketDataModel>(marketData);
                indexViewModel.CryptoPriceChartData = JsonConvert.DeserializeObject<CryptoPriceChartDataModel>(priceData);
                indexViewModel.CryptoMarketData = JsonConvert.DeserializeObject<CryptoMarketDataModel>(marketData);
            }
            else
            {
                _logger.LogError(priceResponse.StatusCode.ToString());
                _logger.LogError(marketDataResponse.StatusCode.ToString());
            }
            return View(indexViewModel);
        }

        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Resume()
        {      
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Message(FormModel formObj)
        {
            if (ModelState.IsValid)
            {
                var clientPassword = formObj.Password;
                var clientEmail = formObj.Email;
                var clientName = formObj.Name;
                var clientMessage = formObj.Message;

                //Check if form HoneyPot was triggered
                if (String.IsNullOrEmpty(clientPassword))
                {
                    //Send contact form info to myself via email
                    var toEmail = "[ENTER DESIRED RECEIVER EMAIL HERE]";
                    var toDisplayName = "[ENTER RECEIVER DISPLAY NAME HERE]";

                    _mailSender.SendPlainTextEmailSendgrid(toEmail, toDisplayName, clientName, clientEmail, clientMessage);

                    return View("MessageConfirmation");
                }
                else
                {
                    //HoneyPot triggered, log spam info
                    _logger.LogWarning("Possible Bot Detected = [Name: " + clientName + "], "
                        + "[Email: " + clientEmail + "], "
                        + "[Message: " + clientMessage + "]");

                    return View("MessageConfirmation");
                }
            }
            return View("Contact");
        }

        [HttpGet]
        public IActionResult MessageConfirmation()
        {
            return View();
        }
    }
}
