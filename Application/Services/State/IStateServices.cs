using Domain.Entities;
using System.Linq.Expressions;

namespace Application.Services.State
{
    public interface IStateServices
    {
        Task<JB_MasterState> GetByIdAsync(int Id);
        IQueryable<JB_MasterState> GetAll(Expression<Func<JB_MasterState, bool>> where);
        Task<JB_MasterState> UpsertAsync(JB_MasterState model);
        Task<bool> IsDuplicateAsync(int Id, string Name);
        Task<bool> DeleteAsync(int Id, int UpdatedBy);
        Task<bool> UpdateStatusAsync(int Id, int UpdatedBy);

        //Task<IList<DropDownModel>> BindCountryDropDown(int SelectedId);
        string GetStateByName(int? Id);
    }
}
