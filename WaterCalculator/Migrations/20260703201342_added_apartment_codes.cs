using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WaterCalculator.Migrations
{
    /// <inheritdoc />
    public partial class added_apartment_codes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "apartment_acces_code",
                table: "apartments");

            migrationBuilder.AddColumn<Guid>(
                name: "AccessCodeId",
                table: "apartments",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "apartment_access_code",
                columns: table => new
                {
                    apartment_code_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    apartment_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    apartment_access_code = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    apartment_code_created_at = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_apartment_access_code", x => x.apartment_code_id);
                    table.ForeignKey(
                        name: "FK_apartment_access_code_apartments_apartment_id",
                        column: x => x.apartment_id,
                        principalTable: "apartments",
                        principalColumn: "apartment_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_apartment_access_code_apartment_id",
                table: "apartment_access_code",
                column: "apartment_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "apartment_access_code");

            migrationBuilder.DropColumn(
                name: "AccessCodeId",
                table: "apartments");

            migrationBuilder.AddColumn<string>(
                name: "apartment_acces_code",
                table: "apartments",
                type: "TEXT",
                maxLength: 255,
                nullable: true);
        }
    }
}
