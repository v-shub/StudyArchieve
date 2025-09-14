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
        private IAuthorRepository _author;
        private ITagRepository _tag;
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
        public IAuthorRepository Author
        {
            get
            {
                if (_author == null)
                {
                    _author = new AuthorRepository(_repoContext);
                }
                return _author;
            }
        }
        public ITagRepository Tag
        {
            get
            {
                if (_tag == null)
                {
                    _tag = new TagRepository(_repoContext);
                }
                return _tag;
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
