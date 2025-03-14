using AutoMapper;
using CarDealershipManagement.Core.Domain.Models;
using CarDealershipManagement.Core.DTOs;
using CarDealershipManagement.Core.Interfaces.Repositories;
using CarDealershipManagement.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CarDealershipManagement.API.Controllers
{
    public class CarModelController : ApiControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileStorageService _fileStorageService;

        public CarModelController(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IFileStorageService fileStorageService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileStorageService = fileStorageService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<CarModelDto>>>> GetAll([FromQuery] CarModelSearchRequest request)
        {
            var carModels = await _unitOfWork.CarModels.SearchAsync(request.SearchTerm, request.OrderBy);
            var carModelDtos = _mapper.Map<IEnumerable<CarModelDto>>(carModels);

            return Success(carModelDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<CarModelDetailDto>>> GetById(int id)
        {
            var carModel = await _unitOfWork.CarModels.GetByIdAsync(id);

            if (carModel == null)
                return Error<CarModelDetailDto>($"Car model with ID {id} not found");

            var carModelDto = _mapper.Map<CarModelDetailDto>(carModel);

            return Success(carModelDto);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<ApiResponse<int>>> Create([FromBody] CarModelCreateRequest request)
        {
            var carModel = _mapper.Map<CarModel>(request);
            carModel.CreatedDate = DateTime.Now;
            carModel.ModifiedDate = DateTime.Now;

            var modelId = await _unitOfWork.CarModels.AddAsync(carModel);

            return Success(modelId, "Car model created successfully");
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<ApiResponse<bool>>> Update(int id, [FromBody] CarModelUpdateRequest request)
        {
            if (id != request.ModelId)
                return Error<bool>("ID mismatch");

            var existingModel = await _unitOfWork.CarModels.GetByIdAsync(id);

            if (existingModel == null)
                return Error<bool>($"Car model with ID {id} not found");

            var carModel = _mapper.Map<CarModel>(request);
            carModel.CreatedDate = existingModel.CreatedDate;
            carModel.ModifiedDate = DateTime.Now;

            var result = await _unitOfWork.CarModels.UpdateAsync(carModel);

            return Success(result, "Car model updated successfully");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
        {
            var carModel = await _unitOfWork.CarModels.GetByIdAsync(id);

            if (carModel == null)
                return Error<bool>($"Car model with ID {id} not found");

            var result = await _unitOfWork.CarModels.DeleteAsync(id);

            return Success(result, "Car model deleted successfully");
        }

        [HttpPost("image")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<ApiResponse<int>>> UploadImage([FromForm] ImageUploadRequest request)
        {
            if (request.Image == null || request.Image.Length == 0)
                return Error<int>("No image file provided");

            // Validate file size (max 5MB)
            if (request.Image.Length > 5 * 1024 * 1024)
                return Error<int>("Image size exceeds 5MB limit");

            // Validate file type
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(request.Image.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(fileExtension))
                return Error<int>("Invalid file type. Only JPG, PNG, and GIF are allowed");

            try
            {
                // Save image to storage
                var imagePath = await _fileStorageService.SaveFileAsync(request.Image, "uploads/models");

                // Add image to database
                var imageId = await _unitOfWork.CarModels.AddImageAsync(request.ModelId, imagePath, request.IsDefault);

                return Success(imageId, "Image uploaded successfully");
            }
            catch (Exception ex)
            {
                return Error<int>($"Failed to upload image: {ex.Message}");
            }
        }

        [HttpDelete("image/{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteImage(int id)
        {
            try
            {
                var result = await _unitOfWork.CarModels.DeleteImageAsync(id);
                return Success(result, "Image deleted successfully");
            }
            catch (Exception ex)
            {
                return Error<bool>($"Failed to delete image: {ex.Message}");
            }
        }

        [HttpPut("image/{id}/default")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<ApiResponse<bool>>> SetDefaultImage(int id)
        {
            try
            {
                var result = await _unitOfWork.CarModels.SetDefaultImageAsync(id);
                return Success(result, "Default image set successfully");
            }
            catch (Exception ex)
            {
                return Error<bool>($"Failed to set default image: {ex.Message}");
            }
        }

        [HttpGet("brands")]
        public async Task<ActionResult<ApiResponse<IEnumerable<BrandDto>>>> GetBrands()
        {
            var brands = await _unitOfWork.CarModels.GetAllBrandsAsync();
            var brandDtos = _mapper.Map<IEnumerable<BrandDto>>(brands);

            return Success(brandDtos);
        }

        [HttpGet("classes")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ClassDto>>>> GetClasses()
        {
            var classes = await _unitOfWork.CarModels.GetAllClassesAsync();
            var classDtos = _mapper.Map<IEnumerable<ClassDto>>(classes);

            return Success(classDtos);
        }
    }
}
