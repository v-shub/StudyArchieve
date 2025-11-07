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
    public class TagService : ITagService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public TagService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper ?? throw new ArgumentNullException(nameof(repositoryWrapper));
        }

        public async Task<List<Tag>> GetAll()
        {
            var that = await _repositoryWrapper.Tag.FindAll();
            return that;
        }
        /*
        public async Task<Tag> GetById(int id)
        {
            var that = await _repositoryWrapper.Tag
                .FindByCondition(x => x.Id == id);
            return that.First();
        }
        */
        public async Task Create(Tag model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            await _repositoryWrapper.Tag.Create(model);
            await _repositoryWrapper.Save();
        }

        public async Task Update(Tag model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            await _repositoryWrapper.Tag.Update(model);
            await _repositoryWrapper.Save();
        }

        public async Task Delete(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Id must be greater than zero", nameof(id));

            var that = await _repositoryWrapper.Tag
                .FindByCondition(x => x.Id == id);

            if (that == null || !that.Any())
                throw new InvalidOperationException($"Tag with id {id} not found");

            await _repositoryWrapper.Tag.Delete(that.First());
            await _repositoryWrapper.Save();
        }
    }
}