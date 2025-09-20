using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace Domain.Interfaces
{
    public interface ISolutionFileService
    {
        Task<List<SolutionFile>> GetAll();
        Task<SolutionFile> GetById(int id);
        Task Create(SolutionFile model);
        Task Update(SolutionFile model);
        Task Delete(int id);
    }
}
