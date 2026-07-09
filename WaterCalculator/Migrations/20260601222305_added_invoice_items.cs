using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WaterCalculator.Migrations
{
    /// <inheritdoc />
    public partial class added_invoice_items : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "invoice_price_per_unit",
                table: "invoices");

            migrationBuilder.CreateTable(
                name: "invoice_items",
                columns: table => new
                {
                    ii_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ii_invoice_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ii_name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    ii_amount = table.Column<decimal>(type: "TEXT", nullable: false),
                    CalculationType = table.Column<int>(type: "ii_calculation_type", nullable: false),
                    ii_created_at = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_invoice_items", x => x.ii_id);
                    table.ForeignKey(
                        name: "FK_invoice_items_invoices_ii_invoice_id",
                        column: x => x.ii_invoice_id,
                        principalTable: "invoices",
                        principalColumn: "invoice_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_invoice_items_ii_invoice_id",
                table: "invoice_items",
                column: "ii_invoice_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "invoice_items");

            migrationBuilder.AddColumn<decimal>(
                name: "invoice_price_per_unit",
                table: "invoices",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
