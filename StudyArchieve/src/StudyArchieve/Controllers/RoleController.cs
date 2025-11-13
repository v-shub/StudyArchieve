using Domain.Interfaces;
using Domain.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudyArchieveApi.Contracts.Role;

namespace StudyArchieveApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }


        /// <summary>
        /// Получение списка всех пользовательских ролей
        /// </summary>
        /// <returns>Список всех пользовательских ролей</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _roleService.GetAll();
            return Ok(list.Adapt<List<GetRoleResponse>>());
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
            await _roleService.Create(role.Adapt<Role>());
            return Ok();
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
            await _roleService.Update(role.Adapt<Role>());
            return Ok();
        }


        /// <summary>
        /// Удаление существующей пользовательской роли
        /// </summary>
        /// <param name="id">Id пользовательской роли</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _roleService.Delete(id);
            return Ok();
        }
    }
}
