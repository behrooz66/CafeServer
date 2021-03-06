﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AuthServer.Models;

namespace AuthServer.Infrastructure
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options) { }

        // entities
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserClaim> UserClaims { get; set; }
        
        public DbSet<Country> Countries { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<OrderType> OrderTypes { get; set; }
        public DbSet<GiftCardType> GiftCardTypes { get; set; }
        public DbSet<ReservationStatus> ReservationStatuses { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<GiftCard> GiftCards { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        public DbSet<CustomerHistory> CustomerHistories { get; set; }
        public DbSet<OrderHistory> OrderHistories { get; set; }
        public DbSet<ReservationHistory> ReservationHistories { get; set; }
        public DbSet<GiftCardHistory> GiftCardHistories { get; set; }

        public DbSet<Message> Messages { get; set; }

    }
}
