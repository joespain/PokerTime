using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PokerTime.API.Migrations
{
    public partial class addedUserIdToHost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Hosts",
                keyColumn: "Id",
                keyValue: new Guid("ae6d1500-c4e9-4fa4-b862-e3b2c91f1a1d"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Hosts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Hosts",
                keyColumn: "Id",
                keyValue: new Guid("50059e0b-6809-4bb5-941f-9707849fa591"));

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Hosts");

        }
    }
}
