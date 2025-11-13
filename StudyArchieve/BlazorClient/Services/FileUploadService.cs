using Microsoft.AspNetCore.Components.Forms;

public class FileUploadService
{
    private readonly HttpClient _httpClient;

    public FileUploadService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> UploadTaskFileAsync(int taskId, IBrowserFile file)
    {
        try
        {
            var formData = new MultipartFormDataContent();
            formData.Add(new StringContent(taskId.ToString()), "TaskId");

            var fileContent = new StreamContent(file.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024));
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
            formData.Add(fileContent, "File", file.Name);

            var response = await _httpClient.PostAsync("api/TaskFile/upload", formData);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> UploadSolutionFileAsync(int solutionId, IBrowserFile file)
    {
        try
        {
            var formData = new MultipartFormDataContent();
            formData.Add(new StringContent(solutionId.ToString()), "SolutionId");

            var fileContent = new StreamContent(file.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024));
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
            formData.Add(fileContent, "File", file.Name);

            var response = await _httpClient.PostAsync("api/SolutionFile/upload", formData);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeleteSolutionFileAsync(int fileId)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/SolutionFile/{fileId}");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}