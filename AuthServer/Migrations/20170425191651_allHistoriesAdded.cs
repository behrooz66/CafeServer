using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AuthServer.Migrations
{
    public partial class allHistoriesAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "OrderHistories");

            migrationBuilder.CreateTable(
                name: "CustomerHistories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Address = table.Column<string>(nullable: true),
                    AddressFound = table.Column<bool>(nullable: false),
                    Cell = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    CustomerId = table.Column<int>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    Home = table.Column<string>(nullable: true),
                    Lat = table.Column<double>(nullable: true),
                    Lon = table.Column<double>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    NoAddress = table.Column<bool>(nullable: false),
                    Notes = table.Column<string>(nullable: true),
                    OtherPhone = table.Column<string>(nullable: true),
                    PostalCode = table.Column<string>(nullable: true),
                    Restaurant = table.Column<string>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    Work = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerHistories_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GiftCardHistories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<decimal>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    ExpiryDate = table.Column<DateTime>(nullable: false),
                    GiftCardId = table.Column<int>(nullable: false),
                    GiftCardType = table.Column<string>(nullable: true),
                    IssueDate = table.Column<DateTime>(nullable: false),
                    Notes = table.Column<string>(nullable: true),
                    Number = table.Column<string>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GiftCardHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GiftCardHistories_GiftCards_GiftCardId",
                        column: x => x.GiftCardId,
                        principalTable: "GiftCards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReservationHistories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    Notes = table.Column<string>(nullable: true),
                    NumberOfPeople = table.Column<int>(nullable: false),
                    ReservationId = table.Column<int>(nullable: false),
                    ReservationStatus = table.Column<string>(nullable: true),
                    Revenue = table.Column<decimal>(nullable: true),
                    Table = table.Column<string>(nullable: true),
                    Time = table.Column<string>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReservationHistories_Reservations_ReservationId",
                        column: x => x.ReservationId,
                        principalTable: "Reservations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerHistories_CustomerId",
                table: "CustomerHistories",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_GiftCardHistories_GiftCardId",
                table: "GiftCardHistories",
                column: "GiftCardId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservationHistories_ReservationId",
                table: "ReservationHistories",
                column: "ReservationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerHistories");

            migrationBuilder.DropTable(
                name: "GiftCardHistories");

            migrationBuilder.DropTable(
                name: "ReservationHistories");

            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "OrderHistories",
                nullable: false,
                defaultValue: 0);
        }
    }
}
