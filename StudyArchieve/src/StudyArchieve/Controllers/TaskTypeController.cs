using Domain.Interfaces;
using Domain.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudyArchieveApi.Contracts.TaskType;
using Microsoft.EntityFrameworkCore;

namespace StudyArchieveApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskTypeController : ControllerBase
    {
        private readonly ITaskTypeService _taskTypeService;
        private readonly ILogger<TaskTypeController> _logger;

        public TaskTypeController(ITaskTypeService taskTypeService, ILogger<TaskTypeController> logger)
        {
            _taskTypeService = taskTypeService;
            _logger = logger;
        }

        /// <summary>
        /// Получение списка всех типов заданий
        /// </summary>
        /// <returns>Список всех типов заданий</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var list = await _taskTypeService.GetAll();
                return Ok(list.Adapt<List<GetTaskTypeResponse>>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении списка типов заданий");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        /// <summary>
        /// Добавление нового типа заданий
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     POST /Todo
        ///     {
        ///         "name": "Домашнее задание"
        ///     }
        ///
        /// </remarks>
        /// <param name="taskType">Тип заданий</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Add(CreateTaskTypeRequest taskType)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _taskTypeService.Create(taskType.Adapt<TaskType>());
                return Ok("Тип заданий успешно создан");
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogWarning(ex, "Передан null при создании типа заданий");
                return BadRequest("Данные типа заданий не могут быть пустыми");
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Некорректные данные при создании типа заданий");
                return BadRequest(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Ошибка базы данных при создании типа заданий");
                return StatusCode(500, "Ошибка при сохранении данных");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизвестная ошибка при создании типа заданий");
                return StatusCode(500, "Внутренняя ошибка сервера при создании типа заданий");
            }
        }

        /// <summary>
        /// Изменение существующего типа заданий
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     PUT /Todo
        ///     {
        ///         "id": 3,
        ///         "name": "Самостоятельная работа"
        ///     }
        ///
        /// </remarks>
        /// <param name="taskType">Тип заданий</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Update(UpdateTaskTypeRequest taskType)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _taskTypeService.Update(taskType.Adapt<TaskType>());
                return Ok("Тип заданий успешно обновлен");
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogWarning(ex, "Передан null при обновлении типа заданий");
                return BadRequest("Данные типа заданий не могут быть пустыми");
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Некорректные данные при обновлении типа заданий: {Id}", taskType.Id);
                return BadRequest(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Ошибка базы данных при обновлении типа заданий: {Id}", taskType.Id);
                return StatusCode(500, "Ошибка при обновлении данных");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизвестная ошибка при обновлении типа заданий: {Id}", taskType.Id);
                return StatusCode(500, "Внутренняя ошибка сервера при обновлении типа заданий");
            }
        }

        /// <summary>
        /// Удаление существующего типа заданий
        /// </summary>
        /// <param name="id">Id типа заданий</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _taskTypeService.Delete(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Некорректный ID при удалении типа заданий: {Id}", id);
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Тип заданий не найден при удалении: {Id}", id);
                return NotFound(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Ошибка базы данных при удалении типа заданий: {Id}", id);
                return StatusCode(500, "Ошибка при удалении данных");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизвестная ошибка при удалении типа заданий: {Id}", id);
                return StatusCode(500, "Внутренняя ошибка сервера при удалении типа заданий");
            }
        }
    }
}