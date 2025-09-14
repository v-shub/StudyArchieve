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
    public class TagService : ITagService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public TagService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public Task<List<Tag>> GetAll()
        {
            return _repositoryWrapper.Tag.FindAll().ToListAsync();
        }

        public Task<Tag> GetById(int id)
        {
            var that = _repositoryWrapper.Tag
                .FindByCondition(x => x.Id == id).First();
            return Task.FromResult(that);
        }

        public Task Create(Tag model)
        {
            _repositoryWrapper.Tag.Create(model);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

        public Task Update(Tag model)
        {
            _repositoryWrapper.Tag.Update(model);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }

        public Task Delete(int id)
        {
            var that = _repositoryWrapper.Tag
                .FindByCondition(x => x.Id == id).First();

            _repositoryWrapper.Tag.Delete(that);
            _repositoryWrapper.Save();
            return Task.CompletedTask;
        }
    }
}
