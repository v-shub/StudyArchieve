using Domain.DTOs;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exercise = Domain.Models.Task;
using Task = System.Threading.Tasks.Task;

namespace Domain.Interfaces
{
    public interface ITaskService
    {
        Task<List<TaskDto>> GetAll();
        Task<List<TaskDto>> GetByFilter(int? sublectId, int? academicYearId, int? typeId, int[]? authorIds, int[]? tagIds);
        Task<FullTaskDto> GetById(int id);
        Task Create(Exercise model);
        Task Update(Exercise model);
        Task Delete(int id);
    }
}
