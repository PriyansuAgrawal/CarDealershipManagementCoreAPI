using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealershipManagement.Core.DTOs
{
    public class SalesmanDto
    {
        public int SalesmanId { get; set; }
        public string Name { get; set; }
        public decimal PreviousYearSales { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public class SalesmanCreateRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal PreviousYearSales { get; set; }
    }
    public class SalesmanUpdateRequest : SalesmanCreateRequest
    {
        [Required]
        public int SalesmanId { get; set; }
    }
    public class SalesRecordDto
    {
        public int RecordId { get; set; }
        public int SalesmanId { get; set; }
        public string SalesmanName { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public int NumberOfCarsSold { get; set; }
        public int SaleMonth { get; set; }
        public int SaleYear { get; set; }
    }
    public class SalesRecordUpsertRequest
    {
        [Required]
        public int SalesmanId { get; set; }

        [Required]
        public int BrandId { get; set; }

        [Required]
        public int ClassId { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int NumberOfCarsSold { get; set; }

        [Required]
        [Range(1, 12)]
        public int Month { get; set; }

        [Required]
        [Range(2000, 2100)]
        public int Year { get; set; }
    }
}
