using Amazon.Extensions.NETCore.Setup;
using Amazon.S3;
using BusinessLogic.Services;
using DataAccess;
using DataAccess.Wrapper;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace StudyArchieveApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<StudyArchieveContext>(
                options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
            builder.Services.AddScoped<ISubjectService, SubjectService>();
            builder.Services.AddScoped<IAcademicYearService, AcademicYearService>();
            builder.Services.AddScoped<ITaskTypeService, TaskTypeService>();
            builder.Services.AddScoped<IAuthorService, AuthorService>();
            builder.Services.AddScoped<ITagService, TagService>();
            builder.Services.AddScoped<ITaskService, TaskService>();
            builder.Services.AddScoped<ISolutionService, SolutionService>();
            builder.Services.AddScoped<ITaskFileService, TaskFileService>();
            builder.Services.AddScoped<ISolutionFileService, SolutionFileService>();
            builder.Services.AddScoped<IRoleService, RoleService>();
            builder.Services.AddScoped<IUserService, UserService>(); builder.Services.AddScoped<IBackblazeService, BackblazeService>();

            // Backblaze B2 Configuration
            builder.Services.AddSingleton<IAmazonS3>(provider =>
            {
                var config = new AmazonS3Config
                {
                    ServiceURL = builder.Configuration["Backblaze:ServiceURL"],
                    ForcePathStyle = true // Рекомендуется для Backblaze B2
                };

                var awsCredentials = new Amazon.Runtime.BasicAWSCredentials(
                    builder.Configuration["Backblaze:KeyId"],
                    builder.Configuration["Backblaze:ApplicationKey"]
                );

                return new AmazonS3Client(awsCredentials, config);
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Архив заданий прошлых лет API",
                    Description = "API для работы с архивом учебных заданий прошлых лет.<br/>" +
                        "Предоставляет возможности поиска, фильтрации и управления заданиями " +
                        "с поддержкой решений и файловых вложений.<br/><br/>" +
                        "<b>Ключевые функции:</b><br/>" +
                        "• Поиск по предметам, авторам, тегам<br/>" +
                        "• Множественные решения к заданиям<br/>" +
                        "• Система тегов и категорий<br/>" +
                        "• Поддержка файловых вложений<br/>" +
                        "• Фильтрация по учебным годам",
                    Contact = new OpenApiContact
                    {
                        Name = "Поддержка StudyArchieve",
                        Email = "victoriya.shubina.mex@gmail.com",
                        Url = new Uri("https://github.com/v-shub")
                    }
                });
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
