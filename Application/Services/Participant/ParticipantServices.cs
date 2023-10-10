using Application.Common.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Participant
{
    public class ParticipantServices : IParticipantServices
    {
        #region Properties
        private readonly IUnitOfWork _unitOfWork;
        #endregion
        #region Ctor
        public ParticipantServices(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        #endregion
        public async Task<Domain.Entities.JB_Participant> GetUserByIdAsync(int Id)
        {
            return await _unitOfWork.ParticipantRepo.FindByIdAsync(Id);
        }

        public async Task<Domain.Entities.JB_Participant> UpsertAsync(Domain.Entities.JB_Participant model)
        {
            try
            {
                if (model.Id > 0)
                    _unitOfWork.ParticipantRepo.Update(model);
                else
                    await _unitOfWork.ParticipantRepo.AddAsync(model);

                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {

            }
            return model;
        }

        public IQueryable<Domain.Entities.JB_Participant> GetAll(Expression<Func<Domain.Entities.JB_Participant, bool>> where)
        {
            return _unitOfWork.ParticipantRepo.TableNoTracking.Where(where);
        }

        public async Task<string> UpdateStatus(int UserId, int UpdatedBy)
        {
            var user = await GetUserByIdAsync(UserId);
            if (user == null) return "User not found.";
            user.IsActive = !user.IsActive;
            user.UpdatedBy = UpdatedBy;
            //user.UpdatedOnUtc = DateTime.UtcNow;
            _unitOfWork.ParticipantRepo.Update(user);
            await _unitOfWork.SaveAsync();
            return "success";
        }
        public async Task<bool> IsDuplicateAsync(int Id, string Email)
        {
            int result = 0;
            if (Id > 0)
                result = await _unitOfWork.ParticipantRepo.TableNoTracking.Where(w =>
                w.IsDeleted == false &&
                w.Id != Id && w.Email.ToLower() == Email.ToLower()).Select(s => s.Id).FirstOrDefaultAsync();
            else
                result = await _unitOfWork.ParticipantRepo.TableNoTracking.Where(w =>
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
            //user.UpdatedOnUtc = DateTime.UtcNow;
            _unitOfWork.ParticipantRepo.Update(user);
            await _unitOfWork.SaveAsync();
            return "success";
        }

        public async Task<Domain.Entities.JB_Participant> GetByIdAsync(int Id)
        {
            return await _unitOfWork.ParticipantRepo.FindByIdAsync(Id);
        }

        public string GetUserNameById(int Id)
        {
            var result = _unitOfWork.ParticipantRepo.TableNoTracking.Where(w => w.Id == Id).Select(s => s.Name + " " + s.Company + " ").FirstOrDefault();
            return result ?? "";
        }
    }
}
