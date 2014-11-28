using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;

namespace VirtualGuide.Models
{

    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer<ApplicationDbContext>(null);
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        
        public DbSet<Travel> Travels { get; set; }
        public DbSet<Excursion> Excursions { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<Place_Excursion> Place_Excursions { get; set; }
        public DbSet<User_Purchased_Travel> User_Purchased_Travels { get; set; }
        public DbSet<PlaceCategory> PlaceCategory { get; set; }
        public DbSet<Icon> Icon { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //cascade delete for Travel children
            modelBuilder.Entity<Travel>()
                .HasMany(i => i.Properties)
                .WithRequired()
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Travel>()
                .HasMany(i => i.Places)
                .WithRequired()
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Travel>()
                .HasMany(i => i.Excursions)
                .WithOptional()
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Place>()
                .HasOptional(i => i.Parent)
                .WithMany(i => i.Children)
                .HasForeignKey(i => i.ParentId);

            modelBuilder.Entity<User>()
                .HasMany(i => i.PurchasedTravels)
                .WithRequired()
                .WillCascadeOnDelete(false);            
        }


        public override int SaveChanges()
        {

            foreach (var travel in ChangeTracker.Entries<Travel>())
            {
                var currentUser = System.Threading.Thread.CurrentPrincipal.Identity.GetUserId();

                if (travel.State == EntityState.Added)
                {
                    travel.Entity.CreatorId = currentUser;
                }
            }


            //var user = this.Ide db.Users.Find(User.Identity.GetUserId());

            //Log time
            foreach (var auditableEntity in ChangeTracker.Entries<IAuditable>())
            {
                if (auditableEntity.State == EntityState.Added ||
                    auditableEntity.State == EntityState.Modified)
                {
                    // modify updated date and updated by column for 
                    // adds of updates.
                    auditableEntity.Entity.UpdatedAt = DateTime.Now;
                    //auditableEntity.Entity.UpdatedBy = currentUser;

                    // pupulate created date and created by columns for
                    // newly added record.
                    if (auditableEntity.State == EntityState.Added)
                    {
                        auditableEntity.Entity.CreatedAt = DateTime.Now;
                    }
                    else
                    {
                        // we also want to make sure that code is not inadvertly
                        // modifying created date and created by columns 
                        auditableEntity.Property(p => p.CreatedAt).IsModified = false;
                        //auditableEntity.Property(p => p.CreatedBy).IsModified = false;
                    }
                }
            }

            try
            {
                return base.SaveChanges();

            } catch (Exception e)
            {
                throw;
            }
        }


    }
}