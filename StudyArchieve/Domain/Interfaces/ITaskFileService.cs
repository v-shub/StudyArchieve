using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace Domain.Interfaces
{
    public interface ITaskFileService
    {
        Task<List<TaskFile>> GetAll();
        Task<TaskFile> GetById(int id);
        Task Create(TaskFile model);
        Task Update(TaskFile model);
        Task Delete(int id);
    }
}
