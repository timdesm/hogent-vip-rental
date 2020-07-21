using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    ID = table.Column<string>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    AddressStreet = table.Column<string>(nullable: true),
                    AddressNumber = table.Column<string>(nullable: true),
                    AddressBus = table.Column<string>(nullable: true),
                    AddressZip = table.Column<string>(nullable: true),
                    AddressCity = table.Column<string>(nullable: true),
                    AddressCounty = table.Column<string>(nullable: true),
                    CompanyName = table.Column<string>(nullable: true),
                    VATNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientID = table.Column<string>(nullable: true),
                    InvoiceDate = table.Column<DateTime>(nullable: false),
                    DiscountPercent = table.Column<double>(nullable: false),
                    Discount = table.Column<double>(nullable: false),
                    VATPercent = table.Column<double>(nullable: false),
                    VAT = table.Column<double>(nullable: false),
                    SubTotal = table.Column<double>(nullable: false),
                    TotalExc = table.Column<double>(nullable: false),
                    TotalInc = table.Column<double>(nullable: false),
                    PaymentDue = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Invoices_Clients_ClientID",
                        column: x => x.ClientID,
                        principalTable: "Clients",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceItems",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    UnitPrice = table.Column<double>(nullable: false),
                    Total = table.Column<double>(nullable: false),
                    InvoiceID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceItems", x => x.ID);
                    table.ForeignKey(
                        name: "FK_InvoiceItems_Invoices_InvoiceID",
                        column: x => x.InvoiceID,
                        principalTable: "Invoices",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientID = table.Column<string>(nullable: true),
                    OrderDate = table.Column<DateTime>(nullable: false),
                    ReservationDate = table.Column<DateTime>(nullable: false),
                    StartLocation = table.Column<string>(nullable: true),
                    EndLocation = table.Column<string>(nullable: true),
                    Arrangement = table.Column<int>(nullable: false),
                    ReservedUntil = table.Column<DateTime>(nullable: false),
                    ReservationEnded = table.Column<DateTime>(nullable: false),
                    InvoiceID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Reservations_Clients_ClientID",
                        column: x => x.ClientID,
                        principalTable: "Clients",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reservations_Invoices_InvoiceID",
                        column: x => x.InvoiceID,
                        principalTable: "Invoices",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Brand = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Color = table.Column<string>(nullable: true),
                    PriceFirst = table.Column<double>(nullable: false),
                    PriceNight = table.Column<double>(nullable: false),
                    PriceWedding = table.Column<double>(nullable: false),
                    PriceWellness = table.Column<double>(nullable: false),
                    Available = table.Column<bool>(nullable: false),
                    ReservationID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Cars_Reservations_ReservationID",
                        column: x => x.ReservationID,
                        principalTable: "Reservations",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cars_ReservationID",
                table: "Cars",
                column: "ReservationID");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceItems_InvoiceID",
                table: "InvoiceItems",
                column: "InvoiceID");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_ClientID",
                table: "Invoices",
                column: "ClientID");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_ClientID",
                table: "Reservations",
                column: "ClientID");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_InvoiceID",
                table: "Reservations",
                column: "InvoiceID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cars");

            migrationBuilder.DropTable(
                name: "InvoiceItems");

            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "Clients");
        }
    }
}
