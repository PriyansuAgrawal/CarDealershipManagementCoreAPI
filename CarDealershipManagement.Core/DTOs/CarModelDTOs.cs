using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealershipManagement.Core.DTOs
{
    public class CarModelDto
    {
        public int ModelId { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public string ModelName { get; set; }
        public string ModelCode { get; set; }
        public decimal Price { get; set; }
        public DateTime DateOfManufacturing { get; set; }
        public bool IsActive { get; set; }
        public int SortOrder { get; set; }
        public string DefaultImage { get; set; }
    }
    public class CarModelDetailDto : CarModelDto
    {
        public string Description { get; set; }
        public string Features { get; set; }
        public List<ModelImageDto> Images { get; set; } = new List<ModelImageDto>();
    }
    public class ModelImageDto
    {
        public int ImageId { get; set; }
        public int ModelId { get; set; }
        public string ImagePath { get; set; }
        public bool IsDefault { get; set; }
        public DateTime UploadDate { get; set; }
    }
    public class CarModelCreateRequest
    {
        [Required]
        public int BrandId { get; set; }

        [Required]
        public int ClassId { get; set; }

        [Required]
        public string ModelName { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 10)]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Model code must contain only alphanumeric characters")]
        public string ModelCode { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Features { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        public DateTime DateOfManufacturing { get; set; }

        public bool IsActive { get; set; } = true;

        [Required]
        public int SortOrder { get; set; }
    }
    public class CarModelUpdateRequest : CarModelCreateRequest
    {
        [Required]
        public int ModelId { get; set; }

    }
    public class CarModelSearchRequest
    {
        public string? SearchTerm { get; set; } = "";
        public string OrderBy { get; set; } = "DateOfManufacturing DESC";
    }
    public class ImageUploadRequest
    {
        [Required]
        public int ModelId { get; set; }

        [Required]
        public IFormFile Image { get; set; }

        public bool IsDefault { get; set; }
    }
    public class BrandDto
    {
        public int BrandId { get; set; }
        public string Name { get; set; }
    }
    public class ClassDto
    {
        public int ClassId { get; set; }
        public string Name { get; set; }
    }
}
