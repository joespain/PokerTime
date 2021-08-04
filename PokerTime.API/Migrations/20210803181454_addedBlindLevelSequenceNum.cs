using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PokerTime.API.Migrations
{
    public partial class addedBlindLevelSequenceNum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DeleteData(
            //    table: "Hosts",
            //    keyColumn: "Id",
            //    keyValue: new Guid("48b51074-220e-4275-b3f6-ed41b8319832"));

            migrationBuilder.AddColumn<int>(
                name: "SequenceNum",
                table: "BlindLevels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            //migrationBuilder.InsertData(
            //    table: "Hosts",
            //    columns: new[] { "Id", "Email", "IsPaidUser", "Name", "Phone" },
            //    values: new object[] { new Guid("41e91ebb-1e8d-4543-a25a-7a82edc1df12"), "JimboSpain@gmail.com", true, "Jim Spain", "0987654321" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //    migrationBuilder.DeleteData(
            //        table: "Hosts",
            //        keyColumn: "Id",
            //        keyValue: new Guid("41e91ebb-1e8d-4543-a25a-7a82edc1df12"));

            migrationBuilder.DropColumn(
                name: "SequenceNum",
                table: "BlindLevels");

            //migrationBuilder.InsertData(
            //    table: "Hosts",
            //    columns: new[] { "Id", "Email", "IsPaidUser", "Name", "Phone" },
            //    values: new object[] { new Guid("48b51074-220e-4275-b3f6-ed41b8319832"), "JimboSpain@gmail.com", true, "Jim Spain", "0987654321" });
        }
    }
}
