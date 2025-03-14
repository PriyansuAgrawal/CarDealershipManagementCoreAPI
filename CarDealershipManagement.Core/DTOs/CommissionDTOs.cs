using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealershipManagement.Core.DTOs
{
    public class CommissionReportRequest
    {
        [Range(1, 12)]
        public int? Month { get; set; }

        [Range(2000, 2100)]
        public int? Year { get; set; }
    }
    public class CommissionDetailDto
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
    public class CommissionSummaryDto
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
    public class CommissionReportDto
    {
        public List<CommissionDetailDto> Details { get; set; } = new List<CommissionDetailDto>();
        public List<CommissionSummaryDto> Summary { get; set; } = new List<CommissionSummaryDto>();
    }
}
