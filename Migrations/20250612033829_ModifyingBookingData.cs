using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FastDrive.Migrations
{
    /// <inheritdoc />
    public partial class ModifyingBookingData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DamageReport",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "Km",
                table: "Bookings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DamageReport",
                table: "Bookings",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Km",
                table: "Bookings",
                type: "int",
                nullable: true);
        }
    }
}
