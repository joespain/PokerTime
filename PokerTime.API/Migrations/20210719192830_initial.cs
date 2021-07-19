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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPaidUser = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TournamentStructures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NumberOfEvents = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentStructures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TournamentStructures_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
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
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    TournamentStructureId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Events_TournamentStructures_TournamentStructureId",
                        column: x => x.TournamentStructureId,
                        principalTable: "TournamentStructures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Invitees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invitees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invitees_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            

            migrationBuilder.CreateTable(
                name: "EventInvitee",
                columns: table => new
                {
                    EventsId = table.Column<int>(type: "int", nullable: false),
                    InviteesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventInvitee", x => new { x.EventsId, x.InviteesId });
                    table.ForeignKey(
                        name: "FK_EventInvitee_Events_EventsId",
                        column: x => x.EventsId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_EventInvitee_Invitees_InviteesId",
                        column: x => x.InviteesId,
                        principalTable: "Invitees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "BlindLevels",
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
                    table.PrimaryKey("PK_BlindLevels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlindLevels_TournamentStructures_TournamentStructureId",
                        column: x => x.TournamentStructureId,
                        principalTable: "TournamentStructures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "IsPaidUser", "Name", "Phone" },
                values: new object[] { 3, "MJSpain@gmail.com", true, "Mike Spain", "12354567890" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "IsPaidUser", "Name", "Phone" },
                values: new object[] { 1, "JimboSpain@gmail.com", false, "Jim Spain", "0987654321" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "IsPaidUser", "Name", "Phone" },
                values: new object[] { 2, "Joe.Spain22@gmail.com", false, "Joe Spain", "7274094210" });

            migrationBuilder.CreateIndex(
                name: "IX_BlindLevels_TournamentStructureId",
                table: "BlindLevels",
                column: "TournamentStructureId");

            migrationBuilder.CreateIndex(
                name: "IX_EventInvitee_InviteesId",
                table: "EventInvitee",
                column: "InviteesId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_UserId",
                table: "Events",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_TournamentStructureId",
                table: "Events",
                column: "TournamentStructureId");

            migrationBuilder.CreateIndex(
                name: "IX_Invitees_UserId",
                table: "Invitees",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentStructures_UserId",
                table: "TournamentStructures",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlindLevels");

            migrationBuilder.DropTable(
                name: "EventInvitee");

            migrationBuilder.DropTable(
                name: "TournamentStructures");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Invitees");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
