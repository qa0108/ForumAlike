namespace Prn231_Project
{
    using DataAccess.DAOs;
    using DataAccess.Models;
    using DataAccess.Repositories.Implementation;
    using DataAccess.Repositories.Interfaces;
    using Microsoft.EntityFrameworkCore;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<ForumDBContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("MyCnn")));

            // Register DAO classes with the dependency injection container
            builder.Services.AddScoped<CategoryDAO>();
            builder.Services.AddScoped<PostDAO>();
            builder.Services.AddScoped<ReplyDAO>();
            builder.Services.AddScoped<RoleDAO>();
            builder.Services.AddScoped<ThreadDAO>();
            builder.Services.AddScoped<UserDAO>();

            // Register repository interfaces and their implementations with the dependency injection container
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IPostRepository, PostRepository>();
            builder.Services.AddScoped<IReplyRepository, ReplyRepository>();
            builder.Services.AddScoped<IRoleRepository, RoleRepository>();
            builder.Services.AddScoped<IThreadRepository, ThreadRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}