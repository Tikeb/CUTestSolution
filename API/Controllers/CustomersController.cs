using Common.Models.Requests.Customer;
using Common.Models.Responses.Customer;
using Infrastructure.Entities;
using Infrastructure.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : Controller
    {
        private readonly ICustomerRepository customerRepository;

        public CustomersController(ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        }

        [HttpGet()]
        public IActionResult GetAll()
        {
            List<Customer> customers = this.customerRepository.GetAll();

            CustomerListResponse customerList = new CustomerListResponse
            {
                Customers = new List<CustomerResponse>(),
            };

            foreach (Customer c in customers)
            {
                customerList.Customers.Add(new CustomerResponse
                {
                    CustomerId = c.CustomerId,
                    Name = c.Name,
                    CompanyRegistrationNumber = c.CompanyRegistrationNumber,
                    IsActive = c.IsActive,
                });
            }

            return this.Ok(customerList);
        }

        [HttpPost]
        public IActionResult Add([FromBody] AddCustomerRequest addCustomerRequest)
        {
            Customer customer = new Customer
            {
                Name = addCustomerRequest.Name,
                CompanyRegistrationNumber = addCustomerRequest.CompanyRegistrationNumber,
                IsActive = addCustomerRequest.IsActive,
            };

            int rowsAffected = this.customerRepository.Add(customer);

            if (rowsAffected == 0)
            {
                return this.BadRequest("Add failed.");
            }

            return this.Ok();
        }
    }
}