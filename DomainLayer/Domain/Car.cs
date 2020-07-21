using System;
using System.Collections.Generic;
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
    }
}
