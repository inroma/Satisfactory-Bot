using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SatisfactoryBot.Data.Migrations
{
    /// <inheritdoc />
    public partial class initproject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DiscordServer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordServer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SatisfactoryServer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Owner = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Token = table.Column<string>(type: "text", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SatisfactoryServer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DiscordRole",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    IsAdmin = table.Column<bool>(type: "boolean", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscordRole_DiscordServer_Id",
                        column: x => x.Id,
                        principalTable: "DiscordServer",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SatisfactoryToDiscord",
                columns: table => new
                {
                    SatisfactoryServerId = table.Column<int>(type: "integer", nullable: false),
                    DiscordServerId = table.Column<int>(type: "integer", nullable: false)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiscordRole");

            migrationBuilder.DropTable(
                name: "SatisfactoryToDiscord");

            migrationBuilder.DropTable(
                name: "DiscordServer");

            migrationBuilder.DropTable(
                name: "SatisfactoryServer");
        }
    }
}
