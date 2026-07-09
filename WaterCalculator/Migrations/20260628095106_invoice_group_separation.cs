using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WaterCalculator.Migrations
{
    /// <inheritdoc />
    public partial class invoice_group_separation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_invoices_groups_GroupId",
                table: "invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Payoffs_groups_GroupId",
                table: "Payoffs");

            migrationBuilder.DropIndex(
                name: "IX_invoices_GroupId",
                table: "invoices");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "invoices");

            migrationBuilder.AddColumn<DateTime>(
                name: "invoice_date",
                table: "invoices",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_Payoffs_groups_GroupId",
                table: "Payoffs",
                column: "GroupId",
                principalTable: "groups",
                principalColumn: "group_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payoffs_groups_GroupId",
                table: "Payoffs");

            migrationBuilder.DropColumn(
                name: "invoice_date",
                table: "invoices");

            migrationBuilder.AddColumn<Guid>(
                name: "GroupId",
                table: "invoices",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_invoices_GroupId",
                table: "invoices",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_invoices_groups_GroupId",
                table: "invoices",
                column: "GroupId",
                principalTable: "groups",
                principalColumn: "group_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payoffs_groups_GroupId",
                table: "Payoffs",
                column: "GroupId",
                principalTable: "groups",
                principalColumn: "group_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
