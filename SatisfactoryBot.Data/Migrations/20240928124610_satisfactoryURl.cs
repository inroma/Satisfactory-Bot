using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SatisfactoryBot.Data.Migrations
{
    /// <inheritdoc />
    public partial class satisfactoryURl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "SatisfactoryServer",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Url",
                table: "SatisfactoryServer");
        }
    }
}
