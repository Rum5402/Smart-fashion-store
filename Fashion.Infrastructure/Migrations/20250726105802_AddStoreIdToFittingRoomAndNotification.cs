using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fashion.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStoreIdToFittingRoomAndNotification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StoreId",
                table: "Notifications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StoreId",
                table: "FittingRoomRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_StoreId",
                table: "Notifications",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_FittingRoomRequests_StoreId",
                table: "FittingRoomRequests",
                column: "StoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_FittingRoomRequests_StoreBrandSettings_StoreId",
                table: "FittingRoomRequests",
                column: "StoreId",
                principalTable: "StoreBrandSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_StoreBrandSettings_StoreId",
                table: "Notifications",
                column: "StoreId",
                principalTable: "StoreBrandSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FittingRoomRequests_StoreBrandSettings_StoreId",
                table: "FittingRoomRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_StoreBrandSettings_StoreId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_StoreId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_FittingRoomRequests_StoreId",
                table: "FittingRoomRequests");

            migrationBuilder.DropColumn(
                name: "StoreId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "StoreId",
                table: "FittingRoomRequests");
        }
    }
}
