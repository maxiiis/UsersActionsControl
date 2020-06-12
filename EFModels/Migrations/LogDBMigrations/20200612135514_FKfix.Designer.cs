﻿// <auto-generated />
using System;
using EFModels.LogsDB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace EFModels.Migrations.LogDBMigrations
{
    [DbContext(typeof(LogDBContext))]
    [Migration("20200612135514_FKfix")]
    partial class FKfix
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("EFModels.LogsDB.Activity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ActivityText")
                        .HasColumnType("text");

                    b.Property<string>("ActivityText2")
                        .HasColumnType("text");

                    b.Property<string>("StatusText")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Activities");
                });

            modelBuilder.Entity("EFModels.LogsDB.EventLog", b =>
                {
                    b.Property<long>("EventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("ActivityId")
                        .HasColumnType("text");

                    b.Property<bool>("AnalysStatus")
                        .HasColumnType("boolean");

                    b.Property<long>("CaseId")
                        .HasColumnType("bigint");

                    b.Property<int>("Cost")
                        .HasColumnType("integer");

                    b.Property<string>("ResourseId")
                        .HasColumnType("text");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("EventId");

                    b.HasIndex("ActivityId");

                    b.HasIndex("ResourseId");

                    b.ToTable("EventLogs");
                });

            modelBuilder.Entity("EFModels.LogsDB.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("Department")
                        .HasColumnType("text");

                    b.Property<string>("Filial")
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<string>("MiddleName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("EFModels.LogsDB.EventLog", b =>
                {
                    b.HasOne("EFModels.LogsDB.Activity", "Activity")
                        .WithMany("EventLog")
                        .HasForeignKey("ActivityId");

                    b.HasOne("EFModels.LogsDB.User", "Resourse")
                        .WithMany("EventLogs")
                        .HasForeignKey("ResourseId");
                });
#pragma warning restore 612, 618
        }
    }
}