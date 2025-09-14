using DataAccess.Interfaces;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exercise = DataAccess.Models.Task;

namespace DataAccess.Repositories
{
    public class TaskRepository : RepositoryBase<Exercise>, ITaskRepository
    {
        public TaskRepository(StudyArchieveContext repositoryContext) : base(repositoryContext) { }
    }
}
