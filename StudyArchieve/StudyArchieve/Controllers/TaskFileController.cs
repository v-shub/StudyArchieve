using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using StudyArchieveApi.Contracts.TaskFile;

namespace StudyArchieveApi.Controllers
{
    // API/Controllers/TaskFilesController.cs
    [ApiController]
    [Route("api/[controller]")]
    public class TaskFileController : ControllerBase
    {
        private readonly ITaskFileService _taskFileService;
        private readonly ILogger<TaskFileController> _logger;

        public TaskFileController(ITaskFileService taskFileService, ILogger<TaskFileController> logger)
        {
            _taskFileService = taskFileService;
            _logger = logger;
        }

        /// <summary>
        /// Выгрузка на сервер нового файла задания
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     POST /Todo
        ///     {
        ///         "taskId": 1,
        ///         "file": "[бинарные данные файла]"
        ///     }
        ///
        /// </remarks>
        /// <param name="request">Запрос</param>
        /// <returns>Id файла задания</returns>
        [HttpPost("upload")]
        public async Task<ActionResult<UploadTaskFileResponse>> UploadFile([FromForm] UploadTaskFileRequest request)
        {
            try
            {
                if (request.File == null || request.File.Length == 0)
                    return BadRequest("File is required");

                var result = await _taskFileService.UploadFileAsync(request.TaskId, request.File);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading task file");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Получение списка файлов по id задания
        /// </summary>
        /// <param name="taskId">Id задания</param>
        /// <returns>Список файлов задания</returns>
        [HttpGet("task/{taskId}")]
        public async Task<ActionResult<List<TaskFileResponse>>> GetFilesByTask(int taskId)
        {
            try
            {
                var files = await _taskFileService.GetFilesByTaskIdAsync(taskId);
                return Ok(files);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting files for task {TaskId}", taskId);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Получение файла задания по его id
        /// </summary>
        /// <param name="id">Id файла задания</param>
        /// <returns>Файл задания</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskFileResponse>> GetFile(int id)
        {
            try
            {
                var file = await _taskFileService.GetFileByIdAsync(id);
                if (file == null) return NotFound();
                return Ok(file);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting file with id {FileId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Удаление существующего файла задания по его id
        /// </summary>
        /// <param name="id">Id файла задания</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFile(int id)
        {
            try
            {
                var success = await _taskFileService.DeleteFileAsync(id);
                if (!success) return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting file with id {FileId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Загрузка файла задания с сервера
        /// </summary>
        /// <param name="id">Id файла задания</param>
        /// <returns>Файл задания</returns>
        [HttpGet("{id}/download")]
        public async Task<IActionResult> DownloadFile(int id)
        {
            try
            {
                var downloadResult = await _taskFileService.DownloadFileAsync(id);
                return File(downloadResult.Content, downloadResult.ContentType, downloadResult.FileName);
            }
            catch (FileNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error downloading file with id {FileId}", id);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
