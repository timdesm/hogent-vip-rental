﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainLayer.Domain
{
     public class InvoiceItem
     {
        public InvoiceItem() { }
        public InvoiceItem(int amount, string description, double unitPrice, double total, int invoiceID)
        {
            Amount = amount;
            Description = description;
            UnitPrice = unitPrice;
            Total = total;
            InvoiceID = invoiceID;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int Amount { get; set; }
        public string Description { get; set; }
        public double UnitPrice { get; set; }
        public double Total { get; set; }
        [ForeignKey("Invoice")]
        public int InvoiceID { get; set; }

        public override string ToString()
        {
            return $"InvoiceItem : {ID},{Amount},{Description},{UnitPrice},{Total},{InvoiceID}";
        }
     }
}