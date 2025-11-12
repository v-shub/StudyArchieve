using Domain.Interfaces;
using Domain.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudyArchieveApi.Contracts.Role;
using Microsoft.EntityFrameworkCore;

namespace StudyArchieveApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly ILogger<RoleController> _logger;

        public RoleController(IRoleService roleService, ILogger<RoleController> logger)
        {
            _roleService = roleService;
            _logger = logger;
        }

        /// <summary>
        /// Получение списка всех пользовательских ролей
        /// </summary>
        /// <returns>Список всех пользовательских ролей</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var list = await _roleService.GetAll();
                return Ok(list.Adapt<List<GetRoleResponse>>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении списка ролей");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        /// <summary>
        /// Добавление новой пользовательской роли
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     POST /Todo
        ///     {
        ///         "roleName": "Модератор"
        ///     }
        ///
        /// </remarks>
        /// <param name="role">Роль пользователей</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Add(CreateRoleRequest role)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _roleService.Create(role.Adapt<Role>());
                return Ok("Роль успешно создана");
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogWarning(ex, "Передан null при создании роли");
                return BadRequest("Данные роли не могут быть пустыми");
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Некорректные данные при создании роли");
                return BadRequest(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Ошибка базы данных при создании роли");
                return StatusCode(500, "Ошибка при сохранении данных");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизвестная ошибка при создании роли");
                return StatusCode(500, "Внутренняя ошибка сервера при создании роли");
            }
        }

        /// <summary>
        /// Изменение существующей пользовательской роли
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     PUT /Todo
        ///     {
        ///         "id": 1,
        ///         "roleName": "Модератор"
        ///     }
        ///
        /// </remarks>
        /// <param name="role">Роль пользователей</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Update(UpdateRoleRequest role)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _roleService.Update(role.Adapt<Role>());
                return Ok("Роль успешно обновлена");
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogWarning(ex, "Передан null при обновлении роли");
                return BadRequest("Данные роли не могут быть пустыми");
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Некорректные данные при обновлении роли: {Id}", role.Id);
                return BadRequest(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Ошибка базы данных при обновлении роли: {Id}", role.Id);
                return StatusCode(500, "Ошибка при обновлении данных");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизвестная ошибка при обновлении роли: {Id}", role.Id);
                return StatusCode(500, "Внутренняя ошибка сервера при обновлении роли");
            }
        }

        /// <summary>
        /// Удаление существующей пользовательской роли
        /// </summary>
        /// <param name="id">Id пользовательской роли</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _roleService.Delete(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Некорректный ID при удалении роли: {Id}", id);
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Роль не найдена при удалении: {Id}", id);
                return NotFound(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Ошибка базы данных при удалении роли: {Id}", id);
                return StatusCode(500, "Ошибка при удалении данных");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизвестная ошибка при удалении роли: {Id}", id);
                return StatusCode(500, "Внутренняя ошибка сервера при удалении роли");
            }
        }
    }
}