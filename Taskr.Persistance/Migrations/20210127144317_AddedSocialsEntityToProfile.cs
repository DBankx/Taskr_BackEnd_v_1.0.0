using Microsoft.EntityFrameworkCore.Migrations;

namespace Taskr.Persistance.Migrations
{
    public partial class AddedSocialsEntityToProfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Twitter",
                table: "AspNetUsers",
                newName: "Socials_Twitter");

            migrationBuilder.RenameColumn(
                name: "Pinterest",
                table: "AspNetUsers",
                newName: "Socials_Pinterest");

            migrationBuilder.RenameColumn(
                name: "Instagram",
                table: "AspNetUsers",
                newName: "Socials_Instagram");

            migrationBuilder.RenameColumn(
                name: "Facebook",
                table: "AspNetUsers",
                newName: "Socials_Facebook");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Socials_Twitter",
                table: "AspNetUsers",
                newName: "Twitter");

            migrationBuilder.RenameColumn(
                name: "Socials_Pinterest",
                table: "AspNetUsers",
                newName: "Pinterest");

            migrationBuilder.RenameColumn(
                name: "Socials_Instagram",
                table: "AspNetUsers",
                newName: "Instagram");

            migrationBuilder.RenameColumn(
                name: "Socials_Facebook",
                table: "AspNetUsers",
                newName: "Facebook");
        }
    }
}
