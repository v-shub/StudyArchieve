using Domain.DTOs;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Mappers
{
    public static class UserMapper
    {
        public static UserDto ToDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                RoleId = user.RoleId,
                RoleName = user.Role.RoleName,
            };
        }
        public static List<UserDto> ToDtoList(List<User> list)
        {
            return list.Select(ToDto).ToList();
        }
    }
}
