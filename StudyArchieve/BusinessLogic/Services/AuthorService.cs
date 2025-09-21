using Domain.Interfaces;
using BusinessLogic.Mappers;
using Domain.DTOs;
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
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<List<AuthorDto>> GetAll()
        {
            var that = await _repositoryWrapper.Author.FindAll();
            return AuthorMapper.ToDtoList(that);
        }
        /*
        public async Task<Author> GetById(int id)
        {
            var that = await _repositoryWrapper.Author
                .FindByCondition(x => x.Id == id);
            return that.First();
        }

        public async Task Create(Author model)
        {
            await _repositoryWrapper.Author.Create(model);
            await _repositoryWrapper.Save();
        }

        public async Task Update(Author model)
        {
            await _repositoryWrapper.Author.Update(model);
            await _repositoryWrapper.Save();
        }

        public async Task Delete(int id)
        {
            var that = await _repositoryWrapper.Author
                .FindByCondition(x => x.Id == id);

            await _repositoryWrapper.Author.Delete(that.First());
            await _repositoryWrapper.Save();
        }*/
    }
}
