using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace UI.Models.ViewModels.Customers
{
    public class CustomerViewModel
    {
        public CustomerViewModel()
        {
            IsActive = true;
        }

        [HiddenInput]
        public int CustomerId { get; set; }

        // AW: Added data annotations on Name, CompanyRegistrationNumber and IsActive for formatting and validation 

        [Required(ErrorMessage = "Please enter a customer name")]
        [StringLength(255)]
        [Display(Name = "Customer Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Company registration number is required")]
        [StringLength(15)]
        [Display(Name = "Company Registration Number")]
        public string CompanyRegistrationNumber { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        // AW: Added new fields with associated data annotations for formating and validation
        [Required(ErrorMessage = "Incorporation date is required")]
        [Display(Name = "Incorporation Date")]
        [DataType(DataType.Date)]
        public DateTime? IncorporationDate { get; set; }

        [DataType(DataType.Currency)]
        public decimal? Turnover { get; set; }
    }
}