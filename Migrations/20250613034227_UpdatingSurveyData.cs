using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FastDrive.Migrations
{
    /// <inheritdoc />
    public partial class UpdatingSurveyData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IDBooking",
                table: "Surveys");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IDBooking",
                table: "Surveys",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
