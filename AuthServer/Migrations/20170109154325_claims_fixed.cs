using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AuthServer.Migrations
{
    public partial class claims_fixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserClaims_Roles_RoleId",
                table: "UserClaims");

            migrationBuilder.DropIndex(
                name: "IX_UserClaims_RoleId",
                table: "UserClaims");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "UserClaims");

            migrationBuilder.AddColumn<string>(
                name: "ClaimType",
                table: "UserClaims",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClaimValue",
                table: "UserClaims",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClaimType",
                table: "UserClaims");

            migrationBuilder.DropColumn(
                name: "ClaimValue",
                table: "UserClaims");

            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "UserClaims",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_RoleId",
                table: "UserClaims",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserClaims_Roles_RoleId",
                table: "UserClaims",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
