using Domain.DTOs;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Mappers
{
    public static class RoleMapper
    {
        public static RoleDto ToDto(Role role)
        {
            return new RoleDto
            {
                Id = role.Id,
                RoleName = role.RoleName,
            };
        }
        public static List<RoleDto> ToDtoList(List<Role> list)
        {
            return list.Select(ToDto).ToList();
        }
    }
}
