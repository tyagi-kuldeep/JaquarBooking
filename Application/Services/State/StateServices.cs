using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.State
{
    public class StateServices : IStateServices
    {

        #region Properties
        private readonly IUnitOfWork _unitOfWork;
        #endregion
        #region Ctor
        public StateServices(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        #endregion       
        public async Task<bool> DeleteAsync(int Id, int UpdatedBy)
        {
            var entity = await GetByIdAsync(Id);
            if (entity == null) return false;
            entity.IsDeleted = true;
            entity.UpdatedBy = UpdatedBy;
            entity.UpdatedDate = DateTime.UtcNow;
            _unitOfWork.StateRepo.Update(entity);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public IQueryable<JB_MasterState> GetAll(Expression<Func<JB_MasterState, bool>> where)
        {
            return _unitOfWork.StateRepo.TableNoTracking.Where(where);
        }

        public async Task<JB_MasterState> GetByIdAsync(int Id)
        {
            return await _unitOfWork.StateRepo.FindByIdAsync(Id);
        }

        public async Task<bool> IsDuplicateAsync(int Id, string Name)
        {
            int result = 0;
            if (Id > 0)
                result = await _unitOfWork.StateRepo.TableNoTracking.Where(w =>
                w.IsDeleted == false &&
                w.Id != Id && w.Name.ToLower() == Name.ToLower()).Select(s => s.Id).FirstOrDefaultAsync();
            else
                result = await _unitOfWork.StateRepo.TableNoTracking.Where(w =>
                w.IsDeleted == false &&
                w.Name.ToLower() == Name.ToLower()).Select(s => s.Id).FirstOrDefaultAsync();
            return result > 0 ? true : false;
        }

        public async Task<bool> UpdateStatusAsync(int Id, int UpdatedBy)
        {
            var entity = await GetByIdAsync(Id);
            if (entity == null) return false;
            entity.IsActive = !entity.IsActive;
            entity.UpdatedBy = UpdatedBy;
            entity.UpdatedDate = DateTime.UtcNow;
            _unitOfWork.StateRepo.Update(entity);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<JB_MasterState> UpsertAsync(JB_MasterState model)
        {
            if (model.Id > 0)
                _unitOfWork.StateRepo.Update(model);
            else
                await _unitOfWork.StateRepo.AddAsync(model);
            await _unitOfWork.SaveAsync();
            return model;
        }
        public string GetStateByName(int? Id)
        {
            if (Id > 0)
            {
                var data = _unitOfWork.StateRepo.TableNoTracking.Where(w => w.Id == Id).Select(s => s.Name).FirstOrDefault();
                return data == null ? "" : data;
            }
            else return "";

        }

    }
}
