using Domain.DTOs;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace Domain.Interfaces
{
    public interface ISubjectService
    {
        Task<List<SubjectDto>> GetAll();/*
        Task<Subject> GetById(int id);
        Task Create(Subject model);
        Task Update(Subject model);
        Task Delete(int id);*/
    }
}
