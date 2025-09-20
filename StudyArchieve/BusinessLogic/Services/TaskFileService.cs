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
    public class TaskFileService : ITaskFileService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public TaskFileService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public Task<List<TaskFile>> GetAll()
        {
            return _repositoryWrapper.TaskFile.FindAll().ToListAsync();
        }

        public Task<TaskFile> GetById(int id)
        {
            var that = _repositoryWrapper.TaskFile
                .FindByCondition(x => x.Id == id).First();
            return Task.FromResult(that);
        }

        public Task Create(TaskFile model)
        {
            _repositoryWrapper.TaskFile.Create(model);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

        public Task Update(TaskFile model)
        {
            _repositoryWrapper.TaskFile.Update(model);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

        public Task Delete(int id)
        {
            var that = _repositoryWrapper.TaskFile
                .FindByCondition(x => x.Id == id).First();

            _repositoryWrapper.TaskFile.Delete(that);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }
    }
}
