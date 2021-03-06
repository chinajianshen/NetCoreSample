﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NineskyStudy;

namespace NineskyStudy.Migrations
{
    [DbContext(typeof(NineskyDbContext))]
    [Migration("20180920061604_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.3-rtm-32065")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("NineskyStudy.Models.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasMaxLength(1000);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int>("Order");

                    b.Property<int>("ParentId");

                    b.Property<string>("ParentPath");

                    b.Property<int>("Target");

                    b.Property<int>("Type");

                    b.Property<string>("View")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.HasKey("CategoryId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("NineskyStudy.Models.CategoryGeneral", b =>
                {
                    b.Property<int>("GeneralId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CategoryId");

                    b.Property<int?>("ContentOrder");

                    b.Property<string>("ContentView")
                        .HasMaxLength(200);

                    b.Property<int?>("ModuleId");

                    b.HasKey("GeneralId");

                    b.HasIndex("CategoryId")
                        .IsUnique();

                    b.ToTable("CategoryGeneral");
                });

            modelBuilder.Entity("NineskyStudy.Models.CategoryLink", b =>
                {
                    b.Property<int>("LinkId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CategoryId");

                    b.Property<string>("Url")
                        .HasMaxLength(500);

                    b.HasKey("LinkId");

                    b.HasIndex("CategoryId")
                        .IsUnique();

                    b.ToTable("CategoryLink");
                });

            modelBuilder.Entity("NineskyStudy.Models.CategoryPage", b =>
                {
                    b.Property<int>("PageId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CategoryId");

                    b.Property<string>("Content")
                        .HasMaxLength(10000);

                    b.HasKey("PageId");

                    b.HasIndex("CategoryId")
                        .IsUnique();

                    b.ToTable("CategoryPage");
                });

            modelBuilder.Entity("NineskyStudy.Models.Module", b =>
                {
                    b.Property<int>("ModuleId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Controller")
                        .HasMaxLength(50);

                    b.Property<string>("Description")
                        .HasMaxLength(1000);

                    b.Property<bool>("Enabled");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("ModuleId");

                    b.ToTable("Modules");
                });

            modelBuilder.Entity("NineskyStudy.Models.ModuleOrder", b =>
                {
                    b.Property<int>("ModuleOrderId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ModuleId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int>("Order");

                    b.HasKey("ModuleOrderId");

                    b.HasIndex("ModuleId");

                    b.ToTable("ModuleOrder");
                });

            modelBuilder.Entity("NineskyStudy.Models.CategoryGeneral", b =>
                {
                    b.HasOne("NineskyStudy.Models.Category", "Category")
                        .WithOne("General")
                        .HasForeignKey("NineskyStudy.Models.CategoryGeneral", "CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NineskyStudy.Models.CategoryLink", b =>
                {
                    b.HasOne("NineskyStudy.Models.Category")
                        .WithOne("Link")
                        .HasForeignKey("NineskyStudy.Models.CategoryLink", "CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NineskyStudy.Models.CategoryPage", b =>
                {
                    b.HasOne("NineskyStudy.Models.Category", "Category")
                        .WithOne("Page")
                        .HasForeignKey("NineskyStudy.Models.CategoryPage", "CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NineskyStudy.Models.ModuleOrder", b =>
                {
                    b.HasOne("NineskyStudy.Models.Module", "Module")
                        .WithMany("ModuleOrders")
                        .HasForeignKey("ModuleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
