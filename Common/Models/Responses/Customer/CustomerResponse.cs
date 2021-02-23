namespace Common.Models.Responses.Customer
{
    public class CustomerResponse
    {
        public int CustomerId { get; set; }

        public string Name { get; set; }

        public string CompanyRegistrationNumber { get; set; }

        public bool IsActive { get; set; }
    }
}