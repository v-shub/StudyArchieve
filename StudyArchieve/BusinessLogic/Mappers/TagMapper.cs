using Domain.DTOs;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Mappers
{
    public static class TagMapper
    {
        public static TagDto ToDto(Tag tag)
        {
            return new TagDto
            {
                Id = tag.Id,
                Name = tag.Name
            };
        }
        public static List<TagDto> ToDtoList(List<Tag> list)
        {
            return list.Select(ToDto).ToList();
        }
    }
}
