using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using StudyArchieveApi.Contracts.SolutionFile;
using Microsoft.EntityFrameworkCore;

namespace StudyArchieveApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SolutionFileController : ControllerBase
    {
        private readonly ISolutionFileService _solutionFileService;
        private readonly ILogger<SolutionFileController> _logger;

        public SolutionFileController(ISolutionFileService solutionFileService, ILogger<SolutionFileController> logger)
        {
            _solutionFileService = solutionFileService;
            _logger = logger;
        }

        /// <summary>
        /// Выгрузка на сервер нового файла решения
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
        /// <returns>Id файла решения</returns>
        [HttpPost("upload")]
        public async Task<ActionResult<UploadSolutionFileResponse>> UploadFile([FromForm] UploadSolutionFileRequest request)
        {
            try
            {
                if (request.File == null || request.File.Length == 0)
                    return BadRequest("Файл обязателен для загрузки");

                var result = await _solutionFileService.UploadFileAsync(request.SolutionId, request.File);
                return Ok(result);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogWarning(ex, "Передан null при загрузке файла решения");
                return BadRequest("Данные файла не могут быть пустыми");
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Некорректные данные при загрузке файла решения");
                return BadRequest(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Ошибка базы данных при загрузке файла решения");
                return StatusCode(500, "Ошибка при сохранении данных файла");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизвестная ошибка при загрузке файла решения");
                return StatusCode(500, "Внутренняя ошибка сервера при загрузке файла");
            }
        }

        /// <summary>
        /// Получение списка файлов по id решения
        /// </summary>
        /// <param name="solutionId">Id решения</param>
        /// <returns>Список файлов решения</returns>
        [HttpGet("solution/{solutionId}")]
        public async Task<ActionResult<List<SolutionFileResponse>>> GetFilesBySolution(int solutionId)
        {
            try
            {
                var files = await _solutionFileService.GetFilesBySolutionIdAsync(solutionId);
                return Ok(files);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Некорректный Solution ID при получении файлов: {SolutionId}", solutionId);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении файлов для решения {SolutionId}", solutionId);
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        /// <summary>
        /// Получение файла решения по его id
        /// </summary>
        /// <param name="id">Id файла решения</param>
        /// <returns>Файл решения</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<SolutionFileResponse>> GetFile(int id)
        {
            try
            {
                var file = await _solutionFileService.GetFileByIdAsync(id);
                if (file == null)
                {
                    _logger.LogWarning("Файл решения не найден: {FileId}", id);
                    return NotFound("Файл решения не найден");
                }
                return Ok(file);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Некорректный ID при получении файла решения: {FileId}", id);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении файла с id {FileId}", id);
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        /// <summary>
        /// Удаление существующего файла решения по его id
        /// </summary>
        /// <param name="id">Id файла решения</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFile(int id)
        {
            try
            {
                var success = await _solutionFileService.DeleteFileAsync(id);
                if (!success)
                {
                    _logger.LogWarning("Файл решения не найден при удалении: {FileId}", id);
                    return NotFound("Файл решения не найден");
                }
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Некорректный ID при удалении файла решения: {FileId}", id);
                return BadRequest(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Ошибка базы данных при удалении файла решения: {FileId}", id);
                return StatusCode(500, "Ошибка при удалении данных файла");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизвестная ошибка при удалении файла с id {FileId}", id);
                return StatusCode(500, "Внутренняя ошибка сервера при удалении файла");
            }
        }

        /// <summary>
        /// Загрузка файла решения с сервера
        /// </summary>
        /// <param name="id">Id файла решения</param>
        /// <returns>Файл решения</returns>
        [HttpGet("{id}/download")]
        public async Task<IActionResult> DownloadFile(int id)
        {
            try
            {
                var downloadResult = await _solutionFileService.DownloadFileAsync(id);
                return File(downloadResult.Content, downloadResult.ContentType, downloadResult.FileName);
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogWarning(ex, "Файл решения не найден при загрузке: {FileId}", id);
                return NotFound("Файл решения не найден");
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Некорректный ID при загрузке файла решения: {FileId}", id);
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