using Domain.Interfaces;
using Domain.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudyArchieveApi.Contracts.AcademicYear;
using StudyArchieveApi.Contracts.Tag;

namespace StudyArchieveApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private ITagService _tagService;
        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        /// <summary>
        /// Получение списка всех тэгов
        /// </summary>
        /// <returns>Список всех тэгов</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _tagService.GetAll();
            return Ok(list.Adapt<List<GetTagResponse>>());
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
            await _tagService.Create(tag.Adapt<Tag>());
            return Ok();
        }

        /// <summary>
        /// Изменение существующего тэга
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     POST /Todo
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
            await _tagService.Update(tag.Adapt<Tag>());
            return Ok();
        }

        /// <summary>
        /// Удаление существующего тэга
        /// </summary>
        /// <param name="id">Id тэга</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _tagService.Delete(id);
            return Ok();
        }
    }
}
