using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AuthServer.Migrations
{
    public partial class messages_added_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReceiverIds",
                table: "Messages",
                newName: "ReceiverId");

            migrationBuilder.AlterColumn<string>(
                name: "ReceiverId",
                table: "Messages",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "MessageId",
                table: "Messages",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ReceiverId",
                table: "Messages",
                column: "ReceiverId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_ReceiverId",
                table: "Messages",
                column: "ReceiverId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_ReceiverId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_ReceiverId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "MessageId",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "ReceiverId",
                table: "Messages",
                newName: "ReceiverIds");

            migrationBuilder.AlterColumn<string>(
                name: "ReceiverIds",
                table: "Messages",
                nullable: true);
        }
    }
}
