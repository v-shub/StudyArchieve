using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ITaskFileRepository : IRepositoryBase<TaskFile>
    {
        Task<List<TaskFile>> GetByTaskIdAsync(int taskId);
        Task<TaskFile?> GetByIdAsync(int id);
    }
}
