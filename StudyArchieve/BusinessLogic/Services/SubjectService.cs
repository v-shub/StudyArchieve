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
            _repositoryWrapper = repositoryWrapper ?? throw new ArgumentNullException(nameof(repositoryWrapper));
        }

        public async Task<List<Subject>> GetAll()
        {
            var that = await _repositoryWrapper.Subject.FindAll();
            return that;
        }
        /*
        public async Task<Subject> GetById(int id)
        {
            var that = await _repositoryWrapper.Subject
                .FindByCondition(x => x.Id == id);
            return that.First();
        }
        */
        public async Task Create(Subject model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            await _repositoryWrapper.Subject.Create(model);
            await _repositoryWrapper.Save();
        }

        public async Task Update(Subject model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            await _repositoryWrapper.Subject.Update(model);
            await _repositoryWrapper.Save();
        }

        public async Task Delete(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Id must be greater than zero", nameof(id));

            var that = await _repositoryWrapper.Subject
                .FindByCondition(x => x.Id == id);

            if (that == null || !that.Any())
                throw new InvalidOperationException($"Subject with id {id} not found");

            await _repositoryWrapper.Subject.Delete(that.First());
            await _repositoryWrapper.Save();
        }
    }
}