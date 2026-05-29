using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WaterCalculator.Migrations
{
    /// <inheritdoc />
    public partial class initial_migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "groups",
                columns: table => new
                {
                    group_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    group_name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    group_details = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_groups", x => x.group_id);
                });

            migrationBuilder.CreateTable(
                name: "invoices",
                columns: table => new
                {
                    invoice_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    invoice_name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    invoice_number = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    invoice_total_price = table.Column<decimal>(type: "TEXT", nullable: false),
                    invoice_total_consumption = table.Column<decimal>(type: "TEXT", nullable: false),
                    invoice_period_from = table.Column<DateTime>(type: "TEXT", nullable: false),
                    invoice_period_to = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_invoices", x => x.invoice_id);
                });

            migrationBuilder.CreateTable(
                name: "apartments",
                columns: table => new
                {
                    apartment_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    apartment_name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    apartment_details = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    GroupId = table.Column<Guid>(type: "TEXT", nullable: true),
                    apartment_created_at = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_apartments", x => x.apartment_id);
                    table.ForeignKey(
                        name: "FK_apartments_groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "groups",
                        principalColumn: "group_id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "reads",
                columns: table => new
                {
                    read_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    amount = table.Column<double>(type: "REAL", nullable: false),
                    ApartmentId = table.Column<Guid>(type: "TEXT", nullable: false),
                    value = table.Column<decimal>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reads", x => x.read_id);
                    table.ForeignKey(
                        name: "FK_reads_apartments_ApartmentId",
                        column: x => x.ApartmentId,
                        principalTable: "apartments",
                        principalColumn: "apartment_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "settlements",
                columns: table => new
                {
                    settlement_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    apartment_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    invoice_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    consumption = table.Column<decimal>(type: "TEXT", nullable: false),
                    amount_to_pay = table.Column<decimal>(type: "TEXT", nullable: false),
                    realization_status = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_settlements", x => x.settlement_id);
                    table.ForeignKey(
                        name: "FK_settlements_apartments_apartment_id",
                        column: x => x.apartment_id,
                        principalTable: "apartments",
                        principalColumn: "apartment_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_settlements_invoices_invoice_id",
                        column: x => x.invoice_id,
                        principalTable: "invoices",
                        principalColumn: "invoice_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_apartments_apartment_name",
                table: "apartments",
                column: "apartment_name");

            migrationBuilder.CreateIndex(
                name: "IX_apartments_GroupId",
                table: "apartments",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_invoices_invoice_period_from",
                table: "invoices",
                column: "invoice_period_from");

            migrationBuilder.CreateIndex(
                name: "IX_invoices_invoice_period_to",
                table: "invoices",
                column: "invoice_period_to");

            migrationBuilder.CreateIndex(
                name: "IX_reads_ApartmentId",
                table: "reads",
                column: "ApartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_reads_CreatedAt",
                table: "reads",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_settlements_apartment_id",
                table: "settlements",
                column: "apartment_id");

            migrationBuilder.CreateIndex(
                name: "IX_settlements_CreatedAt",
                table: "settlements",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_settlements_invoice_id",
                table: "settlements",
                column: "invoice_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "reads");

            migrationBuilder.DropTable(
                name: "settlements");

            migrationBuilder.DropTable(
                name: "apartments");

            migrationBuilder.DropTable(
                name: "invoices");

            migrationBuilder.DropTable(
                name: "groups");
        }
    }
}
