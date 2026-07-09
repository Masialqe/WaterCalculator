using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WaterCalculator.Migrations
{
    /// <inheritdoc />
    public partial class reorganize_structure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_invoices_invoice_period_from",
                table: "invoices");

            migrationBuilder.DropIndex(
                name: "IX_invoices_invoice_period_to",
                table: "invoices");

            migrationBuilder.DropColumn(
                name: "invoice_period_from",
                table: "invoices");

            migrationBuilder.DropColumn(
                name: "invoice_period_to",
                table: "invoices");

            migrationBuilder.AddColumn<decimal>(
                name: "ConsumptionFromLastRead",
                table: "reads",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "PayoffId",
                table: "reads",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Payoffs",
                columns: table => new
                {
                    payoff_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    GroupId = table.Column<Guid>(type: "TEXT", nullable: false),
                    payoff_status = table.Column<int>(type: "INTEGER", nullable: false),
                    PeriodFrom = table.Column<DateTime>(type: "TEXT", nullable: false),
                    payoff_period_to = table.Column<DateTime>(type: "TEXT", nullable: false),
                    payoff_total_meter_value = table.Column<decimal>(type: "TEXT", nullable: false),
                    payoff_total_consumption = table.Column<decimal>(type: "TEXT", nullable: false),
                    payoff_created_at = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payoffs", x => x.payoff_id);
                    table.ForeignKey(
                        name: "FK_Payoffs_groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "groups",
                        principalColumn: "group_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_reads_PayoffId",
                table: "reads",
                column: "PayoffId");

            migrationBuilder.CreateIndex(
                name: "IX_Payoffs_GroupId",
                table: "Payoffs",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Payoffs_payoff_status",
                table: "Payoffs",
                column: "payoff_status");

            migrationBuilder.AddForeignKey(
                name: "FK_reads_Payoffs_PayoffId",
                table: "reads",
                column: "PayoffId",
                principalTable: "Payoffs",
                principalColumn: "payoff_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_reads_Payoffs_PayoffId",
                table: "reads");

            migrationBuilder.DropTable(
                name: "Payoffs");

            migrationBuilder.DropIndex(
                name: "IX_reads_PayoffId",
                table: "reads");

            migrationBuilder.DropColumn(
                name: "ConsumptionFromLastRead",
                table: "reads");

            migrationBuilder.DropColumn(
                name: "PayoffId",
                table: "reads");

            migrationBuilder.AddColumn<DateTime>(
                name: "invoice_period_from",
                table: "invoices",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "invoice_period_to",
                table: "invoices",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_invoices_invoice_period_from",
                table: "invoices",
                column: "invoice_period_from");

            migrationBuilder.CreateIndex(
                name: "IX_invoices_invoice_period_to",
                table: "invoices",
                column: "invoice_period_to");
        }
    }
}
