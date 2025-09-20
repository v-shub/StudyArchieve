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
    public class TaskTypeService : ITaskTypeService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public TaskTypeService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public Task<List<TaskType>> GetAll()
        {
            return _repositoryWrapper.TaskType.FindAll().ToListAsync();
        }

        public Task<TaskType> GetById(int id)
        {
            var that = _repositoryWrapper.TaskType
                .FindByCondition(x => x.Id == id).First();
            return Task.FromResult(that);
        }

        public Task Create(TaskType model)
        {
            _repositoryWrapper.TaskType.Create(model);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

        public Task Update(TaskType model)
        {
            _repositoryWrapper.TaskType.Update(model);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

        public Task Delete(int id)
        {
            var that = _repositoryWrapper.TaskType
                .FindByCondition(x => x.Id == id).First();

            _repositoryWrapper.TaskType.Delete(that);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }
    }
}
