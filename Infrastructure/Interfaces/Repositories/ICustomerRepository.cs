using Infrastructure.Entities;
using System.Collections.Generic;

namespace Infrastructure.Interfaces.Repositories
{
    public interface ICustomerRepository
    {
        int Add(Customer customer);

        List<Customer> GetAll();
    }
}