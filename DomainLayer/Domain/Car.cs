using DomainLayer.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainLayer.Domain
{
    public class Car
    {
        public Car() {}

        public Car(string brand, string type, string color, double priceFirst, double priceNight, double priceWedding, double priceWellness, Boolean available)
        {
            Brand = brand;
            Type = type;
            Color = color;
            PriceFirst = priceFirst;
            PriceNight = priceNight;
            PriceWedding = priceWedding;
            PriceWellness = priceWellness;
            Available = available;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Brand { get; set; }
        public string Type { get; set; }
        public string Color { get; set; }
        public double PriceFirst { get; set; }
        public double PriceNight { get; set; }
        public double PriceWedding { get; set; }
        public double PriceWellness { get; set; }
        public Boolean Available { get; set; }

        public override string ToString()
        {
            return $"Car : {ID},{Brand},{Type},{Color},{PriceFirst},{PriceNight},{PriceWedding},{PriceWellness},{Available}";
        }

        public double GetPrice(DateTime from, DateTime until, ReservationArrangementType arrangement)
        {
            Double price = 0.0;

            TimeSpan diffTime = until - from;
            Double totalHours = diffTime.TotalHours;

            switch (arrangement)
            {
                case ReservationArrangementType.NIGHT:
                    if (totalHours > 7)
                        from.AddHours(7);
                    break;
                case ReservationArrangementType.WEDDING:
                    if (totalHours > 7)
                        from.AddHours(7);
                    break;
                case ReservationArrangementType.WELLNESS:
                    if (totalHours > 10)
                        from.AddHours(10);
                    break;
                default:
                    break;
            }

            TimeSpan fromTime = from.TimeOfDay;
            TimeSpan untilTime = until.TimeOfDay;

            Double normalHours = 0.0;
            Double nightHours = 0.0;

            while (fromTime < untilTime)
            {
                fromTime = fromTime.Add(TimeSpan.FromHours(1));
                if (fromTime >= TimeSpan.FromHours(22) || fromTime <= TimeSpan.FromHours(6))
                    nightHours += 1;
                else
                    normalHours += 1;
            }

            switch (arrangement)
            {
                case ReservationArrangementType.NIGHT:
                    price = this.PriceNight;
                    if (totalHours > 7)
                    {
                        price += normalHours * (this.PriceFirst * 0.65);
                        price += nightHours * (this.PriceFirst * 1.4);
                    }
                    break;
                case ReservationArrangementType.WEDDING:
                    price = this.PriceWedding;
                    if (totalHours > 7)
                    {
                        price += normalHours * (this.PriceFirst * 0.65);
                        price += nightHours * (this.PriceFirst * 1.4);
                    }
                    break;
                case ReservationArrangementType.WELLNESS:
                    price = this.PriceWedding;
                    if (totalHours > 10)
                    {
                        price += normalHours * (this.PriceFirst * 0.65);
                        price += nightHours * (this.PriceFirst * 1.4);
                    }
                    break;
                default:
                    if (!TimeUtilities.IsBetweenTimes(from, TimeSpan.FromHours(22), TimeSpan.FromHours(6)))
                    {
                        price = this.PriceFirst;
                        normalHours -= 1;
                    }
                    price += normalHours * (this.PriceFirst * 0.65);
                    price += nightHours * (this.PriceFirst * 1.4);
                    break;
            }
            return Math.Round(price / 5.0) * 5;
        }
    }
}
