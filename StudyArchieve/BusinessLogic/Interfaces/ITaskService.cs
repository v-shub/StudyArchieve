using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exercise = DataAccess.Models.Task;
using Task = System.Threading.Tasks.Task;

namespace BusinessLogic.Interfaces
{
    public interface ITaskService
    {
        Task<List<Exercise>> GetAll();
        Task<Exercise> GetById(int id);
        Task Create(Exercise model);
        Task Update(Exercise model);
        Task Delete(int id);
    }
}
