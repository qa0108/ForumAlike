namespace WebClient.Services;

using DataAccess.Models;
using Newtonsoft.Json;

public class ReplyService
{
    private readonly HttpClient httpClient;
    public ReplyService(HttpClient httpClient) { this.httpClient = httpClient; }
    
    public async Task<List<Reply>?> GetRepliesByUserId(int userId)
    {
        var response = await httpClient.GetAsync($"http://localhost:5000/api/Reply/");
        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var comments     = JsonConvert.DeserializeObject<List<Reply>>(jsonResponse);
            var userComments = comments?.Where(c => c.UserId == userId).ToList();
            return userComments;
        }
        return null;
    }
}