using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WaterCalculator.Migrations
{
    /// <inheritdoc />
    public partial class apartment_access : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "apartment_acces_code",
                table: "apartments",
                type: "TEXT",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "apartment_public_token",
                table: "apartments",
                type: "TEXT",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "apartment_acces_code",
                table: "apartments");

            migrationBuilder.DropColumn(
                name: "apartment_public_token",
                table: "apartments");
        }
    }
}
