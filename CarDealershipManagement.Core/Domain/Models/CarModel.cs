using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealershipManagement.Core.Domain.Models
{
    public class CarModel
    {
        public int ModelId { get; set; }
        public int BrandId { get; set; }
        public int ClassId { get; set; }
        public string ModelName { get; set; }
        public string ModelCode { get; set; }
        public string Description { get; set; }
        public string Features { get; set; }
        public decimal Price { get; set; }
        public DateTime DateOfManufacturing { get; set; }
        public bool IsActive { get; set; }
        public int SortOrder { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        // Navigation properties
        public string BrandName { get; set; }
        public string ClassName { get; set; }
        public string DefaultImage { get; set; }
        public List<ModelImage> Images { get; set; } = new List<ModelImage>();
    }

    public class ModelImage
    {
        public int ImageId { get; set; }
        public int ModelId { get; set; }
        public string ImagePath { get; set; }
        public bool IsDefault { get; set; }
        public DateTime UploadDate { get; set; }
    }

    public class Brand
    {
        public int BrandId { get; set; }
        public string Name { get; set; }
    }

    public class Class
    {
        public int ClassId { get; set; }
        public string Name { get; set; }
    }
}
