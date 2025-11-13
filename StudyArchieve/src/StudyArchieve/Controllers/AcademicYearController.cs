using Domain.Interfaces;
using Domain.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudyArchieveApi.Contracts.AcademicYear;

namespace StudyArchieveApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AcademicYearController : ControllerBase
    {
        private readonly IAcademicYearService _academicYearService;
        private readonly ILogger<AcademicYearController> _logger;

        public AcademicYearController(
            IAcademicYearService academicYearService,
            ILogger<AcademicYearController> logger)
        {
            _academicYearService = academicYearService;
            _logger = logger;
        }

        /// <summary>
        /// Получение списка всех учебных лет
        /// </summary>
        /// <returns>Список всех учебных лет</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var list = await _academicYearService.GetAll();
                return Ok(list.Adapt<List<GetAcademicYearResponse>>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении списка учебных лет");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        /// <summary>
        /// Добавление нового учебного года
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     POST /Todo
        ///     {
        ///         "yearLabel": "2026-2027"
        ///     }
        ///
        /// </remarks>
        /// <param name="academicYear">Учебный год</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Add(CreateAcademicYearRequest academicYear)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _academicYearService.Create(academicYear.Adapt<AcademicYear>());
                return Ok("Учебный год успешно создан");
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogWarning(ex, "Передан null при создании учебного года");
                return BadRequest("Данные учебного года не могут быть пустыми");
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Некорректные данные при создании учебного года");
                return BadRequest(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Ошибка базы данных при создании учебного года");
                return StatusCode(500, "Ошибка при сохранении данных");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизвестная ошибка при создании учебного года");
                return StatusCode(500, "Внутренняя ошибка сервера при создании учебного года");
            }
        }

        /// <summary>
        /// Изменение существующего учебного года
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     PUT /Todo
        ///     {
        ///         "id": 3,
        ///         "yearLabel": "2026-2027"
        ///     }
        ///
        /// </remarks>
        /// <param name="academicYear">Учебный год</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Update(UpdateAcademicYearRequest academicYear)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _academicYearService.Update(academicYear.Adapt<AcademicYear>());
                return Ok("Учебный год успешно обновлен");
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogWarning(ex, "Передан null при обновлении учебного года");
                return BadRequest("Данные учебного года не могут быть пустыми");
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Некорректные данные при обновлении учебного года: {Id}", academicYear.Id);
                return BadRequest(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Ошибка базы данных при обновлении учебного года: {Id}", academicYear.Id);
                return StatusCode(500, "Ошибка при обновлении данных");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизвестная ошибка при обновлении учебного года: {Id}", academicYear.Id);
                return StatusCode(500, "Внутренняя ошибка сервера при обновлении учебного года");
            }
        }

        /// <summary>
        /// Удаление существующего учебного года
        /// </summary>
        /// <param name="id">Id учебного года</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _academicYearService.Delete(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Некорректный ID при удалении учебного года: {Id}", id);
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Учебный год не найден при удалении: {Id}", id);
                return NotFound(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Ошибка базы данных при удалении учебного года: {Id}", id);
                return StatusCode(500, "Ошибка при удалении данных");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизвестная ошибка при удалении учебного года: {Id}", id);
                return StatusCode(500, "Внутренняя ошибка сервера при удалении учебного года");
            }
        }
    }
}