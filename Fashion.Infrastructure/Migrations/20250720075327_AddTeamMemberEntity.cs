using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fashion.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTeamMemberEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TeamMembers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ManagerId = table.Column<int>(type: "int", nullable: false),
                    Position = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProfileImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ManagerId1 = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamMembers_Managers_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Managers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamMembers_Managers_ManagerId1",
                        column: x => x.ManagerId1,
                        principalTable: "Managers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FittingRoomRequests_HandledByStaffId",
                table: "FittingRoomRequests",
                column: "HandledByStaffId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamMembers_ManagerId",
                table: "TeamMembers",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamMembers_ManagerId1",
                table: "TeamMembers",
                column: "ManagerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_FittingRoomRequests_TeamMembers_HandledByStaffId",
                table: "FittingRoomRequests",
                column: "HandledByStaffId",
                principalTable: "TeamMembers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FittingRoomRequests_TeamMembers_HandledByStaffId",
                table: "FittingRoomRequests");

            migrationBuilder.DropTable(
                name: "TeamMembers");

            migrationBuilder.DropIndex(
                name: "IX_FittingRoomRequests_HandledByStaffId",
                table: "FittingRoomRequests");
        }
    }
}
