using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AuthServer.Migrations
{
    public partial class mostEntitiesDbSetsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_Cities_CityId",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_Customer_Provinces_ProvinceId",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_Customer_Restaurant_RestaurantId",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_GiftCard_Customer_CustomerId",
                table: "GiftCard");

            migrationBuilder.DropForeignKey(
                name: "FK_GiftCard_GiftCardType_GiftCardTypeId",
                table: "GiftCard");

            migrationBuilder.DropForeignKey(
                name: "FK_GiftCardType_GiftCardType_GiftCardTypeId",
                table: "GiftCardType");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Customer_CustomerId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_OrderType_OrderTypeId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservation_Customer_CustomerId",
                table: "Reservation");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservation_ReservationStatus_ReservationStatusId",
                table: "Reservation");

            migrationBuilder.DropForeignKey(
                name: "FK_Restaurant_Cities_CityId",
                table: "Restaurant");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Restaurant_RestaurantId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Restaurant",
                table: "Restaurant");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReservationStatus",
                table: "ReservationStatus");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reservation",
                table: "Reservation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderType",
                table: "OrderType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Order",
                table: "Order");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GiftCardType",
                table: "GiftCardType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GiftCard",
                table: "GiftCard");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Customer",
                table: "Customer");

            migrationBuilder.RenameTable(
                name: "Restaurant",
                newName: "Restaurants");

            migrationBuilder.RenameTable(
                name: "ReservationStatus",
                newName: "ReservationStatuses");

            migrationBuilder.RenameTable(
                name: "Reservation",
                newName: "Reservations");

            migrationBuilder.RenameTable(
                name: "OrderType",
                newName: "OrderTypes");

            migrationBuilder.RenameTable(
                name: "Order",
                newName: "Orders");

            migrationBuilder.RenameTable(
                name: "GiftCardType",
                newName: "GiftCardTypes");

            migrationBuilder.RenameTable(
                name: "GiftCard",
                newName: "GiftCards");

            migrationBuilder.RenameTable(
                name: "Customer",
                newName: "Customers");

            migrationBuilder.RenameIndex(
                name: "IX_Restaurant_CityId",
                table: "Restaurants",
                newName: "IX_Restaurants_CityId");

            migrationBuilder.RenameIndex(
                name: "IX_Reservation_ReservationStatusId",
                table: "Reservations",
                newName: "IX_Reservations_ReservationStatusId");

            migrationBuilder.RenameIndex(
                name: "IX_Reservation_CustomerId",
                table: "Reservations",
                newName: "IX_Reservations_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Order_OrderTypeId",
                table: "Orders",
                newName: "IX_Orders_OrderTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Order_CustomerId",
                table: "Orders",
                newName: "IX_Orders_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_GiftCardType_GiftCardTypeId",
                table: "GiftCardTypes",
                newName: "IX_GiftCardTypes_GiftCardTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_GiftCard_GiftCardTypeId",
                table: "GiftCards",
                newName: "IX_GiftCards_GiftCardTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_GiftCard_CustomerId",
                table: "GiftCards",
                newName: "IX_GiftCards_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Customer_RestaurantId",
                table: "Customers",
                newName: "IX_Customers_RestaurantId");

            migrationBuilder.RenameIndex(
                name: "IX_Customer_ProvinceId",
                table: "Customers",
                newName: "IX_Customers_ProvinceId");

            migrationBuilder.RenameIndex(
                name: "IX_Customer_CityId",
                table: "Customers",
                newName: "IX_Customers_CityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Restaurants",
                table: "Restaurants",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReservationStatuses",
                table: "ReservationStatuses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reservations",
                table: "Reservations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderTypes",
                table: "OrderTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GiftCardTypes",
                table: "GiftCardTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GiftCards",
                table: "GiftCards",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Customers",
                table: "Customers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Cities_CityId",
                table: "Customers",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Provinces_ProvinceId",
                table: "Customers",
                column: "ProvinceId",
                principalTable: "Provinces",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Restaurants_RestaurantId",
                table: "Customers",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_GiftCards_Customers_CustomerId",
                table: "GiftCards",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GiftCards_GiftCardTypes_GiftCardTypeId",
                table: "GiftCards",
                column: "GiftCardTypeId",
                principalTable: "GiftCardTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GiftCardTypes_GiftCardTypes_GiftCardTypeId",
                table: "GiftCardTypes",
                column: "GiftCardTypeId",
                principalTable: "GiftCardTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Customers_CustomerId",
                table: "Orders",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_OrderTypes_OrderTypeId",
                table: "Orders",
                column: "OrderTypeId",
                principalTable: "OrderTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Customers_CustomerId",
                table: "Reservations",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_ReservationStatuses_ReservationStatusId",
                table: "Reservations",
                column: "ReservationStatusId",
                principalTable: "ReservationStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_Cities_CityId",
                table: "Restaurants",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Restaurants_RestaurantId",
                table: "Users",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Cities_CityId",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Provinces_ProvinceId",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Restaurants_RestaurantId",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_GiftCards_Customers_CustomerId",
                table: "GiftCards");

            migrationBuilder.DropForeignKey(
                name: "FK_GiftCards_GiftCardTypes_GiftCardTypeId",
                table: "GiftCards");

            migrationBuilder.DropForeignKey(
                name: "FK_GiftCardTypes_GiftCardTypes_GiftCardTypeId",
                table: "GiftCardTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Customers_CustomerId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_OrderTypes_OrderTypeId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Customers_CustomerId",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_ReservationStatuses_ReservationStatusId",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_Cities_CityId",
                table: "Restaurants");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Restaurants_RestaurantId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Restaurants",
                table: "Restaurants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReservationStatuses",
                table: "ReservationStatuses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reservations",
                table: "Reservations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderTypes",
                table: "OrderTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GiftCardTypes",
                table: "GiftCardTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GiftCards",
                table: "GiftCards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Customers",
                table: "Customers");

            migrationBuilder.RenameTable(
                name: "Restaurants",
                newName: "Restaurant");

            migrationBuilder.RenameTable(
                name: "ReservationStatuses",
                newName: "ReservationStatus");

            migrationBuilder.RenameTable(
                name: "Reservations",
                newName: "Reservation");

            migrationBuilder.RenameTable(
                name: "OrderTypes",
                newName: "OrderType");

            migrationBuilder.RenameTable(
                name: "Orders",
                newName: "Order");

            migrationBuilder.RenameTable(
                name: "GiftCardTypes",
                newName: "GiftCardType");

            migrationBuilder.RenameTable(
                name: "GiftCards",
                newName: "GiftCard");

            migrationBuilder.RenameTable(
                name: "Customers",
                newName: "Customer");

            migrationBuilder.RenameIndex(
                name: "IX_Restaurants_CityId",
                table: "Restaurant",
                newName: "IX_Restaurant_CityId");

            migrationBuilder.RenameIndex(
                name: "IX_Reservations_ReservationStatusId",
                table: "Reservation",
                newName: "IX_Reservation_ReservationStatusId");

            migrationBuilder.RenameIndex(
                name: "IX_Reservations_CustomerId",
                table: "Reservation",
                newName: "IX_Reservation_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_OrderTypeId",
                table: "Order",
                newName: "IX_Order_OrderTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_CustomerId",
                table: "Order",
                newName: "IX_Order_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_GiftCardTypes_GiftCardTypeId",
                table: "GiftCardType",
                newName: "IX_GiftCardType_GiftCardTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_GiftCards_GiftCardTypeId",
                table: "GiftCard",
                newName: "IX_GiftCard_GiftCardTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_GiftCards_CustomerId",
                table: "GiftCard",
                newName: "IX_GiftCard_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Customers_RestaurantId",
                table: "Customer",
                newName: "IX_Customer_RestaurantId");

            migrationBuilder.RenameIndex(
                name: "IX_Customers_ProvinceId",
                table: "Customer",
                newName: "IX_Customer_ProvinceId");

            migrationBuilder.RenameIndex(
                name: "IX_Customers_CityId",
                table: "Customer",
                newName: "IX_Customer_CityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Restaurant",
                table: "Restaurant",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReservationStatus",
                table: "ReservationStatus",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reservation",
                table: "Reservation",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderType",
                table: "OrderType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Order",
                table: "Order",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GiftCardType",
                table: "GiftCardType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GiftCard",
                table: "GiftCard",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Customer",
                table: "Customer",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_Cities_CityId",
                table: "Customer",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_Provinces_ProvinceId",
                table: "Customer",
                column: "ProvinceId",
                principalTable: "Provinces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_Restaurant_RestaurantId",
                table: "Customer",
                column: "RestaurantId",
                principalTable: "Restaurant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GiftCard_Customer_CustomerId",
                table: "GiftCard",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GiftCard_GiftCardType_GiftCardTypeId",
                table: "GiftCard",
                column: "GiftCardTypeId",
                principalTable: "GiftCardType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GiftCardType_GiftCardType_GiftCardTypeId",
                table: "GiftCardType",
                column: "GiftCardTypeId",
                principalTable: "GiftCardType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Customer_CustomerId",
                table: "Order",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_OrderType_OrderTypeId",
                table: "Order",
                column: "OrderTypeId",
                principalTable: "OrderType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservation_Customer_CustomerId",
                table: "Reservation",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservation_ReservationStatus_ReservationStatusId",
                table: "Reservation",
                column: "ReservationStatusId",
                principalTable: "ReservationStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurant_Cities_CityId",
                table: "Restaurant",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Restaurant_RestaurantId",
                table: "Users",
                column: "RestaurantId",
                principalTable: "Restaurant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
