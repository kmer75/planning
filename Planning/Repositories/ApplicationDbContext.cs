using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Repositories.Model;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Repositories.Repositories
{

    public class ApplicationDbContext : IdentityDbContext<User, ApplicationRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            #region User Friend M-M relationship
            /*
            builder.Entity<UserFriend>()
                .HasKey(b => new { b.RequestorId, b.ResponderId });

            builder.Entity<UserFriend>()
                .HasOne(bc => bc.Requestor)
                .WithMany(b => b.UserFriends)
                .HasForeignKey(bc => bc.RequestorId)
                .OnDelete(DeleteBehavior.Restrict);
            */
            #endregion

            #region User Group M-M relationship

            /*
            builder.Entity<UserGroup>()
                .HasKey(p => new { p.GroupId, p.UserId });

            builder.Entity<UserGroup>()
                .HasOne(pc => pc.User)
                .WithMany(p => p.UserGroups)
                .HasForeignKey(pc => pc.UserId);

            builder.Entity<UserGroup>()
                .HasOne(pc => pc.Group)
                .WithMany(p => p.UserGroups)
                .HasForeignKey(pc => pc.GroupId);
                */

            #endregion

            base.OnModelCreating(builder);
        }

        public DbSet<User> Users { get; set; }
        
        #region override SaveChanges Methods

        public override int SaveChanges()
        {
            AddDateTimes();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            AddDateTimes();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            AddDateTimes();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        #endregion

        /// <summary>
        /// save created date and modified date for all entities that implement IModelDefaultProperties 
        /// </summary>
        private void AddDateTimes()
        {
            var entities = ChangeTracker.Entries().Where(x => x.Entity is IModelDefaultProperties && 
            (x.State == EntityState.Added || x.State == EntityState.Modified || x.State == EntityState.Deleted));

            foreach (var entity in entities)
            {
                if (entity.State == EntityState.Added)
                {
                    ((IModelDefaultProperties)entity.Entity).Created = DateTime.Now;
                }

                ((IModelDefaultProperties)entity.Entity).Modified = DateTime.Now;
            }
        }
    }

}
