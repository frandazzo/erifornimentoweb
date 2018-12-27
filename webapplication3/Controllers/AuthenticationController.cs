using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using WebApplication3.Domain.Security;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("api")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {

        readonly UserManager<User> userManager;
        readonly RoleManager<IdentityRole> roleManager;
        readonly IConfiguration configuration;

        public AuthenticationController(IConfiguration configuration,UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.configuration = configuration;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }



        string GenerateJwt(ClaimsIdentity identity)
        {

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.configuration["Jwt:Key"]));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(this.configuration["Jwt:Issuer"],
                  this.configuration["Jwt:Issuer"],
                  identity.Claims,
                  expires: DateTime.Now.AddMinutes(120000),
                  signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
            
        }
        [HttpPost]
        [Route("authenticate")]
        public async Task<ActionResult<LoginResult>> Login([FromBody] LoginCredential credential)
        {

            //if ("ciccillo".Equals(credential.Username) &&
            //    "password".Equals(credential.Password))
            //{
            //    return new LoginResult()
            //    {
            //        Token = Guid.NewGuid().ToString(),
            //        Name = "Franceschino",
            //        Role = "ADMIN",
            //        Error = ""
            //    };
            //}

            //return StatusCode(401,  new LoginResult()
            //{
            //    Token = "",
            //    Name = "",
            //    Role = "",
            //    Error = "Username o password errati"
            //});



            ClaimsIdentity identity = await GetClaimsIdentity(credential.Username, credential.Password);
            if (identity == null)
            {
                return StatusCode(401, new LoginResult()
                {
                    Token = string.Empty,
                    Name = string.Empty,
                    Role = string.Empty,
                    Error = "Username o password errati"
                });
            }

            

            string jwt =  GenerateJwt(identity);


            Request.HttpContext.Response.Headers.Add("Authorization", jwt);


            return new OkObjectResult(

                    new LoginResult()
                    {
                        Token = jwt,
                        Name = identity.Name,
                        Role = identity.Claims.Single(a => a.Type == ClaimTypes.Role).Value,
                        Error = ""
                    }

                );
        }

        private async Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                return await Task.FromResult<ClaimsIdentity>(null);

            // get the user to verifty
            User userToVerify = await userManager.FindByNameAsync(userName);

            if (userToVerify == null) return await Task.FromResult<ClaimsIdentity>(null);
            IList<string> roles = await userManager.GetRolesAsync(userToVerify);
            // check the credentials
            if (await userManager.CheckPasswordAsync(userToVerify, password))
            {
                if (!userToVerify.IsActive)
                    return await Task.FromResult<ClaimsIdentity>(null);
                return await Task.FromResult(GenerateClaimsIdentity(userToVerify, roles));
            }




            // Credentials are invalid, or account doesn't exist
            return await Task.FromResult<ClaimsIdentity>(null);
        }

        private ClaimsIdentity GenerateClaimsIdentity(User userToVerify, IList<string> roles)
        {
            ClaimsIdentity identity = new ClaimsIdentity(new GenericIdentity(userToVerify.UserName, JwtBearerDefaults.AuthenticationScheme));
            identity.AddClaim(new Claim(ClaimTypes.GivenName, userToVerify.Name));
            identity.AddClaim(new Claim(ClaimTypes.Name, userToVerify.UserName));
            identity.AddClaim(new Claim(ClaimTypes.Email, userToVerify.Email));
            identity.AddClaim(new Claim(ClaimTypes.Sid, userToVerify.Id));
            foreach (var item in roles)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, item));
            }
            
            return identity;
        }

        [HttpPost]
        [Route("logout")]
        [Authorize]
        public ActionResult Logout()
        {
            return NoContent();
        }




    }
}
