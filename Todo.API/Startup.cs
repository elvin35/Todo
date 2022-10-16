using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using Todo.API.Profiles;
using Todo.API.Services;
using Todo.Contexts.Models;
using Todo.DAL.Repositories;

namespace TodoApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = new SqlConnectionStringBuilder(Configuration.GetConnectionString("SQLExpress"));
            connectionString.UserID = Configuration["ConnectionCredentials:Username"];
            connectionString.Password = Configuration["ConnectionCredentials:Password"];
            // add MS SQL Server (Express edition)
            services.AddDbContext<TodoContext>(opt =>
            {
                opt
                    .UseSqlServer(connectionString.ToString());
                    //.EnableSensitiveDataLogging();
            });
            // add mapping
            services.AddAutoMapper(typeof(TodoItemProfile));

            services.AddControllers();

            // add swagger
            services.AddSwaggerGen(options =>
            {

                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Todo.API",
                    Description = "Todo app API documentation",
                    Contact = new OpenApiContact
                    {
                        Name = "Elvin Mamedov",
                        Email = "elvin.mamedov.2013@gmail.com"
                    }
                });

                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "Todo.API.xml");
                options.IncludeXmlComments(xmlPath);
            });

            // add repository to DI-container
            services.AddScoped<ITodoRepository, TodoRepository>();
            // add service to DI-container
            services.AddScoped<ITodoService, TodoService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo.API.v1");
                c.RoutePrefix = String.Empty;
            });

        }
    }
}
