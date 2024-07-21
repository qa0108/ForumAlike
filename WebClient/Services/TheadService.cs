using System.Text;
using DataAccess.Models;
using Newtonsoft.Json;
using Thread = DataAccess.Models.Thread;

public class ThreadService
{
    private readonly HttpClient httpClient;

    public ThreadService(HttpClient httpClient) { this.httpClient = httpClient; }

    public async Task<Thread?> GetThreadById(int threadId)
    {
        var response = await httpClient.GetAsync($"http://localhost:5000/api/Thread/{threadId}");
        if (!response.IsSuccessStatusCode) return null;
        var json   = await response.Content.ReadAsStringAsync();
        var thread = JsonConvert.DeserializeObject<Thread>(json);
        return thread;
    }

    public async Task<bool> Follow(User user, Thread thread)
    {
        var followThread = new FollowThread { UserId = user.UserId, ThreadId = thread.ThreadId,FollowedAt = DateTime.Now, User = user, Thread = thread };
        var jsonContent  = JsonConvert.SerializeObject(followThread);
        var httpContent  = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        try
        {
            var response = await httpClient.PostAsync("http://localhost:5000/api/FollowThread", httpContent);
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public async Task Unfollow(int userId, int threadId)
    {
        try
        {
            var response = await httpClient.DeleteAsync($"http://localhost:5000/api/FollowThread/{userId}/{threadId}");
            if (!response.IsSuccessStatusCode)
            {
            }

        }
        catch (Exception ex)
        {
        }
    }
    
    public async Task<bool> IsUserFollowThread(int userId, int threadId)
    {
        var response = await this.httpClient.GetAsync("http://localhost:5000/api/FollowThread");
        if (!response.IsSuccessStatusCode) return false;
        var json          = await response.Content.ReadAsStringAsync();
        var followThreads = JsonConvert.DeserializeObject<List<FollowThread>>(json);
        if (followThreads == null) return false;
        return followThreads.Any(ft => ft.UserId == userId && ft.ThreadId == threadId);
    }
}