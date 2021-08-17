using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PokerTime.API.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Hosts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPaidUser = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hosts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TournamentStructureId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_Hosts_HostId",
                        column: x => x.HostId,
                        principalTable: "Hosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    HostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invitees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invitees_Hosts_HostId",
                        column: x => x.HostId,
                        principalTable: "Hosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    HostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentStructures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TournamentStructures_Hosts_HostId",
                        column: x => x.HostId,
                        principalTable: "Hosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventInvitee",
                columns: table => new
                {
                    EventsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventInvitee_Invitees_InviteesId",
                        column: x => x.InviteesId,
                        principalTable: "Invitees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BlindLevels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TournamentStructureId = table.Column<int>(type: "int", nullable: false),
                    SmallBlind = table.Column<int>(type: "int", nullable: false),
                    BigBlind = table.Column<int>(type: "int", nullable: false),
                    Ante = table.Column<int>(type: "int", nullable: false),
                    Minutes = table.Column<int>(type: "int", nullable: false),
                    SequenceNum = table.Column<int>(type: "int", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "TournamentTrackings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsTournamentRunning = table.Column<bool>(type: "bit", nullable: false),
                    IsTimerRunning = table.Column<bool>(type: "bit", nullable: false),
                    TimeRemaining = table.Column<TimeSpan>(type: "time", nullable: false),
                    CurrentBlindLevelId = table.Column<int>(type: "int", nullable: true),
                    NextBlindLevelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentTrackings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TournamentTrackings_BlindLevels_CurrentBlindLevelId",
                        column: x => x.CurrentBlindLevelId,
                        principalTable: "BlindLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TournamentTrackings_BlindLevels_NextBlindLevelId",
                        column: x => x.NextBlindLevelId,
                        principalTable: "BlindLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Hosts",
                columns: new[] { "Id", "Email", "IsPaidUser", "Name", "Phone" },
                values: new object[] { new Guid("0e9771b9-2112-4256-8594-7cd3af8d34ee"), "JimboSpain@gmail.com", true, "Jim Spain", "0987654321" });

            migrationBuilder.CreateIndex(
                name: "IX_BlindLevels_TournamentStructureId",
                table: "BlindLevels",
                column: "TournamentStructureId");

            migrationBuilder.CreateIndex(
                name: "IX_EventInvitee_InviteesId",
                table: "EventInvitee",
                column: "InviteesId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_HostId",
                table: "Events",
                column: "HostId");

            migrationBuilder.CreateIndex(
                name: "IX_Invitees_HostId",
                table: "Invitees",
                column: "HostId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentStructures_HostId",
                table: "TournamentStructures",
                column: "HostId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentTrackings_CurrentBlindLevelId",
                table: "TournamentTrackings",
                column: "CurrentBlindLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentTrackings_NextBlindLevelId",
                table: "TournamentTrackings",
                column: "NextBlindLevelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventInvitee");

            migrationBuilder.DropTable(
                name: "TournamentTrackings");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Invitees");

            migrationBuilder.DropTable(
                name: "BlindLevels");

            migrationBuilder.DropTable(
                name: "TournamentStructures");

            migrationBuilder.DropTable(
                name: "Hosts");
        }
    }
}
