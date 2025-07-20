using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fashion.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDisplayOrderToStoreBanner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Categories_StoreCategoryId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_StoreCategories_StoreCategoryId1",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "AboutText",
                table: "StoreBrandSettings");

            migrationBuilder.DropColumn(
                name: "AccentColor",
                table: "StoreBrandSettings");

            migrationBuilder.DropColumn(
                name: "ContactEmail",
                table: "StoreBrandSettings");

            migrationBuilder.DropColumn(
                name: "ContactPhone",
                table: "StoreBrandSettings");

            migrationBuilder.DropColumn(
                name: "SecondaryColor",
                table: "StoreBrandSettings");

            migrationBuilder.DropColumn(
                name: "SocialMediaLinks",
                table: "StoreBrandSettings");

            migrationBuilder.DropColumn(
                name: "WebsiteUrl",
                table: "StoreBrandSettings");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "StoreBanners");

            migrationBuilder.RenameColumn(
                name: "StoreCategoryId1",
                table: "Items",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Items_StoreCategoryId1",
                table: "Items",
                newName: "IX_Items_CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Categories_CategoryId",
                table: "Items",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_StoreCategories_StoreCategoryId",
                table: "Items",
                column: "StoreCategoryId",
                principalTable: "StoreCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Categories_CategoryId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_StoreCategories_StoreCategoryId",
                table: "Items");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "Items",
                newName: "StoreCategoryId1");

            migrationBuilder.RenameIndex(
                name: "IX_Items_CategoryId",
                table: "Items",
                newName: "IX_Items_StoreCategoryId1");

            migrationBuilder.AddColumn<string>(
                name: "AboutText",
                table: "StoreBrandSettings",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AccentColor",
                table: "StoreBrandSettings",
                type: "nvarchar(7)",
                maxLength: 7,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactEmail",
                table: "StoreBrandSettings",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactPhone",
                table: "StoreBrandSettings",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecondaryColor",
                table: "StoreBrandSettings",
                type: "nvarchar(7)",
                maxLength: 7,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SocialMediaLinks",
                table: "StoreBrandSettings",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WebsiteUrl",
                table: "StoreBrandSettings",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "StoreBanners",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Categories_StoreCategoryId",
                table: "Items",
                column: "StoreCategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_StoreCategories_StoreCategoryId1",
                table: "Items",
                column: "StoreCategoryId1",
                principalTable: "StoreCategories",
                principalColumn: "Id");
        }
    }
}
