using BusinessLogic.Interfaces;
using DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _academicYearService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _academicYearService.GetById(id));
        }
        /*
        [HttpPost]
        public async Task<IActionResult> Add(AcademicYear academicYear)
        {
            await _academicYearService.Create(academicYear);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update(AcademicYear academicYear)
        {
            await _academicYearService.Update(academicYear);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _academicYearService.Delete(id);
            return Ok();
        }*/
    }
}
