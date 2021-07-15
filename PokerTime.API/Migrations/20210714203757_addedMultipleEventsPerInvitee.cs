using Microsoft.EntityFrameworkCore.Migrations;

namespace PokerTime.API.Migrations
{
    public partial class addedMultipleEventsPerInvitee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invitee_Events_EventId",
                table: "Invitee");

            migrationBuilder.DropIndex(
                name: "IX_Invitee_EventId",
                table: "Invitee");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Invitee");

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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventInvitee_Invitee_InviteesId",
                        column: x => x.InviteesId,
                        principalTable: "Invitee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventInvitee_InviteesId",
                table: "EventInvitee",
                column: "InviteesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventInvitee");

            migrationBuilder.AddColumn<int>(
                name: "EventId",
                table: "Invitee",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Invitee_EventId",
                table: "Invitee",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invitee_Events_EventId",
                table: "Invitee",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
