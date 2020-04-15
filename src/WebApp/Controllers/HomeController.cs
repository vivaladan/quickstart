﻿using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
        public IActionResult Logout()
        {
            return SignOut("Cookies", "oidc");
        }
        
        public async Task<IActionResult> CallApi()
        {
            ViewBag.Json = await HttpRequest("https://localhost:5001/identity");
            return View("Json");
        }

        public async Task<IActionResult> GetProfile()
        {
            ViewBag.Json = await HttpRequest("https://localhost:5000/api/profile");
            return View("Json");
        }
        
        public async Task<IActionResult> GetClients()
        {
            ViewBag.Json = await HttpRequest("https://localhost:5000/api/clients");
            return View("Json");
        }

        private async Task<string> HttpRequest(string url)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var content = await client.GetStringAsync(url);

            return JValue.Parse(content).ToString(Formatting.Indented);
        }
    }
}
