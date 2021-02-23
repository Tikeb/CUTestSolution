using Common.Models.Responses.Home;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using UI.Interfaces.Services;
using UI.Models;
using UI.Models.ViewModels.Home;

namespace UI.Controllers
{
    public class HomeController : Controller
    {
        private IHttpClientService HttpClientService { get; set; }

        public HomeController(IHttpClientService httpClientService)
        {
            this.HttpClientService = httpClientService ?? throw new ArgumentNullException(nameof(httpClientService));
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> IndexAsync()
        {
            APICallResult<HelloResponse> apiCallResult =
                await this.HttpClientService.MakeRequest<HelloResponse>(HttpMethod.Get, "http://localhost:50781/api/home/hello/Username")
                .ConfigureAwait(true);

            HomeViewModel homeViewModel = new HomeViewModel();

            if (apiCallResult.IsSuccessStatusCode)
            {
                homeViewModel.Message = apiCallResult.ResultObject.Message;
            }
            else
            {
                homeViewModel.Message = "An error has occurred whilst contacting the API.";
            }

            return View(homeViewModel);
        }
    }
}