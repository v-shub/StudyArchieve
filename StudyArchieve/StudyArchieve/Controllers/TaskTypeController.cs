using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace StudyArchieveApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskTypeController : ControllerBase
    {
        private ITaskTypeService _taskTypeService;
        public TaskTypeController(ITaskTypeService taskTypeService)
        {
            _taskTypeService = taskTypeService;
        }

        /// <summary>
        /// Получение списка всех типов заданий
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     GET /Todo
        ///     {}
        ///
        /// </remarks>
        /// <returns>Список всех типов заданий</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _taskTypeService.GetAll());
        }
        /*
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _taskTypeService.GetById(id));
        }
        */
        [HttpPost]
        public async Task<IActionResult> Add(TaskType taskType)
        {
            await _taskTypeService.Create(taskType);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update(TaskType taskType)
        {
            await _taskTypeService.Update(taskType);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _taskTypeService.Delete(id);
            return Ok();
        }
    }
}
