using System.Text;
using DataAccess.Models;
using Newtonsoft.Json;
using Thread = System.Threading.Thread;

public class PostService
{
    private readonly HttpClient httpClient;

    public PostService(HttpClient httpClient) { this.httpClient = httpClient; }

    public async Task<List<Post>?> GetAllPosts()
    {
        var response = await this.httpClient.GetAsync("http://localhost:5000/api/Post");

        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var posts        = JsonConvert.DeserializeObject<List<Post>>(jsonResponse);
            return posts;
        }

        return null;
    }
    
    public async Task<List<Thread>> GetThreadsAsync()
    {
        var response = await httpClient.GetAsync("http://localhost:5000/api/Thread");
        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Thread>>(jsonResponse);
        }

        return new List<Thread>();
    }

    public async Task<Post?> GetPostByIdAsync(int postId)
    {
        var response = await httpClient.GetAsync($"http://localhost:5000/api/Post/{postId}");
        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Post>(jsonResponse);
        }

        return null;
    }

    public async Task<List<Post>?> GetPostsByUserIdAsync(int userId)
    {
        var response = await httpClient.GetAsync("http://localhost:5000/api/Post");
        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var posts        = JsonConvert.DeserializeObject<List<Post>>(jsonResponse);
            return posts?.Where(p => p.UserId == userId).ToList();
        }

        return null;
    }

    public async Task<bool> CreatePostAsync(Post newPost)
    {
        var jsonContent = JsonConvert.SerializeObject(newPost);
        var content     = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        var response    = await httpClient.PostAsync("http://localhost:5000/api/Post", content);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdatePostAsync(Post updatedPost)
    {
        var jsonContent = JsonConvert.SerializeObject(updatedPost);
        var content     = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        var response    = await httpClient.PutAsync($"http://localhost:5000/api/Post/{updatedPost.PostId}", content);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeletePostAsync(int postId)
    {
        var response = await httpClient.DeleteAsync($"http://localhost:5000/api/Post/{postId}");
        return response.IsSuccessStatusCode;
    }

    public async Task<List<Reply>?> GetCommentsOnPostAsync(int postId)
    {
        var response = await this.httpClient.GetAsync($"http://localhost:5000/api/Reply/");

        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var replies      = JsonConvert.DeserializeObject<List<Reply>>(jsonResponse);
            var postReplies  = replies?.Where(r => r.PostId == postId).ToList();
            return postReplies;
        }

        return null;
    }
}