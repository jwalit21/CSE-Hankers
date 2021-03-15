﻿using CSE_Hankers.Models.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSE_Hankers.Models.SQLRepositories
{
    public class SQLUserRepository : IUserRepository
    {
        private readonly AppDbContext context;
        public SQLUserRepository(AppDbContext context)
        {
            this.context = context;
        }

        public UserFollowing Follow(UserFollowing userFollowing)
        {
            context.UserFollowings.Add(userFollowing);
            context.SaveChanges();
            return userFollowing;
        }

        public UserFollowing Unfollow(UserFollowing userFollowing)
        {
            context.Remove(userFollowing);
            context.SaveChanges();
            return userFollowing;
        }

        public ApplicationUser Update(ApplicationUser user)
        {
            var updatedUser = context.Users.Attach(user);
            updatedUser.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return user;
        }
    }
}
