using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SatisfactoryBot.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultServer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDefaultServer",
                table: "SatisfactoryServer",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDefaultServer",
                table: "SatisfactoryServer");
        }
    }
}
