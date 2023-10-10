using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Services.Booking
{
    public interface IBookingServices
    {
        Task<JB_Booking> GetByIdAsync(int Id);
        IQueryable<JB_Booking> GetAll(Expression<Func<JB_Booking, bool>> where);
        Task<JB_Booking> UpsertAsync(JB_Booking model);
        Task<bool> IsDuplicateAsync(int Id, string Name);
        Task<bool> DeleteAsync(int Id, int UpdatedBy);
        Task<bool> UpdateStatusAsync(int Id, int UpdatedBy);
        //Task<IList<DropDownModel>> BindCountryDropDown(int SelectedId);
    }
}
