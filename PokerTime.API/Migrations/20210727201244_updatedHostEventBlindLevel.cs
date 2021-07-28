using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PokerTime.API.Migrations
{
    public partial class updatedHostEventBlindLevel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Hosts",
                keyColumn: "Id",
                keyValue: new Guid("8c13e4c0-43d8-4e44-855b-0d6683cac1aa"));

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "LevelNumber",
                table: "BlindLevels");

            migrationBuilder.InsertData(
                table: "Hosts",
                columns: new[] { "Id", "Email", "IsPaidUser", "Name", "Phone" },
                values: new object[] { new Guid("48b51074-220e-4275-b3f6-ed41b8319832"), "JimboSpain@gmail.com", true, "Jim Spain", "0987654321" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Hosts",
                keyColumn: "Id",
                keyValue: new Guid("48b51074-220e-4275-b3f6-ed41b8319832"));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Events",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "LevelNumber",
                table: "BlindLevels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Hosts",
                columns: new[] { "Id", "Email", "IsPaidUser", "Name", "Phone" },
                values: new object[] { new Guid("8c13e4c0-43d8-4e44-855b-0d6683cac1aa"), "JimboSpain@gmail.com", true, "Jim Spain", "0987654321" });
        }
    }
}
