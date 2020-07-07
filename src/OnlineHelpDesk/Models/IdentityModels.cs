using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace OnlineHelpDesk.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [StringLength(32)]
        public string UserIdentityCode { get; set; }

        [Required]
        [StringLength(255)]
        public string FullName { get; set; }

        public string Address { get; set; }

        [DataType(DataType.ImageUrl)]
        public string Avatar { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? CreatedAt { get; set; }

        public virtual ICollection<FacilityHead> FacilityHeads { get; set; }

        public virtual ICollection<Notification> Notifications { get; set; }

        public virtual ICollection<Report> Reports { get; set; }

        public virtual ICollection<Request> Requests { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            // Add custom user claims here
            // Initialize Collections
            FacilityHeads = new HashSet<FacilityHead>();
            Notifications = new HashSet<Notification>();
            Reports = new HashSet<Report>();
            Requests = new HashSet<Request>();

            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public virtual DbSet<Equipment> Equipments { get; set; }
        public virtual DbSet<EquipmentType> EquipmentTypes { get; set; }
        public virtual DbSet<Facility> Facilities { get; set; }
        public virtual DbSet<FacilityHead> FacilityHeads { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<Report> Reports { get; set; }
        public virtual DbSet<Request> Requests { get; set; }
        public virtual DbSet<RequestStatus> RequestStatus { get; set; }
        public virtual DbSet<RequestType> RequestTypes { get; set; }
        public virtual DbSet<StatusType> StatusTypes { get; set; }

        public ApplicationDbContext()
            : base("OhdDatabase", throwIfV1Schema: false)
        {
            Database.SetInitializer<ApplicationDbContext>(
                new CreateDatabaseIfNotExists<ApplicationDbContext>()
                );
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Equipment>()
                .HasMany(e => e.Requests)
                .WithOptional(e => e.Equipment)
                .HasForeignKey(e => e.EquipmentId);

            modelBuilder.Entity<EquipmentType>()
                .HasMany(e => e.Equipments)
                .WithOptional(e => e.EquipmentType)
                .HasForeignKey(e => e.ArtifactId);

            modelBuilder.Entity<Facility>()
                .HasMany(e => e.Equipments)
                .WithOptional(e => e.Facility)
                .HasForeignKey(e => e.FacilityId);

            modelBuilder.Entity<Facility>()
                .HasMany(e => e.FacilityHeads)
                .WithOptional(e => e.Facility)
                .HasForeignKey(e => e.FacilityId);

            modelBuilder.Entity<Request>()
                .HasMany(e => e.RequestStatus)
                .WithOptional(e => e.Request)
                .HasForeignKey(e => e.RequestId);

            modelBuilder.Entity<RequestType>()
                .HasMany(e => e.Requests)
                .WithOptional(e => e.RequestType)
                .HasForeignKey(e => e.RequestTypeId);

            modelBuilder.Entity<StatusType>()
                .HasMany(e => e.RequestStatus)
                .WithOptional(e => e.StatusType)
                .HasForeignKey(e => e.StatusTypeId);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.FacilityHeads)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.Notifications)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.Reports)
                .WithOptional(e => e.Creator)
                .HasForeignKey(e => e.CreatorId);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.Requests)
                .WithOptional(e => e.Petitioner)
                .HasForeignKey(e => e.PetitionerId);
        }

    }
}