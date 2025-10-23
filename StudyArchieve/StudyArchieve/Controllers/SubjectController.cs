using Domain.Interfaces;
using Domain.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudyArchieveApi.Contracts.Subject;

namespace StudyArchieveApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private ISubjectService _subjectService;
        public SubjectController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        /// <summary>
        /// Получение списка всех учебных дисциплин
        /// </summary>
        /// <returns>Список всех учебных дисциплин</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _subjectService.GetAll();
            return Ok(list.Adapt<List<GetSubjectResponse>>());
        }
        /*
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _subjectService.GetById(id));
        }
        */

        /// <summary>
        /// Добавление новой учебной дисциплины
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     POST /Todo
        ///     {
        ///         "name": "Дискретная математика"
        ///     }
        ///
        /// </remarks>
        /// <param name="subject">Учебная дисциплина</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Add(CreateSubjectRequest subject)
        {
            await _subjectService.Create(subject.Adapt<Subject>());
            return Ok();
        }

        /// <summary>
        /// Изменение существующей учебной дисциплины
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     PUT /Todo
        ///     {
        ///         "id": 1,
        ///         "name": "Теория вероятностей"
        ///     }
        ///
        /// </remarks>
        /// <param name="subject">Учебная дисциплина</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Update(UpdateSubjectRequest subject)
        {
            await _subjectService.Update(subject.Adapt<Subject>());
            return Ok();
        }

        /// <summary>
        /// Удаление существующей учебной дисциплины
        /// </summary>
        /// <param name="id">Id учебной дисциплины</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _subjectService.Delete(id);
            return Ok();
        }
    }
}
