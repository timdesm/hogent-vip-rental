using DomainLayer.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer
{
    public class RentalContext : DbContext
    {
        private string connectionString;

        public RentalContext() { }

        public RentalContext(string db = "production") : base()
        {
            SetConnectionString(db);
        }

        private void SetConnectionString(string db = "production")
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json", optional: false);

            var configuration = builder.Build();
            switch (db)
            {
                case "production":
                    connectionString = configuration.GetConnectionString("ProductionSQLConnection").ToString();
                    break;
                case "development":
                    connectionString = configuration.GetConnectionString("DevelopmentSQLConnection").ToString();
                    break;
            }
        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<CarReservation> CarReservations { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (connectionString == null)
            {
                SetConnectionString();
            }
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CarReservation>()
                 .HasKey(c => new { c.CarID, c.ReservationID });
        }
    }
}
