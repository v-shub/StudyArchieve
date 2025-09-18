using BusinessLogic.Interfaces;
using DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _solutionService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _solutionService.GetById(id));
        }
        /*
        [HttpPost]
        public async Task<IActionResult> Add(Solution solution)
        {
            await _solutionService.Create(solution);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update(Solution solution)
        {
            await _solutionService.Update(solution);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _solutionService.Delete(id);
            return Ok();
        }*/
    }
}
