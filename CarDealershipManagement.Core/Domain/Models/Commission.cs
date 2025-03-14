using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealershipManagement.Core.Domain.Models
{
    public class CommissionReport
    {
        public int SalesmanId { get; set; }
        public string SalesmanName { get; set; }
        public string BrandName { get; set; }
        public string ClassName { get; set; }
        public int CarsSold { get; set; }
        public decimal ModelPrice { get; set; }
        public decimal TotalSales { get; set; }
        public decimal FixedCommission { get; set; }
        public decimal ClassCommission { get; set; }
        public decimal BonusCommission { get; set; }
        public decimal TotalCommission { get; set; }
    }

    public class CommissionSummary
    {
        public int SalesmanId { get; set; }
        public string SalesmanName { get; set; }
        public int TotalCarsSold { get; set; }
        public decimal TotalSalesAmount { get; set; }
        public decimal TotalFixedCommission { get; set; }
        public decimal TotalClassCommission { get; set; }
        public decimal TotalBonusCommission { get; set; }
        public decimal GrandTotalCommission { get; set; }
    }
}
