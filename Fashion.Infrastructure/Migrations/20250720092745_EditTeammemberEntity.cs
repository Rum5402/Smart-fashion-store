using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fashion.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EditTeammemberEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Position",
                table: "TeamMembers");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "TeamMembers",
                newName: "Department");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "TeamMembers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IDNumber",
                table: "TeamMembers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "TeamMembers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "TeamMembers");

            migrationBuilder.DropColumn(
                name: "IDNumber",
                table: "TeamMembers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "TeamMembers");

            migrationBuilder.RenameColumn(
                name: "Department",
                table: "TeamMembers",
                newName: "Name");

            migrationBuilder.AddColumn<string>(
                name: "Position",
                table: "TeamMembers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }
    }
}
