using BusinessLogic.Interfaces;
using DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Exercise = DataAccess.Models.Task;

namespace StudyArchieveApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private ITaskService _taskService;
        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }
        /*
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _taskService.GetAll());
        }*/
        [HttpGet]
        public async Task<IActionResult> GetByFilter(int? subjectId, int? academicYearId, int? typeId, [FromQuery] int[]? authorIds, [FromQuery] int[]? tagIds)
        {
            return Ok(await _taskService.GetByFilter(subjectId, academicYearId, typeId, authorIds, tagIds));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _taskService.GetById(id));
        }
        /*
        [HttpPost]
        public async Task<IActionResult> Add(Exercise task)
        {
            await _taskService.Create(task);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update(Exercise task)
        {
            await _taskService.Update(task);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _taskService.Delete(id);
            return Ok();
        }*/
    }
}
