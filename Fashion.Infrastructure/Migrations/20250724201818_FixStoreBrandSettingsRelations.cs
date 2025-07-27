using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fashion.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixStoreBrandSettingsRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_StoreBrandSettings_StoreId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Managers_StoreBrandSettings_StoreId",
                table: "Managers");

            migrationBuilder.DropForeignKey(
                name: "FK_StoreBanners_StoreBrandSettings_StoreId",
                table: "StoreBanners");

            migrationBuilder.DropForeignKey(
                name: "FK_StoreFilters_StoreBrandSettings_StoreId",
                table: "StoreFilters");

            migrationBuilder.AddColumn<string>(
                name: "AboutUs",
                table: "StoreBrandSettings",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Highlights",
                table: "StoreBrandSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Mission",
                table: "StoreBrandSettings",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Values",
                table: "StoreBrandSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Vision",
                table: "StoreBrandSettings",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_StoreBrandSettings_StoreId",
                table: "Items",
                column: "StoreId",
                principalTable: "StoreBrandSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Managers_StoreBrandSettings_StoreId",
                table: "Managers",
                column: "StoreId",
                principalTable: "StoreBrandSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StoreBanners_StoreBrandSettings_StoreId",
                table: "StoreBanners",
                column: "StoreId",
                principalTable: "StoreBrandSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StoreFilters_StoreBrandSettings_StoreId",
                table: "StoreFilters",
                column: "StoreId",
                principalTable: "StoreBrandSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_StoreBrandSettings_StoreId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Managers_StoreBrandSettings_StoreId",
                table: "Managers");

            migrationBuilder.DropForeignKey(
                name: "FK_StoreBanners_StoreBrandSettings_StoreId",
                table: "StoreBanners");

            migrationBuilder.DropForeignKey(
                name: "FK_StoreFilters_StoreBrandSettings_StoreId",
                table: "StoreFilters");

            migrationBuilder.DropColumn(
                name: "AboutUs",
                table: "StoreBrandSettings");

            migrationBuilder.DropColumn(
                name: "Highlights",
                table: "StoreBrandSettings");

            migrationBuilder.DropColumn(
                name: "Mission",
                table: "StoreBrandSettings");

            migrationBuilder.DropColumn(
                name: "Values",
                table: "StoreBrandSettings");

            migrationBuilder.DropColumn(
                name: "Vision",
                table: "StoreBrandSettings");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_StoreBrandSettings_StoreId",
                table: "Items",
                column: "StoreId",
                principalTable: "StoreBrandSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Managers_StoreBrandSettings_StoreId",
                table: "Managers",
                column: "StoreId",
                principalTable: "StoreBrandSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StoreBanners_StoreBrandSettings_StoreId",
                table: "StoreBanners",
                column: "StoreId",
                principalTable: "StoreBrandSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StoreFilters_StoreBrandSettings_StoreId",
                table: "StoreFilters",
                column: "StoreId",
                principalTable: "StoreBrandSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
