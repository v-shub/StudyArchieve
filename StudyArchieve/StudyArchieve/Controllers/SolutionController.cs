using Domain.Interfaces;
using Domain.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudyArchieveApi.Contracts.Solution;

namespace StudyArchieveApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolutionController : ControllerBase
    {
        private ISolutionService _solutionService;
        public SolutionController(ISolutionService solutionService)
        {
            _solutionService = solutionService;
        }

        /// <summary>
        /// Получение списка решений по id задания
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     GET /Todo
        ///     {
        ///        "taskId" : 3
        ///     }
        ///
        /// </remarks>
        /// <param name="taskId">Id задания</param>
        /// <returns>Список решений задания</returns>
        [HttpGet("{taskId}")]
        public async Task<IActionResult> GetByTaskId(int taskId)
        {
            var list = await _solutionService.GetByTaskId(taskId);
            return Ok(list.Adapt<List<GetSolutionResponse>>());
        }

        /// <summary>
        /// Добавление нового решения
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     POST /Todo
        ///     {
        ///         "taskId": 2,
        ///         "solutionText": "Решения нет, это задача-подвох",
        ///         "userAddedId": 1
        ///     }
        ///
        /// </remarks>
        /// <param name="solution">Решение</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Add(CreateSolutionRequest solution)
        {
            await _solutionService.Create(solution.Adapt<Solution>());
            return Ok();
        }

        /// <summary>
        /// Изменение существующего решения
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     PUT /Todo
        ///     {
        ///         "id": 1,
        ///         "solutionText": "Решения нет, это задача-подвох"
        ///     }
        ///
        /// </remarks>
        /// <param name="solution">Решение</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Update(UpdateSolutionRequest solution)
        {
            await _solutionService.Update(solution.Adapt<Solution>());
            return Ok();
        }

        /// <summary>
        /// Удаление существующего решения
        /// </summary>
        /// <param name="id">Id решения</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _solutionService.Delete(id);
            return Ok();
        }
    }
}
