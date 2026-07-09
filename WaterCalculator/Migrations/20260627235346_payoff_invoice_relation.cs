using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WaterCalculator.Migrations
{
    /// <inheritdoc />
    public partial class payoff_invoice_relation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "InvoiceId",
                table: "Payoffs",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "PayoffId",
                table: "invoices",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_invoices_PayoffId",
                table: "invoices",
                column: "PayoffId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_invoices_Payoffs_PayoffId",
                table: "invoices",
                column: "PayoffId",
                principalTable: "Payoffs",
                principalColumn: "payoff_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_invoices_Payoffs_PayoffId",
                table: "invoices");

            migrationBuilder.DropIndex(
                name: "IX_invoices_PayoffId",
                table: "invoices");

            migrationBuilder.DropColumn(
                name: "InvoiceId",
                table: "Payoffs");

            migrationBuilder.DropColumn(
                name: "PayoffId",
                table: "invoices");
        }
    }
}
