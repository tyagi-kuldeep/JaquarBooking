using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Participant
{
    public interface IParticipantServices
    {
        Task<Domain.Entities.JB_Participant> GetByIdAsync(int Id);
        Task<Domain.Entities.JB_Participant> GetUserByIdAsync(int Id);
        IQueryable<Domain.Entities.JB_Participant> GetAll(Expression<Func<Domain.Entities.JB_Participant, bool>> where);
        Task<Domain.Entities.JB_Participant> UpsertAsync(Domain.Entities.JB_Participant model);       
        Task<bool> IsDuplicateAsync(int Id, string Email);
        Task<string> DeleteAsync(int UserId, int UpdatedBy);
        Task<string> UpdateStatus(int UserId, int UpdatedBy);
        string GetUserNameById(int Id);
    }
}
