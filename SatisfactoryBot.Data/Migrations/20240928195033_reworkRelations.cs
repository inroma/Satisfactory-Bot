using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SatisfactoryBot.Data.Migrations
{
    /// <inheritdoc />
    public partial class reworkRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SatisfactoryToDiscord");

            migrationBuilder.AddColumn<int>(
                name: "DiscordServerId",
                table: "SatisfactoryServer",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SatisfactoryServer_DiscordServerId",
                table: "SatisfactoryServer",
                column: "DiscordServerId");

            migrationBuilder.AddForeignKey(
                name: "FK_SatisfactoryServer_DiscordServer_DiscordServerId",
                table: "SatisfactoryServer",
                column: "DiscordServerId",
                principalTable: "DiscordServer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SatisfactoryServer_DiscordServer_DiscordServerId",
                table: "SatisfactoryServer");

            migrationBuilder.DropIndex(
                name: "IX_SatisfactoryServer_DiscordServerId",
                table: "SatisfactoryServer");

            migrationBuilder.DropColumn(
                name: "DiscordServerId",
                table: "SatisfactoryServer");

            migrationBuilder.CreateTable(
                name: "SatisfactoryToDiscord",
                columns: table => new
                {
                    DiscordServerId = table.Column<int>(type: "integer", nullable: false),
                    SatisfactoryServerId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SatisfactoryToDiscord", x => new { x.DiscordServerId, x.SatisfactoryServerId });
                    table.ForeignKey(
                        name: "FK_SatisfactoryToDiscord_DiscordServer_DiscordServerId",
                        column: x => x.DiscordServerId,
                        principalTable: "DiscordServer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SatisfactoryToDiscord_SatisfactoryServer_SatisfactoryServer~",
                        column: x => x.SatisfactoryServerId,
                        principalTable: "SatisfactoryServer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SatisfactoryToDiscord_SatisfactoryServerId",
                table: "SatisfactoryToDiscord",
                column: "SatisfactoryServerId");
        }
    }
}
