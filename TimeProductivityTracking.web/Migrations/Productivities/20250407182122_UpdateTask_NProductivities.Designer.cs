﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TimeProductivityTracking.web.Data;

#nullable disable

namespace TimeProductivityTracking.web.Migrations.Productivities
{
    [DbContext(typeof(ProductivitiesContext))]
    [Migration("20250407182122_UpdateTask_NProductivities")]
    partial class UpdateTask_NProductivities
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TimeProductivityTracking.web.Models.Invoice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ContractorId")
                        .HasColumnType("int");

                    b.Property<decimal>("HourlyRate")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("InvoiceDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("InvoiceNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Month")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("TotalAmount")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("TotalHours")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("statusApproval")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ContractorId");

                    b.ToTable("Invoice", (string)null);
                });

            modelBuilder.Entity("TimeProductivityTracking.web.Models.Productivity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("AchevedDays")
                        .HasColumnType("decimal(5,2)");

                    b.Property<int?>("ContractorId")
                        .HasColumnType("int");

                    b.Property<string>("CountryMentor_A")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CountryMentor_P")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("County")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Monthly")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("PlannedDays")
                        .HasColumnType("decimal(5, 2)");

                    b.Property<decimal>("PlannedNextMonth")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("SECName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Task_N")
                        .HasColumnType("int");

                    b.Property<int>("Task_P")
                        .HasColumnType("int");

                    b.Property<int>("Tasks_A")
                        .HasColumnType("int");

                    b.Property<string>("UserEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("statusApproval")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ContractorId");

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
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RateID");

                    b.ToTable("Rate", (string)null);
                });

            modelBuilder.Entity("TimeProductivityTracking.web.Models.SECContract", b =>
                {
                    b.Property<int>("SECContractId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SECContractId"));

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("County")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PrimaryContract")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SECName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SECContractId");

                    b.ToTable("SECContract", (string)null);
                });

            modelBuilder.Entity("TimeProductivityTracking.web.Models.UserInfo", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("HireDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("LName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("RateID")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<int>("Register")
                        .HasColumnType("int");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("UserId");

                    b.HasIndex("RateID");

                    b.ToTable("UserInfo", (string)null);
                });

            modelBuilder.Entity("TimeProductivityTracking.web.Models.Invoice", b =>
                {
                    b.HasOne("TimeProductivityTracking.web.Models.UserInfo", "Contractor")
                        .WithMany()
                        .HasForeignKey("ContractorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Contractor");
                });

            modelBuilder.Entity("TimeProductivityTracking.web.Models.Productivity", b =>
                {
                    b.HasOne("TimeProductivityTracking.web.Models.UserInfo", "Contractor")
                        .WithMany()
                        .HasForeignKey("ContractorId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Contractor");
                });

            modelBuilder.Entity("TimeProductivityTracking.web.Models.UserInfo", b =>
                {
                    b.HasOne("TimeProductivityTracking.web.Models.Rate", "Rate")
                        .WithMany()
                        .HasForeignKey("RateID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Rate");
                });
#pragma warning restore 612, 618
        }
    }
}
