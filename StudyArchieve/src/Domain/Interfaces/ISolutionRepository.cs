using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ISolutionRepository : IRepositoryBase<Solution>
    {
        Task<List<Solution>> GetSolutionsByTaskId(int taskId);
        Task<Solution> GetById(int id);
    }
}
