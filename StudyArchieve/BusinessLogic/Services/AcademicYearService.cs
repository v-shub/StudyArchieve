using BusinessLogic.Interfaces;
using DataAccess.Models;
using DataAccess.Wrapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace BusinessLogic.Services
{
    public class AcademicYearService : IAcademicYearService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public AcademicYearService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public Task<List<AcademicYear>> GetAll()
        {
            return _repositoryWrapper.AcademicYear.FindAll().ToListAsync();
        }

        public Task<AcademicYear> GetById(int id)
        {
            var that = _repositoryWrapper.AcademicYear
                .FindByCondition(x => x.Id == id).First();
            return Task.FromResult(that);
        }

        public Task Create(AcademicYear model)
        {
            _repositoryWrapper.AcademicYear.Create(model);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

        public Task Update(AcademicYear model)
        {
            _repositoryWrapper.AcademicYear.Update(model);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

        public Task Delete(int id)
        {
            var that = _repositoryWrapper.AcademicYear
                .FindByCondition(x => x.Id == id).First();

            _repositoryWrapper.AcademicYear.Delete(that);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }
    }
}
