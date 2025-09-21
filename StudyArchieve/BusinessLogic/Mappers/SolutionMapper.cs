using Domain.DTOs;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Mappers
{
    public static class SolutionMapper
    {
        public static SolutionDto ToDto(Solution solution)
        {
            return new SolutionDto
            {
                Id = solution.Id,
                SolutionText = solution.SolutionText,
                DateAdded = solution.DateAdded,
                IsOriginal = solution.IsOriginal,
                SolutionFiles = solution.SolutionFiles?.Select(ToSolutionFileDto).ToList() ?? new()
            };
        }
        public static SolutionFileDto ToSolutionFileDto(SolutionFile solutionFile)
        {
            return new SolutionFileDto
            {
                Id = solutionFile.Id,
                FileName = solutionFile.FileName,
                FilePath = solutionFile.FilePath
            };
        }
    }
}
