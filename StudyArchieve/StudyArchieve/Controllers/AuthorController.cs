using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        /// <remarks>
        /// Пример запроса:
        ///
        ///     GET /Todo
        ///     {}
        ///
        /// </remarks>
        /// <returns>Список всех авторов заданий</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _authorService.GetAll());
        }
        /*
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _authorService.GetById(id));
        }
        */
        [HttpPost]
        public async Task<IActionResult> Add(Author author)
        {
            await _authorService.Create(author);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update(Author author)
        {
            await _authorService.Update(author);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _authorService.Delete(id);
            return Ok();
        }
    }
}
