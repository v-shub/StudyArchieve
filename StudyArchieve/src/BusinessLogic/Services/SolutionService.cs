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
            _repositoryWrapper = repositoryWrapper ?? throw new ArgumentNullException(nameof(repositoryWrapper));
        }

        public async Task<List<Solution>> GetByTaskId(int taskId)
        {
            if (taskId <= 0)
                throw new ArgumentException("Task ID must be greater than zero", nameof(taskId));

            return await _repositoryWrapper.Solution.GetSolutionsByTaskId(taskId);
        }

        public async Task<Solution> GetById(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID must be greater than zero", nameof(id));

            return await _repositoryWrapper.Solution.GetById(id);
        }

        public async Task Create(Solution model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            await _repositoryWrapper.Solution.Create(model);
            await _repositoryWrapper.Save();
        }

        public async Task Update(Solution model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            await _repositoryWrapper.Solution.Update(model);
            await _repositoryWrapper.Save();
        }

        public async Task Delete(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID must be greater than zero", nameof(id));

            var that = await _repositoryWrapper.Solution
                .FindByCondition(x => x.Id == id);

            if (that == null || !that.Any())
                throw new InvalidOperationException($"Solution with id {id} not found");

            await _repositoryWrapper.Solution.Delete(that.First());
            await _repositoryWrapper.Save();
        }
    }
}