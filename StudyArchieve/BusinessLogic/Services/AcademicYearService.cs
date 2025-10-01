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
    public class AcademicYearService : IAcademicYearService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public AcademicYearService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<List<AcademicYear>> GetAll()
        {
            var that = await _repositoryWrapper.AcademicYear.FindAll();
            return that;
        }
        /*
        public async Task<AcademicYear> GetById(int id)
        {
            var that = await _repositoryWrapper.AcademicYear
                .FindByCondition(x => x.Id == id);
            return that.First();
        }
        */

        public async Task Create(AcademicYear model)
        {
            await _repositoryWrapper.AcademicYear.Create(model);
            await _repositoryWrapper.Save();
        }

        public async Task Update(AcademicYear model)
        {
            await _repositoryWrapper.AcademicYear.Update(model);
            await _repositoryWrapper.Save();
        }

        public async Task Delete(int id)
        {
            var that = await _repositoryWrapper.AcademicYear
                .FindByCondition(x => x.Id == id);

            await _repositoryWrapper.AcademicYear.Delete(that.First());
            await _repositoryWrapper.Save();
        }
    }
}
