using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FastDrive.Migrations
{
    /// <inheritdoc />
    public partial class AddPabloToSurvey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Pablo",
                table: "Surveys",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pablo",
                table: "Surveys");
        }
    }
}
