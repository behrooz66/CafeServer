using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AuthServer.Migrations
{
    public partial class orderHistory_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GiftCardTypes_GiftCardTypes_GiftCardTypeId",
                table: "GiftCardTypes");

            migrationBuilder.DropIndex(
                name: "IX_GiftCardTypes_GiftCardTypeId",
                table: "GiftCardTypes");

            migrationBuilder.DropColumn(
                name: "GiftCardTypeId",
                table: "GiftCardTypes");

            migrationBuilder.CreateTable(
                name: "OrderHistories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CustomerId = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    Notes = table.Column<string>(nullable: true),
                    OrderId = table.Column<int>(nullable: false),
                    OrderType = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderHistories_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderHistories_OrderId",
                table: "OrderHistories",
                column: "OrderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderHistories");

            migrationBuilder.AddColumn<int>(
                name: "GiftCardTypeId",
                table: "GiftCardTypes",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GiftCardTypes_GiftCardTypeId",
                table: "GiftCardTypes",
                column: "GiftCardTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_GiftCardTypes_GiftCardTypes_GiftCardTypeId",
                table: "GiftCardTypes",
                column: "GiftCardTypeId",
                principalTable: "GiftCardTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
