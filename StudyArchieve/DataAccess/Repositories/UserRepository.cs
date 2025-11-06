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
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(StudyArchieveContext repositoryContext) : base(repositoryContext) { }
        public async Task<User> GetById(int id)
        {
            return await RepositoryContext.Users
                .Include(t => t.Role)
                .Where(t => t.Id == id)
                .AsNoTracking()
                .FirstAsync();
        }
    }
}
