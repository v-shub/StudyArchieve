using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class SolutionRepository : RepositoryBase<Solution>, ISolutionRepository
    {
        public SolutionRepository(StudyArchieveContext repositoryContext) : base(repositoryContext) { }
        public async Task<List<Solution>> GetSolutionsByTaskId(int taskId)
        {
            return await RepositoryContext.Solutions
                .Include(t => t.UserAdded)
                .ThenInclude(u => u.Role)
                .Where(t => t.TaskId == taskId)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
