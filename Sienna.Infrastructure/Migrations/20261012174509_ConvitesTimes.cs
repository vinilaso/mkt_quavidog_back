using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sienna.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ConvitesTimes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WORKFLOW_TEAM_INVITATIONS",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    TEAM_ID = table.Column<Guid>(type: "uuid", nullable: false),
                    EMAIL = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    ROLE = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    STATUS = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    INVITED_BY_ID = table.Column<Guid>(type: "uuid", nullable: false),
                    CREATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EXPIRES_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RESPONDED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WORKFLOW_TEAM_INVITATIONS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_WORKFLOW_TEAM_INVITATIONS_IDENTITY_USERS_INVITED_BY_ID",
                        column: x => x.INVITED_BY_ID,
                        principalTable: "IDENTITY_USERS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WORKFLOW_TEAM_INVITATIONS_WORKFLOW_TEAMS_TEAM_ID",
                        column: x => x.TEAM_ID,
                        principalTable: "WORKFLOW_TEAMS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WORKFLOW_TEAM_INVITATIONS_EMAIL_STATUS",
                table: "WORKFLOW_TEAM_INVITATIONS",
                columns: new[] { "EMAIL", "STATUS" });

            migrationBuilder.CreateIndex(
                name: "IX_WORKFLOW_TEAM_INVITATIONS_INVITED_BY_ID",
                table: "WORKFLOW_TEAM_INVITATIONS",
                column: "INVITED_BY_ID");

            migrationBuilder.CreateIndex(
                name: "IX_WORKFLOW_TEAM_INVITATIONS_TEAM_ID_EMAIL",
                table: "WORKFLOW_TEAM_INVITATIONS",
                columns: new[] { "TEAM_ID", "EMAIL" },
                unique: true,
                filter: "\"STATUS\" = 'Pending'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WORKFLOW_TEAM_INVITATIONS");
        }
    }
}
