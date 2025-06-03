using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FastDrive.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Users_UserIDUser",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_UserIDUser",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "UserIDUser",
                table: "Bookings");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_IDUser",
                table: "Bookings",
                column: "IDUser");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Users_IDUser",
                table: "Bookings",
                column: "IDUser",
                principalTable: "Users",
                principalColumn: "IDUser",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Users_IDUser",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_IDUser",
                table: "Bookings");

            migrationBuilder.AddColumn<int>(
                name: "UserIDUser",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_UserIDUser",
                table: "Bookings",
                column: "UserIDUser");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Users_UserIDUser",
                table: "Bookings",
                column: "UserIDUser",
                principalTable: "Users",
                principalColumn: "IDUser",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
