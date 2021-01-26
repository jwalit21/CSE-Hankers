using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CSE_Hankers.Models.ViewModels
{
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email (Username)")]
        public string email { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string password { get; set; }

        [Display(Name = "Remember me")]
        public bool rememberMe { get; set; }

        public string role { get; set; }
    }
}
