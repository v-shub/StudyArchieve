using Domain.Interfaces;
using Domain.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudyArchieveApi.Contracts.Tag;
using Microsoft.EntityFrameworkCore;

namespace StudyArchieveApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly ITagService _tagService;
        private readonly ILogger<TagController> _logger;

        public TagController(ITagService tagService, ILogger<TagController> logger)
        {
            _tagService = tagService;
            _logger = logger;
        }

        /// <summary>
        /// Получение списка всех тэгов
        /// </summary>
        /// <returns>Список всех тэгов</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var list = await _tagService.GetAll();
                return Ok(list.Adapt<List<GetTagResponse>>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении списка тэгов");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        /// <summary>
        /// Добавление нового тэга
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     POST /Todo
        ///     {
        ///         "name": "MS SQL Server"
        ///     }
        ///
        /// </remarks>
        /// <param name="tag">Тэг</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Add(CreateTagRequest tag)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _tagService.Create(tag.Adapt<Tag>());
                return Ok("Тэг успешно создан");
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogWarning(ex, "Передан null при создании тэга");
                return BadRequest("Данные тэга не могут быть пустыми");
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Некорректные данные при создании тэга");
                return BadRequest(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Ошибка базы данных при создании тэга");
                return StatusCode(500, "Ошибка при сохранении данных");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизвестная ошибка при создании тэга");
                return StatusCode(500, "Внутренняя ошибка сервера при создании тэга");
            }
        }

        /// <summary>
        /// Изменение существующего тэга
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     PUT /Todo
        ///     {
        ///         "id": 3,
        ///         "name": "Regex"
        ///     }
        ///
        /// </remarks>
        /// <param name="tag">Тэг</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Update(UpdateTagRequest tag)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _tagService.Update(tag.Adapt<Tag>());
                return Ok("Тэг успешно обновлен");
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogWarning(ex, "Передан null при обновлении тэга");
                return BadRequest("Данные тэга не могут быть пустыми");
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Некорректные данные при обновлении тэга: {Id}", tag.Id);
                return BadRequest(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Ошибка базы данных при обновлении тэга: {Id}", tag.Id);
                return StatusCode(500, "Ошибка при обновлении данных");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизвестная ошибка при обновлении тэга: {Id}", tag.Id);
                return StatusCode(500, "Внутренняя ошибка сервера при обновлении тэга");
            }
        }

        /// <summary>
        /// Удаление существующего тэга
        /// </summary>
        /// <param name="id">Id тэга</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _tagService.Delete(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Некорректный ID при удалении тэга: {Id}", id);
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Тэг не найден при удалении: {Id}", id);
                return NotFound(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Ошибка базы данных при удалении тэга: {Id}", id);
                return StatusCode(500, "Ошибка при удалении данных");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизвестная ошибка при удалении тэга: {Id}", id);
                return StatusCode(500, "Внутренняя ошибка сервера при удалении тэга");
            }
        }
    }
}