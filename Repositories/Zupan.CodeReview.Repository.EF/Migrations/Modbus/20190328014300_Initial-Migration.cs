using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Zupan.CodeReview.Repository.EF.Migrations.Modbus
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "ModbusContactConfigurations",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    CreatedByUserLogin = table.Column<string>(nullable: true),
                    ModifiedByUserLogin = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModbusContactConfigurations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ModbusDevices",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    CreatedByUserLogin = table.Column<string>(nullable: true),
                    ModifiedByUserLogin = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Manufacturer = table.Column<string>(nullable: true),
                    Model = table.Column<string>(nullable: true),
                    Serial = table.Column<string>(nullable: true),
                    MACAdress = table.Column<string>(nullable: true),
                    IpAddress = table.Column<string>(nullable: true),
                    EdgeId = table.Column<int>(nullable: false),
                    MachineId = table.Column<int>(nullable: false),
                    PollInterval = table.Column<int>(nullable: false),
                    IsOnline = table.Column<bool>(nullable: false),
                    Port = table.Column<int>(nullable: false),
                    MoteGuid = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModbusDevices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ModbusContacts",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    CreatedByUserLogin = table.Column<string>(nullable: true),
                    ModifiedByUserLogin = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Guid = table.Column<string>(nullable: true),
                    ModbusDeviceId = table.Column<int>(nullable: false),
                    ChannelNumber = table.Column<int>(nullable: false),
                    MessageTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModbusContacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModbusContacts_ModbusContactConfigurations_MessageTypeId",
                        column: x => x.MessageTypeId,
                        principalSchema: "public",
                        principalTable: "ModbusContactConfigurations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModbusContacts_ModbusDevices_ModbusDeviceId",
                        column: x => x.ModbusDeviceId,
                        principalSchema: "public",
                        principalTable: "ModbusDevices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ModbusContacts_MessageTypeId",
                schema: "public",
                table: "ModbusContacts",
                column: "MessageTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ModbusContacts_ModbusDeviceId",
                schema: "public",
                table: "ModbusContacts",
                column: "ModbusDeviceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ModbusContacts",
                schema: "public");

            migrationBuilder.DropTable(
                name: "ModbusContactConfigurations",
                schema: "public");

            migrationBuilder.DropTable(
                name: "ModbusDevices",
                schema: "public");
        }
    }
}
