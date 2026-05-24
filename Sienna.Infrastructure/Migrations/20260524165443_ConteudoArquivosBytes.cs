using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sienna.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ConteudoArquivosBytes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"ALTER TABLE ""MEDIA_MEDIAS"" ALTER COLUMN ""CONTENT"" TYPE bytea USING decode(""CONTENT"", 'base64');");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"ALTER TABLE ""MEDIA_MEDIAS"" ALTER COLUMN ""CONTENT"" TYPE bytea USING decode(""CONTENT"", 'base64');");
        }
    }
}
