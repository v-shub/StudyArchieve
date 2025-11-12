using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace DataAccess.Repositories
{
    public class TagRepository : RepositoryBase<Tag>, ITagRepository
    {
        public TagRepository(StudyArchieveContext repositoryContext) : base(repositoryContext) { }
        public async Task<List<Tag>> GetByIdsAsync(IEnumerable<int> ids)
        {
            return await RepositoryContext.Tags
                .Where(a => ids.Contains(a.Id))
                .AsNoTracking() // Важно: без отслеживания!
                .ToListAsync();
        }
        public async Task AttachAsync(Tag tag)
        {
            RepositoryContext.Tags.Attach(tag);
        }
    }
}
