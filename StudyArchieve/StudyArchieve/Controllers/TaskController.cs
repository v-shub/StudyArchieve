using Domain.Interfaces;
using Domain.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudyArchieveApi.Contracts.Author;
using StudyArchieveApi.Contracts.Tag;
using StudyArchieveApi.Contracts.Task;
using System;
using System.Collections.Generic;
using Exercise = Domain.Models.Task;

namespace StudyArchieveApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private ITaskService _taskService;
        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }
        /*
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _taskService.GetAll());
        }*/

        /// <summary>
        /// Получение списка заданий по условию
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     GET /Todo
        ///     {
        ///        "subjectId" : 3,
        ///        "academicYearId" : 2,
        ///        "typeId" : 1,
        ///        "authorIds" : [1, 2],
        ///        "tagIds" : [2, 4, 5]
        ///     }
        ///
        /// </remarks>
        /// <param name="subjectId">Id учебной дисциплины</param>
        /// <param name="academicYearId">Id учебного года</param>
        /// <param name="typeId">Id типа задания</param>
        /// <param name="authorIds">Набор id авторов задания</param>
        /// <param name="tagIds">Набор id тэгов</param>
        /// <returns>Список заданий, удовлетворяющих условию поиска</returns>
        [HttpGet]
        public async Task<IActionResult> GetByFilter(int? subjectId, int? academicYearId, int? typeId, [FromQuery] int[]? authorIds, [FromQuery] int[]? tagIds)
        {
            var list = await _taskService.GetByFilter(subjectId, academicYearId, typeId, authorIds, tagIds);
            return Ok(list.Adapt<List<GetTaskResponse>>());
        }

        /// <summary>
        /// Получение задания по его id
        /// </summary>
        /// <param name="id">Id задания</param>
        /// <returns>Задание</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var list = await _taskService.GetById(id);
            return Ok(list.Adapt<List<GetTaskResponse>>());
        }

        /// <summary>
        /// Добавление нового задания
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     POST /Todo
        ///     {
        ///         "title": "Новое задание",
        ///         "conditionText": "Содержание задания",
        ///         "subjectId" : 3,
        ///         "academicYearId" : 2,
        ///         "typeId" : 1,
        ///         "userAddedId": 1,
        ///         "authorIds" : [2,5],
        ///         "tagIds": [1,3]
        ///     }
        ///
        /// </remarks>
        /// <param name="task">Задание</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Add(CreateTaskRequest task)
        {
            var exercise = new Exercise
            {
                Title = task.Title,
                ConditionText = task.ConditionText,
                SubjectId = task.SubjectId,
                AcademicYearId = task.AcademicYearId,
                TypeId = task.TypeId,
                UserAddedId = task.UserAddedId,

                // Создаем временные сущности только с ID
                Authors = task.AuthorIds.Select(id => new Author { Id = id }).ToList(),
                Tags = task.TagIds.Select(id => new Tag { Id = id }).ToList()
            };

            await _taskService.Create(exercise);
            return Ok();
        }

        /// <summary>
        /// Изменение существующего задания
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     PUT /Todo
        ///     {
        ///         "id": 1,
        ///         "title": "Новое задание",
        ///         "conditionText": "Содержание задания",
        ///         "subjectId" : 3,
        ///         "academicYearId" : 2,
        ///         "typeId" : 1,
        ///         "authorIds" : [2,5],
        ///         "tagIds": [1,3]
        ///     }
        ///
        /// </remarks>
        /// <param name="task">Задание</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Update(UpdateTaskRequest task)
        {
            var exercise = new Exercise
            {
                Id = task.Id,
                Title = task.Title,
                ConditionText = task.ConditionText,
                SubjectId = task.SubjectId,
                AcademicYearId = task.AcademicYearId,
                TypeId = task.TypeId,

                // Создаем временные сущности только с ID
                Authors = task.AuthorIds.Select(id => new Author { Id = id }).ToList(),
                Tags = task.TagIds.Select(id => new Tag { Id = id }).ToList()
            };
            await _taskService.Update(exercise);
            return Ok();
        }

        /// <summary>
        /// Удаление существующего задания
        /// </summary>
        /// <param name="id">Id задания</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _taskService.Delete(id);
            return Ok();
        }
    }
}
