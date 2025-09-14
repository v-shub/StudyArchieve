using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace BusinessLogic.Interfaces
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
