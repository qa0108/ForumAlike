namespace WebClient.Services;

using DataAccess.Models;
using Newtonsoft.Json;

public class UserService
{
    private readonly HttpClient httpClient;
    
    public UserService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

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
}