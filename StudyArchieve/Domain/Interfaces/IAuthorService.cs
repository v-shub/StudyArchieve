using Domain.DTOs;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace Domain.Interfaces
{
    public interface IAuthorService
    {
        Task<List<AuthorDto>> GetAll();
        //Task<Author> GetById(int id);
        Task Create(Author model);
        Task Update(Author model);
        Task Delete(int id);
    }
}
