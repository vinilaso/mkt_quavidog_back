using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sienna.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCampaignEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WORKFLOW_CAMPAIGNS",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    TEAM_ID = table.Column<Guid>(type: "uuid", nullable: true),
                    NAME = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    CREATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    STATUS = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WORKFLOW_CAMPAIGNS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_WORKFLOW_CAMPAIGNS_WORKFLOW_TEAMS_TEAM_ID",
                        column: x => x.TEAM_ID,
                        principalTable: "WORKFLOW_TEAMS",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_WORKFLOW_CAMPAIGNS_TEAM_ID",
                table: "WORKFLOW_CAMPAIGNS",
                column: "TEAM_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WORKFLOW_CAMPAIGNS");
        }
    }
}
