using System.Net.Http.Json;
using BlazorClient.Services.Contracts.Task;
using BlazorClient.Services.Contracts.SolutionFile;

public class ApiService
{
    private readonly HttpClient _httpClient;

    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://studyarchieve.onrender.com");
    }

    public async Task<List<T>> GetListFromJsonAsync<T>(string url)
    {
        return await _httpClient.GetFromJsonAsync<List<T>>(url) ?? new List<T>();
    }

    public async Task<T> GetFromJsonAsync<T>(string url)
    {
        return await _httpClient.GetFromJsonAsync<T>(url);
    }

    public async Task<HttpResponseMessage> PostAsJsonAsync<T>(string url, T data)
    {
        return await _httpClient.PostAsJsonAsync(url, data);
    }

    public async Task<HttpResponseMessage> PutAsJsonAsync<T>(string url, T data)
    {
        return await _httpClient.PutAsJsonAsync(url, data);
    }

    public async Task<HttpResponseMessage> DeleteAsync(string url)
    {
        return await _httpClient.DeleteAsync(url);
    }

    public async Task<List<GetTaskResponse>> GetTasksByFilter(int? subjectId, int? academicYearId, int? typeId, int[]? authorIds, int[]? tagIds)
    {
        var queryParams = new List<string>();

        if (subjectId.HasValue)
            queryParams.Add($"subjectId={subjectId}");
        if (academicYearId.HasValue)
            queryParams.Add($"academicYearId={academicYearId}");
        if (typeId.HasValue)
            queryParams.Add($"typeId={typeId}");
        if (authorIds != null && authorIds.Length > 0)
            queryParams.Add($"authorIds={string.Join(",", authorIds)}");
        if (tagIds != null && tagIds.Length > 0)
            queryParams.Add($"tagIds={string.Join(",", tagIds)}");

        var queryString = queryParams.Any() ? "?" + string.Join("&", queryParams) : "";
        var url = $"api/Task{queryString}";

        return await GetListFromJsonAsync<GetTaskResponse>(url);
    }
}