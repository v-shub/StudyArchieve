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
    public class TaskFileRepository : RepositoryBase<TaskFile>, ITaskFileRepository
{
    public TaskFileRepository(StudyArchieveContext context) : base(context) { }

    public async Task<List<TaskFile>> GetByTaskIdAsync(int taskId)
    {
        return await FindByCondition(tf => tf.TaskId == taskId);
    }

    public async Task<TaskFile?> GetByIdAsync(int id)
    {
        return await RepositoryContext.Set<TaskFile>()
            .FirstOrDefaultAsync(tf => tf.Id == id);
    }
}
}
