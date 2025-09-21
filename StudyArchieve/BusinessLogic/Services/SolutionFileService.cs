using Domain.Interfaces;
using Domain.Models;
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
        /*
        public async Task<List<SolutionFile>> GetAll()
        {
            return await _repositoryWrapper.SolutionFile.FindAll();
        }

        public async Task<SolutionFile> GetById(int id)
        {
            var that = await _repositoryWrapper.SolutionFile
                .FindByCondition(x => x.Id == id);
            return that.First();
        }

        public async Task Create(SolutionFile model)
        {
            await _repositoryWrapper.SolutionFile.Create(model);
            await _repositoryWrapper.Save();
        }

        public async Task Update(SolutionFile model)
        {
            await _repositoryWrapper.SolutionFile.Update(model);
            await _repositoryWrapper.Save();
        }

        public async Task Delete(int id)
        {
            var that = await _repositoryWrapper.SolutionFile
                .FindByCondition(x => x.Id == id);

            await _repositoryWrapper.SolutionFile.Delete(that.First());
            await _repositoryWrapper.Save();
        }*/
    }
}
