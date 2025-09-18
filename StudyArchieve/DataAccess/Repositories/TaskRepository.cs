using DataAccess.Interfaces;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exercise = DataAccess.Models.Task;
using Task = System.Threading.Tasks.Task;

namespace DataAccess.Repositories
{
    public class TaskRepository : RepositoryBase<Exercise>, ITaskRepository
    {
        public TaskRepository(StudyArchieveContext repositoryContext) : base(repositoryContext) { }
        public IQueryable<Exercise> GetTasksWithDetails()
        {
            return RepositoryContext.Tasks
                .Include(t => t.Authors)
                .Include(t => t.Tags)
                .Include(t => t.AcademicYear)
                .Include(t => t.Subject)
                .Include(t => t.Type)
                .AsNoTracking();
        }
        public IQueryable<Exercise> GetOneTaskWithAllConnected(int id)
        {
            return RepositoryContext.Tasks
                .Include(t => t.Authors)
                .Include(t => t.Tags)
                .Include(t => t.Solutions)
                    .ThenInclude(s => s.SolutionFiles)
                .Include(t => t.TaskFiles)
                .Include(t => t.AcademicYear)
                .Include(t => t.Subject)
                .Include(t => t.Type)
                .Where(t => t.Id == id)
                .AsNoTracking();
        }
    }
}
