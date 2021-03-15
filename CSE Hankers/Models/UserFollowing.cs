using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CSE_Hankers.Models
{
    public class UserFollowing
    {
        [Key]
        public int userFollowingId { get; set; }
        public ApplicationUser user { get; set; }
        public ApplicationUser follower { get; set; }

    }
}
