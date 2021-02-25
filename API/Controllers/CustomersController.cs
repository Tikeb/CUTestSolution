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
        public IActionResult GetAllActiveOrderedByName() // AW: Changed method name to better represent what the method is doing
        {
            List<Customer> customers = this.customerRepository.GetAllActiveOrderedByName();

            // AW: Refactored code to avoid duplication
            CustomerListResponse customerList = PackageGetData(customers);

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
                IncorporationDate = addCustomerRequest.IncorporationDate,
                Turnover = addCustomerRequest.Turnover
            };

            int rowsAffected = this.customerRepository.Add(customer);

            if (rowsAffected == 0)
            {
                return this.BadRequest("Add failed.");
            }

            return this.Ok();
        }

        [HttpGet("GetById")]
        public IActionResult GetById(int customerId)
        {
            var customer = this.customerRepository.GetById(customerId);
            return this.Ok(customer);
        }

        [HttpPut]
        public IActionResult Update(Customer customer)
        {
            this.customerRepository.Update(customer);
            return this.Ok();
        }

        [HttpGet("Search")]
        public IActionResult Search(string name, string companyRegistrationNumber)
        {
            List<Customer> customers = this.customerRepository.Search(name, companyRegistrationNumber);

            CustomerListResponse customerList = PackageGetData(customers);

            return this.Ok(customerList);
        }

        // AW: Refactored code to avoid duplication
        private CustomerListResponse PackageGetData(List<Customer> customers)
        {
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

            return customerList;
        }
    }
}