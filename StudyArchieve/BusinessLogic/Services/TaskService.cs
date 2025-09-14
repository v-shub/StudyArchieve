using BusinessLogic.Interfaces;
using DataAccess.Models;
using DataAccess.Wrapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exercise = DataAccess.Models.Task;
using Task = System.Threading.Tasks.Task;

namespace BusinessLogic.Services
{
    public class TaskService : ITaskService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public TaskService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public Task<List<Exercise>> GetAll()
        {
            return _repositoryWrapper.Exercise.FindAll().ToListAsync();
        }

        public Task<Exercise> GetById(int id)
        {
            var that = _repositoryWrapper.Exercise
                .FindByCondition(x => x.Id == id).First();
            return Task.FromResult(that);
        }

        public Task Create(Exercise model)
        {
            _repositoryWrapper.Exercise.Create(model);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

        public Task Update(Exercise model)
        {
            _repositoryWrapper.Exercise.Update(model);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

        public Task Delete(int id)
        {
            var that = _repositoryWrapper.Exercise
                .FindByCondition(x => x.Id == id).First();

            _repositoryWrapper.Exercise.Delete(that);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }
    }
}
