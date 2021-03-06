﻿// <auto-generated />
using EFModels;
using EFModels.MainDB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace EFModels.Migrations.MainDBMigration
{
    [DbContext(typeof(MainDBContext))]
    [Migration("20200611083605_addStandartCase")]
    partial class addStandartCase
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("EFModels.MainDB.BP", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("ConnectionString")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("SelectionCondition")
                        .HasColumnType("text");

                    b.Property<string>("Source")
                        .HasColumnType("text");

                    b.Property<StandartCase>("StandartCase")
                        .HasColumnType("json");

                    b.Property<string>("Structure")
                        .HasColumnType("text");

                    b.Property<long>("SystemId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("SystemId");

                    b.ToTable("BPs");
                });

            modelBuilder.Entity("EFModels.MainDB.BPCase", b =>
                {
                    b.Property<long>("CaseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<long>("BPId")
                        .HasColumnType("bigint");

                    b.HasKey("CaseId");

                    b.HasIndex("BPId");

                    b.ToTable("BPCases");
                });

            modelBuilder.Entity("EFModels.MainDB.System", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Systems");
                });

            modelBuilder.Entity("EFModels.MainDB.BP", b =>
                {
                    b.HasOne("EFModels.MainDB.System", "System")
                        .WithMany("BPs")
                        .HasForeignKey("SystemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EFModels.MainDB.BPCase", b =>
                {
                    b.HasOne("EFModels.MainDB.BP", "BP")
                        .WithMany("BPCases")
                        .HasForeignKey("BPId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
