using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PokerTime.API.Migrations
{
    public partial class addedEventLinkId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DeleteData(
            //    table: "Hosts",
            //    keyColumn: "Id",
            //    keyValue: new Guid("41e91ebb-1e8d-4543-a25a-7a82edc1df12"));

            migrationBuilder.AddColumn<Guid>(
                name: "EventLinkId",
                table: "Events",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            //migrationBuilder.InsertData(
            //    table: "Hosts",
            //    columns: new[] { "Id", "Email", "IsPaidUser", "Name", "Phone" },
            //    values: new object[] { new Guid("1cb984ea-96b7-4a35-8966-96786b385534"), "JimboSpain@gmail.com", true, "Jim Spain", "0987654321" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DeleteData(
            //    table: "Hosts",
            //    keyColumn: "Id",
            //    keyValue: new Guid("1cb984ea-96b7-4a35-8966-96786b385534"));

            migrationBuilder.DropColumn(
                name: "EventLinkId",
                table: "Events");

            //migrationBuilder.InsertData(
            //    table: "Hosts",
            //    columns: new[] { "Id", "Email", "IsPaidUser", "Name", "Phone" },
            //    values: new object[] { new Guid("41e91ebb-1e8d-4543-a25a-7a82edc1df12"), "JimboSpain@gmail.com", true, "Jim Spain", "0987654321" });
        }
    }
}
