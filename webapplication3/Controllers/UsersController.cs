using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication3.Db;
using WebApplication3.Domain.Security;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {


        readonly ApplicationContext dbContext;
        readonly UserManager<User> userManager;
        readonly IMapper mapper;


        public UsersController(ApplicationContext dbContext, UserManager<User> userManager, IMapper mapper)
        {
            this.mapper = mapper;

            this.userManager = userManager;

            this.dbContext = dbContext;
        }



        // GET: api/Users
        private async Task<string> FindRoleStringAsync(User e)
        {
            IList<string> roles = await userManager.GetRolesAsync(e);
            return String.Join(",", roles);
        }


        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public IEnumerable<UserDTO> Get()
        {
            List<User> list = dbContext.Users.ToList();
            //return mapper.Map<List<User>, List<UserDTO>>(list);


            return mapper.Map(list,typeof(List<User>), typeof(List<UserDTO>)) as List<UserDTO>;

            //var data = dbContext.Users.ToList().Select(async e =>
            //{
            //    UserDTO dto = new UserDTO();
            //    dto.Active = e.IsActive;
            //    dto.Id = e.Id;
            //    dto.Mail = e.Email;
            //    dto.Name = e.Name;
            //    dto.Role = await FindRoleStringAsync(e);
            //    return dto;
            //});

            //return await Task.WhenAll(data);

        }


        [HttpGet("{id}", Name = "Get")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<UserDTO>> Get(string id)
        {
            User item = await dbContext.Users.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            return mapper.Map<User, UserDTO>(item);

            //return new UserDTO()
            //{

            //    Active = item.IsActive,
            //    Id = item.Id,
            //    Mail = item.Email,
            //    Name = item.Name,
            //    Role = await FindRoleStringAsync(item)
            //};

        }

        // POST: api/Users
        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Post([FromBody] UserDTO value)
        {
            string role = value.Role;

            User current = new User()
            {
                IsActive = value.Active,
                Email = value.Mail,
                Name = value.Name,
                UserName = value.Mail

            };

            string check = await PasswordCheck(current, value.Password);
            if (!String.IsNullOrEmpty(check))
                return BadRequest("Invalid password: " + check);

            IdentityResult res = await userManager.CreateAsync(current);
            if (res.Succeeded)
            {
                await userManager.AddPasswordAsync(current, value.Password);
                IdentityResult roleResult = await userManager.AddToRoleAsync(current, role);
                if (roleResult.Succeeded)
                {
                    return Ok(new UserDTO()
                    {
                        Active = current.IsActive,
                        Id = current.Id,
                        Mail = current.Email,
                        Name = current.Name,
                        Role = value.Role
                    });
                }
                return BadRequest("Invalid role");
            }

            return BadRequest(String.Join(",", res.Errors.Select(a => a.Description).ToList()));

        }


        async Task<string> PasswordCheck(User user, string password)
        {

            List<string> errors = new List<string>();
            foreach (var item in userManager.PasswordValidators)
            {
                IdentityResult res = await item.ValidateAsync(userManager, user, password);
                if (!res.Succeeded)
                {
                    errors.Add(String.Join(",", res.Errors.Select(a => a.Description)));
                }

            }
            return await Task.FromResult<string>(String.Join(",", errors));
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<UserDTO>> Put([FromRoute]string id, [FromBody] UserDTO value)
        {
            if (id != value.Id)
                return BadRequest();

            User current = await userManager.FindByIdAsync(id);
            if (current == null)
                return NotFound();


            current.IsActive = value.Active;
            current.Name = value.Name;
            current.Email = value.Mail;
            current.UserName = value.Mail;

            IdentityResult result = await userManager.UpdateAsync(current);
            if (result.Succeeded)
            {
                if (!String.IsNullOrEmpty(value.Password))
                {
                    string check = await PasswordCheck(current, value.Password);
                    if (string.IsNullOrEmpty(check))
                    {
                        //var newPasswordHash = userManager.PasswordHasher.HashPassword(current ,value.Password);
                        //await store.SetPasswordHashAsync(current, newPasswordHash);
                        await userManager.RemovePasswordAsync(current);
                        await userManager.AddPasswordAsync(current, value.Password);
                    }
                    else
                    {
                        return BadRequest(check);

                    }
                }

                return Ok(new UserDTO()
                {

                    Active = current.IsActive,
                    Id = current.Id,
                    Mail = current.Email,
                    Name = current.Name,

                });
            }
            return BadRequest(String.Join(",", result.Errors.Select(a => a.Description).ToList()));


        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Delete([FromRoute]string id)
        {
            User current = await userManager.FindByIdAsync(id);
            if (current == null)
                return NotFound();
            await userManager.DeleteAsync(current);
            return NoContent();
        }
    }
}
