using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Role
{
    public class RoleServices : IRoleServices
    {
        private readonly IUnitOfWork _unitOfWork;
        public RoleServices(IUnitOfWork unitOfWork)
        {            
            _unitOfWork = unitOfWork;
        }
    
        public object DeleteRightsByRoleId(int RoleId)
        {
            throw new NotImplementedException();
        }

        public IQueryable<AuthRole> GetAllRoles(Expression<Func<AuthRole, bool>> where)
        {
            return _unitOfWork.RoleRepo.TableNoTracking.Where(where);
        }

        public async Task<AuthRole> GetByIdAsync(int Id)
        {
            return await _unitOfWork.RoleRepo.FindByIdAsync(Id);
        }

        public DataSet GetRightsByRoleId(int RoleId)
        {
            throw new NotImplementedException();
        }

        public DataSet GetRoleByUserId(int UserId, int LoggedInUserId)
        {
            throw new NotImplementedException();
        }

        public int GetRoleCountByUserId(int UserId)
        {
            throw new NotImplementedException();
        }

        public object UpdateLdapRole(int UserId, string role)
        {
            throw new NotImplementedException();
        }

        public object UpdateRightsByRoleId(List<string> selectedrights, int RoleId)
        {
            throw new NotImplementedException();
        }

        public object UpdateRoleByUserId(int UserId, List<string> selectedroles)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> IsDuplicateAsync(int Id, string Name)
        {
            int result = 0;
            if (Id > 0)
                result = await _unitOfWork.RoleRepo.TableNoTracking.Where(w=>w.Id != Id && w.Name.ToLower() == Name.ToLower()).Select(s => s.Id).FirstOrDefaultAsync();
            else
                result = await _unitOfWork.RoleRepo.TableNoTracking.Where(w =>w.Name.ToLower() == Name.ToLower()).Select(s => s.Id).FirstOrDefaultAsync();
            return result > 0 ? true : false;
        }
        public async Task<AuthRole> UpsertAsync(AuthRole model)
        {
            if (model.Id > 0)
                _unitOfWork.RoleRepo.Update(model);
            else
                await _unitOfWork.RoleRepo.AddAsync(model);
            await _unitOfWork.SaveAsync();
            return model;
        }
    }
}
