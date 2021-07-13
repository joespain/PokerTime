using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PokerTime.API.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPaidUser = table.Column<bool>(type: "bit", nullable: false),
                    EventId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TournamentStructures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HostId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NumberOfEvents = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentStructures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TournamentStructures_Users_HostId",
                        column: x => x.HostId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BlindLevel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TournamentStructureId = table.Column<int>(type: "int", nullable: false),
                    LevelNumber = table.Column<int>(type: "int", nullable: false),
                    SmallBlind = table.Column<int>(type: "int", nullable: false),
                    BigBlind = table.Column<int>(type: "int", nullable: false),
                    Ante = table.Column<int>(type: "int", nullable: false),
                    Minutes = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlindLevel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlindLevel_TournamentStructures_TournamentStructureId",
                        column: x => x.TournamentStructureId,
                        principalTable: "TournamentStructures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    HostId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StructureId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_TournamentStructures_StructureId",
                        column: x => x.StructureId,
                        principalTable: "TournamentStructures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Events_Users_HostId",
                        column: x => x.HostId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "TournamentStructures",
                columns: new[] { "Id", "DateCreated", "HostId", "Name", "NumberOfEvents" },
                values: new object[] { 1, new DateTime(2021, 7, 13, 0, 0, 0, 0, DateTimeKind.Local), null, "Joe's Structure of Champions!", 0 });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "EventId", "IsPaidUser", "Name", "Phone", "UserId" },
                values: new object[,]
                {
                    { new Guid("ba461614-db91-4f06-ab65-1ca507efaaee"), "MJSpain@gmail.com", null, true, "Mike Spain", "12354567890", null },
                    { new Guid("2065e32e-50d0-44a1-af0d-62b4a2f850e6"), "JimboSpain@gmail.com", null, false, "Jim Spain", "0987654321", null },
                    { new Guid("bdf42440-0a03-4c3d-a63b-99450b0f7c4e"), "Joe.Spain22@gmail.com", null, false, "Joe Spain", "7274094210", null }
                });

            migrationBuilder.InsertData(
                table: "BlindLevel",
                columns: new[] { "Id", "Ante", "BigBlind", "LevelNumber", "Minutes", "SmallBlind", "TournamentStructureId" },
                values: new object[] { 1, 20, 100, 1, 30, 200, 1 });

            migrationBuilder.InsertData(
                table: "BlindLevel",
                columns: new[] { "Id", "Ante", "BigBlind", "LevelNumber", "Minutes", "SmallBlind", "TournamentStructureId" },
                values: new object[] { 2, 25, 150, 2, 30, 250, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_BlindLevel_TournamentStructureId",
                table: "BlindLevel",
                column: "TournamentStructureId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_HostId",
                table: "Events",
                column: "HostId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_StructureId",
                table: "Events",
                column: "StructureId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentStructures_HostId",
                table: "TournamentStructures",
                column: "HostId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_EventId",
                table: "Users",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserId",
                table: "Users",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Events_EventId",
                table: "Users",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_TournamentStructures_StructureId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Users_HostId",
                table: "Events");

            migrationBuilder.DropTable(
                name: "BlindLevel");

            migrationBuilder.DropTable(
                name: "TournamentStructures");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Events");
        }
    }
}
