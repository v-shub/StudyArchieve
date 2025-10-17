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
    public class SolutionFileRepository : RepositoryBase<SolutionFile>, ISolutionFileRepository
    {
        public SolutionFileRepository(StudyArchieveContext context) : base(context) { }

        public async Task<List<SolutionFile>> GetBySolutionIdAsync(int solutionId)
        {
            return await FindByCondition(tf => tf.SolutionId == solutionId);
        }

        public async Task<SolutionFile?> GetByIdAsync(int id)
        {
            return await RepositoryContext.Set<SolutionFile>()
                .FirstOrDefaultAsync(tf => tf.Id == id);
        }
    }
}
