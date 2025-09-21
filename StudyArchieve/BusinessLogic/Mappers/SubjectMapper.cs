using Domain.DTOs;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Mappers
{
    public static class SubjectMapper
    {
        public static SubjectDto ToDto(Subject subject)
        {
            return new SubjectDto
            {
                Id = subject.Id,
                Name = subject.Name
            };
        }
        public static List<SubjectDto> ToDtoList(List<Subject> list)
        {
            return list.Select(ToDto).ToList();
        }
    }
}
