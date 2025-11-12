using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ISolutionFileRepository : IRepositoryBase<SolutionFile>
    {
        Task<List<SolutionFile>> GetBySolutionIdAsync(int solutionId);
        Task<SolutionFile?> GetByIdAsync(int id);
    }
}
