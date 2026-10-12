using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sienna.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InsightsPublicacoes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SOCIAL_PUBLICATION_INSIGHT_SNAPSHOTS",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    PUBLICATION_ID = table.Column<Guid>(type: "uuid", nullable: false),
                    COLLECTED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    VIEWS = table.Column<int>(type: "integer", nullable: true),
                    REACH = table.Column<int>(type: "integer", nullable: true),
                    LIKES = table.Column<int>(type: "integer", nullable: true),
                    COMMENTS = table.Column<int>(type: "integer", nullable: true),
                    SHARES = table.Column<int>(type: "integer", nullable: true),
                    SAVED = table.Column<int>(type: "integer", nullable: true),
                    REPLIES = table.Column<int>(type: "integer", nullable: true),
                    TOTAL_INTERACTIONS = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SOCIAL_PUBLICATION_INSIGHT_SNAPSHOTS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SOCIAL_PUBLICATION_INSIGHT_SNAPSHOTS_SOCIAL_POST_PUBLICATIO~",
                        column: x => x.PUBLICATION_ID,
                        principalTable: "SOCIAL_POST_PUBLICATIONS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SOCIAL_PUBLICATION_INSIGHT_SNAPSHOTS_PUBLICATION_ID_COLLECT~",
                table: "SOCIAL_PUBLICATION_INSIGHT_SNAPSHOTS",
                columns: new[] { "PUBLICATION_ID", "COLLECTED_AT" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SOCIAL_PUBLICATION_INSIGHT_SNAPSHOTS");
        }
    }
}
