using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fashion.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStoreIdToEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StoreId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StoreId",
                table: "StoreSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StoreId",
                table: "StoreFilters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StoreId",
                table: "StoreCategories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StoreId",
                table: "StoreBanners",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StoreId",
                table: "Managers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StoreId",
                table: "Items",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "AboutUs",
                table: "StoreBrandSettings",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Mission",
                table: "StoreBrandSettings",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Vision",
                table: "StoreBrandSettings",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Values",
                table: "StoreBrandSettings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Highlights",
                table: "StoreBrandSettings",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_StoreId",
                table: "Users",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreSettings_StoreId",
                table: "StoreSettings",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreFilters_StoreId",
                table: "StoreFilters",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreCategories_StoreId",
                table: "StoreCategories",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreBanners_StoreId",
                table: "StoreBanners",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Managers_StoreId",
                table: "Managers",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_StoreId",
                table: "Items",
                column: "StoreId");

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
                name: "FK_StoreCategories_StoreBrandSettings_StoreId",
                table: "StoreCategories",
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
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StoreSettings_StoreBrandSettings_StoreId",
                table: "StoreSettings",
                column: "StoreId",
                principalTable: "StoreBrandSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_StoreBrandSettings_StoreId",
                table: "Users",
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
                name: "FK_StoreCategories_StoreBrandSettings_StoreId",
                table: "StoreCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_StoreFilters_StoreBrandSettings_StoreId",
                table: "StoreFilters");

            migrationBuilder.DropForeignKey(
                name: "FK_StoreSettings_StoreBrandSettings_StoreId",
                table: "StoreSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_StoreBrandSettings_StoreId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_StoreId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_StoreSettings_StoreId",
                table: "StoreSettings");

            migrationBuilder.DropIndex(
                name: "IX_StoreFilters_StoreId",
                table: "StoreFilters");

            migrationBuilder.DropIndex(
                name: "IX_StoreCategories_StoreId",
                table: "StoreCategories");

            migrationBuilder.DropIndex(
                name: "IX_StoreBanners_StoreId",
                table: "StoreBanners");

            migrationBuilder.DropIndex(
                name: "IX_Managers_StoreId",
                table: "Managers");

            migrationBuilder.DropIndex(
                name: "IX_Items_StoreId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "StoreId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "StoreId",
                table: "StoreSettings");

            migrationBuilder.DropColumn(
                name: "StoreId",
                table: "StoreFilters");

            migrationBuilder.DropColumn(
                name: "StoreId",
                table: "StoreCategories");

            migrationBuilder.DropColumn(
                name: "StoreId",
                table: "StoreBanners");

            migrationBuilder.DropColumn(
                name: "StoreId",
                table: "Managers");

            migrationBuilder.DropColumn(
                name: "StoreId",
                table: "Items");
        }
    }
}
