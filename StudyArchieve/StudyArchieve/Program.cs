using Amazon.Extensions.NETCore.Setup;
using Amazon.S3;
using BusinessLogic.Authorization;
using BusinessLogic.Helpers;
using BusinessLogic.Services;
using DataAccess;
using DataAccess.Wrapper;
using Domain.Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using StudyArchieveApi.Authorization;
using StudyArchieveApi.Helpers;
using System.Reflection;

namespace StudyArchieveApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

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
            builder.Services.AddScoped<IUserService, UserService>();
            
            builder.Services.AddScoped<IBackblazeService, BackblazeService>();

            builder.Services.AddScoped<IJwtUtils, JwtUtils>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IEmailService, EmailService>();

            // Backblaze B2 Configuration
            builder.Services.AddSingleton<IAmazonS3>(provider =>
            {
                var config = new AmazonS3Config
                {
                    ServiceURL = builder.Configuration["Backblaze:ServiceURL"],
                    ForcePathStyle = true // ������������� ��� Backblaze B2
                };

                var awsCredentials = new Amazon.Runtime.BasicAWSCredentials(
                    builder.Configuration["Backblaze:KeyId"],
                    builder.Configuration["Backblaze:ApplicationKey"]
                );

                return new AmazonS3Client(awsCredentials, config);
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddMapster();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "����� ������� ������� ��� API",
                    Description = "API ��� ������ � ������� ������� ������� ������� ���.<br/>" +
                        "������������� ����������� ������, ���������� � ���������� ��������� " +
                        "� ���������� ������� � �������� ��������.<br/><br/>" +
                        "<b>�������� �������:</b><br/>" +
                        "� ����� �� ���������, �������, �����<br/>" +
                        "� ������������� ������� � ��������<br/>" +
                        "� ������� ����� � ���������<br/>" +
                        "� ��������� �������� ��������<br/>" +
                        "� ���������� �� ������� �����",
                    Contact = new OpenApiContact
                    {
                        Name = "��������� StudyArchieve",
                        Email = "victoriya.shubina.mex@gmail.com",
                        Url = new Uri("https://github.com/v-shub")
                    }
                });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                  {
                    {
                      new OpenApiSecurityScheme
                      {
                        Reference = new OpenApiReference
                          {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                          },
                          Scheme = "oauth2",
                          Name = "Bearer",
                          In = ParameterLocation.Header,

                        },
                        new List<string>()
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

            app.UseMiddleware<ErrorHandlerMiddleware>();
            app.UseMiddleware<JwtMiddleware>();

            app.MapControllers();

            app.Run();
        }
    }
}
