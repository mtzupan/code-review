﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Zupan.CodeReview.Repository.EF.Contexts;

namespace Zupan.CodeReview.Repository.EF.Migrations.Modbus
{
    [DbContext(typeof(ModbusContext))]
    [Migration("20190328014300_Initial-Migration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("public")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.3-servicing-35854")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Zupan.CodeReview.Domains.Modbus.ModbusContactConfigurations", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedByUserLogin");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("Description");

                    b.Property<string>("ModifiedByUserLogin");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.ToTable("ModbusContactConfigurations");
                });

            modelBuilder.Entity("Zupan.CodeReview.Domains.Modbus.ModbusContacts", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ChannelNumber");

                    b.Property<string>("CreatedByUserLogin");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("Description");

                    b.Property<string>("Guid");

                    b.Property<int>("MessageTypeId");

                    b.Property<int>("ModbusDeviceId");

                    b.Property<string>("ModifiedByUserLogin");

                    b.Property<DateTime?>("ModifiedOn");

                    b.HasKey("Id");

                    b.HasIndex("MessageTypeId");

                    b.HasIndex("ModbusDeviceId");

                    b.ToTable("ModbusContacts");
                });

            modelBuilder.Entity("Zupan.CodeReview.Domains.Modbus.ModbusDevices", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedByUserLogin");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("Description");

                    b.Property<int>("EdgeId");

                    b.Property<string>("IpAddress");

                    b.Property<bool>("IsOnline");

                    b.Property<string>("MACAdress");

                    b.Property<int>("MachineId");

                    b.Property<string>("Manufacturer");

                    b.Property<string>("Model");

                    b.Property<string>("ModifiedByUserLogin");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<string>("MoteGuid");

                    b.Property<int>("PollInterval");

                    b.Property<int>("Port");

                    b.Property<string>("Serial");

                    b.HasKey("Id");

                    b.ToTable("ModbusDevices");
                });

            modelBuilder.Entity("Zupan.CodeReview.Domains.Modbus.ModbusContacts", b =>
                {
                    b.HasOne("Zupan.CodeReview.Domains.Modbus.ModbusContactConfigurations", "MessageType")
                        .WithMany()
                        .HasForeignKey("MessageTypeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Zupan.CodeReview.Domains.Modbus.ModbusDevices", "ModbusDevice")
                        .WithMany()
                        .HasForeignKey("ModbusDeviceId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}