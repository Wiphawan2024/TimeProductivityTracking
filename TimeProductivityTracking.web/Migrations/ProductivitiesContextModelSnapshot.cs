﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TimeProductivityTracking.web.Data;

#nullable disable

namespace TimeProductivityTracking.web.Migrations
{
    [DbContext(typeof(ProductivitiesContext))]
    partial class ProductivitiesContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TimeProductivityTracking.web.Models.Productivity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal?>("AchevedDays")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("CounryMentor_A")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CounryMentor_P")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("County")
                        .HasColumnType("int");

                    b.Property<DateTime?>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Monthly")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("PlannedDays")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("SECName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Task_P")
                        .HasColumnType("int");

                    b.Property<int?>("Tasks_A")
                        .HasColumnType("int");

                    b.HasKey("Id");

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
#pragma warning restore 612, 618
        }
    }
}
