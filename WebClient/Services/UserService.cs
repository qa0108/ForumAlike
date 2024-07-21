namespace WebClient.Services;

using System.Text;
using DataAccess.Models;
using Newtonsoft.Json;

public class UserService
{
    private readonly HttpClient httpClient;

    public UserService(HttpClient httpClient) { this.httpClient = httpClient; }

    public async Task<List<User>?> GetAllUsers()
    {
        var response = await this.httpClient.GetAsync("http://localhost:5000/api/User");

        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var users        = JsonConvert.DeserializeObject<List<User>>(jsonResponse);
            return users;
        }

        return null;
    }

    public async Task<User?> GetUserById(int userId)
    {
        var response = await this.httpClient.GetAsync($"http://localhost:5000/api/User/{userId}");

        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var user         = JsonConvert.DeserializeObject<User>(jsonResponse);
            return user;
        }

        return null;
    }

    public async Task Promote(int userId)
    {
        var user = await this.GetUserById(userId);
        if (user == null || user.RoleId <= 2) return;
        user.RoleId = 2;
        var jsonContent = JsonConvert.SerializeObject(user);
        var content     = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        await this.httpClient.PutAsync($"http://localhost:5000/api/User/{userId}", content);
    }

    public async Task Demote(int userId)
    {
        var user = await this.GetUserById(userId);
        if (user == null || user.RoleId == 1) return;
        user.RoleId = 3;
        var jsonContent = JsonConvert.SerializeObject(user);
        var content     = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        await this.httpClient.PutAsync($"http://localhost:5000/api/User/{userId}", content);
    }
}