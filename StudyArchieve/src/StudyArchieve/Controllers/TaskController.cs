using Domain.Interfaces;
using Domain.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudyArchieveApi.Contracts.Task;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Exercise = Domain.Models.Task;

namespace StudyArchieveApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly ILogger<TaskController> _logger;

        public TaskController(ITaskService taskService, ILogger<TaskController> logger)
        {
            _taskService = taskService;
            _logger = logger;
        }

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
            try
            {
                var list = await _taskService.GetByFilter(subjectId, academicYearId, typeId, authorIds, tagIds);
                return Ok(list.Adapt<List<GetTaskResponse>>());
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Некорректные параметры фильтра");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении заданий по фильтру");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        /// <summary>
        /// Получение задания по его id
        /// </summary>
        /// <param name="id">Id задания</param>
        /// <returns>Задание</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var that = await _taskService.GetById(id);
                return Ok(that.Adapt<GetTaskResponse>());
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Некорректный ID при получении задания: {Id}", id);
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Задание не найдено: {Id}", id);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении задания по ID: {Id}", id);
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
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
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

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
                return Ok("Задание успешно создано");
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogWarning(ex, "Передан null при создании задания");
                return BadRequest("Данные задания не могут быть пустыми");
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Некорректные данные при создании задания");
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Ошибка связей при создании задания");
                return BadRequest(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Ошибка базы данных при создании задания");
                return StatusCode(500, "Ошибка при сохранении данных");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизвестная ошибка при создании задания");
                return StatusCode(500, "Внутренняя ошибка сервера при создании задания");
            }
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
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

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
                return Ok("Задание успешно обновлено");
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogWarning(ex, "Передан null при обновлении задания");
                return BadRequest("Данные задания не могут быть пустыми");
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Некорректные данные при обновлении задания: {Id}", task.Id);
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Ошибка связей при обновлении задания: {Id}", task.Id);
                return BadRequest(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Ошибка базы данных при обновлении задания: {Id}", task.Id);
                return StatusCode(500, "Ошибка при обновлении данных");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизвестная ошибка при обновлении задания: {Id}", task.Id);
                return StatusCode(500, "Внутренняя ошибка сервера при обновлении задания");
            }
        }

        /// <summary>
        /// Удаление существующего задания
        /// </summary>
        /// <param name="id">Id задания</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _taskService.Delete(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Некорректный ID при удалении задания: {Id}", id);
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Задание не найдено при удалении: {Id}", id);
                return NotFound(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Ошибка базы данных при удалении задания: {Id}", id);
                return StatusCode(500, "Ошибка при удалении данных");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизвестная ошибка при удалении задания: {Id}", id);
                return StatusCode(500, "Внутренняя ошибка сервера при удалении задания");
            }
        }
    }
}