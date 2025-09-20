using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace Domain.Interfaces
{
    public interface IAcademicYearService
    {
        Task<List<AcademicYear>> GetAll();
        Task<AcademicYear> GetById(int id);
        Task Create(AcademicYear model);
        Task Update(AcademicYear model);
        Task Delete(int id);
    }
}
