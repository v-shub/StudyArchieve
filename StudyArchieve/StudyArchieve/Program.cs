using BusinessLogic.Services;
using DataAccess;
using DataAccess.Wrapper;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace StudyArchieveApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<StudyArchieveContext>(
                options => options.UseSqlServer(
                    "Server= shubina.mssql.somee.com ;Database= shubina;User Id= v_shub_SQLLogin_1;Password= xLy-9UQ-nkT-8dW;TrustServerCertificate=True"));

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

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
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
