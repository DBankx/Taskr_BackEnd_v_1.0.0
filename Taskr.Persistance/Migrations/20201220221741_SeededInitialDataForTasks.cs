using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Taskr.Persistance.Migrations
{
    public partial class SeededInitialDataForTasks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Description", "InitialPrice", "Title" },
                values: new object[] { new Guid("64fa643f-2d35-46e7-b3f8-31fa673d719b"), "Please my garden needs trimmin, Im in lagos", 20.30m, "Garden Trimming in Lagos Ajah" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: new Guid("64fa643f-2d35-46e7-b3f8-31fa673d719b"));
        }
    }
}
