﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Study_Step_Server.Migrations
{
    /// <inheritdoc />
    public partial class AddToUserChatDeletedColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "UserChats",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "UserChats");
        }
    }
}
