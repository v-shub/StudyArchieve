using DataAccess.Models;
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
