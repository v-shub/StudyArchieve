using BusinessLogic.Interfaces;
using DataAccess.Models;
using DataAccess.Wrapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace BusinessLogic.Services
{
    public class SolutionFileService : ISolutionFileService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public SolutionFileService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public Task<List<SolutionFile>> GetAll()
        {
            return _repositoryWrapper.SolutionFile.FindAll().ToListAsync();
        }

        public Task<SolutionFile> GetById(int id)
        {
            var that = _repositoryWrapper.SolutionFile
                .FindByCondition(x => x.Id == id).First();
            return Task.FromResult(that);
        }

        public Task Create(SolutionFile model)
        {
            _repositoryWrapper.SolutionFile.Create(model);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

        public Task Update(SolutionFile model)
        {
            _repositoryWrapper.SolutionFile.Update(model);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

        public Task Delete(int id)
        {
            var that = _repositoryWrapper.SolutionFile
                .FindByCondition(x => x.Id == id).First();

            _repositoryWrapper.SolutionFile.Delete(that);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }
    }
}
