using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text;

namespace DomainLayer.Domain
{
    public class Client
    {
        public Client() {}
        
        public Client(ClientType type, string firstName, string lastName, string email, string phone, string addressStreet, string addressNumber, string addressBus, string addressZip, string addressCity, string addressCountry, string companyName, string vatNumber)
        {
            Type = type;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Phone = phone;
            AddressStreet = addressStreet;
            AddressNumber = addressNumber;
            AddressBus = addressBus;
            AddressZip = addressZip;
            AddressCity = addressCity;
            AddressCounty = addressCountry;
            CompanyName = companyName;
            VATNumber = vatNumber;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public ClientType Type { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; }

        public string AddressStreet { get; set; }
        public string AddressNumber { get; set; }
        public string AddressBus { get; set; }
        public string AddressZip { get; set; }
        public string AddressCity { get; set; }
        public string AddressCounty { get; set; }

        public string CompanyName { get; set; }
        public string  VATNumber { get; set; }

        public override string ToString()
        {
            return $"Client : {ID},{Type},{FirstName},{LastName},{Email},{Phone},{AddressStreet},{AddressNumber},{AddressBus},{AddressZip},{AddressCity},{AddressCounty},{CompanyName},{VATNumber}";
        }

        public double GetDiscount(int totalReservations)
        {
            double discount = 0;

            switch (this.Type)
            {
                case ClientType.VIP:
                    if (totalReservations >= 2 && totalReservations < 7) discount = 5.0;
                    else if (totalReservations >= 7 && totalReservations < 15) discount = 7.5;
                    else if (totalReservations >= 15) discount = 10.0;
                    break;
                case ClientType.PLANNER:
                    if (totalReservations >= 5 && totalReservations < 10) discount = 7.5;
                    else if (totalReservations >= 10 && totalReservations < 15) discount = 10.0;
                    else if (totalReservations >= 15 && totalReservations < 20) discount = 12.5;
                    else if (totalReservations >= 20 && totalReservations < 25) discount = 15.0;
                    else if (totalReservations >= 25) discount = 25.0;
                    break;
            }
            return discount;
        }
    }
}
