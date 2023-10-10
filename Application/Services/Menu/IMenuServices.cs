using Domain.Entities;
using System.Linq.Expressions;

namespace Application.Services.Menu
{
    public interface IMenuServices
    {
        Task<JB_MasterMenu> GetByIdAsync(int Id);
        IQueryable<JB_MasterMenu> GetAll(Expression<Func<JB_MasterMenu, bool>> where);
        Task<JB_MasterMenu> UpsertAsync(JB_MasterMenu model);
        Task<bool> DeleteAsync(int Id, int UpdatedBy);
        Task<bool> UpdateStatusAsync(int Id, int UpdatedBy);

        //Task<IList<DropDownModel>> BindCountryDropDown(int SelectedId);
    }
}