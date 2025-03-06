﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TimeProductivityTracking.web.Data;

#nullable disable

namespace TimeProductivityTracking.web.Migrations
{
    [DbContext(typeof(ProductivitiesContext))]
    [Migration("20250306100129_MyMigrationV4")]
    partial class MyMigrationV4
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TimeProductivityTracking.web.Models.Productivities", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("AchevedDays")
                        .HasColumnType("int");

                    b.Property<int>("ContractorId_FK")
                        .HasColumnType("int");

                    b.Property<string>("CounryMentor_A")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CounryMentor_P")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("County")
                        .HasColumnType("int");

                    b.Property<DateTime>("Monthly")
                        .HasColumnType("datetime2");

                    b.Property<int?>("PlannedDays")
                        .HasColumnType("int");

                    b.Property<int?>("SECContractId")
                        .HasColumnType("int");

                    b.Property<string>("SECName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Task")
                        .HasColumnType("int");

                    b.Property<int?>("Tasks_A")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SECContractId");

                    b.ToTable("Productivities", (string)null);
                });

            modelBuilder.Entity("TimeProductivityTracking.web.Models.Rate", b =>
                {
                    b.Property<int>("RateID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RateID"));

                    b.Property<double>("HourlyWage")
                        .HasColumnType("float");

                    b.Property<string>("RateName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RateID");

                    b.ToTable("Rate", (string)null);
                });

            modelBuilder.Entity("TimeProductivityTracking.web.Models.SECContract", b =>
                {
                    b.Property<int>("SECContractId")
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("County")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PrimaryContract")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SECName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SECContractId");

                    b.ToTable("SECContract", (string)null);

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("TimeProductivityTracking.web.Models.UserInfo", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("HireDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("LName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RateID")
                        .HasColumnType("int");

                    b.Property<int>("Register")
                        .HasColumnType("int");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("UserId");

                    b.ToTable("UserInfo", (string)null);
                });

            modelBuilder.Entity("TimeProductivityTracking.web.Models.Contractor", b =>
                {
                    b.HasBaseType("TimeProductivityTracking.web.Models.SECContract");

                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<DateTime>("Monthly")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId_FK")
                        .HasColumnType("int");

                    b.Property<int>("UserInfoUserId")
                        .HasColumnType("int");

                    b.HasIndex("UserInfoUserId");

                    b.ToTable("Contractor", (string)null);
                });

            modelBuilder.Entity("TimeProductivityTracking.web.Models.Productivities", b =>
                {
                    b.HasOne("TimeProductivityTracking.web.Models.SECContract", null)
                        .WithMany("Productivities")
                        .HasForeignKey("SECContractId");
                });

            modelBuilder.Entity("TimeProductivityTracking.web.Models.Contractor", b =>
                {
                    b.HasOne("TimeProductivityTracking.web.Models.SECContract", null)
                        .WithOne()
                        .HasForeignKey("TimeProductivityTracking.web.Models.Contractor", "SECContractId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TimeProductivityTracking.web.Models.UserInfo", "UserInfo")
                        .WithMany()
                        .HasForeignKey("UserInfoUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserInfo");
                });

            modelBuilder.Entity("TimeProductivityTracking.web.Models.SECContract", b =>
                {
                    b.Navigation("Productivities");
                });
#pragma warning restore 612, 618
        }
    }
}
