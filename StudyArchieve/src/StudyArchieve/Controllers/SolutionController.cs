using Domain.Interfaces;
using Domain.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudyArchieveApi.Contracts.Solution;
using Microsoft.EntityFrameworkCore;

namespace StudyArchieveApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolutionController : ControllerBase
    {
        private readonly ISolutionService _solutionService;
        private readonly ILogger<SolutionController> _logger;

        public SolutionController(ISolutionService solutionService, ILogger<SolutionController> logger)
        {
            _solutionService = solutionService;
            _logger = logger;
        }

        /// <summary>
        /// Получение списка решений по id задания
        /// </summary>
        /// <param name="taskId">Id задания</param>
        /// <returns>Список решений задания</returns>
        [HttpGet("task/{taskId}")]
        public async Task<IActionResult> GetByTaskId(int taskId)
        {
            try
            {
                var list = await _solutionService.GetByTaskId(taskId);
                return Ok(list.Adapt<List<GetSolutionResponse>>());
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Некорректный Task ID при получении решений: {TaskId}", taskId);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении решений по Task ID: {TaskId}", taskId);
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        /// <summary>
        /// Получение решения по его id
        /// </summary>
        /// <param name="id">Id решения</param>
        /// <returns>Решение</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var that = await _solutionService.GetById(id);
                return Ok(that.Adapt<GetSolutionResponse>());
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Некорректный ID при получении решения: {Id}", id);
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Решение не найдено: {Id}", id);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении решения по ID: {Id}", id);
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        /// <summary>
        /// Добавление нового решения
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     POST /Todo
        ///     {
        ///         "taskId": 2,
        ///         "solutionText": "Решения нет, это задача-подвох",
        ///         "userAddedId": 1
        ///     }
        ///
        /// </remarks>
        /// <param name="solution">Решение</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Add(CreateSolutionRequest solution)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _solutionService.Create(solution.Adapt<Solution>());
                return Ok("Решение успешно создано");
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogWarning(ex, "Передан null при создании решения");
                return BadRequest("Данные решения не могут быть пустыми");
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Некорректные данные при создании решения");
                return BadRequest(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Ошибка базы данных при создании решения");
                return StatusCode(500, "Ошибка при сохранении данных");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизвестная ошибка при создании решения");
                return StatusCode(500, "Внутренняя ошибка сервера при создании решения");
            }
        }

        /// <summary>
        /// Изменение существующего решения
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     PUT /Todo
        ///     {
        ///         "id": 1,
        ///         "solutionText": "Решения нет, это задача-подвох"
        ///     }
        ///
        /// </remarks>
        /// <param name="solution">Решение</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Update(UpdateSolutionRequest solution)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Получаем существующее решение
                var existingSolution = await _solutionService.GetById(solution.Id);

                // Обновляем только нужные поля
                existingSolution.SolutionText = solution.SolutionText;

                await _solutionService.Update(existingSolution);
                return Ok("Решение успешно обновлено");
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogWarning(ex, "Передан null при обновлении решения");
                return BadRequest("Данные решения не могут быть пустыми");
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Некорректные данные при обновлении решения: {Id}", solution.Id);
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Решение не найдено при обновлении: {Id}", solution.Id);
                return NotFound(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Ошибка базы данных при обновлении решения: {Id}", solution.Id);
                return StatusCode(500, "Ошибка при обновлении данных");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизвестная ошибка при обновлении решения: {Id}", solution.Id);
                return StatusCode(500, "Внутренняя ошибка сервера при обновлении решения");
            }
        }

        /// <summary>
        /// Удаление существующего решения
        /// </summary>
        /// <param name="id">Id решения</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _solutionService.Delete(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Некорректный ID при удалении решения: {Id}", id);
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Решение не найдено при удалении: {Id}", id);
                return NotFound(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Ошибка базы данных при удалении решения: {Id}", id);
                return StatusCode(500, "Ошибка при удалении данных");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизвестная ошибка при удалении решения: {Id}", id);
                return StatusCode(500, "Внутренняя ошибка сервера при удалении решения");
            }
        }
    }
}