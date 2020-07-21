using System;
using System.Collections.Generic;
using System.Text;

namespace DomainLayer.Domain
{
     public class InvoiceItem
     {
        public InvoiceItem() { }
        public InvoiceItem(int amount, string description, double unitPrice, double total)
        {
            Amount = amount;
            Description = description;
            UnitPrice = unitPrice;
            Total = total;
        }
        
        public int ID { get; set; }
        public int Amount { get; set; }
        public string Description { get; set; }
        public double UnitPrice { get; set; }
        public double Total { get; set; }

        public override string ToString()
        {
            return $"InvoiceItem : {ID},{Amount},{Description},{UnitPrice},{Total}";
        }
     }
}