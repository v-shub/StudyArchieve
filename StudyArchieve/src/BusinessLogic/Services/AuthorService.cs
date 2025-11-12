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
    public class AuthorService : IAuthorService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public AuthorService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper ?? throw new ArgumentNullException(nameof(repositoryWrapper));
        }

        public async Task<List<Author>> GetAll()
        {
            var that = await _repositoryWrapper.Author.FindAll();
            return that;
        }
        /*
        public async Task<Author> GetById(int id)
        {
            var that = await _repositoryWrapper.Author
                .FindByCondition(x => x.Id == id);
            return that.First();
        }*/

        public async Task Create(Author model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            await _repositoryWrapper.Author.Create(model);
            await _repositoryWrapper.Save();
        }

        public async Task Update(Author model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            await _repositoryWrapper.Author.Update(model);
            await _repositoryWrapper.Save();
        }

        public async Task Delete(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Id must be greater than zero", nameof(id));

            var that = await _repositoryWrapper.Author
                .FindByCondition(x => x.Id == id);

            if (that == null || !that.Any())
                throw new InvalidOperationException($"Author with id {id} not found");

            await _repositoryWrapper.Author.Delete(that.First());
            await _repositoryWrapper.Save();
        }
    }
}