using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSE_Hankers.Models.ViewModels
{
    public class UpdateUser: RegisterUser
    {
        public string existingPhotoPath { get; set; }
    }
}
