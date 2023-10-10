using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastrucure.Persistence
{
    public class UnitOfWork:IUnitOfWork
    {
        #region Properties
        private readonly ApplicationDbContext _context;
        IDbContextTransaction dbContextTransaction;

        private IRepository<Users> _usersRepo;
        private IRepository<JB_MasterCountry> _countryRepo;
        private IRepository<JB_MasterState> _stateRepo;
        private IRepository<AuthRole> _roleRepo;
        private IRepository<JB_MasterMenu> _menuRepo;
        private IRepository<JB_Booking> _bookingRepo;
        private IRepository<JB_Participant> _participantRepo;

        public IRepository<Users> UsersRepo
        {
            get
            {
                if (this._usersRepo == null)
                    this._usersRepo = new EfRepository<Users>(_context);
                return _usersRepo;
            }
        }
        public IRepository<AuthRole> RoleRepo
        {
            get
            {
                if (this._roleRepo == null)
                    this._roleRepo = new EfRepository<AuthRole>(_context);
                return _roleRepo;
            }
        }
        public IRepository<JB_MasterCountry> CountryRepo
        {
            get
            {
                if (this._countryRepo == null)
                    this._countryRepo = new EfRepository<JB_MasterCountry>(_context);
                return _countryRepo;
            }
        }
        public IRepository<JB_MasterState> StateRepo
        {
            get
            {
                if (this._stateRepo == null)
                    this._stateRepo = new EfRepository<JB_MasterState>(_context);
                return _stateRepo;
            }
        }
        public IRepository<JB_MasterMenu> MenuRepo
        {
            get
            {
                if (this._menuRepo == null)
                    this._menuRepo = new EfRepository<JB_MasterMenu>(_context);
                return _menuRepo;
            }
        }

        public IRepository<JB_Booking> BookingRepo
        {
            get
            {
                if (this._bookingRepo == null)
                    this._bookingRepo = new EfRepository<JB_Booking>(_context);
                return _bookingRepo;
            }
        }
        public IRepository<JB_Participant> ParticipantRepo
        {
            get
            {
                if (this._participantRepo == null)
                    this._participantRepo = new EfRepository<JB_Participant>(_context);
                return _participantRepo;
            }
        }
        #endregion
        #region Ctor
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion
        #region Methods
        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public int Save()
        {
            return _context.SaveChanges();
        }
        public void BeginTransaction()
        {
            dbContextTransaction = _context.Database.BeginTransaction();
        }
        public void CommitTransaction()
        {
            if (dbContextTransaction != null)
            {
                dbContextTransaction.Commit();
            }
        }
        public void RollbackTransaction()
        {
            if (dbContextTransaction != null)
            {
                dbContextTransaction.Rollback();
            }
        }
        private bool disposed = false;
        //private object _participantRepo;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
