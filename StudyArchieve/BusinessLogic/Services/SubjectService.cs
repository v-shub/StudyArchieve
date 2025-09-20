using Domain.Interfaces;
using Domain.Models;
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

        public async Task<List<Subject>> GetAll()
        {
            return await _repositoryWrapper.Subject.FindAll();
        }

        public async Task<Subject> GetById(int id)
        {
            var that = await _repositoryWrapper.Subject
                .FindByCondition(x => x.Id == id);
            return that.First();
        }

        public async Task Create(Subject model)
        {
            await _repositoryWrapper.Subject.Create(model);
            await _repositoryWrapper.Save();
        }

        public async Task Update(Subject model)
        {
            await _repositoryWrapper.Subject.Update(model);
            await _repositoryWrapper.Save();
        }

        public async Task Delete(int id)
        {
            var that = await _repositoryWrapper.Subject
                .FindByCondition(x => x.Id == id);

            await _repositoryWrapper.Subject.Delete(that.First());
            await _repositoryWrapper.Save();
        }
    }
}
