using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fashion.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStoreBrandSettingsFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "StoreBrandSettings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactEmail",
                table: "StoreBrandSettings",
                type: "nvarchar(100)",
                maxLength: 100,
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
                name: "SocialMedia",
                table: "StoreBrandSettings",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StoreAddress",
                table: "StoreBrandSettings",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StoreDescription",
                table: "StoreBrandSettings",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "SocialMedia",
                table: "StoreBrandSettings");

            migrationBuilder.DropColumn(
                name: "StoreAddress",
                table: "StoreBrandSettings");

            migrationBuilder.DropColumn(
                name: "StoreDescription",
                table: "StoreBrandSettings");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "StoreBrandSettings",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }
    }
}
