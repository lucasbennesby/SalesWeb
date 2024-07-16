using System.ComponentModel.DataAnnotations;

namespace SalesWeb.Models
{
    public class Department
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="{0} Cannot be null")]
        public string Name { get; set; }

        public ICollection<Seller> Sellers { get; set; } = new List<Seller>();

        public Department()
        {
        }

        public Department(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public void Add(Seller s)
        {
            Sellers.Add(s);
        }

        public double TotalSales(DateTime initial, DateTime final)
        {
            return Sellers.Sum(s => s.totalSales(initial, final));
        }
    }
}
