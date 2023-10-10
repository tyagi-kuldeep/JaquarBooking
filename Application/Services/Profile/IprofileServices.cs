using Domain.Entities;
using System.Linq.Expressions;

namespace Application.Services.Profile
{
    public interface IProfileServices
    {
        Task<Domain.Entities.Users> GetByIdAsync(int Id);
        Task<Domain.Entities.Users> GetUserByIdAsync(int Id);
        IQueryable<Domain.Entities.Users> GetAllUsers(Expression<Func<Domain.Entities.Users, bool>> where);
        Task<Domain.Entities.Users> UpsertAsync(Domain.Entities.Users model);
        Task<string> ChangePasswordAsync(int UserId, string Password);
        Task<bool> IsDuplicateAsync(int Id, string Email);
        Task<string> DeleteAsync(int UserId, int UpdatedBy);
        Task<string> UpdateStatus(int UserId, int UpdatedBy);
        string GetUserNameById(int Id);
    }
}