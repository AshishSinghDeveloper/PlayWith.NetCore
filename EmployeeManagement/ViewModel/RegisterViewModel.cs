using EmployeeManagement.Utilities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.ViewModel
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Remote(action:"IsEmailInUse", controller:"Account")]
        [ValidateEmailDomain(allowedDomain: "mycompany.com", ErrorMessage = "Email Domain must be mycompany.com")]
        public String Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage ="Password and Confirm Password do not match")]
        public string ConfirmPassword { get; set; }
    }
}
