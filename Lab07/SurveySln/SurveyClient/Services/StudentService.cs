using System.Net.Http.Json;
using SurveyClient.Models;

namespace SurveyClient.Services;

public class StudentService(HttpClient httpClient)
{
    public async Task<List<Student>> GetAllAsync()
        => await httpClient.GetFromJsonAsync<List<Student>>("api/students") ?? [];

    public Task<HttpResponseMessage> CreateAsync(Student student)
        => httpClient.PostAsJsonAsync("api/students", student);

    public Task<HttpResponseMessage> UpdateAsync(Student student)
        => httpClient.PutAsJsonAsync($"api/students/{student.Id}", student);

    public Task<HttpResponseMessage> DeleteAsync(int id)
        => httpClient.DeleteAsync($"api/students/{id}");
}
