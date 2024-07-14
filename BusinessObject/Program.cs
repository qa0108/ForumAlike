using DataAccess.DAOs;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

class Program
{
    static void Main(string[] args)
    {
        // Create a new ServiceCollection
        var serviceCollection = new ServiceCollection();

        // Register ForumDBContext with the dependency injection container
        serviceCollection.AddDbContext<ForumDBContext>(options =>
            options.UseSqlServer("YourConnectionStringHere"));

        // Register DAO classes with the dependency injection container
        serviceCollection.AddScoped<CategoryDAO>();
        serviceCollection.AddScoped<PostDAO>();
        serviceCollection.AddScoped<ReplyDAO>();
        serviceCollection.AddScoped<RoleDAO>();
        serviceCollection.AddScoped<ThreadDAO>();
        serviceCollection.AddScoped<UserDAO>();
    }
}