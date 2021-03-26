using Microsoft.EntityFrameworkCore.Migrations;

namespace Taskr.Persistance.Migrations
{
    public partial class changedCityToCountry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "City",
                table: "AspNetUsers",
                newName: "Country");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Country",
                table: "AspNetUsers",
                newName: "City");
        }
    }
}
