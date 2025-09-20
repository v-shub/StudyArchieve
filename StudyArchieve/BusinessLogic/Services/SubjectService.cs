using Domain.Interfaces;
using Domain.Models;
using Domain.Wrapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace BusinessLogic.Services
{
    public class SubjectService : ISubjectService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public SubjectService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public Task<List<Subject>> GetAll()
        {
            return _repositoryWrapper.Subject.FindAll().ToListAsync();
        }

        public Task<Subject> GetById(int id)
        {
            var that = _repositoryWrapper.Subject
                .FindByCondition(x => x.Id == id).First();
            return Task.FromResult(that);
        }

        public Task Create(Subject model)
        {
            _repositoryWrapper.Subject.Create(model);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

        public Task Update(Subject model)
        {
            _repositoryWrapper.Subject.Update(model);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

        public Task Delete(int id)
        {
            var that = _repositoryWrapper.Subject
                .FindByCondition(x => x.Id == id).First();

            _repositoryWrapper.Subject.Delete(that);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }
    }
}
