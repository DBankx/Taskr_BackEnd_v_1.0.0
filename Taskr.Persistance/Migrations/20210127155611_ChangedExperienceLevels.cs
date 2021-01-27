using Microsoft.EntityFrameworkCore.Migrations;

namespace Taskr.Persistance.Migrations
{
    public partial class ChangedExperienceLevels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SkillExperience",
                table: "Skill",
                newName: "ExperienceLevel");

            migrationBuilder.RenameColumn(
                name: "LanguageExperienceLevel",
                table: "Language",
                newName: "ExperienceLevel");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExperienceLevel",
                table: "Skill",
                newName: "SkillExperience");

            migrationBuilder.RenameColumn(
                name: "ExperienceLevel",
                table: "Language",
                newName: "LanguageExperienceLevel");
        }
    }
}
