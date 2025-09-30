using BusinessLogic.Mappers;
using Domain.DTOs;
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
    public class TaskTypeService : ITaskTypeService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public TaskTypeService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<List<TaskTypeDto>> GetAll()
        {
            var that = await _repositoryWrapper.TaskType.FindAll();
            return TaskTypeMapper.ToDtoList(that);
        }
        /*
        public async Task<TaskType> GetById(int id)
        {
            var that = await _repositoryWrapper.TaskType
                .FindByCondition(x => x.Id == id);
            return that.First();
        }
        */
        public async Task Create(TaskType model)
        {
            await _repositoryWrapper.TaskType.Create(model);
            await _repositoryWrapper.Save();
        }

        public async Task Update(TaskType model)
        {
            await _repositoryWrapper.TaskType.Update(model);
            await _repositoryWrapper.Save();
        }

        public async Task Delete(int id)
        {
            var that = await _repositoryWrapper.TaskType
                .FindByCondition(x => x.Id == id);

            await _repositoryWrapper.TaskType.Delete(that.First());
            await _repositoryWrapper.Save();
        }
    }
}
