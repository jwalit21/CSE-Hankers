using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CSE_Hankers.Models.ViewModels
{
    public class RegisterUser
    {
        public int id { get; set; }

        [Required]
        [StringLength(25, ErrorMessage = "Name should be less than 25 characters.")]
        [Display(Name = "Full Name")]
        public string name { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email (Username)")]
        public string email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "password")]
        public string password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("password", ErrorMessage = "Password and Confirm Password do not match")]
        public string confirmPassword { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Mobile number")]
        public string mobile { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "Organization name should be less than 500 characters.")]
        [Display(Name = "Organization Name")]
        public string organization { get; set; }

        [Required(ErrorMessage ="Profile Image is required")]
        [Display(Name = "Select Profile photo")]
        public IFormFile photoPath { get; set; }

    }
}
