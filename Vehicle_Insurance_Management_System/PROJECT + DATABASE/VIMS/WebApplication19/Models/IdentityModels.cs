using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using VIMS.Models;
using WebApplication19.Models;

namespace VIMS.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public DateTime DateOfJoining { get; set; }
        //public string PhoneNumber { get; set; }
        public ICollection<PolicyType> PolicyTypes { get; set; }
        public ICollection<VehicleInformation> VehicleInformations { get; set; }
        public ICollection<CustomerPolicyRecord> CustomerPolicyRecord { get; set; }
        public ICollection<Estimate> Estimates { get; set; }
        public ICollection<ClaimDetail> ClaimDetails { get; set; }
        public ICollection<Expense> Expenses { get; set; }



        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("Connection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Properties<DateTime>().Configure(c => c.HasColumnType("datetime2"));
        }

        public virtual DbSet<VehicleInformation> VehicleInformations { get; set; }
        public virtual DbSet<Estimate> Estimates { get; set; }
        public virtual DbSet<CustomerPolicyRecord> CustomerPolicyRecords { get; set; }
        public virtual DbSet<CustomerBillingInformation> CustomerBillingInformations { get; set; }
        public virtual DbSet<PolicyType> PolicyTypes { get; set; }
        public virtual DbSet<ClaimDetail> ClaimDetails { get; set; }
        public virtual DbSet<Expense> Expenses { get; set; }
        public virtual DbSet<VehicleImages> VehicleImages { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public IEnumerable ApplicationUsers { get; internal set; }
    }
}