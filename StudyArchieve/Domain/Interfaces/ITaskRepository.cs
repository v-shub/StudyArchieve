using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exercise = Domain.Models.Task;


namespace Domain.Interfaces
{
    public interface ITaskRepository : IRepositoryBase<Exercise>
    {
        Task<List<Exercise>> GetTasksWithDetails();
        Task<Exercise> GetOneTaskWithAllConnected(int id);
    }
}
