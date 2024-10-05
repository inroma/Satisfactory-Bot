using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SatisfactoryBot.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitProject : Migration
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
                    GuildId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordServer", x => x.Id);
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
                name: "SatisfactoryServer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Owner = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Url = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Token = table.Column<string>(type: "text", nullable: true),
                    IsDefaultServer = table.Column<bool>(type: "boolean", nullable: false),
                    DiscordServerId = table.Column<int>(type: "integer", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SatisfactoryServer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SatisfactoryServer_DiscordServer_DiscordServerId",
                        column: x => x.DiscordServerId,
                        principalTable: "DiscordServer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SatisfactoryServer_DiscordServerId",
                table: "SatisfactoryServer",
                column: "DiscordServerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiscordRole");

            migrationBuilder.DropTable(
                name: "SatisfactoryServer");

            migrationBuilder.DropTable(
                name: "DiscordServer");
        }
    }
}
