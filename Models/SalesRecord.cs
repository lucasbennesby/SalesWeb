using SalesWeb.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace SalesWeb.Models
{
    public class SalesRecord
    {
        public int Id { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [DisplayFormat(DataFormatString = "R$ {0:n2}")]
        public double Amount { get; set; }
        public SaleStatus Status { get; set; }  

        public Seller Seller { get; set; }
        public int SellerId {  get; set; }


        public SalesRecord() { }

        public SalesRecord(int id, DateTime date, double amount, SaleStatus status, Seller seller)
        {
            Id = id;
            Date = date;
            Amount = amount;
            Status = status;
            Seller = seller;
        }
    }
}
