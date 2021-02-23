using Infrastructure.Context;
using Infrastructure.Entities;
using Infrastructure.Interfaces.Repositories;
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

        public List<Customer> GetAll()
        {
            return this.context.Customer.ToList();
        }
    }
}