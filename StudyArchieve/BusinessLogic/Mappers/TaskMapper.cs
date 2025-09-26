using Domain.DTOs;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exercise = Domain.Models.Task;

namespace BusinessLogic.Mappers
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
                SubjectName = task.Subject.Name,
                AcademicYearId = task.AcademicYearId,
                AcademicYearLabel = task.AcademicYear?.YearLabel,
                TypeId = task.TypeId,
                TypeName = task.Type.Name,
                UserAdded = task.UserAdded,

                Authors = task.Authors.Select(AuthorMapper.ToDto).ToList(),
                Tags = task.Tags.Select(TagMapper.ToDto).ToList()
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
                SubjectName = task.Subject.Name,
                AcademicYearId = task.AcademicYearId,
                AcademicYearLabel = task.AcademicYear?.YearLabel,
                TypeId = task.TypeId,
                TypeName = task.Type.Name,
                UserAdded = task.UserAdded,
                Authors = task.Authors.Select(AuthorMapper.ToDto).ToList(),
                Tags = task.Tags.Select(TagMapper.ToDto).ToList(),
                Solutions = task.Solutions.Select(SolutionMapper.ToDto).ToList(),
                TaskFiles = task.TaskFiles.Select(ToTaskFileDto).ToList()
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
        public static List<TaskDto> ToDtoList(List<Exercise> list)
        {
            return list.Select(ToDto).ToList();
        }
    }
}
