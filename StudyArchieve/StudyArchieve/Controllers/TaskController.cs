using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Exercise = Domain.Models.Task;

namespace StudyArchieveApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private ITaskService _taskService;
        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }
        /*
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _taskService.GetAll());
        }*/

        /// <summary>
        /// Получение списка заданий по условию
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     GET /Todo
        ///     {
        ///        "subjectId" : 3,
        ///        "academicYearId" : 2,
        ///        "typeId" : 1,
        ///        "authorIds" : [1, 2],
        ///        "tagIds" : [2, 4, 5]
        ///     }
        ///
        /// </remarks>
        /// <param name="subjectId">Id учебной дисциплины</param>
        /// <param name="academicYearId">Id учебного года</param>
        /// <param name="typeId">Id типа задания</param>
        /// <param name="authorIds">Набор id авторов задания</param>
        /// <param name="tagIds">Набор id тэгов</param>
        /// <returns>Список заданий, удовлетворяющих условию поиска</returns>
        [HttpGet]
        public async Task<IActionResult> GetByFilter(int? subjectId, int? academicYearId, int? typeId, [FromQuery] int[]? authorIds, [FromQuery] int[]? tagIds)
        {
            return Ok(await _taskService.GetByFilter(subjectId, academicYearId, typeId, authorIds, tagIds));
        }

        /// <summary>
        /// Получение задания со всеми его решениями и файлами по его id
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
        /// <param name="id">Id задания</param>
        /// <returns>Задание со всеми его решениями и файлами</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var that = await _taskService.GetById(id);
            return Ok(that);
        }
        
        [HttpPost]
        public async Task<IActionResult> Add(Exercise task)
        {
            await _taskService.Create(task);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update(Exercise task)
        {
            await _taskService.Update(task);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _taskService.Delete(id);
            return Ok();
        }
    }
}
