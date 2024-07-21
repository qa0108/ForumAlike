namespace Prn231_Project
{
    using System.Text;
    using System.Text.Json.Serialization;
    using DataAccess.DAOs;
    using DataAccess.Models;
    using DataAccess.Repositories.Implementation;
    using DataAccess.Repositories.Interfaces;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.OData;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.OData.ModelBuilder;
    using Prn231_Project.Services;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSecretKeyHere")),
                        ValidateIssuer           = false,
                        ValidateAudience         = false
                    };
                });

            // Add services to the container.
            builder.Services.AddControllers();
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
            builder.Services.AddScoped<FollowThreadDAO>();

            // Register repository interfaces and their implementations with the dependency injection container
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IPostRepository, PostRepository>();
            builder.Services.AddScoped<IReplyRepository, ReplyRepository>();
            builder.Services.AddScoped<IRoleRepository, RoleRepository>();
            builder.Services.AddScoped<IThreadRepository, ThreadRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IFollowThreadRepository, FollowThreadRepository>();

            // Add OData services
            builder.Services.AddControllers().AddOData(opt =>
            {
                var odataBuilder = new ODataConventionModelBuilder();
                odataBuilder.EntitySet<Category>("Categories");
                odataBuilder.EntitySet<Post>("Posts");
                odataBuilder.EntitySet<Reply>("Replies");
                odataBuilder.EntitySet<Role>("Roles");
                odataBuilder.EntitySet<Thread>("Threads");
                odataBuilder.EntitySet<User>("Users");

                opt.AddRouteComponents("odata", odataBuilder.GetEdmModel())
                    .Select()
                    .Filter()
                    .OrderBy()
                    .Expand()
                    .Count()
                    .SetMaxTop(100);
            }).AddJsonOptions(options =>
            {
                // Use camel case property names in JSON
                options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;

                // Ignore null values in JSON output
                options.JsonSerializerOptions.IgnoreNullValues = true;
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;

                // Configure date format, etc.
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                // Add any other necessary configuration as per your requirements
            });

            builder.Services.AddScoped<UserService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}