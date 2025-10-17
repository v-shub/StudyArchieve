using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using StudyArchieveApi.Contracts.SolutionFile;

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

        [HttpPost("upload")]
        public async Task<ActionResult<UploadSolutionFileResponse>> UploadFile([FromForm] UploadSolutionFileRequest request)
        {
            try
            {
                if (request.File == null || request.File.Length == 0)
                    return BadRequest("File is required");

                var result = await _solutionFileService.UploadFileAsync(request.SolutionId, request.File);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading solution file");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("solution/{solutionId}")]
        public async Task<ActionResult<List<SolutionFileResponse>>> GetFilesBySolution(int solutionId)
        {
            try
            {
                var files = await _solutionFileService.GetFilesBySolutionIdAsync(solutionId);
                return Ok(files);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting files for solution {SolutionId}", solutionId);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SolutionFileResponse>> GetFile(int id)
        {
            try
            {
                var file = await _solutionFileService.GetFileByIdAsync(id);
                if (file == null) return NotFound();
                return Ok(file);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting file with id {FileId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFile(int id)
        {
            try
            {
                var success = await _solutionFileService.DeleteFileAsync(id);
                if (!success) return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting file with id {FileId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}/download")]
        public async Task<IActionResult> DownloadFile(int id)
        {
            try
            {
                var downloadResult = await _solutionFileService.DownloadFileAsync(id);
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