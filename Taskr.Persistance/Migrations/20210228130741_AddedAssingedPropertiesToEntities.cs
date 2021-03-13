using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Taskr.Persistance.Migrations
{
    public partial class AddedAssingedPropertiesToEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_AspNetUsers_UserId",
                table: "Jobs");

            migrationBuilder.DeleteData(
                table: "Jobs",
                keyColumn: "Id",
                keyValue: new Guid("64fa643f-2d35-46e7-b3f8-31fa673d719b"));

            migrationBuilder.AddColumn<string>(
                name: "AssignedUserId",
                table: "Jobs",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_AssignedUserId",
                table: "Jobs",
                column: "AssignedUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_AspNetUsers_AssignedUserId",
                table: "Jobs",
                column: "AssignedUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_AspNetUsers_UserId",
                table: "Jobs",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_AspNetUsers_AssignedUserId",
                table: "Jobs");

            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_AspNetUsers_UserId",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_AssignedUserId",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "AssignedUserId",
                table: "Jobs");

            migrationBuilder.InsertData(
                table: "Jobs",
                columns: new[] { "Id", "Address", "Category", "CreatedAt", "DeliveryDate", "DeliveryType", "Description", "InitialPrice", "JobStatus", "PostCode", "Title", "UserId", "Views" },
                values: new object[] { new Guid("64fa643f-2d35-46e7-b3f8-31fa673d719b"), null, 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "Please my garden needs trimmin, Im in lagos", 20.30m, 0, null, "Garden Trimming in Lagos Ajah", null, 0 });

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_AspNetUsers_UserId",
                table: "Jobs",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
