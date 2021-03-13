using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Taskr.Persistance.Migrations
{
    public partial class AddedBankAccountEntityToUserModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BankAccount_AccountHolderName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankAccount_AccountHolderType",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankAccount_AccountNumber",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankAccount_Country",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankAccount_Currency",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BankAccount_Id",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankAccount_RoutingNumber",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankAccount_StripeAccountId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BankAccount_AccountHolderName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BankAccount_AccountHolderType",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BankAccount_AccountNumber",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BankAccount_Country",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BankAccount_Currency",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BankAccount_Id",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BankAccount_RoutingNumber",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BankAccount_StripeAccountId",
                table: "AspNetUsers");
        }
    }
}
