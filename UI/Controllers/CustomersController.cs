using Common.Models.Requests.Customer;
using Common.Models.Responses.Customer;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using UI.Interfaces.Services;
using UI.Models;
using UI.Models.ViewModels.Customers;

namespace UI.Controllers
{
    [Route("[controller]")]
    public class CustomersController : Controller
    {
        private IHttpClientService HttpClientService { get; set; }
        private JsonSerializerOptions apiJsonSerializerOptions;

        public CustomersController(IHttpClientService httpClientService)
        {
            this.HttpClientService = httpClientService ?? throw new ArgumentNullException(nameof(httpClientService));
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            CustomerListViewModel customerListViewModel = new CustomerListViewModel
            {
                Customers = new List<CustomerViewModel>(),
            };

            APICallResult<CustomerListResponse> apiCallResult =
                await this.HttpClientService.MakeRequest<CustomerListResponse>(HttpMethod.Get, "http://localhost:50781/api/customers/")
                .ConfigureAwait(true);

            if (apiCallResult.IsSuccessStatusCode)
            {
                if (apiCallResult.ResultObject.Customers != null && apiCallResult.ResultObject.Customers.Count > 0)
                {
                    foreach (CustomerResponse cr in apiCallResult.ResultObject.Customers)
                    {
                        customerListViewModel.Customers.Add(
                            new CustomerViewModel
                            {
                                CustomerId = cr.CustomerId,
                                Name = cr.Name,
                                CompanyRegistrationNumber = cr.CompanyRegistrationNumber,
                                IsActive = cr.IsActive
                            });
                    }
                }
            }
            else
            {
                // Display error.
            }

            return View(customerListViewModel);
        }

        [HttpGet("Add")]
        public IActionResult Add()
        {
            return View("AddUpdate");
        }

        [HttpPost("Add")]
        public async Task<IActionResult> ProcessAdd(CustomerViewModel customerViewModel)
        {
            AddCustomerRequest addCustomerRequest = new AddCustomerRequest
            {
                Name = customerViewModel.Name,
                CompanyRegistrationNumber = customerViewModel.CompanyRegistrationNumber,
                IsActive = customerViewModel.IsActive,
                IncorporationDate = customerViewModel.IncorporationDate.Value,
                Turnover = customerViewModel.Turnover.HasValue ? customerViewModel.Turnover.Value : 0
            };

            APICallResult<CustomerResponse> apiCallResult = await this.HttpClientService.MakeRequest<CustomerResponse>(
                HttpMethod.Post,
                "http://localhost:50781/api/customers/",
                this.Serialize<AddCustomerRequest>(addCustomerRequest)).ConfigureAwait(true);

            if (apiCallResult.IsSuccessStatusCode)
            {
                // Update successful.
            }
            else
            {
                // Display error.
            }

            return View("~/Views/Customers/AddUpdate.cshtml");
        }

        [HttpGet("Update")]
        public IActionResult Update(int customerId)
        {
            var model = new CustomerViewModel();
            return View("AddUpdate", model);
        }

        [HttpGet("GetCustomerById")]
        public async Task<IActionResult> GetCustomerById(int customerId)
        {
            CustomerViewModel customerViewModel;

            APICallResult<CustomerResponse> apiCallResult =
                await this.HttpClientService.MakeRequest<CustomerResponse>(HttpMethod.Get, string.Format("http://localhost:50781/api/customers/GetById/?customerId={0}", customerId))
                .ConfigureAwait(true);

            if (apiCallResult.IsSuccessStatusCode)
            {
                if (apiCallResult.ResultObject != null)
                {
                    var cr = apiCallResult.ResultObject;

                    customerViewModel = new CustomerViewModel
                    {
                        CustomerId = cr.CustomerId,
                        Name = cr.Name,
                        CompanyRegistrationNumber = cr.CompanyRegistrationNumber,
                        IsActive = cr.IsActive,
                        IncorporationDate = cr.IncorporationDate,
                        Turnover = cr.Turnover
                    };

                    return View("~/Views/Customers/View.cshtml", customerViewModel);
                }
            }
            else
            {
                // Display error.
            }

            return View("~/Views/Customers/AddUpdate.cshtml");
        }

        private string Serialize<T>(T objectToBeSerialized)
        {
            return JsonSerializer.Serialize<T>(objectToBeSerialized, this.ApiJsonSerializerOptions);
        }

        protected JsonSerializerOptions ApiJsonSerializerOptions
        {
            get
            {
                if (this.apiJsonSerializerOptions == null)
                {
                    this.apiJsonSerializerOptions = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        WriteIndented = true,
                        PropertyNameCaseInsensitive = true,
                    };
                }

                return this.apiJsonSerializerOptions;
            }
        }
    }
}