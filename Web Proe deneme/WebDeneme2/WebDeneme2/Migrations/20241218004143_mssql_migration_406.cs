using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebDeneme2.Migrations
{
    /// <inheritdoc />
    public partial class mssql_migration_406 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RandevuDurum",
                table: "Randevular",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RandevuDurum",
                table: "Randevular");
        }
    }
}
