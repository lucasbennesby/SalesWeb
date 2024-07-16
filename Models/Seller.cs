using Microsoft.AspNetCore.Antiforgery;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Security.Policy;

namespace SalesWeb.Models
{
    public class Seller
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} Cannot be null")]    
        public string Name { get; set; }

        [Required(ErrorMessage ="{0} Cannot be null")]
        [EmailAddress(ErrorMessage ="Enter a valid email")]
        public string Email { get; set; }

        [Display(Name = "Birth Date")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "{0} Cannot be null")]
        public DateTime BirthDate { get; set; }

        [Display(Name="Base Salary")]
        [Required(ErrorMessage = "{0} Cannot be null")]

        [DisplayFormat(DataFormatString ="{0:F2}")]
        public double BaseSalary { get; set; }

        public Department Department { get; set; }
        public int DepartmentId { get; set; }

        public ICollection<SalesRecord> Sales { get; set; } = new List<SalesRecord>();

        public Seller()
        {
        }

        public Seller(int id, string name, string email, DateTime birthDate, double baseSalary, Department department)
        {
            Id = id;
            Name = name;
            Email = email;
            BirthDate = birthDate;
            BaseSalary = baseSalary;
            Department = department;

        }

        public void addSales(SalesRecord sr)
        {
            Sales.Add(sr);
        }
        public void removeSales(SalesRecord sr)
        {
            Sales.Remove(sr);

        }
        public double totalSales(DateTime initial, DateTime final)
        {
            return Sales.Where(sr => sr.Date >= initial && sr.Date <= final).Sum(sr=>sr.Amount);

        }
    }
}
