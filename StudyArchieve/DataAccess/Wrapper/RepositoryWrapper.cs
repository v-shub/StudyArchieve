using DataAccess.Interfaces;
using DataAccess.Models;
using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Wrapper
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private StudyArchieveContext _repoContext;
        private ISubjectRepository _subject;
        private IAcademicYearRepository _academicYear;
        private ITaskTypeRepository _taskType;
        public ISubjectRepository Subject
        {
            get
            {
                if (_subject == null)
                {
                    _subject = new SubjectRepository(_repoContext);
                }
                return _subject;
            }
        }
        public IAcademicYearRepository AcademicYear
        {
            get
            {
                if (_academicYear == null)
                {
                    _academicYear = new AcademicYearRepository(_repoContext);
                }
                return _academicYear;
            }
        }
        public ITaskTypeRepository TaskType
        {
            get
            {
                if (_taskType == null)
                {
                    _taskType = new TaskTypeRepository(_repoContext);
                }
                return _taskType;
            }
        }
        public RepositoryWrapper(StudyArchieveContext repositoryContext)
        {
            _repoContext = repositoryContext;
        }
        public void Save()
        {
            _repoContext.SaveChanges();
        }
    }
}
