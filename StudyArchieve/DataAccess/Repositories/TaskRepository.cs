using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exercise = Domain.Models.Task;
using Task = System.Threading.Tasks.Task;

namespace DataAccess.Repositories
{
    public class TaskRepository : RepositoryBase<Exercise>, ITaskRepository
    {
        public TaskRepository(StudyArchieveContext repositoryContext) : base(repositoryContext) { }
        public async Task<List<Exercise>> GetTasksWithDetails()
        {
            return await RepositoryContext.Tasks
                .Include(t => t.Authors)
                .Include(t => t.Tags)
                .Include(t => t.AcademicYear)
                .Include(t => t.Subject)
                .Include(t => t.Type)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<Exercise> GetOneTaskWithAllConnected(int id)
        {
            return await RepositoryContext.Tasks
                .Include(t => t.Authors)
                .Include(t => t.Tags)
                .Include(t => t.Solutions)
                    .ThenInclude(s => s.SolutionFiles)
                .Include(t => t.TaskFiles)
                .Include(t => t.AcademicYear)
                .Include(t => t.Subject)
                .Include(t => t.Type)
                .Where(t => t.Id == id)
                .AsNoTracking()
                .FirstAsync();
        }
    }
}
