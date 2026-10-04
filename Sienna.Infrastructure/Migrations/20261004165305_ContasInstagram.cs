using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sienna.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ContasInstagram : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SOCIAL_INSTAGRAM_ACCOUNTS",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    TEAM_ID = table.Column<Guid>(type: "uuid", nullable: false),
                    INSTAGRAM_USER_ID = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    USERNAME = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    PROTECTED_ACCESS_TOKEN = table.Column<string>(type: "text", nullable: false),
                    CONNECTED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SOCIAL_INSTAGRAM_ACCOUNTS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SOCIAL_INSTAGRAM_ACCOUNTS_WORKFLOW_TEAMS_TEAM_ID",
                        column: x => x.TEAM_ID,
                        principalTable: "WORKFLOW_TEAMS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SOCIAL_INSTAGRAM_ACCOUNTS_TEAM_ID",
                table: "SOCIAL_INSTAGRAM_ACCOUNTS",
                column: "TEAM_ID",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SOCIAL_INSTAGRAM_ACCOUNTS");
        }
    }
}
