using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Sienna.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialIdentitySchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IDENTITY_ROLE",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    NAME = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NORMALIZED_NAME = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CONCURRENCY_STAMP = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IDENTITY_ROLE", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "IDENTITY_USER",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    FULL_NAME = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    USER_NAME = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NORMALIZED_USER_NAME = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EMAIL = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NORMALIZED_EMAIL = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EMAIL_CONFIRMED = table.Column<bool>(type: "boolean", nullable: false),
                    PASSWORD_HASH = table.Column<string>(type: "text", nullable: true),
                    SECURITY_STAMP = table.Column<string>(type: "text", nullable: true),
                    CONCURRENCY_STAMP = table.Column<string>(type: "text", nullable: true),
                    PHONE_NUMBER = table.Column<string>(type: "text", nullable: true),
                    PHONE_NUMBER_CONFIRMED = table.Column<bool>(type: "boolean", nullable: false),
                    TWO_FACTOR_ENABLED = table.Column<bool>(type: "boolean", nullable: false),
                    LOCKOUT_END = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LOCKOUT_ENABLED = table.Column<bool>(type: "boolean", nullable: false),
                    ACCESS_FAILED_COUNT = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IDENTITY_USER", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "IDENTITY_ROLE_CLAIM",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ROLE_ID = table.Column<Guid>(type: "uuid", nullable: false),
                    CLAIM_TYPE = table.Column<string>(type: "text", nullable: true),
                    CLAIM_VALUE = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IDENTITY_ROLE_CLAIM", x => x.ID);
                    table.ForeignKey(
                        name: "FK_IDENTITY_ROLE_CLAIM_IDENTITY_ROLE_ROLE_ID",
                        column: x => x.ROLE_ID,
                        principalTable: "IDENTITY_ROLE",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IDENTITY_USER_CLAIM",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    USER_ID = table.Column<Guid>(type: "uuid", nullable: false),
                    CLAIM_TYPE = table.Column<string>(type: "text", nullable: true),
                    CLAIM_VALUE = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IDENTITY_USER_CLAIM", x => x.ID);
                    table.ForeignKey(
                        name: "FK_IDENTITY_USER_CLAIM_IDENTITY_USER_USER_ID",
                        column: x => x.USER_ID,
                        principalTable: "IDENTITY_USER",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IDENTITY_USER_LOGIN",
                columns: table => new
                {
                    LOGIN_PROVIDER = table.Column<string>(type: "text", nullable: false),
                    PROVIDER_KEY = table.Column<string>(type: "text", nullable: false),
                    PROVIDER_DISPLAY_NAME = table.Column<string>(type: "text", nullable: true),
                    USER_ID = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IDENTITY_USER_LOGIN", x => new { x.LOGIN_PROVIDER, x.PROVIDER_KEY });
                    table.ForeignKey(
                        name: "FK_IDENTITY_USER_LOGIN_IDENTITY_USER_USER_ID",
                        column: x => x.USER_ID,
                        principalTable: "IDENTITY_USER",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IDENTITY_USER_ROLE",
                columns: table => new
                {
                    USER_ID = table.Column<Guid>(type: "uuid", nullable: false),
                    ROLE_ID = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IDENTITY_USER_ROLE", x => new { x.USER_ID, x.ROLE_ID });
                    table.ForeignKey(
                        name: "FK_IDENTITY_USER_ROLE_IDENTITY_ROLE_ROLE_ID",
                        column: x => x.ROLE_ID,
                        principalTable: "IDENTITY_ROLE",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IDENTITY_USER_ROLE_IDENTITY_USER_USER_ID",
                        column: x => x.USER_ID,
                        principalTable: "IDENTITY_USER",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IDENTITY_USER_TOKEN",
                columns: table => new
                {
                    USER_ID = table.Column<Guid>(type: "uuid", nullable: false),
                    LOGIN_PROVIDER = table.Column<string>(type: "text", nullable: false),
                    NAME = table.Column<string>(type: "text", nullable: false),
                    VALUE = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IDENTITY_USER_TOKEN", x => new { x.USER_ID, x.LOGIN_PROVIDER, x.NAME });
                    table.ForeignKey(
                        name: "FK_IDENTITY_USER_TOKEN_IDENTITY_USER_USER_ID",
                        column: x => x.USER_ID,
                        principalTable: "IDENTITY_USER",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "IDENTITY_ROLE",
                column: "NORMALIZED_NAME",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IDENTITY_ROLE_CLAIM_ROLE_ID",
                table: "IDENTITY_ROLE_CLAIM",
                column: "ROLE_ID");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "IDENTITY_USER",
                column: "NORMALIZED_EMAIL");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "IDENTITY_USER",
                column: "NORMALIZED_USER_NAME",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IDENTITY_USER_CLAIM_USER_ID",
                table: "IDENTITY_USER_CLAIM",
                column: "USER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_IDENTITY_USER_LOGIN_USER_ID",
                table: "IDENTITY_USER_LOGIN",
                column: "USER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_IDENTITY_USER_ROLE_ROLE_ID",
                table: "IDENTITY_USER_ROLE",
                column: "ROLE_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IDENTITY_ROLE_CLAIM");

            migrationBuilder.DropTable(
                name: "IDENTITY_USER_CLAIM");

            migrationBuilder.DropTable(
                name: "IDENTITY_USER_LOGIN");

            migrationBuilder.DropTable(
                name: "IDENTITY_USER_ROLE");

            migrationBuilder.DropTable(
                name: "IDENTITY_USER_TOKEN");

            migrationBuilder.DropTable(
                name: "IDENTITY_ROLE");

            migrationBuilder.DropTable(
                name: "IDENTITY_USER");
        }
    }
}
