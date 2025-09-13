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
        public ISubjectRepository Subject {
            get
            {
                if (_subject == null)
                {
                    _subject = new SubjectRepository(_repoContext);
                }
                return _subject;
            }
        }
        public RepositoryWrapper(StudyArchieveContext reposytoryContext)
        {
            _repoContext = reposytoryContext;
        }
        public void Save()
        {
            _repoContext.SaveChanges();
        }
    }
}
