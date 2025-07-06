using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RandevuSistemi.Models;

namespace RandevuSistemi.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<AppointmentType> AppointmentTypes { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // AppointmentType - Appointment ilişkisi
            builder.Entity<Appointment>()
                .HasOne(a => a.AppointmentType)
                .WithMany(at => at.Appointments)
                .HasForeignKey(a => a.AppointmentTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Service - Appointment ilişkisi (Ana hizmet)
            builder.Entity<Appointment>()
                .HasOne(a => a.ServiceType)
                .WithMany()
                .HasForeignKey(a => a.ServiceTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Service - Appointment ilişkisi (Alt hizmet)
            builder.Entity<Appointment>()
                .HasOne(a => a.SubServiceType)
                .WithMany()
                .HasForeignKey(a => a.SubServiceTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Employee - Appointment ilişkisi
            builder.Entity<Appointment>()
                .HasOne(a => a.Employee)
                .WithMany(e => e.Appointments)
                .HasForeignKey(a => a.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Service - Service ilişkisi (Ana hizmet - Alt hizmet)
            builder.Entity<Service>()
                .HasOne(s => s.ParentService)
                .WithMany(s => s.SubServices)
                .HasForeignKey(s => s.ParentServiceId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
} 