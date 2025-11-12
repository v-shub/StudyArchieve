using Domain.Interfaces;
using Domain.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudyArchieveApi.Contracts.User;
using Microsoft.EntityFrameworkCore;

namespace StudyArchieveApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Получение пользователя по его id
        /// </summary>
        /// <param name="id">Id пользователя</param>
        /// <returns>Пользователь</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var that = await _userService.GetById(id);
                return Ok(that.Adapt<GetUserResponse>());
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Некорректный ID при получении пользователя: {Id}", id);
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Пользователь не найден: {Id}", id);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении пользователя по ID: {Id}", id);
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        /// <summary>
        /// Добавление нового пользователя
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     POST /Todo
        ///     {
        ///         "userName": "newUser1234",
        ///         "email": "youremail@gmail.com",
        ///         "password": "password1234",
        ///         "roleId": 3,
        ///     }
        ///
        /// </remarks>
        /// <param name="user">Пользователь</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Add(CreateUserRequest user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _userService.Create(user.Adapt<User>());
                return Ok("Пользователь успешно создан");
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogWarning(ex, "Передан null при создании пользователя");
                return BadRequest("Данные пользователя не могут быть пустыми");
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Некорректные данные при создании пользователя");
                return BadRequest(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Ошибка базы данных при создании пользователя");
                return StatusCode(500, "Ошибка при сохранении данных");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизвестная ошибка при создании пользователя");
                return StatusCode(500, "Внутренняя ошибка сервера при создании пользователя");
            }
        }

        /// <summary>
        /// Изменение существующего пользователя
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     PUT /Todo
        ///     {
        ///         "id": 2,
        ///         "userName": "newUser1234",
        ///         "email": "youremail@gmail.com",
        ///         "password": "password1234",
        ///         "roleId": 3,
        ///     }
        ///
        /// </remarks>
        /// <param name="user">Пользователь</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Update(UpdateUserRequest user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _userService.Update(user.Adapt<User>());
                return Ok("Пользователь успешно обновлен");
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogWarning(ex, "Передан null при обновлении пользователя");
                return BadRequest("Данные пользователя не могут быть пустыми");
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Некорректные данные при обновлении пользователя: {Id}", user.Id);
                return BadRequest(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Ошибка базы данных при обновлении пользователя: {Id}", user.Id);
                return StatusCode(500, "Ошибка при обновлении данных");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизвестная ошибка при обновлении пользователя: {Id}", user.Id);
                return StatusCode(500, "Внутренняя ошибка сервера при обновлении пользователя");
            }
        }

        /// <summary>
        /// Удаление существующего пользователя
        /// </summary>
        /// <param name="id">Id пользователя</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _userService.Delete(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Некорректный ID при удалении пользователя: {Id}", id);
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Пользователь не найден при удалении: {Id}", id);
                return NotFound(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Ошибка базы данных при удалении пользователя: {Id}", id);
                return StatusCode(500, "Ошибка при удалении данных");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизвестная ошибка при удалении пользователя: {Id}", id);
                return StatusCode(500, "Внутренняя ошибка сервера при удалении пользователя");
            }
        }
    }
}