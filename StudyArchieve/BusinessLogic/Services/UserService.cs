using BusinessLogic.Mappers;
using Domain.DTOs;
using Domain.Interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace BusinessLogic.Services
{
    public class UserService : IUserService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public UserService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<List<UserDto>> GetAll()
        {
            var that = await _repositoryWrapper.User.FindAll();
            return UserMapper.ToDtoList(that);
        }
        
        public async Task<UserDto> GetById(int id)
        {
            var that = await _repositoryWrapper.User
                .FindByCondition(x => x.Id == id);
            return UserMapper.ToDto(that.First());
        }
        
        public async Task Create(User model)
        {
            await _repositoryWrapper.User.Create(model);
            await _repositoryWrapper.Save();
        }

        public async Task Update(User model)
        {
            await _repositoryWrapper.User.Update(model);
            await _repositoryWrapper.Save();
        }

        public async Task Delete(int id)
        {
            var that = await _repositoryWrapper.User
                .FindByCondition(x => x.Id == id);

            await _repositoryWrapper.User.Delete(that.First());
            await _repositoryWrapper.Save();
        }
    }
}
