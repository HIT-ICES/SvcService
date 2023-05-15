﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SvcService.Data;

#nullable disable

namespace SvcService.Migrations
{
    [DbContext(typeof(ServiceDbContext))]
    partial class ServiceDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("SvcService.Data.InterfaceEntity", b =>
                {
                    b.Property<string>("ServiceId")
                        .HasColumnType("varchar(32)");

                    b.Property<string>("IdSuffix")
                        .HasMaxLength(32)
                        .HasColumnType("varchar(32)");

                    b.Property<int>("InputSize")
                        .HasColumnType("int");

                    b.Property<string>("OutputSize")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)");

                    b.HasKey("ServiceId", "IdSuffix");

                    b.ToTable("Interfaces");
                });

            modelBuilder.Entity("SvcService.Data.ServiceEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(32)
                        .HasColumnType("varchar(32)");

                    b.Property<int>("DesiredCapability")
                        .HasColumnType("int");

                    b.Property<decimal>("DesiredCpu")
                        .HasColumnType("decimal(16,4)");

                    b.Property<decimal>("DesiredDisk")
                        .HasColumnType("decimal(16,4)");

                    b.Property<decimal>("DesiredGpuCore")
                        .HasColumnType("decimal(16,4)");

                    b.Property<decimal>("DesiredGpuMem")
                        .HasColumnType("decimal(16,4)");

                    b.Property<decimal>("DesiredRam")
                        .HasColumnType("decimal(16,4)");

                    b.Property<bool>("HasIdleResource")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("HasVersion")
                        .HasColumnType("tinyint(1)");

                    b.Property<decimal>("IdleCpu")
                        .HasColumnType("decimal(16,4)");

                    b.Property<decimal>("IdleDisk")
                        .HasColumnType("decimal(16,4)");

                    b.Property<decimal>("IdleGpuCore")
                        .HasColumnType("decimal(16,4)");

                    b.Property<decimal>("IdleGpuMem")
                        .HasColumnType("decimal(16,4)");

                    b.Property<decimal>("IdleRam")
                        .HasColumnType("decimal(16,4)");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("varchar(32)");

                    b.Property<string>("Repo")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.Property<string>("VersionMajor")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("varchar(16)");

                    b.Property<string>("VersionMinor")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("varchar(16)");

                    b.Property<string>("VersionPatch")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("varchar(32)");

                    b.HasKey("Id");

                    b.HasIndex("Name");

                    b.ToTable("Services");
                });

            modelBuilder.Entity("SvcService.Data.InterfaceEntity", b =>
                {
                    b.HasOne("SvcService.Data.ServiceEntity", "Service")
                        .WithMany("Interfaces")
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Service");
                });

            modelBuilder.Entity("SvcService.Data.ServiceEntity", b =>
                {
                    b.Navigation("Interfaces");
                });
#pragma warning restore 612, 618
        }
    }
}
