using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sienna.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PublicacoesInstagram : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SOCIAL_POST_PUBLICATIONS",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    POST_ID = table.Column<Guid>(type: "uuid", nullable: false),
                    TEAM_ID = table.Column<Guid>(type: "uuid", nullable: false),
                    FORMAT = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    STATUS = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    SCHEDULED_FOR = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    REQUESTED_BY_ID = table.Column<Guid>(type: "uuid", nullable: false),
                    REQUESTED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    REVIEWED_BY_ID = table.Column<Guid>(type: "uuid", nullable: true),
                    REVIEWED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    REJECTION_REASON = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    PUBLISHED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EXTERNAL_IDS = table.Column<string[]>(type: "text[]", nullable: false),
                    FAILURE_REASON = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SOCIAL_POST_PUBLICATIONS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SOCIAL_POST_PUBLICATIONS_IDENTITY_USERS_REQUESTED_BY_ID",
                        column: x => x.REQUESTED_BY_ID,
                        principalTable: "IDENTITY_USERS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SOCIAL_POST_PUBLICATIONS_IDENTITY_USERS_REVIEWED_BY_ID",
                        column: x => x.REVIEWED_BY_ID,
                        principalTable: "IDENTITY_USERS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_SOCIAL_POST_PUBLICATIONS_MEDIA_POSTS_POST_ID",
                        column: x => x.POST_ID,
                        principalTable: "MEDIA_POSTS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SOCIAL_POST_PUBLICATIONS_WORKFLOW_TEAMS_TEAM_ID",
                        column: x => x.TEAM_ID,
                        principalTable: "WORKFLOW_TEAMS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SOCIAL_POST_PUBLICATIONS_POST_ID_FORMAT",
                table: "SOCIAL_POST_PUBLICATIONS",
                columns: new[] { "POST_ID", "FORMAT" },
                unique: true,
                filter: "\"STATUS\" IN ('PendingApproval', 'Approved', 'Publishing', 'Published')");

            migrationBuilder.CreateIndex(
                name: "IX_SOCIAL_POST_PUBLICATIONS_REQUESTED_BY_ID",
                table: "SOCIAL_POST_PUBLICATIONS",
                column: "REQUESTED_BY_ID");

            migrationBuilder.CreateIndex(
                name: "IX_SOCIAL_POST_PUBLICATIONS_REVIEWED_BY_ID",
                table: "SOCIAL_POST_PUBLICATIONS",
                column: "REVIEWED_BY_ID");

            migrationBuilder.CreateIndex(
                name: "IX_SOCIAL_POST_PUBLICATIONS_STATUS_SCHEDULED_FOR",
                table: "SOCIAL_POST_PUBLICATIONS",
                columns: new[] { "STATUS", "SCHEDULED_FOR" });

            migrationBuilder.CreateIndex(
                name: "IX_SOCIAL_POST_PUBLICATIONS_TEAM_ID_SCHEDULED_FOR",
                table: "SOCIAL_POST_PUBLICATIONS",
                columns: new[] { "TEAM_ID", "SCHEDULED_FOR" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SOCIAL_POST_PUBLICATIONS");
        }
    }
}
