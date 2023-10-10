using Domain.Entities;
using System.Linq.Expressions;

namespace Application.Services.Country
{
    public interface ICountryServices
    {
        Task<JB_MasterCountry> GetByIdAsync(int Id);
        IQueryable<JB_MasterCountry> GetAll(Expression<Func<JB_MasterCountry, bool>> where);
        Task<JB_MasterCountry> UpsertAsync(JB_MasterCountry model);
        Task<bool> IsDuplicateAsync(int Id, string Name);
        Task<bool> DeleteAsync(int Id, int UpdatedBy);
        Task<bool> UpdateStatusAsync(int Id, int UpdatedBy);

        string GetCountryByName(int? Id);

        //Task<IList<DropDownModel>> BindCountryDropDown(int SelectedId);
    }
}