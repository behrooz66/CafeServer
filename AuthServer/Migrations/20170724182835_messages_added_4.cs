using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AuthServer.Migrations
{
    public partial class messages_added_4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Messages_ReplyToId",
                table: "Messages");

            migrationBuilder.AlterColumn<int>(
                name: "ReplyToId",
                table: "Messages",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Messages_ReplyToId",
                table: "Messages",
                column: "ReplyToId",
                principalTable: "Messages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Messages_ReplyToId",
                table: "Messages");

            migrationBuilder.AlterColumn<int>(
                name: "ReplyToId",
                table: "Messages",
                nullable: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Messages_ReplyToId",
                table: "Messages",
                column: "ReplyToId",
                principalTable: "Messages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
