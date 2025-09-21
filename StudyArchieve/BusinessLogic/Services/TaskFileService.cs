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
    public class TaskFileService : ITaskFileService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public TaskFileService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }
        /*
        public async Task<List<TaskFile>> GetAll()
        {
            return await _repositoryWrapper.TaskFile.FindAll();
        }

        public async Task<TaskFile> GetById(int id)
        {
            var that = await _repositoryWrapper.TaskFile
                .FindByCondition(x => x.Id == id);
            return that.First();
        }

        public async Task Create(TaskFile model)
        {
            await _repositoryWrapper.TaskFile.Create(model);
            await _repositoryWrapper.Save();
        }

        public async Task Update(TaskFile model)
        {
            await _repositoryWrapper.TaskFile.Update(model);
            await _repositoryWrapper.Save();
        }

        public async Task Delete(int id)
        {
            var that = await _repositoryWrapper.TaskFile
                .FindByCondition(x => x.Id == id);

            await _repositoryWrapper.TaskFile.Delete(that.First());
            await _repositoryWrapper.Save();
        }*/
    }
}
