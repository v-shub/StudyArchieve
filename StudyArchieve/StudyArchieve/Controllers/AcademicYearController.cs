using Domain.Interfaces;
using Domain.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudyArchieveApi.Contracts.AcademicYear;

namespace StudyArchieveApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AcademicYearController : ControllerBase
    {
        private IAcademicYearService _academicYearService;
        public AcademicYearController(IAcademicYearService academicYearService)
        {
            _academicYearService = academicYearService;
        }
        /// <summary>
        /// Получение списка всех учебных лет
        /// </summary>
        /// <returns>Список всех учебных лет</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _academicYearService.GetAll();
            return Ok(list.Adapt<List<GetAcademicYearResponse>>());
        }
        /*
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _academicYearService.GetById(id));
        }
        */


        /// <summary>
        /// Добавление нового учебного года
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     POST /Todo
        ///     {
        ///         "yearLabel": "2026-2027"
        ///     }
        ///
        /// </remarks>
        /// <param name="academicYear">Учебный год</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Add(CreateAcademicYearRequest academicYear)
        {
            await _academicYearService.Create(academicYear.Adapt<AcademicYear>());
            return Ok();
        }


        /// <summary>
        /// Изменение существующего учебного года
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     PUT /Todo
        ///     {
        ///         "id": 3,
        ///         "yearLabel": "2026-2027"
        ///     }
        ///
        /// </remarks>
        /// <param name="academicYear">Учебный год</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Update(UpdateAcademicYearRequest academicYear)
        {
            await _academicYearService.Update(academicYear.Adapt<AcademicYear>());
            return Ok();
        }


        /// <summary>
        /// Удаление существующего учебного года
        /// </summary>
        /// <param name="id">Id учебного года</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _academicYearService.Delete(id);
            return Ok();
        }
    }
}
