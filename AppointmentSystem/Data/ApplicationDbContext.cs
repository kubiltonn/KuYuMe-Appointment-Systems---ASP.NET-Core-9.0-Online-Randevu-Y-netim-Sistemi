using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using AppointmentSystem.Models;

namespace AppointmentSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Service> Services { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Role IDs
            const string AdminRoleId = "1";
            const string EmployeeRoleId = "2";
            const string ClientRoleId = "3";

            // User ID
            const string AdminUserId = "1";

            // Seed Roles
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = AdminRoleId, Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = EmployeeRoleId, Name = "Employee", NormalizedName = "EMPLOYEE" },
                new IdentityRole { Id = ClientRoleId, Name = "Client", NormalizedName = "CLIENT" }
            );

            // Seed Admin User
            var adminUser = new ApplicationUser
            {
                Id = AdminUserId,
                UserName = "admin@example.com",
                Email = "admin@example.com",
                EmailConfirmed = true,
                FullName = "Admin User",
                Name = "Admin",
                Surname = "User",
                PhoneNumber = "555-0000",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                SecurityStamp = "00000000-0000-0000-0000-000000000000",
                PasswordHash = "AQAAAAEAACcQAAAAEJ8Qw8Qw8Qw8Qw8Qw8Qw8Qw8Qw8Qw8Qw8Qw8Qw8Qw8Qw8Qw8Qw8Qw8Qw8Qw=="
            };

            builder.Entity<ApplicationUser>().HasData(adminUser);

            // Seed Admin Role Assignment
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string> { UserId = AdminUserId, RoleId = AdminRoleId }
            );

            // Seed Ana Hizmetler
            var kuaförId = 1;
            var doktorId = 2;
            var danışmanlıkId = 3;
            var araçHizmetiId = 4;
            var webSiteId = 5;
            var psikologId = 6;
            var sorunBildirId = 7;

            builder.Entity<Service>().HasData(
                // Ana Hizmetler
                new Service { Id = kuaförId, Name = "Kuaför", Description = "Saç ve güzellik hizmetleri", IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Service { Id = doktorId, Name = "Doktor", Description = "Sağlık hizmetleri", IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Service { Id = danışmanlıkId, Name = "Danışmanlık", Description = "Profesyonel danışmanlık hizmetleri", IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Service { Id = araçHizmetiId, Name = "Araç Hizmeti", Description = "Araç bakım ve onarım hizmetleri", IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Service { Id = webSiteId, Name = "Web Site", Description = "Web tasarım ve geliştirme hizmetleri", IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Service { Id = psikologId, Name = "Psikolog", Description = "Psikolojik danışmanlık hizmetleri", IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Service { Id = sorunBildirId, Name = "Sorun Bildir", Description = "Teknik destek ve sorun bildirme hizmetleri", IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },

                // Kuaför Alt Hizmetleri
                new Service { Id = 8, Name = "Saç Kesimi", Description = "Profesyonel saç kesimi hizmeti", Duration = 30, Price = 100, IsActive = true, ParentServiceId = kuaförId, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Service { Id = 9, Name = "Saç Boyama", Description = "Profesyonel saç boyama hizmeti", Duration = 120, Price = 300, IsActive = true, ParentServiceId = kuaförId, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Service { Id = 10, Name = "Saç Bakımı", Description = "Saç bakım ve onarım hizmeti", Duration = 60, Price = 150, IsActive = true, ParentServiceId = kuaförId, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Service { Id = 11, Name = "Sakal Tıraşı", Description = "Profesyonel sakal tıraşı hizmeti", Duration = 30, Price = 80, IsActive = true, ParentServiceId = kuaförId, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },

                // Doktor Alt Hizmetleri
                new Service { Id = 12, Name = "Genel Muayene", Description = "Genel sağlık kontrolü", Duration = 30, Price = 200, IsActive = true, ParentServiceId = doktorId, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Service { Id = 13, Name = "Kontrol Muayenesi", Description = "Düzenli kontrol muayenesi", Duration = 20, Price = 150, IsActive = true, ParentServiceId = doktorId, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Service { Id = 14, Name = "Acil Muayene", Description = "Acil durum muayenesi", Duration = 45, Price = 300, IsActive = true, ParentServiceId = doktorId, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },

                // Danışmanlık Alt Hizmetleri
                new Service { Id = 15, Name = "Kariyer Danışmanlığı", Description = "Kariyer planlama ve geliştirme danışmanlığı", Duration = 60, Price = 250, IsActive = true, ParentServiceId = danışmanlıkId, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Service { Id = 16, Name = "Eğitim Danışmanlığı", Description = "Eğitim ve öğrenci danışmanlığı", Duration = 45, Price = 200, IsActive = true, ParentServiceId = danışmanlıkId, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Service { Id = 17, Name = "İş Danışmanlığı", Description = "İş ve yönetim danışmanlığı", Duration = 90, Price = 400, IsActive = true, ParentServiceId = danışmanlıkId, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },

                // Araç Hizmeti Alt Hizmetleri
                new Service { Id = 18, Name = "Periyodik Bakım", Description = "Araç periyodik bakım hizmeti", Duration = 180, Price = 500, IsActive = true, ParentServiceId = araçHizmetiId, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Service { Id = 19, Name = "Yağ Değişimi", Description = "Motor yağı değişim hizmeti", Duration = 60, Price = 200, IsActive = true, ParentServiceId = araçHizmetiId, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Service { Id = 20, Name = "Lastik Değişimi", Description = "Lastik değişim ve balans hizmeti", Duration = 90, Price = 300, IsActive = true, ParentServiceId = araçHizmetiId, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },

                // Web Site Alt Hizmetleri
                new Service { Id = 21, Name = "Web Tasarım", Description = "Profesyonel web site tasarımı", Duration = 1440, Price = 5000, IsActive = true, ParentServiceId = webSiteId, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Service { Id = 22, Name = "Web Geliştirme", Description = "Web uygulaması geliştirme", Duration = 2880, Price = 10000, IsActive = true, ParentServiceId = webSiteId, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Service { Id = 23, Name = "SEO Optimizasyonu", Description = "Arama motoru optimizasyonu", Duration = 720, Price = 2000, IsActive = true, ParentServiceId = webSiteId, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },

                // Psikolog Alt Hizmetleri
                new Service { Id = 24, Name = "Bireysel Terapi", Description = "Bireysel psikolojik danışmanlık", Duration = 50, Price = 300, IsActive = true, ParentServiceId = psikologId, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Service { Id = 25, Name = "Çift Terapisi", Description = "Çift terapisi ve danışmanlık", Duration = 90, Price = 500, IsActive = true, ParentServiceId = psikologId, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Service { Id = 26, Name = "Aile Terapisi", Description = "Aile terapisi ve danışmanlık", Duration = 90, Price = 500, IsActive = true, ParentServiceId = psikologId, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },

                // Sorun Bildir Alt Hizmetleri
                new Service { Id = 27, Name = "Teknik Destek", Description = "Teknik sorunlar için destek", Duration = 30, Price = 100, IsActive = true, ParentServiceId = sorunBildirId, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Service { Id = 28, Name = "Hata Bildirimi", Description = "Sistem hatalarını bildirme", Duration = 15, Price = 0, IsActive = true, ParentServiceId = sorunBildirId, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Service { Id = 29, Name = "Öneri Bildirimi", Description = "Sistem geliştirme önerileri", Duration = 15, Price = 0, IsActive = true, ParentServiceId = sorunBildirId, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
            );

            // Appointment configurations
            builder.Entity<Appointment>()
                .HasOne(a => a.Customer)
                .WithMany(u => u.Appointments)
                .HasForeignKey(a => a.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Appointment>()
                .HasOne(a => a.Employee)
                .WithMany(e => e.Appointments)
                .HasForeignKey(a => a.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Appointment>()
                .HasOne(a => a.Service)
                .WithMany(s => s.Appointments)
                .HasForeignKey(a => a.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            // Employee configurations
            builder.Entity<Employee>()
                .HasOne(e => e.User)
                .WithOne(u => u.Employee)
                .HasForeignKey<Employee>(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Service-Employee many-to-many relationship
            builder.Entity<Employee>()
                .HasMany(e => e.Services)
                .WithMany(s => s.Employees)
                .UsingEntity(j => j.ToTable("EmployeeServices"));

            // Notification configurations
            builder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Notification>()
                .HasOne(n => n.Appointment)
                .WithMany()
                .HasForeignKey(n => n.AppointmentId)
                .OnDelete(DeleteBehavior.SetNull);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
            base.OnConfiguring(optionsBuilder);
        }

        public bool TestDatabaseConnection()
        {
            try
            {
                return Database.CanConnect();
            }
            catch
            {
                return false;
            }
        }
    }
} 