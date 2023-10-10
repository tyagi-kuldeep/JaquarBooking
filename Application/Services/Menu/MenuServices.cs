using Application.Common.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Menu
{
    public class MenuServices: IMenuServices
    {
        #region Properties
        private readonly IUnitOfWork _unitOfWork;
        #endregion
        #region Ctor
        public MenuServices(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        #endregion       
        public async Task<bool> DeleteAsync(int Id, int UpdatedBy)
        {
            var entity = await GetByIdAsync(Id);
            if (entity == null) return false;
            await _unitOfWork.SaveAsync();
            return true;
        }

        public IQueryable<JB_MasterMenu> GetAll(Expression<Func<JB_MasterMenu, bool>> where)
        {
            return _unitOfWork.MenuRepo.TableNoTracking.Where(where);
        }

        public async Task<JB_MasterMenu> GetByIdAsync(int Id)
        {
            return await _unitOfWork.MenuRepo.FindByIdAsync(Id);
        }

        public async Task<bool> UpdateStatusAsync(int Id, int UpdatedBy)
        {
            var entity = await GetByIdAsync(Id);
            if (entity == null) return false;           
            _unitOfWork.MenuRepo.Update(entity);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<JB_MasterMenu> UpsertAsync(JB_MasterMenu model)
        {
            if (model.Id > 0)
                _unitOfWork.MenuRepo.Update(model);
            else
                await _unitOfWork.MenuRepo.AddAsync(model);
            await _unitOfWork.SaveAsync();
            return model;
        }

    }
}