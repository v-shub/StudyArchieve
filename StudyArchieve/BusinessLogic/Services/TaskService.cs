using BusinessLogic.DTOs.Task;
using BusinessLogic.Interfaces;
using DataAccess.Models;
using DataAccess.Wrapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
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

        public Task<List<TaskDto>> GetAll()
        {
            var tasks = _repositoryWrapper.Exercise
                .GetTasksWithDetails().ToList();
            return Task.FromResult(TaskMapper.ToDtoList(tasks));
        }
        public Task<List<TaskDto>> GetByFilter(int? subjectId = null, int? academicYearId = null, int? typeId = null, int[]? authorIds = null, int[]? tagIds = null)
        {
            var query = _repositoryWrapper.Exercise.GetTasksWithDetails();

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
            return Task.FromResult(TaskMapper.ToDtoList(tasks));
        }

        public Task<FullTaskDto> GetById(int id)
        {
            var that = _repositoryWrapper.Exercise
                .GetOneTaskWithAllConnected(id).First();
            return Task.FromResult(TaskMapper.ToFullDto(that));
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
