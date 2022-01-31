﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RocketLaunchJournal.Entities;

#nullable disable

namespace RocketLaunchJournal.Entities.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20220131145347_updateV6")]
    partial class updateV6
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.DeviceFlowCodes", b =>
                {
                    b.Property<string>("UserCode")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("ClientId")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasMaxLength(50000)
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("DeviceCode")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateTime?>("Expiration")
                        .IsRequired()
                        .HasColumnType("datetime2");

                    b.Property<string>("SessionId")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("SubjectId")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("UserCode");

                    b.HasIndex("DeviceCode")
                        .IsUnique();

                    b.HasIndex("Expiration");

                    b.ToTable("DeviceCodes", (string)null);
                });

            modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.Key", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Algorithm")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("DataProtected")
                        .HasColumnType("bit");

                    b.Property<bool>("IsX509Certificate")
                        .HasColumnType("bit");

                    b.Property<string>("Use")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Version")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Use");

                    b.ToTable("Keys", (string)null);
                });

            modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.PersistedGrant", b =>
                {
                    b.Property<string>("Key")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("ClientId")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateTime?>("ConsumedTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasMaxLength(50000)
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateTime?>("Expiration")
                        .HasColumnType("datetime2");

                    b.Property<string>("SessionId")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("SubjectId")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Key");

                    b.HasIndex("ConsumedTime");

                    b.HasIndex("Expiration");

                    b.HasIndex("SubjectId", "ClientId", "Type");

                    b.HasIndex("SubjectId", "SessionId", "Type");

                    b.ToTable("PersistedGrants", (string)null);
                });

            modelBuilder.Entity("RocketLaunchJournal.Model.Adhoc.Report", b =>
                {
                    b.Property<int>("ReportId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ReportId"), 1L, 1);

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsShared")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("ReportSourceId")
                        .HasColumnType("int");

                    b.Property<int?>("UserId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.HasKey("ReportId");

                    b.HasIndex("ReportSourceId");

                    b.HasIndex("UserId");

                    b.ToTable("Reports");
                });

            modelBuilder.Entity("RocketLaunchJournal.Model.Adhoc.ReportSource", b =>
                {
                    b.Property<int>("ReportSourceId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("SQLName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("ReportSourceId");

                    b.ToTable("ReportSources");
                });

            modelBuilder.Entity("RocketLaunchJournal.Model.APILog", b =>
                {
                    b.Property<long>("APILogId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("APILogId"), 1L, 1);

                    b.Property<bool>("IncomingRequest")
                        .HasColumnType("bit");

                    b.Property<string>("RequestContentBlock")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ResponseContentBlock")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ResponseDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("TargetURL")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateTime>("TransmissionDateTime")
                        .HasColumnType("datetime2");

                    b.HasKey("APILogId");

                    b.ToTable("APILogs");
                });

            modelBuilder.Entity("RocketLaunchJournal.Model.Launch", b =>
                {
                    b.Property<int>("LaunchId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LaunchId"), 1L, 1);

                    b.Property<decimal?>("Altitude")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("ApogeeToEjectionTime")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("AverageAcceleration")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("BurnTime")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("CoastToApogeeTime")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("Date");

                    b.Property<decimal?>("DescentSpeed")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("Duration")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("EjectionAltitude")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("LaunchNumber")
                        .HasColumnType("int");

                    b.Property<string>("Motors")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Note")
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<decimal?>("PeakAcceleration")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("RocketId")
                        .HasColumnType("int");

                    b.Property<decimal?>("TopSpeed")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("LaunchId");

                    b.HasIndex("RocketId");

                    b.ToTable("Launches");
                });

            modelBuilder.Entity("RocketLaunchJournal.Model.LogType", b =>
                {
                    b.Property<byte>("LogTypeId")
                        .HasColumnType("tinyint");

                    b.Property<string>("Name")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("LogTypeId");

                    b.ToTable("LogTypes");
                });

            modelBuilder.Entity("RocketLaunchJournal.Model.Rocket", b =>
                {
                    b.Property<int>("RocketId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RocketId"), 1L, 1);

                    b.Property<decimal?>("BlackPowderForApogee")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("BlackPowderForMain")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("Diameter")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("Length")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Recovery")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<decimal?>("Weight")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("RocketId");

                    b.HasIndex("UserId");

                    b.ToTable("Rockets");
                });

            modelBuilder.Entity("RocketLaunchJournal.Model.SystemLog", b =>
                {
                    b.Property<long>("LogId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("LogId"), 1L, 1);

                    b.Property<DateTimeOffset>("EventDateTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("EventDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IpAddress")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<Guid?>("LogGroupKey")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte>("LogTypeId")
                        .HasColumnType("tinyint");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("LogId");

                    b.HasIndex("LogTypeId");

                    b.HasIndex("UserId");

                    b.ToTable("SystemLogs");
                });

            modelBuilder.Entity("RocketLaunchJournal.Model.UserIdentity.Role", b =>
                {
                    b.Property<byte>("RoleId")
                        .HasColumnType("tinyint");

                    b.Property<string>("ConcurrencyStamp")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Data")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Level")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Type")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("RoleId");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("RocketLaunchJournal.Model.UserIdentity.RoleClaim", b =>
                {
                    b.Property<int>("RoleClaimId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RoleClaimId"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("ClaimValue")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<byte>("RoleId")
                        .HasColumnType("tinyint");

                    b.HasKey("RoleClaimId");

                    b.HasIndex("RoleId");

                    b.ToTable("RoleClaims");
                });

            modelBuilder.Entity("RocketLaunchJournal.Model.UserIdentity.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"), 1L, 1);

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<int?>("AuthyUserId")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("IpAddress")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("IsLoginEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateTimeOffset?>("LastPasswordChangeDateTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("LastSignInDateTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("PasswordHash")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("RocketLaunchJournal.Model.UserIdentity.UserClaim", b =>
                {
                    b.Property<int>("UserClaimId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserClaimId"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("ClaimValue")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("UserClaimId");

                    b.HasIndex("UserId");

                    b.ToTable("UserClaims");
                });

            modelBuilder.Entity("RocketLaunchJournal.Model.UserIdentity.UserLogin", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<string>("ProviderDisplayName")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("UserLogins");
                });

            modelBuilder.Entity("RocketLaunchJournal.Model.UserIdentity.UserRefreshToken", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ExpiresOnDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int?>("ImpersonationUserId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("UserId");

                    b.HasIndex("ImpersonationUserId");

                    b.ToTable("UserRefreshTokens");
                });

            modelBuilder.Entity("RocketLaunchJournal.Model.UserIdentity.UserRole", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<byte>("RoleId")
                        .HasColumnType("tinyint");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("RocketLaunchJournal.Model.UserIdentity.UserToken", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<string>("Name")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("UserTokens");
                });

            modelBuilder.Entity("RocketLaunchJournal.Model.Adhoc.Report", b =>
                {
                    b.HasOne("RocketLaunchJournal.Model.Adhoc.ReportSource", "ReportSource")
                        .WithMany()
                        .HasForeignKey("ReportSourceId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("RocketLaunchJournal.Model.UserIdentity.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.OwnsOne("RocketLaunchJournal.Model.OwnedTypes.AuditFields", "AuditFields", b1 =>
                        {
                            b1.Property<int>("ReportId")
                                .HasColumnType("int");

                            b1.Property<int>("CreatedById")
                                .HasColumnType("int")
                                .HasColumnName("CreatedById");

                            b1.Property<DateTimeOffset>("CreatedDateTime")
                                .HasColumnType("datetimeoffset")
                                .HasColumnName("CreatedDateTime");

                            b1.Property<int?>("InactivatedById")
                                .HasColumnType("int")
                                .HasColumnName("InactivatedById");

                            b1.Property<DateTimeOffset?>("InactiveDateTime")
                                .HasColumnType("datetimeoffset")
                                .HasColumnName("InactiveDateTime");

                            b1.Property<int>("UpdatedById")
                                .HasColumnType("int")
                                .HasColumnName("UpdatedById");

                            b1.Property<DateTimeOffset>("UpdatedDateTime")
                                .HasColumnType("datetimeoffset")
                                .HasColumnName("UpdatedDateTime");

                            b1.HasKey("ReportId");

                            b1.ToTable("Reports");

                            b1.WithOwner()
                                .HasForeignKey("ReportId");
                        });

                    b.Navigation("AuditFields");

                    b.Navigation("ReportSource");

                    b.Navigation("User");
                });

            modelBuilder.Entity("RocketLaunchJournal.Model.Launch", b =>
                {
                    b.HasOne("RocketLaunchJournal.Model.Rocket", "Rocket")
                        .WithMany("Launches")
                        .HasForeignKey("RocketId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.OwnsOne("RocketLaunchJournal.Model.OwnedTypes.AuditFields", "AuditFields", b1 =>
                        {
                            b1.Property<int>("LaunchId")
                                .HasColumnType("int");

                            b1.Property<int>("CreatedById")
                                .HasColumnType("int")
                                .HasColumnName("CreatedById");

                            b1.Property<DateTimeOffset>("CreatedDateTime")
                                .HasColumnType("datetimeoffset")
                                .HasColumnName("CreatedDateTime");

                            b1.Property<int?>("InactivatedById")
                                .HasColumnType("int")
                                .HasColumnName("InactivatedById");

                            b1.Property<DateTimeOffset?>("InactiveDateTime")
                                .HasColumnType("datetimeoffset")
                                .HasColumnName("InactiveDateTime");

                            b1.Property<int>("UpdatedById")
                                .HasColumnType("int")
                                .HasColumnName("UpdatedById");

                            b1.Property<DateTimeOffset>("UpdatedDateTime")
                                .HasColumnType("datetimeoffset")
                                .HasColumnName("UpdatedDateTime");

                            b1.HasKey("LaunchId");

                            b1.ToTable("Launches");

                            b1.WithOwner()
                                .HasForeignKey("LaunchId");
                        });

                    b.Navigation("AuditFields");

                    b.Navigation("Rocket");
                });

            modelBuilder.Entity("RocketLaunchJournal.Model.Rocket", b =>
                {
                    b.HasOne("RocketLaunchJournal.Model.UserIdentity.User", "User")
                        .WithMany("Rockets")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.OwnsOne("RocketLaunchJournal.Model.OwnedTypes.AuditFields", "AuditFields", b1 =>
                        {
                            b1.Property<int>("RocketId")
                                .HasColumnType("int");

                            b1.Property<int>("CreatedById")
                                .HasColumnType("int")
                                .HasColumnName("CreatedById");

                            b1.Property<DateTimeOffset>("CreatedDateTime")
                                .HasColumnType("datetimeoffset")
                                .HasColumnName("CreatedDateTime");

                            b1.Property<int?>("InactivatedById")
                                .HasColumnType("int")
                                .HasColumnName("InactivatedById");

                            b1.Property<DateTimeOffset?>("InactiveDateTime")
                                .HasColumnType("datetimeoffset")
                                .HasColumnName("InactiveDateTime");

                            b1.Property<int>("UpdatedById")
                                .HasColumnType("int")
                                .HasColumnName("UpdatedById");

                            b1.Property<DateTimeOffset>("UpdatedDateTime")
                                .HasColumnType("datetimeoffset")
                                .HasColumnName("UpdatedDateTime");

                            b1.HasKey("RocketId");

                            b1.ToTable("Rockets");

                            b1.WithOwner()
                                .HasForeignKey("RocketId");
                        });

                    b.Navigation("AuditFields");

                    b.Navigation("User");
                });

            modelBuilder.Entity("RocketLaunchJournal.Model.SystemLog", b =>
                {
                    b.HasOne("RocketLaunchJournal.Model.LogType", "LogType")
                        .WithMany("SystemLogs")
                        .HasForeignKey("LogTypeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("RocketLaunchJournal.Model.UserIdentity.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("LogType");

                    b.Navigation("User");
                });

            modelBuilder.Entity("RocketLaunchJournal.Model.UserIdentity.RoleClaim", b =>
                {
                    b.HasOne("RocketLaunchJournal.Model.UserIdentity.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("RocketLaunchJournal.Model.UserIdentity.User", b =>
                {
                    b.OwnsOne("RocketLaunchJournal.Model.OwnedTypes.AuditFields", "AuditFields", b1 =>
                        {
                            b1.Property<int>("UserId")
                                .HasColumnType("int");

                            b1.Property<int>("CreatedById")
                                .HasColumnType("int")
                                .HasColumnName("CreatedById");

                            b1.Property<DateTimeOffset>("CreatedDateTime")
                                .HasColumnType("datetimeoffset")
                                .HasColumnName("CreatedDateTime");

                            b1.Property<int?>("InactivatedById")
                                .HasColumnType("int")
                                .HasColumnName("InactivatedById");

                            b1.Property<DateTimeOffset?>("InactiveDateTime")
                                .HasColumnType("datetimeoffset")
                                .HasColumnName("InactiveDateTime");

                            b1.Property<int>("UpdatedById")
                                .HasColumnType("int")
                                .HasColumnName("UpdatedById");

                            b1.Property<DateTimeOffset>("UpdatedDateTime")
                                .HasColumnType("datetimeoffset")
                                .HasColumnName("UpdatedDateTime");

                            b1.HasKey("UserId");

                            b1.ToTable("Users");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.Navigation("AuditFields");
                });

            modelBuilder.Entity("RocketLaunchJournal.Model.UserIdentity.UserClaim", b =>
                {
                    b.HasOne("RocketLaunchJournal.Model.UserIdentity.User", "User")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("RocketLaunchJournal.Model.UserIdentity.UserLogin", b =>
                {
                    b.HasOne("RocketLaunchJournal.Model.UserIdentity.User", "User")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("RocketLaunchJournal.Model.UserIdentity.UserRefreshToken", b =>
                {
                    b.HasOne("RocketLaunchJournal.Model.UserIdentity.User", "ImpersonationUser")
                        .WithMany()
                        .HasForeignKey("ImpersonationUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("RocketLaunchJournal.Model.UserIdentity.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ImpersonationUser");

                    b.Navigation("User");
                });

            modelBuilder.Entity("RocketLaunchJournal.Model.UserIdentity.UserRole", b =>
                {
                    b.HasOne("RocketLaunchJournal.Model.UserIdentity.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("RocketLaunchJournal.Model.UserIdentity.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("RocketLaunchJournal.Model.UserIdentity.UserToken", b =>
                {
                    b.HasOne("RocketLaunchJournal.Model.UserIdentity.User", "User")
                        .WithMany("Tokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("RocketLaunchJournal.Model.LogType", b =>
                {
                    b.Navigation("SystemLogs");
                });

            modelBuilder.Entity("RocketLaunchJournal.Model.Rocket", b =>
                {
                    b.Navigation("Launches");
                });

            modelBuilder.Entity("RocketLaunchJournal.Model.UserIdentity.Role", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("RocketLaunchJournal.Model.UserIdentity.User", b =>
                {
                    b.Navigation("Claims");

                    b.Navigation("Logins");

                    b.Navigation("Rockets");

                    b.Navigation("Tokens");

                    b.Navigation("UserRoles");
                });
#pragma warning restore 612, 618
        }
    }
}
