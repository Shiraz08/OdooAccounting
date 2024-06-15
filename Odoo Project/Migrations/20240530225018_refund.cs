using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Odoo_Project.Migrations
{
    public partial class refund : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Note",
                table: "CreditNotes",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "CreditNotes",
                newName: "Total");

            migrationBuilder.AddColumn<DateTime>(
                name: "InvoiceDate",
                table: "CreditNotes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Number",
                table: "CreditNotes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Payment",
                table: "CreditNotes",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Tax",
                table: "CreditNotes",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TaxExcluded",
                table: "CreditNotes",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvoiceDate",
                table: "CreditNotes");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "CreditNotes");

            migrationBuilder.DropColumn(
                name: "Payment",
                table: "CreditNotes");

            migrationBuilder.DropColumn(
                name: "Tax",
                table: "CreditNotes");

            migrationBuilder.DropColumn(
                name: "TaxExcluded",
                table: "CreditNotes");

            migrationBuilder.RenameColumn(
                name: "Total",
                table: "CreditNotes",
                newName: "Amount");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "CreditNotes",
                newName: "Note");
        }
    }
}
