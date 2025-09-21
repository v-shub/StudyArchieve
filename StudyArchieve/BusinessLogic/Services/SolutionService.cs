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
    public class SolutionService : ISolutionService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public SolutionService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }
        /*
        public async Task<List<Solution>> GetAll()
        {
            return await _repositoryWrapper.Solution.FindAll();
        }

        public async Task<Solution> GetById(int id)
        {
            var that = await _repositoryWrapper.Solution
                .FindByCondition(x => x.Id == id);
            return that.First();
        }

        public async Task Create(Solution model)
        {
            await _repositoryWrapper.Solution.Create(model);
            await _repositoryWrapper.Save();
        }

        public async Task Update(Solution model)
        {
            await _repositoryWrapper.Solution.Update(model);
            await _repositoryWrapper.Save();
        }

        public async Task Delete(int id)
        {
            var that = await _repositoryWrapper.Solution
                .FindByCondition(x => x.Id == id);

            await _repositoryWrapper.Solution.Delete(that.First());
            await _repositoryWrapper.Save();
        }*/
    }
}
