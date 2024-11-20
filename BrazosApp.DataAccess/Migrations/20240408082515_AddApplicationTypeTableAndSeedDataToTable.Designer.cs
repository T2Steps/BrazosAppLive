﻿// <auto-generated />
using System;
using BrazosApp.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BrazosApp.DataAccess.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240408082515_AddApplicationTypeTableAndSeedDataToTable")]
    partial class AddApplicationTypeTableAndSeedDataToTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.28")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("BrazosApp.Models.ApplicationType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int>("LanguageTypeId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.HasKey("Id");

                    b.HasIndex("LanguageTypeId");

                    b.ToTable("ApplicationType");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            IsActive = true,
                            LanguageTypeId = 1,
                            Name = "Retail Food New"
                        },
                        new
                        {
                            Id = 2,
                            IsActive = true,
                            LanguageTypeId = 2,
                            Name = "Retail Food New (Spanish)"
                        },
                        new
                        {
                            Id = 3,
                            IsActive = true,
                            LanguageTypeId = 1,
                            Name = "Mobile Food New"
                        },
                        new
                        {
                            Id = 4,
                            IsActive = true,
                            LanguageTypeId = 2,
                            Name = "Mobile Food New (Spanish)"
                        },
                        new
                        {
                            Id = 5,
                            IsActive = true,
                            LanguageTypeId = 1,
                            Name = "Retail Food Change of Owner"
                        },
                        new
                        {
                            Id = 6,
                            IsActive = true,
                            LanguageTypeId = 2,
                            Name = "Retail Food Change of Owner (Spanish)"
                        },
                        new
                        {
                            Id = 7,
                            IsActive = true,
                            LanguageTypeId = 1,
                            Name = "Mobile Food Change of Owner"
                        },
                        new
                        {
                            Id = 8,
                            IsActive = true,
                            LanguageTypeId = 2,
                            Name = "Mobile Food Change of Owner (Spanish)"
                        },
                        new
                        {
                            Id = 9,
                            IsActive = true,
                            LanguageTypeId = 1,
                            Name = "Temporary Food"
                        },
                        new
                        {
                            Id = 10,
                            IsActive = true,
                            LanguageTypeId = 2,
                            Name = "Temporary Food (Spanish)"
                        },
                        new
                        {
                            Id = 11,
                            IsActive = true,
                            LanguageTypeId = 1,
                            Name = "Foster Home"
                        },
                        new
                        {
                            Id = 12,
                            IsActive = true,
                            LanguageTypeId = 1,
                            Name = "Daycare Sanitation"
                        },
                        new
                        {
                            Id = 13,
                            IsActive = true,
                            LanguageTypeId = 1,
                            Name = "Food Handler Enrollment Application"
                        },
                        new
                        {
                            Id = 14,
                            IsActive = true,
                            LanguageTypeId = 2,
                            Name = "Food Handler Enrollment Application (Spanish)"
                        },
                        new
                        {
                            Id = 15,
                            IsActive = true,
                            LanguageTypeId = 1,
                            Name = "Pools"
                        },
                        new
                        {
                            Id = 16,
                            IsActive = true,
                            LanguageTypeId = 1,
                            Name = "Complaints"
                        });
                });

            modelBuilder.Entity("BrazosApp.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("BrazosApp.Models.LanguageType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Code")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.HasKey("Id");

                    b.ToTable("LanguageType");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Code = "E",
                            IsActive = true,
                            Name = "English"
                        },
                        new
                        {
                            Id = 2,
                            Code = "SP",
                            IsActive = true,
                            Name = "Spanish"
                        });
                });

            modelBuilder.Entity("BrazosApp.Models.PermitStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("PermitStatus");
                });

            modelBuilder.Entity("BrazosApp.Models.PermitType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("PermitType");
                });

            modelBuilder.Entity("BrazosApp.Models.RiskCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Code")
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("RiskCategory");
                });

            modelBuilder.Entity("BrazosApp.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

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
                            IsActive = true,
                            Name = "SuperAdmin"
                        },
                        new
                        {
                            Id = 2,
                            IsActive = true,
                            Name = "Admin Inspector"
                        },
                        new
                        {
                            Id = 3,
                            IsActive = true,
                            Name = "Admin"
                        },
                        new
                        {
                            Id = 4,
                            IsActive = true,
                            Name = "Inspector"
                        },
                        new
                        {
                            Id = 5,
                            IsActive = true,
                            Name = "Clerk"
                        },
                        new
                        {
                            Id = 6,
                            IsActive = true,
                            Name = "View Only"
                        });
                });

            modelBuilder.Entity("BrazosApp.Models.SubCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Code")
                        .HasMaxLength(4)
                        .HasColumnType("nvarchar(4)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("SubCategory");
                });

            modelBuilder.Entity("BrazosApp.Models.Territory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ColorCode")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int?>("UpdatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedOn")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Territories");
                });

            modelBuilder.Entity("BrazosApp.Models.TerritoryAssignedType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("TerritoryAssignedTypes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "System Assigned"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Default Assigned"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Normal Assigned"
                        });
                });

            modelBuilder.Entity("BrazosApp.Models.TerritoryWiseInspectors", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AssignedUserId")
                        .HasColumnType("int");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("TerritoryId")
                        .HasColumnType("int");

                    b.Property<int>("TypeId")
                        .HasColumnType("int");

                    b.Property<int?>("UpdatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedOn")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AssignedUserId")
                        .IsUnique();

                    b.HasIndex("TerritoryId");

                    b.HasIndex("TypeId");

                    b.ToTable("TerritoryWiseInspectors");
                });

            modelBuilder.Entity("BrazosApp.Models.Users", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("BHCD")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CertifiedPoolOperator")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("DesignatedRepresentative")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EmailId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<bool>("IsLoggedIn")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastSeenTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Password")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("RegisteredSanitarian")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<byte[]>("Salt")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("SanitarianInTrain")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SignFileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UpdatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedOn")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BrazosApp.Models.ApplicationType", b =>
                {
                    b.HasOne("BrazosApp.Models.LanguageType", "LanguageType")
                        .WithMany()
                        .HasForeignKey("LanguageTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("LanguageType");
                });

            modelBuilder.Entity("BrazosApp.Models.SubCategory", b =>
                {
                    b.HasOne("BrazosApp.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("BrazosApp.Models.TerritoryWiseInspectors", b =>
                {
                    b.HasOne("BrazosApp.Models.Users", "AssignedUser")
                        .WithOne("TerritoryWiseInspectors")
                        .HasForeignKey("BrazosApp.Models.TerritoryWiseInspectors", "AssignedUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BrazosApp.Models.Territory", "Territory")
                        .WithMany("Inspectors")
                        .HasForeignKey("TerritoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BrazosApp.Models.TerritoryAssignedType", "Type")
                        .WithMany()
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AssignedUser");

                    b.Navigation("Territory");

                    b.Navigation("Type");
                });

            modelBuilder.Entity("BrazosApp.Models.Users", b =>
                {
                    b.HasOne("BrazosApp.Models.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("BrazosApp.Models.Territory", b =>
                {
                    b.Navigation("Inspectors");
                });

            modelBuilder.Entity("BrazosApp.Models.Users", b =>
                {
                    b.Navigation("TerritoryWiseInspectors")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
