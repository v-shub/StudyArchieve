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
    public class RoleService : IRoleService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public RoleService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<List<Role>> GetAll()
        {
            var that = await _repositoryWrapper.Role.FindAll();
            return that;
        }
        /*
        public async Task<Role> GetById(int id)
        {
            var that = await _repositoryWrapper.Role
                .FindByCondition(x => x.Id == id);
            return that.First();
        }
        */
        public async Task Create(Role model)
        {
            await _repositoryWrapper.Role.Create(model);
            await _repositoryWrapper.Save();
        }

        public async Task Update(Role model)
        {
            await _repositoryWrapper.Role.Update(model);
            await _repositoryWrapper.Save();
        }

        public async Task Delete(int id)
        {
            var that = await _repositoryWrapper.Role
                .FindByCondition(x => x.Id == id);

            await _repositoryWrapper.Role.Delete(that.First());
            await _repositoryWrapper.Save();
        }
    }
}
