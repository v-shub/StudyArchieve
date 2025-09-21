using Domain.DTOs;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Mappers
{
    public static class AuthorMapper
    {
        public static AuthorDto ToDto(Author author)
        {
            return new AuthorDto
            {
                Id = author.Id,
                Name = author.Name
            };
        }
        public static List<AuthorDto> ToDtoList(List<Author> list)
        {
            return list.Select(ToDto).ToList();
        }
    }
}
