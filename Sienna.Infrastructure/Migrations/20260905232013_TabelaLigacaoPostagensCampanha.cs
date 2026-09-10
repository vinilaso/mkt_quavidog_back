using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sienna.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TabelaLigacaoPostagensCampanha : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WORKFLOW_CAMPAIGN_POSTS",
                columns: table => new
                {
                    POST_ID = table.Column<Guid>(type: "uuid", nullable: false),
                    CAMPAIGN_ID = table.Column<Guid>(type: "uuid", nullable: false),
                    ASSIGNMENT_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WORKFLOW_CAMPAIGN_POSTS", x => new { x.POST_ID, x.CAMPAIGN_ID });
                    table.ForeignKey(
                        name: "FK_WORKFLOW_CAMPAIGN_POSTS_MEDIA_POSTS_POST_ID",
                        column: x => x.POST_ID,
                        principalTable: "MEDIA_POSTS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WORKFLOW_CAMPAIGN_POSTS_WORKFLOW_CAMPAIGNS_CAMPAIGN_ID",
                        column: x => x.CAMPAIGN_ID,
                        principalTable: "WORKFLOW_CAMPAIGNS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WORKFLOW_CAMPAIGN_POSTS_CAMPAIGN_ID",
                table: "WORKFLOW_CAMPAIGN_POSTS",
                column: "CAMPAIGN_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WORKFLOW_CAMPAIGN_POSTS");
        }
    }
}
