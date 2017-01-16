using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using AuthServer.Infrastructure;

namespace AuthServer.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20170115215308_countryAdded")]
    partial class countryAdded
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AuthServer.Models.City", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Lat");

                    b.Property<double>("Lon");

                    b.Property<double>("NELat");

                    b.Property<double>("NELon");

                    b.Property<double>("NWLat");

                    b.Property<double>("NWLon");

                    b.Property<string>("Name");

                    b.Property<int>("ProvinceId");

                    b.Property<double>("SELat");

                    b.Property<double>("SELon");

                    b.Property<double>("SWLat");

                    b.Property<double>("SWLon");

                    b.HasKey("Id");

                    b.HasIndex("ProvinceId");

                    b.ToTable("Cities");
                });

            modelBuilder.Entity("AuthServer.Models.Country", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("AuthServer.Models.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<bool>("AddressFound");

                    b.Property<string>("Cell");

                    b.Property<int>("CityId");

                    b.Property<bool>("Deleted");

                    b.Property<string>("Email");

                    b.Property<string>("Home");

                    b.Property<double?>("Lat");

                    b.Property<double?>("Lon");

                    b.Property<string>("Name");

                    b.Property<bool>("NoAddress");

                    b.Property<string>("Notes");

                    b.Property<string>("OtherPhone");

                    b.Property<string>("PostalCode");

                    b.Property<int>("ProvinceId");

                    b.Property<int>("RestaurantId");

                    b.Property<DateTime>("UpdatedAt");

                    b.Property<string>("UpdatedBy");

                    b.Property<string>("Work");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.HasIndex("ProvinceId");

                    b.HasIndex("RestaurantId");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("AuthServer.Models.GiftCard", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Amount");

                    b.Property<int>("CustomerId");

                    b.Property<bool>("Deleted");

                    b.Property<DateTime>("ExpiryDate");

                    b.Property<int>("GiftCardTypeId");

                    b.Property<DateTime>("IssueDate");

                    b.Property<string>("Notes");

                    b.Property<string>("Number");

                    b.Property<DateTime>("UpdatedAt");

                    b.Property<string>("UpdatedBy");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("GiftCardTypeId");

                    b.ToTable("GiftCards");
                });

            modelBuilder.Entity("AuthServer.Models.GiftCardType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("GiftCardTypeId");

                    b.Property<string>("Notes");

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.HasIndex("GiftCardTypeId");

                    b.ToTable("GiftCardTypes");
                });

            modelBuilder.Entity("AuthServer.Models.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CustomerId");

                    b.Property<DateTime>("Date");

                    b.Property<bool>("Deleted");

                    b.Property<string>("Notes");

                    b.Property<int>("OrderTypeId");

                    b.Property<decimal>("Price");

                    b.Property<DateTime>("UpdatedAt");

                    b.Property<string>("UpdatedBy");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("OrderTypeId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("AuthServer.Models.OrderType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Notes");

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.ToTable("OrderTypes");
                });

            modelBuilder.Entity("AuthServer.Models.Province", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CountryId");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.ToTable("Provinces");
                });

            modelBuilder.Entity("AuthServer.Models.Reservation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CustomerId");

                    b.Property<DateTime>("Date");

                    b.Property<bool>("Deleted");

                    b.Property<string>("Notes");

                    b.Property<int>("NumberOfPeople");

                    b.Property<int>("ReservationStatusId");

                    b.Property<decimal?>("Revenue");

                    b.Property<string>("Table");

                    b.Property<string>("Time")
                        .HasMaxLength(5);

                    b.Property<DateTime>("UpdatedAt");

                    b.Property<string>("UpdatedBy");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("ReservationStatusId");

                    b.ToTable("Reservations");
                });

            modelBuilder.Entity("AuthServer.Models.ReservationStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Notes");

                    b.Property<string>("Status");

                    b.HasKey("Id");

                    b.ToTable("ReservationStatuses");
                });

            modelBuilder.Entity("AuthServer.Models.Restaurant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<int>("CityId");

                    b.Property<string>("Name");

                    b.Property<string>("Notes");

                    b.Property<string>("PostalCode");

                    b.Property<bool>("Verified");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.ToTable("Restaurants");
                });

            modelBuilder.Entity("AuthServer.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Notes");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("AuthServer.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<string>("Email");

                    b.Property<string>("Name");

                    b.Property<string>("Password");

                    b.Property<int>("RestaurantId");

                    b.Property<string>("Username");

                    b.Property<bool>("Verified");

                    b.HasKey("Id");

                    b.HasIndex("RestaurantId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("AuthServer.Models.UserClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserClaims");
                });

            modelBuilder.Entity("AuthServer.Models.City", b =>
                {
                    b.HasOne("AuthServer.Models.Province", "Province")
                        .WithMany("Cities")
                        .HasForeignKey("ProvinceId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AuthServer.Models.Customer", b =>
                {
                    b.HasOne("AuthServer.Models.City", "City")
                        .WithMany()
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AuthServer.Models.Province", "Province")
                        .WithMany()
                        .HasForeignKey("ProvinceId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AuthServer.Models.Restaurant", "Restaurant")
                        .WithMany("Customers")
                        .HasForeignKey("RestaurantId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AuthServer.Models.GiftCard", b =>
                {
                    b.HasOne("AuthServer.Models.Customer", "Customer")
                        .WithMany("GiftCards")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AuthServer.Models.GiftCardType", "GiftCardType")
                        .WithMany()
                        .HasForeignKey("GiftCardTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AuthServer.Models.GiftCardType", b =>
                {
                    b.HasOne("AuthServer.Models.GiftCardType")
                        .WithMany("GiftCards")
                        .HasForeignKey("GiftCardTypeId");
                });

            modelBuilder.Entity("AuthServer.Models.Order", b =>
                {
                    b.HasOne("AuthServer.Models.Customer", "Customer")
                        .WithMany("Orders")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AuthServer.Models.OrderType", "OrderType")
                        .WithMany("Orders")
                        .HasForeignKey("OrderTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AuthServer.Models.Province", b =>
                {
                    b.HasOne("AuthServer.Models.Country", "Country")
                        .WithMany("Provinces")
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AuthServer.Models.Reservation", b =>
                {
                    b.HasOne("AuthServer.Models.Customer", "Customer")
                        .WithMany("Reservations")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AuthServer.Models.ReservationStatus", "ReservationStatus")
                        .WithMany("Reservations")
                        .HasForeignKey("ReservationStatusId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AuthServer.Models.Restaurant", b =>
                {
                    b.HasOne("AuthServer.Models.City", "City")
                        .WithMany()
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AuthServer.Models.User", b =>
                {
                    b.HasOne("AuthServer.Models.Restaurant", "Restaurant")
                        .WithMany("Users")
                        .HasForeignKey("RestaurantId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AuthServer.Models.UserClaim", b =>
                {
                    b.HasOne("AuthServer.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });
        }
    }
}
