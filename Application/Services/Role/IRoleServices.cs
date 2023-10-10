using Domain.Entities;
using System.Data;
using System.Linq.Expressions;

namespace Application.Services.Role
{
    public interface IRoleServices 
    {
        Task<AuthRole> GetByIdAsync(int Id);
        IQueryable<AuthRole> GetAllRoles(Expression<Func<AuthRole, bool>> where);
        DataSet GetRoleByUserId(int UserId, int LoggedInUserId);
        object UpdateRoleByUserId(int UserId, List<string> selectedroles);
        DataSet GetRightsByRoleId(int RoleId);
        object DeleteRightsByRoleId(int RoleId);
        object UpdateRightsByRoleId(List<string> selectedrights, int RoleId);
        object UpdateLdapRole(int UserId, string role);
        //Task<IList<DropDownModel>> BindRolesDropDown(int SelectedId);
        Task<AuthRole> UpsertAsync(AuthRole model);
        Task<bool> IsDuplicateAsync(int Id, string Name);
        int GetRoleCountByUserId(int UserId);
    }
}