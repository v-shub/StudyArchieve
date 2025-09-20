using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _subjectService.GetAll());
        }
        /*
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _subjectService.GetById(id));
        }
        /*
        [HttpPost]
        public async Task<IActionResult> Add(Subject subject)
        {
            await _subjectService.Create(subject);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update(Subject subject)
        {
            await _subjectService.Update(subject);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _subjectService.Delete(id);
            return Ok();
        }*/
    }
}
