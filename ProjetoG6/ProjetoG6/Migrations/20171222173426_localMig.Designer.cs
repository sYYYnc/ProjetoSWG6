﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using ProjetoG6.Data;
using System;

namespace ProjetoG6.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20171222173426_localMig")]
    partial class localMig
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("ProjetoG6.Models.AccountViewModels.Help", b =>
                {
                    b.Property<int>("HelpID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Campo");

                    b.Property<string>("Descricao");

                    b.HasKey("HelpID");

                    b.ToTable("Help");
                });

            modelBuilder.Entity("ProjetoG6.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("ProjetoG6.Models.Candidatos", b =>
                {
                    b.Property<int>("CandidatoID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BolsaID");

                    b.Property<int>("CandidaturaID");

                    b.Property<DateTime>("DataNascimento");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<int>("EntervistaID");

                    b.Property<string>("Nome")
                        .IsRequired();

                    b.Property<string>("NumeroAluno")
                        .IsRequired()
                        .HasMaxLength(9);

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<string>("PasswordConfirmacao")
                        .IsRequired();

                    b.HasKey("CandidatoID");

                    b.ToTable("Candidatos");
                });

            modelBuilder.Entity("ProjetoG6.Models.Candidatura", b =>
                {
                    b.Property<int>("CandidaturaID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AdministradorID");

                    b.Property<DateTime>("DataCandidatura");

                    b.Property<string>("Estado")
                        .IsRequired()
                        .HasColumnName("Estado")
                        .HasMaxLength(225);

                    b.Property<int>("PaisID");

                    b.Property<int>("ProgramaMobilidadeID");

                    b.HasKey("CandidaturaID");

                    b.ToTable("Candidatura");
                });

            modelBuilder.Entity("ProjetoG6.Models.Paises", b =>
                {
                    b.Property<int>("PaisID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Pais");

                    b.HasKey("PaisID");

                    b.ToTable("Paises");
                });

            modelBuilder.Entity("ProjetoG6.Models.ProgramaMobilidade", b =>
                {
                    b.Property<int>("ProgramaMobilidadeID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(225);

                    b.HasKey("ProgramaMobilidadeID");

                    b.ToTable("ProgramaMobilidade");
                });

            modelBuilder.Entity("ProjetoG6.Models.ProgramaMobilidadePais", b =>
                {
                    b.Property<int>("ProgramaMobilidadePaisID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("PaisID");

                    b.Property<int>("ProgramaMobilidadeID");

                    b.HasKey("ProgramaMobilidadePaisID");

                    b.HasIndex("PaisID");

                    b.HasIndex("ProgramaMobilidadeID");

                    b.ToTable("ProgramaMobilidadePais");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("ProjetoG6.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("ProjetoG6.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ProjetoG6.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("ProjetoG6.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ProjetoG6.Models.ProgramaMobilidadePais", b =>
                {
                    b.HasOne("ProjetoG6.Models.Paises", "Paises")
                        .WithMany()
                        .HasForeignKey("PaisID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ProjetoG6.Models.ProgramaMobilidade", "ProgramaMobilidade")
                        .WithMany()
                        .HasForeignKey("ProgramaMobilidadeID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
