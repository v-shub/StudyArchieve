using Domain.DTOs;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Mappers
{
    public static class AcademicYearMapper
    {
        public static AcademicYearDto ToDto(AcademicYear academicYear)
        {
            return new AcademicYearDto
            {
                Id = academicYear.Id,
                YearLabel = academicYear.YearLabel
            };
        }
        public static List<AcademicYearDto> ToDtoList(List<AcademicYear> list)
        {
            return list.Select(ToDto).ToList();
        }
    }
}
