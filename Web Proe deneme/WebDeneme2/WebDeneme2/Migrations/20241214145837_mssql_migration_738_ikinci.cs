using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebDeneme2.Migrations
{
    /// <inheritdoc />
    public partial class mssql_migration_738_ikinci : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Aciklama",
                table: "Hizmetler",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Hizmetler",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Aciklama",
                table: "Hizmetler");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Hizmetler");
        }
    }
}
