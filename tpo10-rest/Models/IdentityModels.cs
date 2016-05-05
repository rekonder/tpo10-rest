﻿using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Data.Entity;

namespace tpo10_rest.Models
{
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            return userIdentity;
        }

        public DateTime? LastLogin { get; set; }
        public string LastLoginIp { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("tpo10db", throwIfV1Schema: false)
        {
        }
        
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public virtual DbSet<Profile> Profiles { get; set; }
        public virtual DbSet<PatientProfileContact> PatientProfileContacts { get; set; }
        public virtual DbSet<HealthCareProvider> HealthCareProviders { get; set; }
        public virtual DbSet<Post> Posts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<PatientProfile>()
                .HasRequired(e => e.Post)
                .WithMany()
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<PatientProfileContact>()
                .HasRequired(e => e.Post)
                .WithMany()
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<DoctorProfile>()
               .HasRequired(e => e.HealthCareProvider)
               .WithMany()
               .WillCascadeOnDelete(false);
            modelBuilder.Entity<NurseProfile>()
               .HasRequired(e => e.HealthCareProvider)
               .WithMany()
               .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Doctor>()
            //    .HasOptional(e => e.DoctorProfile)
            //    .WithOptionalPrincipal(e => e.Doctor);

            //modelBuilder.Entity<Nurse>()
            //    .HasOptional(e => e.NurseProfile)
            //    .WithOptionalPrincipal(e => e.Nurse);

            //modelBuilder.Entity<PatientProfile>()
            //    .HasRequired(e => e.PatientProfileContact)
            //    .WithRequiredPrincipal(e => e.PatientProfile);
        }
    }
}