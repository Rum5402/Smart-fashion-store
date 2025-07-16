using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fashion.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class editItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Categories_StoreCategoryId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_StoreCategories_StoreCategoryId1",
                table: "Items");

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
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_StoreCategories_StoreCategoryId",
                table: "Items",
                column: "StoreCategoryId",
                principalTable: "StoreCategories",
                principalColumn: "Id");
        }
    }
}
