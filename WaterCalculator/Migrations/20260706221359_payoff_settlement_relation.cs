using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WaterCalculator.Migrations
{
    /// <inheritdoc />
    public partial class payoff_settlement_relation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PayoffId",
                table: "settlements",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_settlements_PayoffId",
                table: "settlements",
                column: "PayoffId");

            migrationBuilder.AddForeignKey(
                name: "FK_settlements_Payoffs_PayoffId",
                table: "settlements",
                column: "PayoffId",
                principalTable: "Payoffs",
                principalColumn: "payoff_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_settlements_Payoffs_PayoffId",
                table: "settlements");

            migrationBuilder.DropIndex(
                name: "IX_settlements_PayoffId",
                table: "settlements");

            migrationBuilder.DropColumn(
                name: "PayoffId",
                table: "settlements");
        }
    }
}
