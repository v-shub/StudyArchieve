using Domain.Interfaces;
using BusinessLogic.Services;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace StudyArchieveApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskFileController : ControllerBase
    {
        private ITaskFileService _taskFileService;
        public TaskFileController(ITaskFileService taskFileService)
        {
            _taskFileService = taskFileService;
        }
        /*
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _taskFileService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _taskFileService.GetById(id));
        }
        /*
        [HttpPost]
        public async Task<IActionResult> Add(TaskFile taskFile)
        {
            await _taskFileService.Create(taskFile);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update(TaskFile taskFile)
        {
            await _taskFileService.Update(taskFile);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _taskFileService.Delete(id);
            return Ok();
        }*/
    }
}
