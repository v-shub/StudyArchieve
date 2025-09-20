using Domain.DTOs.Task;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exercise = Domain.Models.Task;

namespace BusinessLogic.Services
{

    public static class TaskMapper
    {
        public static TaskDto ToDto(Exercise task)
        {
            return new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                ConditionText = task.ConditionText,
                DateAdded = task.DateAdded,
                SubjectId = task.SubjectId,
                SubjectName = task.Subject?.Name ?? string.Empty,
                AcademicYearId = task.AcademicYearId,
                AcademicYearLabel = task.AcademicYear?.YearLabel,
                TypeId = task.TypeId,
                TypeName = task.Type?.Name ?? string.Empty,
                Authors = task.Authors.Select(ToAuthorDto).ToList(),
                Tags = task.Tags.Select(ToTagDto).ToList()
            };
        }

        public static FullTaskDto ToFullDto(Exercise task)
        {
            return new FullTaskDto
            {
                Id = task.Id,
                Title = task.Title,
                ConditionText = task.ConditionText,
                DateAdded = task.DateAdded,
                SubjectId = task.SubjectId,
                SubjectName = task.Subject?.Name ?? string.Empty,
                AcademicYearId = task.AcademicYearId,
                AcademicYearLabel = task.AcademicYear?.YearLabel,
                TypeId = task.TypeId,
                TypeName = task.Type?.Name ?? string.Empty,
                Authors = task.Authors.Select(ToAuthorDto).ToList(),
                Tags = task.Tags.Select(ToTagDto).ToList(),
                Solutions = task.Solutions?.Select(ToSolutionDto).ToList() ?? new(),
                TaskFiles = task.TaskFiles?.Select(ToTaskFileDto).ToList() ?? new()
            };
        }

        public static AuthorDto ToAuthorDto(Author author)
        {
            return new AuthorDto
            {
                Id = author.Id,
                Name = author.Name
            };
        }

        public static TagDto ToTagDto(Tag tag)
        {
            return new TagDto
            {
                Id = tag.Id,
                Name = tag.Name
            };
        }

        public static SolutionDto ToSolutionDto(Solution solution)
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

        public static TaskFileDto ToTaskFileDto(TaskFile taskFile)
        {
            return new TaskFileDto
            {
                Id = taskFile.Id,
                FileName = taskFile.FileName,
                FilePath = taskFile.FilePath
            };
        }

        public static List<TaskDto> ToDtoList(List<Exercise> tasks)
        {
            return tasks.Select(ToDto).ToList();
        }
    }
}
