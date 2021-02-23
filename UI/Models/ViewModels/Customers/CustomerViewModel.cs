namespace UI.Models.ViewModels.Customers
{
    public class CustomerViewModel
    {
        public int CustomerId { get; set; }

        public string Name { get; set; }

        public string CompanyRegistrationNumber { get; set; }

        public bool IsActive { get; set; }
    }
}