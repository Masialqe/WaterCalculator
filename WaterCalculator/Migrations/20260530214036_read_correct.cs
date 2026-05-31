using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WaterCalculator.Migrations
{
    /// <inheritdoc />
    public partial class read_correct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_reads_apartments_ApartmentId",
                table: "reads");

            migrationBuilder.DropColumn(
                name: "amount",
                table: "reads");

            migrationBuilder.RenameColumn(
                name: "ApartmentId",
                table: "reads",
                newName: "apartment_id");

            migrationBuilder.RenameIndex(
                name: "IX_reads_ApartmentId",
                table: "reads",
                newName: "IX_reads_apartment_id");

            migrationBuilder.AddColumn<DateTime>(
                name: "read_date",
                table: "reads",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_reads_apartments_apartment_id",
                table: "reads",
                column: "apartment_id",
                principalTable: "apartments",
                principalColumn: "apartment_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_reads_apartments_apartment_id",
                table: "reads");

            migrationBuilder.DropColumn(
                name: "read_date",
                table: "reads");

            migrationBuilder.RenameColumn(
                name: "apartment_id",
                table: "reads",
                newName: "ApartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_reads_apartment_id",
                table: "reads",
                newName: "IX_reads_ApartmentId");

            migrationBuilder.AddColumn<double>(
                name: "amount",
                table: "reads",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddForeignKey(
                name: "FK_reads_apartments_ApartmentId",
                table: "reads",
                column: "ApartmentId",
                principalTable: "apartments",
                principalColumn: "apartment_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
