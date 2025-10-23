using Domain.Interfaces;
using Domain.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudyArchieveApi.Contracts.Tag;
using StudyArchieveApi.Contracts.TaskType;

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
        /// <returns>Список всех типов заданий</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _taskTypeService.GetAll();
            return Ok(list.Adapt<List<GetTaskTypeResponse>>());
        }

        /// <summary>
        /// Добавление нового типа заданий
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     POST /Todo
        ///     {
        ///         "name": "Домашнее задание"
        ///     }
        ///
        /// </remarks>
        /// <param name="taskType">Тип заданий</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Add(CreateTaskTypeRequest taskType)
        {
            await _taskTypeService.Create(taskType.Adapt<TaskType>());
            return Ok();
        }

        /// <summary>
        /// Изменение существующего типа заданий
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     PUT /Todo
        ///     {
        ///         "id": 3,
        ///         "name": "Самостоятельная работа"
        ///     }
        ///
        /// </remarks>
        /// <param name="taskType">Тип заданий</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Update(UpdateTaskTypeRequest taskType)
        {
            await _taskTypeService.Update(taskType.Adapt<TaskType>());
            return Ok();
        }

        /// <summary>
        /// Удаление существующего типа заданий
        /// </summary>
        /// <param name="id">Id типа заданий</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _taskTypeService.Delete(id);
            return Ok();
        }
    }
}
