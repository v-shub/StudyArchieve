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
    public class SolutionService : ISolutionService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public SolutionService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public Task<List<Solution>> GetAll()
        {
            return _repositoryWrapper.Solution.FindAll().ToListAsync();
        }

        public Task<Solution> GetById(int id)
        {
            var that = _repositoryWrapper.Solution
                .FindByCondition(x => x.Id == id).First();
            return Task.FromResult(that);
        }

        public Task Create(Solution model)
        {
            _repositoryWrapper.Solution.Create(model);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

        public Task Update(Solution model)
        {
            _repositoryWrapper.Solution.Update(model);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

        public Task Delete(int id)
        {
            var that = _repositoryWrapper.Solution
                .FindByCondition(x => x.Id == id).First();

            _repositoryWrapper.Solution.Delete(that);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }
    }
}
