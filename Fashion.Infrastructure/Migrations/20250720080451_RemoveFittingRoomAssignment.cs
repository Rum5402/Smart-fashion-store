using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fashion.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveFittingRoomAssignment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FittingRoomRequests_FittingRoom_AssignedFittingRoomId",
                table: "FittingRoomRequests");

            migrationBuilder.DropTable(
                name: "FittingRoom");

            migrationBuilder.DropIndex(
                name: "IX_FittingRoomRequests_AssignedFittingRoomId",
                table: "FittingRoomRequests");

            migrationBuilder.DropColumn(
                name: "AssignedAt",
                table: "FittingRoomRequests");

            migrationBuilder.DropColumn(
                name: "AssignedFittingRoomId",
                table: "FittingRoomRequests");

            migrationBuilder.DropColumn(
                name: "ExpectedEndTime",
                table: "FittingRoomRequests");

            migrationBuilder.DropColumn(
                name: "ExpectedStartTime",
                table: "FittingRoomRequests");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AssignedAt",
                table: "FittingRoomRequests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AssignedFittingRoomId",
                table: "FittingRoomRequests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpectedEndTime",
                table: "FittingRoomRequests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpectedStartTime",
                table: "FittingRoomRequests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FittingRoom",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CurrentUser = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ReservedUntil = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RoomNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FittingRoom", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FittingRoomRequests_AssignedFittingRoomId",
                table: "FittingRoomRequests",
                column: "AssignedFittingRoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_FittingRoomRequests_FittingRoom_AssignedFittingRoomId",
                table: "FittingRoomRequests",
                column: "AssignedFittingRoomId",
                principalTable: "FittingRoom",
                principalColumn: "Id");
        }
    }
}
