using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Study_Step_Server.Migrations
{
    /// <inheritdoc />
    public partial class AddDeletedChatTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "UserChats");

            migrationBuilder.CreateTable(
                name: "DeletedChats",
                columns: table => new
                {
                    DeletedChatId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    ChatId = table.Column<int>(type: "integer", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeletedChats", x => x.DeletedChatId);
                    table.ForeignKey(
                        name: "FK_DeletedChats_Chats_ChatId",
                        column: x => x.ChatId,
                        principalTable: "Chats",
                        principalColumn: "ChatId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeletedChats_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeletedChats_ChatId",
                table: "DeletedChats",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_DeletedChats_UserId",
                table: "DeletedChats",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeletedChats");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "UserChats",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
