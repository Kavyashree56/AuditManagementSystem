﻿// <auto-generated />
using System;
using AuthorizationAPI.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AuthorizationAPI.Migrations
{
    [DbContext(typeof(AuditManagementSystemContext))]
    [Migration("20220802121756_onmodelcreating")]
    partial class onmodelcreating
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AuthorizationAPI.Model.Audit", b =>
                {
                    b.Property<int>("Auditid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ApplicationOwnerName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("AuditDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("AuditType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProjectExecutionStatus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProjectManagerName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProjectName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RemedialActionDuration")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Auditid");

                    b.ToTable("Audit");
                });

            modelBuilder.Entity("AuthorizationAPI.Model.Userdetails", b =>
                {
                    b.Property<int>("Userid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RefreshTokenExpiryTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Userid");

                    b.ToTable("Userdetails");

                    b.HasData(
                        new
                        {
                            Userid = 1,
                            Email = "kavya@gmail.com",
                            Password = "abc@123",
                            RefreshTokenExpiryTime = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
