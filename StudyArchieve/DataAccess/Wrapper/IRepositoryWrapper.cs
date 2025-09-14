using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Wrapper
{
    public interface IRepositoryWrapper
    {
        ISubjectRepository Subject { get; }
        IAcademicYearRepository AcademicYear { get; }
        void Save();
    }
}
