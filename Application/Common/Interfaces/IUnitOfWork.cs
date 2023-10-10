using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IUnitOfWork
    {
        #region Properties
        IRepository<Users> UsersRepo { get; }
        IRepository<JB_MasterCountry> CountryRepo { get; }
        IRepository<JB_MasterState> StateRepo { get; }
        IRepository<AuthRole> RoleRepo { get; }
        IRepository<JB_MasterMenu>MenuRepo { get; }
        IRepository<JB_Booking> BookingRepo { get; }
        IRepository<JB_Participant> ParticipantRepo { get; }
        #endregion
        #region Methods
        Task<int> SaveAsync();
        int Save();
        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
        #endregion
    }
}
