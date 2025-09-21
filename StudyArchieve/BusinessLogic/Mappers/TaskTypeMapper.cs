using Domain.DTOs;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Mappers
{
    public static class TaskTypeMapper
    {
        public static TaskTypeDto ToDto(TaskType type)
        {
            return new TaskTypeDto
            {
                Id = type.Id,
                Name = type.Name
            };
        }
        public static List<TaskTypeDto> ToDtoList(List<TaskType> list)
        {
            return list.Select(ToDto).ToList();
        }
    }
}
