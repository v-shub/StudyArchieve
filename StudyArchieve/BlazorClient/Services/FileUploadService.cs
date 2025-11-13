using BlazorClient.Services.Contracts.TaskFile;
using BlazorClient.Services.Contracts.SolutionFile;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Headers;

public class FileUploadService
{
    private readonly HttpClient _httpClient;

    public FileUploadService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://studyarchieve.onrender.com");
    }

    // Загрузка файла задания
    public async Task<bool> UploadTaskFileAsync(int taskId, IBrowserFile file)
    {
        try
        {
            using var content = new MultipartFormDataContent();
            using var fileContent = new StreamContent(file.OpenReadStream(maxAllowedSize: 104857600)); // 100MB

            fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
            content.Add(fileContent, "File", file.Name);
            content.Add(new StringContent(taskId.ToString()), "TaskId");

            var response = await _httpClient.PostAsync("api/TaskFile/upload", content);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка загрузки файла задания: {ex.Message}");
            return false;
        }
    }

    // Загрузка файла решения
    public async Task<bool> UploadSolutionFileAsync(int solutionId, IBrowserFile file)
    {
        try
        {
            using var content = new MultipartFormDataContent();
            using var fileContent = new StreamContent(file.OpenReadStream(maxAllowedSize: 104857600)); // 100MB

            fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
            content.Add(fileContent, "File", file.Name);
            content.Add(new StringContent(solutionId.ToString()), "SolutionId");

            var response = await _httpClient.PostAsync("api/SolutionFile/upload", content);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка загрузки файла решения: {ex.Message}");
            return false;
        }
    }

    // Удаление файла задания
    public async Task<bool> DeleteTaskFileAsync(int fileId)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/TaskFile/{fileId}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка удаления файла задания: {ex.Message}");
            return false;
        }
    }

    // Удаление файла решения
    public async Task<bool> DeleteSolutionFileAsync(int fileId)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/SolutionFile/{fileId}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка удаления файла решения: {ex.Message}");
            return false;
        }
    }
}