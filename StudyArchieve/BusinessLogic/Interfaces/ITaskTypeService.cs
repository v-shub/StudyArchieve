using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace BusinessLogic.Interfaces
{
    public interface ITaskTypeService
    {
        Task<List<TaskType>> GetAll();
        Task<TaskType> GetById(int id);
        Task Create(TaskType model);
        Task Update(TaskType model);
        Task Delete(int id);
    }
}
