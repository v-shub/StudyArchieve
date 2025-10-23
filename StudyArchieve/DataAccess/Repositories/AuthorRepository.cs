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
    public class AuthorRepository : RepositoryBase<Author>, IAuthorRepository
    {
        public AuthorRepository(StudyArchieveContext repositoryContext) : base(repositoryContext) {}
        public async Task<List<Author>> GetByIdsAsync(IEnumerable<int> ids)
        {
            return await RepositoryContext.Authors
                .Where(a => ids.Contains(a.Id))
                .AsNoTracking() // Важно: без отслеживания!
                .ToListAsync();
        }
        public async Task AttachAsync(Author author)
        {
            RepositoryContext.Authors.Attach(author);
        }
    }
}
