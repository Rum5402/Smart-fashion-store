using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fashion.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixStoreInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "StoreBrandSettings",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "StoreBrandSettings",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Floor",
                table: "StoreBrandSettings",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "StoreBrandSettings",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocationName",
                table: "StoreBrandSettings",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "StoreBrandSettings",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MallName",
                table: "StoreBrandSettings",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "StoreBrandSettings",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "StoreBrandSettings",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecondaryPhoneNumber",
                table: "StoreBrandSettings",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SocialMediaLinks",
                table: "StoreBrandSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "StoreBrandSettings",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WhatsAppNumber",
                table: "StoreBrandSettings",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "StoreBrandSettings");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "StoreBrandSettings");

            migrationBuilder.DropColumn(
                name: "Floor",
                table: "StoreBrandSettings");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "StoreBrandSettings");

            migrationBuilder.DropColumn(
                name: "LocationName",
                table: "StoreBrandSettings");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "StoreBrandSettings");

            migrationBuilder.DropColumn(
                name: "MallName",
                table: "StoreBrandSettings");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "StoreBrandSettings");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "StoreBrandSettings");

            migrationBuilder.DropColumn(
                name: "SecondaryPhoneNumber",
                table: "StoreBrandSettings");

            migrationBuilder.DropColumn(
                name: "SocialMediaLinks",
                table: "StoreBrandSettings");

            migrationBuilder.DropColumn(
                name: "Website",
                table: "StoreBrandSettings");

            migrationBuilder.DropColumn(
                name: "WhatsAppNumber",
                table: "StoreBrandSettings");
        }
    }
}
