using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PokerTime.API.Migrations
{
    public partial class addedTournamentTracking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "TournamentTrackings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsTournamentRunning = table.Column<bool>(type: "bit", nullable: false),
                    IsTimerRunning = table.Column<bool>(type: "bit", nullable: false),
                    TimeRemaining = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                name: "TournamentTrackings");
        }
    }
}
