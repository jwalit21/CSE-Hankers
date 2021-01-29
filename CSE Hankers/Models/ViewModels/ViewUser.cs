using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSE_Hankers.Models.ViewModels
{
    public class ViewUser
    {
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string mobile { get; set; }
        public string organization { get; set; }
        public string photoPath { get; set; }
    }
}
