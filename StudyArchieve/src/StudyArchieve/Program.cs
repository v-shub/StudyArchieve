using Amazon.Extensions.NETCore.Setup;
using Amazon.S3;
using BusinessLogic.Services;
using DataAccess;
using DataAccess.Wrapper;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text;

namespace StudyArchieveApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Установка кодировки по умолчанию для всего приложения
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Console.OutputEncoding = Encoding.UTF8;

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            // Настройка Entity Framework с поддержкой Unicode
            builder.Services.AddDbContext<StudyArchieveContext>(options =>
            {
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    sqlOptions =>
                    {
                        sqlOptions.CommandTimeout(60);
                    });
            });

            // Регистрация сервисов
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
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IBackblazeService, BackblazeService>();

            // Настройка для обработки больших файлов
            builder.Services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 104857600; // 100MB
                options.MultipartBodyLengthLimit = 104857600; // 100MB
                options.ValueLengthLimit = int.MaxValue;
                options.MultipartBoundaryLengthLimit = int.MaxValue;
                options.MultipartHeadersCountLimit = int.MaxValue;
                options.MultipartHeadersLengthLimit = int.MaxValue;
            });

            // Backblaze B2 Configuration
            builder.Services.AddSingleton<IAmazonS3>(provider =>
            {
                var config = new AmazonS3Config
                {
                    ServiceURL = builder.Configuration["Backblaze:ServiceURL"],
                    ForcePathStyle = true,
                    Timeout = TimeSpan.FromMinutes(5),
                };

                var awsCredentials = new Amazon.Runtime.BasicAWSCredentials(
                    builder.Configuration["Backblaze:KeyId"],
                    builder.Configuration["Backblaze:ApplicationKey"]
                );

                return new AmazonS3Client(awsCredentials, config);
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "StudyArchieve API",
                    Description = "API для управления учебным архивом заданий и решений.<br/>" +
                        "Обеспечивает хранение, поиск и управление учебными материалами " +
                        "и связанными метаданными.<br/><br/>" +
                        "<b>Основные возможности:</b><br/>" +
                        "• Управление заданиями, предметами, авторами, тегами<br/>" +
                        "• Загрузка решений и файлов<br/>" +
                        "• Поиск и фильтрация заданий<br/>" +
                        "• Управление пользователями и ролями<br/>",
                    Contact = new OpenApiContact
                    {
                        Name = "StudyArchieve Team",
                        Email = "victoriya.shubina.mex@gmail.com",
                        Url = new Uri("https://github.com/v-shub")
                    }
                });

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            // Настройка CORS
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins("https://studyarchieveclient.onrender.com")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });

            var app = builder.Build();

            app.UseCors();
            // Middleware pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "StudyArchieve API v1");
                    options.RoutePrefix = string.Empty;
                });
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            // Инициализация базы данных
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<StudyArchieveContext>();
                    // Можно добавить миграции или инициализацию данных при необходимости
                    Console.WriteLine("База данных подключена успешно");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при подключении к базе данных: {ex.Message}");
                }
            }

            Console.WriteLine("Приложение запущено");
            app.Run();
        }
    }
}
