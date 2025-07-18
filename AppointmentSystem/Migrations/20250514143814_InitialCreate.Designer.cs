﻿// <auto-generated />
using System;
using AppointmentSystem.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AppointmentSystem.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250514143814_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AppointmentSystem.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("Region")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Specialization")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("WorkingHours")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "1",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "fb88eabf-f324-4786-97d7-38943219906c",
                            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Email = "admin@example.com",
                            EmailConfirmed = true,
                            FullName = "Admin User",
                            LockoutEnabled = false,
                            Name = "Admin",
                            PasswordHash = "AQAAAAEAACcQAAAAEJ8Qw8Qw8Qw8Qw8Qw8Qw8Qw8Qw8Qw8Qw8Qw8Qw8Qw8Qw8Qw8Qw8Qw8Qw8Qw==",
                            PhoneNumber = "555-0000",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "00000000-0000-0000-0000-000000000000",
                            Surname = "User",
                            TwoFactorEnabled = false,
                            UserName = "admin@example.com"
                        });
                });

            modelBuilder.Entity("AppointmentSystem.Models.Appointment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("AppointmentDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CustomerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<int>("ServiceId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("ServiceId");

                    b.ToTable("Appointments");
                });

            modelBuilder.Entity("AppointmentSystem.Models.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Expertise")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("WorkingHours")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("AppointmentSystem.Models.Notification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("AppointmentId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsRead")
                        .HasColumnType("bit");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ReadAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("AppointmentId");

                    b.HasIndex("UserId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("AppointmentSystem.Models.Service", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Duration")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ParentServiceId")
                        .HasColumnType("int");

                    b.Property<decimal?>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Services");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Description = "Saç ve güzellik hizmetleri",
                            IsActive = true,
                            Name = "Kuaför"
                        },
                        new
                        {
                            Id = 2,
                            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Description = "Sağlık hizmetleri",
                            IsActive = true,
                            Name = "Doktor"
                        },
                        new
                        {
                            Id = 3,
                            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Description = "Profesyonel danışmanlık hizmetleri",
                            IsActive = true,
                            Name = "Danışmanlık"
                        },
                        new
                        {
                            Id = 4,
                            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Description = "Araç bakım ve onarım hizmetleri",
                            IsActive = true,
                            Name = "Araç Hizmeti"
                        },
                        new
                        {
                            Id = 5,
                            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Description = "Web tasarım ve geliştirme hizmetleri",
                            IsActive = true,
                            Name = "Web Site"
                        },
                        new
                        {
                            Id = 6,
                            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Description = "Psikolojik danışmanlık hizmetleri",
                            IsActive = true,
                            Name = "Psikolog"
                        },
                        new
                        {
                            Id = 7,
                            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Description = "Teknik destek ve sorun bildirme hizmetleri",
                            IsActive = true,
                            Name = "Sorun Bildir"
                        },
                        new
                        {
                            Id = 8,
                            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Description = "Profesyonel saç kesimi hizmeti",
                            Duration = 30,
                            IsActive = true,
                            Name = "Saç Kesimi",
                            ParentServiceId = 1,
                            Price = 100m
                        },
                        new
                        {
                            Id = 9,
                            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Description = "Profesyonel saç boyama hizmeti",
                            Duration = 120,
                            IsActive = true,
                            Name = "Saç Boyama",
                            ParentServiceId = 1,
                            Price = 300m
                        },
                        new
                        {
                            Id = 10,
                            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Description = "Saç bakım ve onarım hizmeti",
                            Duration = 60,
                            IsActive = true,
                            Name = "Saç Bakımı",
                            ParentServiceId = 1,
                            Price = 150m
                        },
                        new
                        {
                            Id = 11,
                            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Description = "Profesyonel sakal tıraşı hizmeti",
                            Duration = 30,
                            IsActive = true,
                            Name = "Sakal Tıraşı",
                            ParentServiceId = 1,
                            Price = 80m
                        },
                        new
                        {
                            Id = 12,
                            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Description = "Genel sağlık kontrolü",
                            Duration = 30,
                            IsActive = true,
                            Name = "Genel Muayene",
                            ParentServiceId = 2,
                            Price = 200m
                        },
                        new
                        {
                            Id = 13,
                            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Description = "Düzenli kontrol muayenesi",
                            Duration = 20,
                            IsActive = true,
                            Name = "Kontrol Muayenesi",
                            ParentServiceId = 2,
                            Price = 150m
                        },
                        new
                        {
                            Id = 14,
                            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Description = "Acil durum muayenesi",
                            Duration = 45,
                            IsActive = true,
                            Name = "Acil Muayene",
                            ParentServiceId = 2,
                            Price = 300m
                        },
                        new
                        {
                            Id = 15,
                            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Description = "Kariyer planlama ve geliştirme danışmanlığı",
                            Duration = 60,
                            IsActive = true,
                            Name = "Kariyer Danışmanlığı",
                            ParentServiceId = 3,
                            Price = 250m
                        },
                        new
                        {
                            Id = 16,
                            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Description = "Eğitim ve öğrenci danışmanlığı",
                            Duration = 45,
                            IsActive = true,
                            Name = "Eğitim Danışmanlığı",
                            ParentServiceId = 3,
                            Price = 200m
                        },
                        new
                        {
                            Id = 17,
                            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Description = "İş ve yönetim danışmanlığı",
                            Duration = 90,
                            IsActive = true,
                            Name = "İş Danışmanlığı",
                            ParentServiceId = 3,
                            Price = 400m
                        },
                        new
                        {
                            Id = 18,
                            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Description = "Araç periyodik bakım hizmeti",
                            Duration = 180,
                            IsActive = true,
                            Name = "Periyodik Bakım",
                            ParentServiceId = 4,
                            Price = 500m
                        },
                        new
                        {
                            Id = 19,
                            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Description = "Motor yağı değişim hizmeti",
                            Duration = 60,
                            IsActive = true,
                            Name = "Yağ Değişimi",
                            ParentServiceId = 4,
                            Price = 200m
                        },
                        new
                        {
                            Id = 20,
                            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Description = "Lastik değişim ve balans hizmeti",
                            Duration = 90,
                            IsActive = true,
                            Name = "Lastik Değişimi",
                            ParentServiceId = 4,
                            Price = 300m
                        },
                        new
                        {
                            Id = 21,
                            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Description = "Profesyonel web site tasarımı",
                            Duration = 1440,
                            IsActive = true,
                            Name = "Web Tasarım",
                            ParentServiceId = 5,
                            Price = 5000m
                        },
                        new
                        {
                            Id = 22,
                            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Description = "Web uygulaması geliştirme",
                            Duration = 2880,
                            IsActive = true,
                            Name = "Web Geliştirme",
                            ParentServiceId = 5,
                            Price = 10000m
                        },
                        new
                        {
                            Id = 23,
                            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Description = "Arama motoru optimizasyonu",
                            Duration = 720,
                            IsActive = true,
                            Name = "SEO Optimizasyonu",
                            ParentServiceId = 5,
                            Price = 2000m
                        },
                        new
                        {
                            Id = 24,
                            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Description = "Bireysel psikolojik danışmanlık",
                            Duration = 50,
                            IsActive = true,
                            Name = "Bireysel Terapi",
                            ParentServiceId = 6,
                            Price = 300m
                        },
                        new
                        {
                            Id = 25,
                            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Description = "Çift terapisi ve danışmanlık",
                            Duration = 90,
                            IsActive = true,
                            Name = "Çift Terapisi",
                            ParentServiceId = 6,
                            Price = 500m
                        },
                        new
                        {
                            Id = 26,
                            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Description = "Aile terapisi ve danışmanlık",
                            Duration = 90,
                            IsActive = true,
                            Name = "Aile Terapisi",
                            ParentServiceId = 6,
                            Price = 500m
                        },
                        new
                        {
                            Id = 27,
                            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Description = "Teknik sorunlar için destek",
                            Duration = 30,
                            IsActive = true,
                            Name = "Teknik Destek",
                            ParentServiceId = 7,
                            Price = 100m
                        },
                        new
                        {
                            Id = 28,
                            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Description = "Sistem hatalarını bildirme",
                            Duration = 15,
                            IsActive = true,
                            Name = "Hata Bildirimi",
                            ParentServiceId = 7,
                            Price = 0m
                        },
                        new
                        {
                            Id = 29,
                            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Description = "Sistem geliştirme önerileri",
                            Duration = 15,
                            IsActive = true,
                            Name = "Öneri Bildirimi",
                            ParentServiceId = 7,
                            Price = 0m
                        });
                });

            modelBuilder.Entity("EmployeeService", b =>
                {
                    b.Property<int>("EmployeesId")
                        .HasColumnType("int");

                    b.Property<int>("ServicesId")
                        .HasColumnType("int");

                    b.HasKey("EmployeesId", "ServicesId");

                    b.HasIndex("ServicesId");

                    b.ToTable("EmployeeServices", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "1",
                            Name = "Admin",
                            NormalizedName = "ADMIN"
                        },
                        new
                        {
                            Id = "2",
                            Name = "Employee",
                            NormalizedName = "EMPLOYEE"
                        },
                        new
                        {
                            Id = "3",
                            Name = "Client",
                            NormalizedName = "CLIENT"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);

                    b.HasData(
                        new
                        {
                            UserId = "1",
                            RoleId = "1"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("AppointmentSystem.Models.Appointment", b =>
                {
                    b.HasOne("AppointmentSystem.Models.ApplicationUser", "Customer")
                        .WithMany("Appointments")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("AppointmentSystem.Models.Employee", "Employee")
                        .WithMany("Appointments")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("AppointmentSystem.Models.Service", "Service")
                        .WithMany("Appointments")
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Employee");

                    b.Navigation("Service");
                });

            modelBuilder.Entity("AppointmentSystem.Models.Employee", b =>
                {
                    b.HasOne("AppointmentSystem.Models.ApplicationUser", "User")
                        .WithOne("Employee")
                        .HasForeignKey("AppointmentSystem.Models.Employee", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("AppointmentSystem.Models.Notification", b =>
                {
                    b.HasOne("AppointmentSystem.Models.Appointment", "Appointment")
                        .WithMany()
                        .HasForeignKey("AppointmentId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("AppointmentSystem.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Appointment");

                    b.Navigation("User");
                });

            modelBuilder.Entity("EmployeeService", b =>
                {
                    b.HasOne("AppointmentSystem.Models.Employee", null)
                        .WithMany()
                        .HasForeignKey("EmployeesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AppointmentSystem.Models.Service", null)
                        .WithMany()
                        .HasForeignKey("ServicesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("AppointmentSystem.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("AppointmentSystem.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AppointmentSystem.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("AppointmentSystem.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AppointmentSystem.Models.ApplicationUser", b =>
                {
                    b.Navigation("Appointments");

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("AppointmentSystem.Models.Employee", b =>
                {
                    b.Navigation("Appointments");
                });

            modelBuilder.Entity("AppointmentSystem.Models.Service", b =>
                {
                    b.Navigation("Appointments");
                });
#pragma warning restore 612, 618
        }
    }
}
