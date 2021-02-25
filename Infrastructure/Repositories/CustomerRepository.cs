using Infrastructure.Context;
using Infrastructure.Entities;
using Infrastructure.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly InMemoryDbContext context;

        public CustomerRepository(InMemoryDbContext context)
        {
            this.context = context;
        }

        public int Add(Customer customer)
        {
            this.context.Add(customer);

            return this.context.SaveChanges(); ;
        }

        // AW: Changed name of method from GetAll to GetAllActiveOrderedByName to properly represent what this method is doing
        public List<Customer> GetAllActiveOrderedByName()
        {
            // AW: Updated GetAll method to return only Active and to order by Name
            return this.context.Customer
                .Where(x => x.IsActive)
                .OrderBy(x => x.Name)
                .ToList();
        }

        // AW: Added new methods for getting a customer by Id and Updating a customer
        public Customer GetById(int customerId)
        {
            return this.context.Customer.Single(x => x.CustomerId == customerId);
        }

        public void Update(Customer customer)
        {
            this.context.Update(customer);
            this.context.SaveChanges();
        }

        public List<Customer> Search(string name, string companyRegistrationNumber)
        {
            return this.context.Customer
                .Where(x => x.IsActive && 
                    (x.Name.Contains(name, StringComparison.InvariantCultureIgnoreCase) || 
                    x.CompanyRegistrationNumber.Contains(companyRegistrationNumber, StringComparison.InvariantCultureIgnoreCase)))
                .ToList();
        }
    }
}