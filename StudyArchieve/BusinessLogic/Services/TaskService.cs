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
        public async Task<List<Exercise>> GetByFilter(int? subjectId = null, int? academicYearId = null, int? typeId = null, int[]? authorIds = null, int[]? tagIds = null)
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
            return tasks;
        }

        public async Task<Exercise> GetById(int id)
        {
            if (id <= 0)
                throw new ArgumentException("id");
            var that = await _repositoryWrapper.Exercise
                .GetOneTaskWithDetails(id);
            return that ?? null;
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
            // Обрабатываем авторов
            if (model.Authors != null && model.Authors.Any())
            {
                var authorIds = model.Authors.Select(a => a.Id).ToList();
                var existingAuthors = await _repositoryWrapper.Author.GetByIdsAsync(authorIds);

                // Очищаем и добавляем прикрепленных авторов
                model.Authors.Clear();
                foreach (var author in existingAuthors)
                {
                    await _repositoryWrapper.Author.AttachAsync(author);
                    model.Authors.Add(author);
                }
            }

            // Обрабатываем теги
            if (model.Tags != null && model.Tags.Any())
            {
                var tagIds = model.Tags.Select(t => t.Id).ToList();
                var existingTags = await _repositoryWrapper.Tag.GetByIdsAsync(tagIds);

                model.Tags.Clear();
                foreach (var tag in existingTags)
                {
                    await _repositoryWrapper.Tag.AttachAsync(tag);
                    model.Tags.Add(tag);
                }
            }

            await _repositoryWrapper.Exercise.Create(model);
            await _repositoryWrapper.Save();
        }

        public async Task Update(Exercise model)
        {
            // Находим существующую задачу
            var existingTasks = await _repositoryWrapper.Exercise.FindByCondition(t => t.Id == model.Id);
            var existingTask = existingTasks.FirstOrDefault();

            if (existingTask == null)
                throw new ArgumentException("Task not found");

            // Обновляем простые свойства
            existingTask.Title = model.Title;
            existingTask.ConditionText = model.ConditionText;
            existingTask.SubjectId = model.SubjectId;
            existingTask.AcademicYearId = model.AcademicYearId;
            existingTask.TypeId = model.TypeId;
            existingTask.UserAddedId = model.UserAddedId;

            // Обновляем авторов
            if (model.Authors != null)
            {
                var authorIds = model.Authors.Select(a => a.Id).ToList();
                var existingAuthors = await _repositoryWrapper.Author.GetByIdsAsync(authorIds);

                existingTask.Authors.Clear();
                foreach (var author in existingAuthors)
                {
                    await _repositoryWrapper.Author.AttachAsync(author);
                    existingTask.Authors.Add(author);
                }
            }

            // Обновляем теги
            if (model.Tags != null)
            {
                var tagIds = model.Tags.Select(t => t.Id).ToList();
                var existingTags = await _repositoryWrapper.Tag.GetByIdsAsync(tagIds);

                existingTask.Tags.Clear();
                foreach (var tag in existingTags)
                {
                    await _repositoryWrapper.Tag.AttachAsync(tag);
                    existingTask.Tags.Add(tag);
                }
            }

            await _repositoryWrapper.Exercise.Update(existingTask);
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
