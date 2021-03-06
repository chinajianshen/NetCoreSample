﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApp2.Models;

namespace WebApp2.Migrations
{
    [DbContext(typeof(HelloWorldDBContext))]
    [Migration("20180913044815_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.3-rtm-32065");

            modelBuilder.Entity("WebApp2.Models.Employee", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Flag");

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.ToTable("Employees");
                });
#pragma warning restore 612, 618
        }
    }
}
