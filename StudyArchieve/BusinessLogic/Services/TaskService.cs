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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Exercise = Domain.Models.Task;
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
        public async Task<List<TaskDto>> GetByFilter(int? subjectId = null, int? academicYearId = null, int? typeId = null, int[]? authorIds = null, int[]? tagIds = null)
        {
            var allTasks = await _repositoryWrapper.Exercise.GetTasksWithDetails();
            var query = allTasks.AsQueryable();

            // Фильтрация по subjectId (если передан)
            if (subjectId.HasValue)
            {
                query = query.Where(x => x.SubjectId == subjectId.Value);
            }

            // Фильтрация по academicYearId (если передан)
            if (academicYearId.HasValue)
            {
                query = query.Where(x => x.AcademicYearId == academicYearId.Value);
            }

            // Фильтрация по typeId (если передан)
            if (typeId.HasValue)
            {
                query = query.Where(x => x.TypeId == typeId.Value);
            }

            // Фильтр по авторам: должны присутствовать ВСЕ указанные authorIds (если передан массив)
            if (authorIds != null && authorIds.Length > 0)
            {
                query = query.Where(x => authorIds.All(id => x.Authors.Any(a => a.Id == id)));
            }

            // Фильтр по тегам: должны присутствовать ВСЕ указанные tagIds (если передан массив)
            if (tagIds != null && tagIds.Length > 0)
            {
                query = query.Where(x => tagIds.All(id => x.Tags.Any(t => t.Id == id)));
            }

            var tasks = query.ToList();
            return TaskMapper.ToDtoList(tasks);
        }

        public async Task<FullTaskDto> GetById(int id)
        {
            if (id <= 0)
                throw new ArgumentException("id");
            var that = await _repositoryWrapper.Exercise
                .GetOneTaskWithAllConnected(id);
            return that == null ? null : TaskMapper.ToFullDto(that);
        }/*
          
        
        public async Task<List<TaskDto>> GetAll()
        {
            var tasks = await _repositoryWrapper.Exercise
                .GetTasksWithDetails();
            return TaskMapper.ToDtoList(tasks);
        }
        */
        public async Task Create(Exercise model)
        {
            await _repositoryWrapper.Exercise.Create(model);
            await _repositoryWrapper.Save();
        }

        public async Task Update(Exercise model)
        {
            await _repositoryWrapper.Exercise.Update(model);
            await _repositoryWrapper.Save();
        }

        public async Task Delete(int id)
        {
            var that = await _repositoryWrapper.Exercise
                .FindByCondition(x => x.Id == id);

            await _repositoryWrapper.Exercise.Delete(that.First());
            await _repositoryWrapper.Save();
        }
    }
}
