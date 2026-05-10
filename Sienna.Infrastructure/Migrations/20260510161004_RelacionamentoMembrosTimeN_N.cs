using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sienna.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RelacionamentoMembrosTimeN_N : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IDENTITY_USER_CLAIM_IDENTITY_USER_USER_ID",
                table: "IDENTITY_USER_CLAIM");

            migrationBuilder.DropForeignKey(
                name: "FK_IDENTITY_USER_LOGIN_IDENTITY_USER_USER_ID",
                table: "IDENTITY_USER_LOGIN");

            migrationBuilder.DropForeignKey(
                name: "FK_IDENTITY_USER_ROLE_IDENTITY_USER_USER_ID",
                table: "IDENTITY_USER_ROLE");

            migrationBuilder.DropForeignKey(
                name: "FK_IDENTITY_USER_TOKEN_IDENTITY_USER_USER_ID",
                table: "IDENTITY_USER_TOKEN");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IDENTITY_USER",
                table: "IDENTITY_USER");

            migrationBuilder.RenameTable(
                name: "IDENTITY_USER",
                newName: "IDENTITY_USERS");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IDENTITY_USERS",
                table: "IDENTITY_USERS",
                column: "ID");

            migrationBuilder.CreateTable(
                name: "WORKFLOW_TEAMS",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    NAME = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    CREATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WORKFLOW_TEAMS", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "WORKFLOW_TEAM_MEMBERS",
                columns: table => new
                {
                    TEAM_ID = table.Column<Guid>(type: "uuid", nullable: false),
                    MEMBER_ID = table.Column<Guid>(type: "uuid", nullable: false),
                    ASSOCIATION_DATE = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ROLE = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WORKFLOW_TEAM_MEMBERS", x => new { x.TEAM_ID, x.MEMBER_ID });
                    table.ForeignKey(
                        name: "FK_WORKFLOW_TEAM_MEMBERS_IDENTITY_USERS_MEMBER_ID",
                        column: x => x.MEMBER_ID,
                        principalTable: "IDENTITY_USERS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WORKFLOW_TEAM_MEMBERS_WORKFLOW_TEAMS_TEAM_ID",
                        column: x => x.TEAM_ID,
                        principalTable: "WORKFLOW_TEAMS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WORKFLOW_TEAM_MEMBERS_MEMBER_ID",
                table: "WORKFLOW_TEAM_MEMBERS",
                column: "MEMBER_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_IDENTITY_USER_CLAIM_IDENTITY_USERS_USER_ID",
                table: "IDENTITY_USER_CLAIM",
                column: "USER_ID",
                principalTable: "IDENTITY_USERS",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IDENTITY_USER_LOGIN_IDENTITY_USERS_USER_ID",
                table: "IDENTITY_USER_LOGIN",
                column: "USER_ID",
                principalTable: "IDENTITY_USERS",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IDENTITY_USER_ROLE_IDENTITY_USERS_USER_ID",
                table: "IDENTITY_USER_ROLE",
                column: "USER_ID",
                principalTable: "IDENTITY_USERS",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IDENTITY_USER_TOKEN_IDENTITY_USERS_USER_ID",
                table: "IDENTITY_USER_TOKEN",
                column: "USER_ID",
                principalTable: "IDENTITY_USERS",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IDENTITY_USER_CLAIM_IDENTITY_USERS_USER_ID",
                table: "IDENTITY_USER_CLAIM");

            migrationBuilder.DropForeignKey(
                name: "FK_IDENTITY_USER_LOGIN_IDENTITY_USERS_USER_ID",
                table: "IDENTITY_USER_LOGIN");

            migrationBuilder.DropForeignKey(
                name: "FK_IDENTITY_USER_ROLE_IDENTITY_USERS_USER_ID",
                table: "IDENTITY_USER_ROLE");

            migrationBuilder.DropForeignKey(
                name: "FK_IDENTITY_USER_TOKEN_IDENTITY_USERS_USER_ID",
                table: "IDENTITY_USER_TOKEN");

            migrationBuilder.DropTable(
                name: "WORKFLOW_TEAM_MEMBERS");

            migrationBuilder.DropTable(
                name: "WORKFLOW_TEAMS");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IDENTITY_USERS",
                table: "IDENTITY_USERS");

            migrationBuilder.RenameTable(
                name: "IDENTITY_USERS",
                newName: "IDENTITY_USER");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IDENTITY_USER",
                table: "IDENTITY_USER",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_IDENTITY_USER_CLAIM_IDENTITY_USER_USER_ID",
                table: "IDENTITY_USER_CLAIM",
                column: "USER_ID",
                principalTable: "IDENTITY_USER",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IDENTITY_USER_LOGIN_IDENTITY_USER_USER_ID",
                table: "IDENTITY_USER_LOGIN",
                column: "USER_ID",
                principalTable: "IDENTITY_USER",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IDENTITY_USER_ROLE_IDENTITY_USER_USER_ID",
                table: "IDENTITY_USER_ROLE",
                column: "USER_ID",
                principalTable: "IDENTITY_USER",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IDENTITY_USER_TOKEN_IDENTITY_USER_USER_ID",
                table: "IDENTITY_USER_TOKEN",
                column: "USER_ID",
                principalTable: "IDENTITY_USER",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
