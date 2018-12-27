using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication3.Models;

namespace WebApplication3.Facades
{
    public class UserProvider
    {
        private static UserProvider _instance;
        private UserProvider() { }

        private List<UserDTO> _dtos = new List<UserDTO>() {
            new UserDTO()
            {
                Id = "1",
                Active = true,
                Role = "ADMIN",
                Mail = "ciccillo",
                Password = "password",
                Name = "Franceschino Beati"

            }
        };

        public List<UserDTO> Users
        {
            get
            {
                return _dtos;
            }
        }

        public static UserProvider Instance
        {
            get{
                if (_instance == null)
                    _instance = new UserProvider();
                return _instance;
            }
        }



        public UserDTO Add(UserDTO dto)
        {
            int next = new Random().Next(100, 10000000);
            dto.Id = next.ToString();

            _dtos.Add(dto);

            return dto;
        }

        public void Remove(UserDTO dto)
        {
            UserDTO found = _dtos.Find(a => a.Id == dto.Id);
            if (found != null)
                _dtos.Remove(found);
           
        }

        public UserDTO GetById(string id)
        {
            return _dtos.Find(a => a.Id == id);
        }

        public UserDTO Update (UserDTO toUpdate, string id)
        {
            UserDTO dto = GetById(id);
            if (dto == null)
                return null;

            dto.Active = toUpdate.Active;
            dto.Name = toUpdate.Name;
            dto.Mail = toUpdate.Mail;
            dto.Role = toUpdate.Role;
            dto.Password = toUpdate.Password;

            return dto;

        }

    }
}
