using Domain.Interfaces;
using Domain.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudyArchieveApi.Contracts.User;

namespace StudyArchieveApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /*
        /// <summary>
        /// Получение списка всех пользователей
        /// </summary>
        /// <returns>Список всех пользователей</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _userService.GetAll());
        }
        */

        /// <summary>
        /// Получение пользователя по его id
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     GET /Todo
        ///     {
        ///        "id" : 3
        ///     }
        ///
        /// </remarks>
        /// <param name="id">Id пользователя</param>
        /// <returns>Пользователь</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var that = await _userService.GetById(id);
            return Ok(that.Adapt<GetUserResponse>());
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
            await _userService.Create(user.Adapt<User>());
            return Ok();
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
            await _userService.Update(user.Adapt<User>());
            return Ok();
        }

        /// <summary>
        /// Удаление существующего пользователя
        /// </summary>
        /// <param name="id">Id пользователя</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _userService.Delete(id);
            return Ok();
        }

    }
}
