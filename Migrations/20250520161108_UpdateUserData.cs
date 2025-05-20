using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FastDrive.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Surveys");

            migrationBuilder.DropTable(
                name: "Cars");

            migrationBuilder.AlterColumn<string>(
                name: "UserType",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "UserType",
                table: "Users",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    Patent = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CarStatus = table.Column<int>(type: "int", nullable: false),
                    Km = table.Column<int>(type: "int", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.Patent);
                });

            migrationBuilder.CreateTable(
                name: "Surveys",
                columns: table => new
                {
                    IDSurvey = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IDBooking = table.Column<int>(type: "int", nullable: false),
                    IDUser = table.Column<int>(type: "int", nullable: false),
                    ServiceCalification = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Surveys", x => x.IDSurvey);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    IDBooking = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarPatent = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserIDUser = table.Column<int>(type: "int", nullable: false),
                    DateEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IDUser = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.IDBooking);
                    table.ForeignKey(
                        name: "FK_Bookings_Cars_CarPatent",
                        column: x => x.CarPatent,
                        principalTable: "Cars",
                        principalColumn: "Patent",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bookings_Users_UserIDUser",
                        column: x => x.UserIDUser,
                        principalTable: "Users",
                        principalColumn: "IDUser",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_CarPatent",
                table: "Bookings",
                column: "CarPatent");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_UserIDUser",
                table: "Bookings",
                column: "UserIDUser");
        }
    }
}
