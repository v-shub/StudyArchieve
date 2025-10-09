using Domain.Interfaces;
using Domain.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudyArchieveApi.Contracts.Author;

namespace StudyArchieveApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private IAuthorService _authorService;
        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        /// <summary>
        /// Получение списка всех авторов заданий
        /// </summary>
        /// <returns>Список всех авторов заданий</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _authorService.GetAll();
            return Ok(list.Adapt<List<GetAuthorResponse>>());
        }
        /*
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _authorService.GetById(id));
        }
        */


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
            await _authorService.Create(author.Adapt<Author>());
            return Ok();
        }


        /// <summary>
        /// Изменение существующего автора заданий
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     POST /Todo
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
            await _authorService.Update(author.Adapt<Author>());
            return Ok();
        }


        /// <summary>
        /// Удаление существующего автора заданий
        /// </summary>
        /// <param name="id">Id автора заданий</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _authorService.Delete(id);
            return Ok();
        }
    }
}
