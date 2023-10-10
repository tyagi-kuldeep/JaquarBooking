using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Booking
{
    public class BookingServices : IBookingServices
    {
        #region Properties
        private readonly IUnitOfWork _unitOfWork;
        #endregion
        #region Ctor
        public BookingServices(IUnitOfWork unitOfWork)
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
            _unitOfWork.BookingRepo.Update(entity);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public IQueryable<JB_Booking> GetAll(Expression<Func<JB_Booking, bool>> where)
        {
            return _unitOfWork.BookingRepo.TableNoTracking.Where(where);
        }

        public async Task<JB_Booking> GetByIdAsync(int Id)
        {
            return await _unitOfWork.BookingRepo.FindByIdAsync(Id);
        }

        public async Task<bool> IsDuplicateAsync(int Id, string Name)
        {
            int result = 0;
            if (Id > 0)
                result = await _unitOfWork.BookingRepo.TableNoTracking.Where(w =>
                w.IsDeleted == false).Select(s => s.Id).FirstOrDefaultAsync();
            else
                result = await _unitOfWork.BookingRepo.TableNoTracking.Where(w =>
                w.IsDeleted == false &&
                w.City.ToLower() == Name.ToLower()).Select(s => s.Id).FirstOrDefaultAsync();
            return result > 0 ? true : false;
        }

        public async Task<bool> UpdateStatusAsync(int Id, int UpdatedBy)
        {
            var entity = await GetByIdAsync(Id);
            if (entity == null) return false;
            entity.IsActive = !entity.IsActive;
            entity.UpdatedBy = UpdatedBy;
            entity.UpdatedDate = DateTime.UtcNow;
            _unitOfWork.BookingRepo.Update(entity);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<JB_Booking> UpsertAsync(JB_Booking model)
        {
            if (model.Id > 0)
                _unitOfWork.BookingRepo.Update(model);
            else
                await _unitOfWork.BookingRepo.AddAsync(model);
            await _unitOfWork.SaveAsync();
            return model;
        }
        public string GetBookingByName(int? Id)
        {
            if (Id > 0)
            {
                var data = _unitOfWork.BookingRepo.TableNoTracking.Where(w => w.Id == Id).Select(s => s.City).FirstOrDefault();
                return data == null ? "" : data;
            }
            else return "";

        }

    }
}

