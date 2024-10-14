﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OnlineVotingSystemAPI.Data;

#nullable disable

namespace OnlineVotingSystemAPI.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("OnlineVotingSystemAPI.Models.Admin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.ToTable("Admins");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedAt = new DateTime(2024, 10, 14, 6, 50, 39, 393, DateTimeKind.Utc).AddTicks(2917),
                            Email = "admin@example.com",
                            FirstName = "Admin",
                            LastName = "User",
                            Password = "vh82WhmTD/Db0OSj8KysR/qqkQyWX8IcNxT0fbm/LGQvVXe8p28XUrD4POMdu+BKGdPuvosS129IIFGRY6e94Q==:oWMdvhn0r89hi6qfoNkhSiREbMrE5ZU0Hu2EesQgRCI="
                        });
                });

            modelBuilder.Entity("OnlineVotingSystemAPI.Models.AdminRole", b =>
                {
                    b.Property<int>("AdminId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("AdminId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AdminRoles");
                });

            modelBuilder.Entity("OnlineVotingSystemAPI.Models.Candidate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Party")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("VotingEventId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("VotingEventId");

                    b.ToTable("Candidates");
                });

            modelBuilder.Entity("OnlineVotingSystemAPI.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Admin"
                        },
                        new
                        {
                            Id = 2,
                            Name = "User"
                        });
                });

            modelBuilder.Entity("OnlineVotingSystemAPI.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "admin@example.com",
                            FirstName = "Admin",
                            LastName = "User",
                            Password = "ht4Bk1j4pLA7f63/r/LjZ9zWcEZF2PupF0Kn2Ax/HkUxPquzIsR+MGV4WEUzDNNCao6mKtK7Tc3qdb7PPxq+DQ==:Krmd+ra9yAdAcTtD6FZ+QayX5GQE6AmE7F+7KiylpKA="
                        });
                });

            modelBuilder.Entity("OnlineVotingSystemAPI.Models.VotingEvent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("EventDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("EventName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("VotingEvents");
                });

            modelBuilder.Entity("RoleUser", b =>
                {
                    b.Property<int>("RolesId")
                        .HasColumnType("int");

                    b.Property<int>("UsersId")
                        .HasColumnType("int");

                    b.HasKey("RolesId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("RoleUser");
                });

            modelBuilder.Entity("OnlineVotingSystemAPI.Models.AdminRole", b =>
                {
                    b.HasOne("OnlineVotingSystemAPI.Models.Admin", "Admin")
                        .WithMany("Roles")
                        .HasForeignKey("AdminId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OnlineVotingSystemAPI.Models.Role", "Role")
                        .WithMany("AdminRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Admin");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("OnlineVotingSystemAPI.Models.Candidate", b =>
                {
                    b.HasOne("OnlineVotingSystemAPI.Models.VotingEvent", "VotingEvent")
                        .WithMany("Candidates")
                        .HasForeignKey("VotingEventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("VotingEvent");
                });

            modelBuilder.Entity("RoleUser", b =>
                {
                    b.HasOne("OnlineVotingSystemAPI.Models.Role", null)
                        .WithMany()
                        .HasForeignKey("RolesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OnlineVotingSystemAPI.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("OnlineVotingSystemAPI.Models.Admin", b =>
                {
                    b.Navigation("Roles");
                });

            modelBuilder.Entity("OnlineVotingSystemAPI.Models.Role", b =>
                {
                    b.Navigation("AdminRoles");
                });

            modelBuilder.Entity("OnlineVotingSystemAPI.Models.VotingEvent", b =>
                {
                    b.Navigation("Candidates");
                });
#pragma warning restore 612, 618
        }
    }
}
