using Application.Common.Interfaces;
using Application.Services.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Profile
{
    public class ProfileServices : IProfileServices
    {
        #region Properties
        private readonly IUnitOfWork _unitOfWork;
        #endregion
        #region Ctor
        public ProfileServices(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        #endregion
        public async Task<Domain.Entities.Users> GetUserByIdAsync(int Id)
        {
            return await _unitOfWork.UsersRepo.FindByIdAsync(Id);
        }
        public async Task<string> ChangePasswordAsync(int UserId, string Password)
        {
            var user = await GetUserByIdAsync(UserId);
            if (user == null) return "User not found.";
            try
            {
                var hasher = new PasswordHasher<Domain.Entities.Users>();
                user.PasswordHash = hasher.HashPassword(user, Password);
                _unitOfWork.UsersRepo.Update(user);
                await _unitOfWork.SaveAsync();
                return "success";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public async Task<Domain.Entities.Users> UpsertAsync(Domain.Entities.Users model)
        {
            if (model.Id > 0)
                _unitOfWork.UsersRepo.Update(model);
            else
                await _unitOfWork.UsersRepo.AddAsync(model);

            await _unitOfWork.SaveAsync();
            return model;
        }

        public IQueryable<Domain.Entities.Users> GetAllUsers(Expression<Func<Domain.Entities.Users, bool>> where)
        {
            return _unitOfWork.UsersRepo.TableNoTracking.Where(where);
        }

        public async Task<string> UpdateStatus(int UserId, int UpdatedBy)
        {
            var user = await GetUserByIdAsync(UserId);
            if (user == null) return "User not found.";
            user.IsActive = !user.IsActive;
            user.UpdatedBy = UpdatedBy;
            user.UpdatedOnUtc = DateTime.UtcNow;
            _unitOfWork.UsersRepo.Update(user);
            await _unitOfWork.SaveAsync();
            return "success";
        }
        public async Task<bool> IsDuplicateAsync(int Id, string Email)
        {
            int result = 0;
            if (Id > 0)
                result = await _unitOfWork.UsersRepo.TableNoTracking.Where(w =>
                w.IsDeleted == false &&
                w.Id != Id && w.Email.ToLower() == Email.ToLower()).Select(s => s.Id).FirstOrDefaultAsync();
            else
                result = await _unitOfWork.UsersRepo.TableNoTracking.Where(w =>
                w.IsDeleted == false &&
                w.Email.ToLower() == Email.ToLower()).Select(s => s.Id).FirstOrDefaultAsync();
            return result > 0 ? true : false;
        }

        public async Task<string> DeleteAsync(int UserId, int UpdatedBy)
        {
            var user = await GetUserByIdAsync(UserId);
            if (user == null) return "User not found.";
            user.IsDeleted = true;
            user.UpdatedBy = UpdatedBy;
            user.UpdatedOnUtc = DateTime.UtcNow;
            _unitOfWork.UsersRepo.Update(user);
            await _unitOfWork.SaveAsync();
            return "success";
        }

        public async Task<Domain.Entities.Users> GetByIdAsync(int Id)
        {
            return await _unitOfWork.UsersRepo.FindByIdAsync(Id);
        }

        public string GetUserNameById(int Id)
        {
            var result = _unitOfWork.UsersRepo.TableNoTracking.Where(w => w.Id == Id).Select(s => s.FirstName + " " + s.MiddleName + " " + s.LastName).FirstOrDefault();
            return result ?? "";
        }
    }
}
