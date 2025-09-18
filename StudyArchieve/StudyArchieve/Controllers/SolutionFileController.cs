using BusinessLogic.Interfaces;
using DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace StudyArchieveApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolutionFileController : ControllerBase
    {
        private ISolutionFileService _solutionFileService;
        public SolutionFileController(ISolutionFileService solutionFileService)
        {
            _solutionFileService = solutionFileService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _solutionFileService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _solutionFileService.GetById(id));
        }
        /*
        [HttpPost]
        public async Task<IActionResult> Add(SolutionFile solutionFile)
        {
            await _solutionFileService.Create(solutionFile);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update(SolutionFile solutionFile)
        {
            await _solutionFileService.Update(solutionFile);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _solutionFileService.Delete(id);
            return Ok();
        }*/
    }
}
