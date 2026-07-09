using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WaterCalculator.Migrations
{
    /// <inheritdoc />
    public partial class invoice_group_relation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_invoices_groups_GroupId",
                table: "invoices");

            migrationBuilder.DropIndex(
                name: "IX_invoices_GroupId",
                table: "invoices");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "invoices");
        }
    }
}
