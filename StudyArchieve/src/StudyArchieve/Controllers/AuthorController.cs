using Domain.Interfaces;
using Domain.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudyArchieveApi.Contracts.Author;
using Microsoft.EntityFrameworkCore;

namespace StudyArchieveApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;
        private readonly ILogger<AuthorController> _logger;

        public AuthorController(IAuthorService authorService, ILogger<AuthorController> logger)
        {
            _authorService = authorService;
            _logger = logger;
        }

        /// <summary>
        /// Получение списка всех авторов заданий
        /// </summary>
        /// <returns>Список всех авторов заданий</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var list = await _authorService.GetAll();
                return Ok(list.Adapt<List<GetAuthorResponse>>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении списка авторов");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        /// <summary>
        /// Добавление нового автора заданий
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     POST /Todo
        ///     {
        ///         "name": "Ларионов Дмитрий Ильич"
        ///     }
        ///
        /// </remarks>
        /// <param name="author">Автор заданий</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Add(CreateAuthorRequest author)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _authorService.Create(author.Adapt<Author>());
                return Ok("Автор успешно создан");
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogWarning(ex, "Передан null при создании автора");
                return BadRequest("Данные автора не могут быть пустыми");
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Некорректные данные при создании автора");
                return BadRequest(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Ошибка базы данных при создании автора");
                return StatusCode(500, "Ошибка при сохранении данных");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизвестная ошибка при создании автора");
                return StatusCode(500, "Внутренняя ошибка сервера при создании автора");
            }
        }

        /// <summary>
        /// Изменение существующего автора заданий
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     PUT /Todo
        ///     {
        ///         "id": 1,
        ///         "name": "Ларионов Дмитрий Ильич"
        ///     }
        ///
        /// </remarks>
        /// <param name="author">Автор заданий</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Update(UpdateAuthorRequest author)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _authorService.Update(author.Adapt<Author>());
                return Ok("Автор успешно обновлен");
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogWarning(ex, "Передан null при обновлении автора");
                return BadRequest("Данные автора не могут быть пустыми");
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Некорректные данные при обновлении автора: {Id}", author.Id);
                return BadRequest(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Ошибка базы данных при обновлении автора: {Id}", author.Id);
                return StatusCode(500, "Ошибка при обновлении данных");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизвестная ошибка при обновлении автора: {Id}", author.Id);
                return StatusCode(500, "Внутренняя ошибка сервера при обновлении автора");
            }
        }

        /// <summary>
        /// Удаление существующего автора заданий
        /// </summary>
        /// <param name="id">Id автора заданий</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _authorService.Delete(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Некорректный ID при удалении автора: {Id}", id);
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Автор не найден при удалении: {Id}", id);
                return NotFound(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Ошибка базы данных при удалении автора: {Id}", id);
                return StatusCode(500, "Ошибка при удалении данных");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизвестная ошибка при удалении автора: {Id}", id);
                return StatusCode(500, "Внутренняя ошибка сервера при удалении автора");
            }
        }
    }
}