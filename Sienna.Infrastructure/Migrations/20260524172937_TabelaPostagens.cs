using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sienna.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TabelaPostagens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MEDIA_POSTS",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    AUTHOR_ID = table.Column<Guid>(type: "uuid", nullable: true),
                    CAPTION = table.Column<string>(type: "text", nullable: false),
                    CREATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    STATUS = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MEDIA_POSTS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MEDIA_POSTS_IDENTITY_USERS_AUTHOR_ID",
                        column: x => x.AUTHOR_ID,
                        principalTable: "IDENTITY_USERS",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MEDIA_POSTS_AUTHOR_ID",
                table: "MEDIA_POSTS",
                column: "AUTHOR_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MEDIA_POSTS");
        }
    }
}
