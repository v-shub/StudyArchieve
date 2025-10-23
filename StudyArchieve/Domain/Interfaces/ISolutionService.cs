using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace Domain.Interfaces
{
    public interface ISolutionService
    {
        Task<List<Solution>> GetByTaskId(int taskId);
        Task Create(Solution model);
        Task Update(Solution model);
        Task Delete(int id);
    }
}
