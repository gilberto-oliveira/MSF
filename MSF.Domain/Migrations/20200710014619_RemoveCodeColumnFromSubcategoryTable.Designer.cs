﻿// <auto-generated />
using System;
using MSF.Domain.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MSF.Domain.Migrations
{
    [DbContext(typeof(MSFDbContext))]
    [Migration("20200710014619_RemoveCodeColumnFromSubcategoryTable")]
    partial class RemoveCodeColumnFromSubcategoryTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("MSF")
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MSF.Domain.Models.Audit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Action");

                    b.Property<DateTime>("DateTime")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("KeyValues");

                    b.Property<string>("NewValues");

                    b.Property<string>("OldValues");

                    b.Property<string>("TableName");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.ToTable("Audits","Security");
                });

            modelBuilder.Entity("MSF.Domain.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(5);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("MSF.Domain.Models.Operation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Amount");

                    b.Property<DateTime>("Date");

                    b.Property<int>("ProductId");

                    b.Property<int>("ProviderId");

                    b.Property<string>("Type")
                        .HasMaxLength(2);

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("decimal (18,4)");

                    b.Property<int>("UserId");

                    b.Property<int>("WorkCenterControlId");

                    b.Property<int?>("WorkCenterId");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("ProviderId");

                    b.HasIndex("WorkCenterControlId");

                    b.HasIndex("WorkCenterId");

                    b.ToTable("Operations");
                });

            modelBuilder.Entity("MSF.Domain.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasMaxLength(256);

                    b.Property<decimal>("Profit")
                        .HasColumnType("decimal (18,4)");

                    b.Property<int>("SubcategoryId");

                    b.HasKey("Id");

                    b.HasIndex("SubcategoryId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("MSF.Domain.Models.Provider", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code")
                        .HasMaxLength(120);

                    b.Property<string>("Name")
                        .HasMaxLength(30);

                    b.Property<int>("StateId");

                    b.HasKey("Id");

                    b.HasIndex("StateId");

                    b.ToTable("Providers");
                });

            modelBuilder.Entity("MSF.Domain.Models.Shop", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code")
                        .HasMaxLength(10);

                    b.Property<string>("Description")
                        .HasMaxLength(30);

                    b.HasKey("Id");

                    b.ToTable("Shops");
                });

            modelBuilder.Entity("MSF.Domain.Models.State", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Initials")
                        .HasMaxLength(3);

                    b.Property<string>("Name")
                        .HasMaxLength(30);

                    b.HasKey("Id");

                    b.ToTable("States");
                });

            modelBuilder.Entity("MSF.Domain.Models.Stock", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Amount");

                    b.Property<DateTime>("Date");

                    b.Property<int>("ProductId");

                    b.Property<int>("ProviderId");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("decimal (18,4)");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("ProviderId");

                    b.ToTable("Stocks");
                });

            modelBuilder.Entity("MSF.Domain.Models.Subcategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CategoryId");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Subcategories");
                });

            modelBuilder.Entity("MSF.Domain.Models.WorkCenter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code")
                        .HasMaxLength(10);

                    b.Property<string>("Description")
                        .HasMaxLength(30);

                    b.Property<int>("ShopId");

                    b.HasKey("Id");

                    b.HasIndex("ShopId");

                    b.ToTable("WorkCenters");
                });

            modelBuilder.Entity("MSF.Domain.Models.WorkCenterControl", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("FinalDate");

                    b.Property<DateTime>("StartDate");

                    b.Property<int>("WorkCenterId");

                    b.HasKey("Id");

                    b.HasIndex("WorkCenterId");

                    b.ToTable("WorkCenterControls");
                });

            modelBuilder.Entity("MSF.Domain.Models.Operation", b =>
                {
                    b.HasOne("MSF.Domain.Models.Product", "Product")
                        .WithMany("Operations")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MSF.Domain.Models.Provider", "Provider")
                        .WithMany("Operations")
                        .HasForeignKey("ProviderId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MSF.Domain.Models.WorkCenterControl", "WorkCenterControl")
                        .WithMany("Operations")
                        .HasForeignKey("WorkCenterControlId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MSF.Domain.Models.WorkCenter")
                        .WithMany("Operations")
                        .HasForeignKey("WorkCenterId");
                });

            modelBuilder.Entity("MSF.Domain.Models.Product", b =>
                {
                    b.HasOne("MSF.Domain.Models.Subcategory", "Subcategory")
                        .WithMany("Products")
                        .HasForeignKey("SubcategoryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MSF.Domain.Models.Provider", b =>
                {
                    b.HasOne("MSF.Domain.Models.State", "State")
                        .WithMany("Providers")
                        .HasForeignKey("StateId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MSF.Domain.Models.Stock", b =>
                {
                    b.HasOne("MSF.Domain.Models.Product", "Product")
                        .WithMany("Stocks")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MSF.Domain.Models.Provider", "Provider")
                        .WithMany("Stocks")
                        .HasForeignKey("ProviderId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MSF.Domain.Models.Subcategory", b =>
                {
                    b.HasOne("MSF.Domain.Models.Category", "Category")
                        .WithMany("Subcategories")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MSF.Domain.Models.WorkCenter", b =>
                {
                    b.HasOne("MSF.Domain.Models.Shop", "Shop")
                        .WithMany("WorkCenters")
                        .HasForeignKey("ShopId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MSF.Domain.Models.WorkCenterControl", b =>
                {
                    b.HasOne("MSF.Domain.Models.WorkCenter", "WorkCenter")
                        .WithMany("WorkCenterControls")
                        .HasForeignKey("WorkCenterId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
