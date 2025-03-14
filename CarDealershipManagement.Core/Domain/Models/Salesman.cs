using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealershipManagement.Core.Domain.Models
{
    public class Salesman
    {
        public int SalesmanId { get; set; }
        public string Name { get; set; }
        public decimal PreviousYearSales { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class SalesRecord
    {
        public int RecordId { get; set; }
        public int SalesmanId { get; set; }
        public int BrandId { get; set; }
        public int ClassId { get; set; }
        public int NumberOfCarsSold { get; set; }
        public int SaleMonth { get; set; }
        public int SaleYear { get; set; }

        // Navigation properties
        public string SalesmanName { get; set; }
        public string BrandName { get; set; }
        public string ClassName { get; set; }
    }
}
