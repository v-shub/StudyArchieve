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
        public async Task<User> GetByIdWithToken(int id)
        {
            return await RepositoryContext.Users
                .Include(t => t.Role)
                .Include(t => t.RefreshTokens)
                .Where(t => t.Id == id)
                .AsNoTracking()
                .FirstAsync();
        }
        public async Task<User> GetByEmailWithToken(string email)
        {
            return await RepositoryContext.Users
                .Include(t => t.Role)
                .Include(t => t.RefreshTokens)
                .Where(t => t.Email == email)
                .AsNoTracking()
                .FirstAsync();
        }
    }
}
