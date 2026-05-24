using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sienna.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TabelaAssets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MEDIA_POST_ASSETS",
                columns: table => new
                {
                    POST_ID = table.Column<Guid>(type: "uuid", nullable: false),
                    MEDIA_ID = table.Column<Guid>(type: "uuid", nullable: false),
                    SEQUENCE_ORDER = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MEDIA_POST_ASSETS", x => new { x.POST_ID, x.MEDIA_ID });
                    table.ForeignKey(
                        name: "FK_MEDIA_POST_ASSETS_MEDIA_MEDIAS_MEDIA_ID",
                        column: x => x.MEDIA_ID,
                        principalTable: "MEDIA_MEDIAS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MEDIA_POST_ASSETS_MEDIA_POSTS_POST_ID",
                        column: x => x.POST_ID,
                        principalTable: "MEDIA_POSTS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MEDIA_POST_ASSETS_MEDIA_ID",
                table: "MEDIA_POST_ASSETS",
                column: "MEDIA_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MEDIA_POST_ASSETS");
        }
    }
}
