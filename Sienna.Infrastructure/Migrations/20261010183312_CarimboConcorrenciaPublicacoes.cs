using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sienna.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CarimboConcorrenciaPublicacoes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CONCURRENCY_STAMP",
                table: "SOCIAL_POST_PUBLICATIONS",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CONCURRENCY_STAMP",
                table: "SOCIAL_POST_PUBLICATIONS");
        }
    }
}
