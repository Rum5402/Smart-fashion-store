using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fashion.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addStoreEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FittingRoom_Items_CurrentItemId",
                table: "FittingRoom");

            migrationBuilder.DropForeignKey(
                name: "FK_FittingRoom_Users_CurrentUserId",
                table: "FittingRoom");

            migrationBuilder.DropIndex(
                name: "IX_FittingRoom_CurrentItemId",
                table: "FittingRoom");

            migrationBuilder.DropIndex(
                name: "IX_FittingRoom_CurrentUserId",
                table: "FittingRoom");

            migrationBuilder.DropColumn(
                name: "CurrentItemId",
                table: "FittingRoom");

            migrationBuilder.DropColumn(
                name: "CurrentUserId",
                table: "FittingRoom");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "FittingRoom");

            migrationBuilder.DropColumn(
                name: "ExpectedEndTime",
                table: "FittingRoom");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "FittingRoom");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "FittingRoom");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "FittingRoom");

            migrationBuilder.RenameColumn(
                name: "OccupiedAt",
                table: "FittingRoom",
                newName: "ReservedUntil");

            migrationBuilder.AddColumn<int>(
                name: "StoreCategoryId",
                table: "Items",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrentUser",
                table: "FittingRoom",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoomNumber",
                table: "FittingRoom",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "StoreBanners",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    LinkUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreBanners", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StoreBrandSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoreName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Tagline = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    LogoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PrimaryColor = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: true),
                    SecondaryColor = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: true),
                    AccentColor = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    AboutText = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ContactEmail = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ContactPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    WebsiteUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SocialMediaLinks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreBrandSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StoreCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ParentCategoryId = table.Column<int>(type: "int", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreCategories_StoreCategories_ParentCategoryId",
                        column: x => x.ParentCategoryId,
                        principalTable: "StoreCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StoreFilters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Options = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    SelectionType = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreFilters", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Items_StoreCategoryId",
                table: "Items",
                column: "StoreCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreCategories_ParentCategoryId",
                table: "StoreCategories",
                column: "ParentCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_StoreCategories_StoreCategoryId",
                table: "Items",
                column: "StoreCategoryId",
                principalTable: "StoreCategories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_StoreCategories_StoreCategoryId",
                table: "Items");

            migrationBuilder.DropTable(
                name: "StoreBanners");

            migrationBuilder.DropTable(
                name: "StoreBrandSettings");

            migrationBuilder.DropTable(
                name: "StoreCategories");

            migrationBuilder.DropTable(
                name: "StoreFilters");

            migrationBuilder.DropIndex(
                name: "IX_Items_StoreCategoryId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "StoreCategoryId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CurrentUser",
                table: "FittingRoom");

            migrationBuilder.DropColumn(
                name: "RoomNumber",
                table: "FittingRoom");

            migrationBuilder.RenameColumn(
                name: "ReservedUntil",
                table: "FittingRoom",
                newName: "OccupiedAt");

            migrationBuilder.AddColumn<int>(
                name: "CurrentItemId",
                table: "FittingRoom",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrentUserId",
                table: "FittingRoom",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "FittingRoom",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpectedEndTime",
                table: "FittingRoom",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "FittingRoom",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "FittingRoom",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "FittingRoom",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FittingRoom_CurrentItemId",
                table: "FittingRoom",
                column: "CurrentItemId");

            migrationBuilder.CreateIndex(
                name: "IX_FittingRoom_CurrentUserId",
                table: "FittingRoom",
                column: "CurrentUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FittingRoom_Items_CurrentItemId",
                table: "FittingRoom",
                column: "CurrentItemId",
                principalTable: "Items",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FittingRoom_Users_CurrentUserId",
                table: "FittingRoom",
                column: "CurrentUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
