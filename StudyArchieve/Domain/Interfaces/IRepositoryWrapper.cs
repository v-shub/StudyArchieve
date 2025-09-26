using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IRepositoryWrapper
    {
        ISubjectRepository Subject { get; }
        IAcademicYearRepository AcademicYear { get; }
        ITaskTypeRepository TaskType { get; }
        IAuthorRepository Author { get; }
        ITagRepository Tag { get; }
        ITaskRepository Exercise { get; }
        ISolutionRepository Solution { get; }
        ITaskFileRepository TaskFile { get; }
        ISolutionFileRepository SolutionFile { get; }
        IRoleRepository Role { get; }
        IUserRepository User { get; }
        Task Save();
    }
}
