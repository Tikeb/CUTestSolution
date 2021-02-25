using Infrastructure.Entities;
using System.Collections.Generic;

namespace Infrastructure.Interfaces.Repositories
{
    public interface ICustomerRepository
    {
        int Add(Customer customer);

        // AW: Changed name of method from GetAll to GetAllActiveOrderedByName to properly represent what this method is doing
        List<Customer> GetAllActiveOrderedByName();

        // AW: Added GetById, Update and search
        Customer GetById(int customerId);

        void Update(Customer customer);
        
        List<Customer> Search(string name, string companyRegistrationNumber);
    }
}