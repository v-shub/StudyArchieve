using Microsoft.JSInterop;

public class FileDownloadService
{
    private readonly IJSRuntime _jsRuntime;
    private readonly HttpClient _httpClient;

    public FileDownloadService(IJSRuntime jsRuntime, HttpClient httpClient)
    {
        _jsRuntime = jsRuntime;
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://studyarchieve.onrender.com");
    }

    public async Task DownloadTaskFileAsync(int fileId, string fileName)
    {
        try
        {
            // Создаем URL для скачивания
            var url = $"/api/TaskFile/{fileId}/download";

            // Используем JavaScript для скачивания файла
            await _jsRuntime.InvokeVoidAsync("downloadFile", url, fileName);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка скачивания файла задания: {ex.Message}");
        }
    }

    public async Task DownloadSolutionFileAsync(int fileId, string fileName)
    {
        try
        {
            // Создаем URL для скачивания
            var url = $"/api/SolutionFile/{fileId}/download";

            // Используем JavaScript для скачивания файла
            await _jsRuntime.InvokeVoidAsync("downloadFile", url, fileName);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка скачивания файла решения: {ex.Message}");
        }
    }
}