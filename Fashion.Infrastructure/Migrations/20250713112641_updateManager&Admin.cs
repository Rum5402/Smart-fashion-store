using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fashion.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateManagerAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admins");

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
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CurrentUserId = table.Column<int>(type: "int", nullable: true),
                    CurrentItemId = table.Column<int>(type: "int", nullable: true),
                    OccupiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpectedEndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FittingRoom", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FittingRoom_Items_CurrentItemId",
                        column: x => x.CurrentItemId,
                        principalTable: "Items",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FittingRoom_Users_CurrentUserId",
                        column: x => x.CurrentUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Managers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    StoreName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    StoreDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    StoreAddress = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ManagerId = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Department = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Position = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsSuperManager = table.Column<bool>(type: "bit", nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Managers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StoreSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoreActivities = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductStyle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreSettings", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FittingRoomRequests_AssignedFittingRoomId",
                table: "FittingRoomRequests",
                column: "AssignedFittingRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_FittingRoom_CurrentItemId",
                table: "FittingRoom",
                column: "CurrentItemId");

            migrationBuilder.CreateIndex(
                name: "IX_FittingRoom_CurrentUserId",
                table: "FittingRoom",
                column: "CurrentUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FittingRoomRequests_FittingRoom_AssignedFittingRoomId",
                table: "FittingRoomRequests",
                column: "AssignedFittingRoomId",
                principalTable: "FittingRoom",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FittingRoomRequests_FittingRoom_AssignedFittingRoomId",
                table: "FittingRoomRequests");

            migrationBuilder.DropTable(
                name: "FittingRoom");

            migrationBuilder.DropTable(
                name: "Managers");

            migrationBuilder.DropTable(
                name: "StoreSettings");

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

            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Department = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsSuperAdmin = table.Column<bool>(type: "bit", nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Position = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.Id);
                });
        }
    }
}
