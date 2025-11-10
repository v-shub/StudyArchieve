using Domain.Interfaces;
using Domain.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudyArchieveApi.Contracts.Subject;
using Microsoft.EntityFrameworkCore;

namespace StudyArchieveApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService _subjectService;
        private readonly ILogger<SubjectController> _logger;

        public SubjectController(ISubjectService subjectService, ILogger<SubjectController> logger)
        {
            _subjectService = subjectService;
            _logger = logger;
        }

        /// <summary>
        /// Получение списка всех учебных дисциплин
        /// </summary>
        /// <returns>Список всех учебных дисциплин</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var list = await _subjectService.GetAll();
                return Ok(list.Adapt<List<GetSubjectResponse>>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении списка учебных дисциплин");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        /// <summary>
        /// Добавление новой учебной дисциплины
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     POST /Todo
        ///     {
        ///         "name": "Дискретная математика"
        ///     }
        ///
        /// </remarks>
        /// <param name="subject">Учебная дисциплина</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Add(CreateSubjectRequest subject)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _subjectService.Create(subject.Adapt<Subject>());
                return Ok("Учебная дисциплина успешно создана");
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogWarning(ex, "Передан null при создании учебной дисциплины");
                return BadRequest("Данные учебной дисциплины не могут быть пустыми");
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Некорректные данные при создании учебной дисциплины");
                return BadRequest(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Ошибка базы данных при создании учебной дисциплины");
                return StatusCode(500, "Ошибка при сохранении данных");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизвестная ошибка при создании учебной дисциплины");
                return StatusCode(500, "Внутренняя ошибка сервера при создании учебной дисциплины");
            }
        }

        /// <summary>
        /// Изменение существующей учебной дисциплины
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     PUT /Todo
        ///     {
        ///         "id": 1,
        ///         "name": "Теория вероятностей"
        ///     }
        ///
        /// </remarks>
        /// <param name="subject">Учебная дисциплина</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Update(UpdateSubjectRequest subject)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _subjectService.Update(subject.Adapt<Subject>());
                return Ok("Учебная дисциплина успешно обновлена");
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogWarning(ex, "Передан null при обновлении учебной дисциплины");
                return BadRequest("Данные учебной дисциплины не могут быть пустыми");
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Некорректные данные при обновлении учебной дисциплины: {Id}", subject.Id);
                return BadRequest(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Ошибка базы данных при обновлении учебной дисциплины: {Id}", subject.Id);
                return StatusCode(500, "Ошибка при обновлении данных");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизвестная ошибка при обновлении учебной дисциплины: {Id}", subject.Id);
                return StatusCode(500, "Внутренняя ошибка сервера при обновлении учебной дисциплины");
            }
        }

        /// <summary>
        /// Удаление существующей учебной дисциплины
        /// </summary>
        /// <param name="id">Id учебной дисциплины</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _subjectService.Delete(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Некорректный ID при удалении учебной дисциплины: {Id}", id);
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Учебная дисциплина не найдена при удалении: {Id}", id);
                return NotFound(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Ошибка базы данных при удалении учебной дисциплины: {Id}", id);
                return StatusCode(500, "Ошибка при удалении данных");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизвестная ошибка при удалении учебной дисциплины: {Id}", id);
                return StatusCode(500, "Внутренняя ошибка сервера при удалении учебной дисциплины");
            }
        }
    }
}