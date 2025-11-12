using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using StudyArchieveApi.Contracts.TaskFile;
using Microsoft.EntityFrameworkCore;

namespace StudyArchieveApi.Controllers
{
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
                    return BadRequest("Файл обязателен для загрузки");

                var result = await _taskFileService.UploadFileAsync(request.TaskId, request.File);
                return Ok(result);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogWarning(ex, "Передан null при загрузке файла задания");
                return BadRequest("Данные файла не могут быть пустыми");
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Некорректные данные при загрузке файла задания");
                return BadRequest(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Ошибка базы данных при загрузке файла задания");
                return StatusCode(500, "Ошибка при сохранении данных файла");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизвестная ошибка при загрузке файла задания");
                return StatusCode(500, "Внутренняя ошибка сервера при загрузке файла");
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
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Некорректный Task ID при получении файлов: {TaskId}", taskId);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении файлов для задания {TaskId}", taskId);
                return StatusCode(500, "Внутренняя ошибка сервера");
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
                if (file == null)
                {
                    _logger.LogWarning("Файл задания не найден: {FileId}", id);
                    return NotFound("Файл задания не найден");
                }
                return Ok(file);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Некорректный ID при получении файла задания: {FileId}", id);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении файла с id {FileId}", id);
                return StatusCode(500, "Внутренняя ошибка сервера");
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
                if (!success)
                {
                    _logger.LogWarning("Файл задания не найден при удалении: {FileId}", id);
                    return NotFound("Файл задания не найден");
                }
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Некорректный ID при удалении файла задания: {FileId}", id);
                return BadRequest(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Ошибка базы данных при удалении файла задания: {FileId}", id);
                return StatusCode(500, "Ошибка при удалении данных файла");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизвестная ошибка при удалении файла с id {FileId}", id);
                return StatusCode(500, "Внутренняя ошибка сервера при удалении файла");
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
            catch (FileNotFoundException ex)
            {
                _logger.LogWarning(ex, "Файл задания не найден при загрузке: {FileId}", id);
                return NotFound("Файл задания не найден");
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Некорректный ID при загрузке файла задания: {FileId}", id);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при загрузке файла с id {FileId}", id);
                return StatusCode(500, "Внутренняя ошибка сервера при загрузке файла");
            }
        }
    }
}