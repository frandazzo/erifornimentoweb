using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication3.Domain.Security;
using WebApplication3.Models;

namespace WebApplication3.Facades.ValueResolvers
{
    public class RoleResolver : IValueResolver<User, UserDTO, string>
    {
        private readonly UserManager<User> userManager;


        public RoleResolver(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public string Resolve(User source, UserDTO destination, string destMember, ResolutionContext context)
        {
            Task<IList<string>> rolesTask = userManager.GetRolesAsync(source);
            rolesTask.Wait();
            return String.Join(",", rolesTask.Result);
        }
    }
}