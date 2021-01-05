using Microsoft.EntityFrameworkCore.Migrations;

namespace Taskr.Persistance.Migrations
{
    public partial class ChangedEndDateToDeliveryDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "Jobs",
                newName: "DeliveryDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DeliveryDate",
                table: "Jobs",
                newName: "EndDate");
        }
    }
}
