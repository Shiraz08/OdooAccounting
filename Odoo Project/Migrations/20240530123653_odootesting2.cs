using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Odoo_Project.Migrations
{
    public partial class odootesting2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Activities",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "TaxIncluded",
                table: "Invoices");

            migrationBuilder.AddColumn<decimal>(
                name: "TaxExcluded",
                table: "Invoices",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaxExcluded",
                table: "Invoices");

            migrationBuilder.AddColumn<string>(
                name: "Activities",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "Invoices",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "TaxIncluded",
                table: "Invoices",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
