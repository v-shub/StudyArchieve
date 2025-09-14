using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace BusinessLogic.Interfaces
{
    public interface IAuthorService
    {
        Task<List<Author>> GetAll();
        Task<Author> GetById(int id);
        Task Create(Author model);
        Task Update(Author model);
        Task Delete(int id);
    }
}
