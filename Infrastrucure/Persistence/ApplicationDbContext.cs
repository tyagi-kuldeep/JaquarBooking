using Application.Common.Interfaces;
using Domain.Common;
using Domain.Entities;
using Infrastrucure.Persistence.Seed;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastrucure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<Users, AuthRole, int>, IApplicationDbContext
    {
        #region Ctor
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        #endregion
        #region DbSet
        public DbSet<JB_MasterCountry> JB_MasterCountry {get; set;}
        #endregion
        #region Methods
        public Task<int> SaveChangesAsync()
        {
            foreach (var entry in ChangeTracker.Entries<IAuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedOnUtc = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        //entry.Entity.UpdatedBy = 1; //Get Current UsereID
                        entry.Entity.UpdatedOnUtc = DateTime.UtcNow;
                        break;
                }
            }
            return base.SaveChangesAsync();
        }

        /// <summary>
        /// Creates a DbSet that can be used to query and save instances of entity
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <returns>A set for the given entity type</returns>
        //public virtual new DbSet<TEntity> Set<TEntity>() where TEntity : class
        //{
        //    return base.Set<TEntity>();
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>(i =>
            {
                i.ToTable("AuthUsers");
                i.HasKey(x => x.Id);
            });
            modelBuilder.Entity<AuthRole>(i =>
            {
                i.ToTable("AuthRole");
                i.HasKey(x => x.Id);
            });
            modelBuilder.Entity<IdentityUserRole<int>>(i =>
            {
                i.ToTable("AuthUserRoles");
                i.HasKey(x => new { x.RoleId, x.UserId });
            });
            modelBuilder.Entity<IdentityUserLogin<int>>(i =>
            {
                i.ToTable("AuthUserLogins");
                i.HasKey(x => new { x.ProviderKey, x.LoginProvider });
            });
            modelBuilder.Entity<IdentityRoleClaim<int>>(i =>
            {
                i.ToTable("AuthRoleClaims");
                i.HasKey(x => x.Id);
            });
            modelBuilder.Entity<IdentityUserClaim<int>>(i =>
            {
                i.ToTable("AuthUserClaims");
                i.HasKey(x => x.Id);
            });
            modelBuilder.Entity<IdentityUserToken<int>>(i =>
            {
                i.ToTable("AuthUserToken");
                i.HasKey(x => new { x.UserId, x.LoginProvider, x.Name });
            });
            SeedInitialData.SuperAdmin(modelBuilder);
            SeedInitialData.Roles(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
        #endregion

    }
}
