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
    public interface ITaskTypeService
    {
        Task<List<TaskTypeDto>> GetAll();/*
        Task<TaskType> GetById(int id);
        Task Create(TaskType model);
        Task Update(TaskType model);
        Task Delete(int id);*/
    }
}
