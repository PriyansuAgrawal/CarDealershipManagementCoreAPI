using CarDealershipManagement.Core.Domain.Models;
using CarDealershipManagement.Core.Interfaces.Repositories;
using CarDealershipManagement.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealershipManagement.Infrastructure.Repositories
{
    public class CarModelRepository : ICarModelRepository
    {
        private readonly SqlHelper _sqlHelper;

        public CarModelRepository(SqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }

        public async Task<CarModel> GetByIdAsync(int id)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@ModelId", id)
            };

            using var reader = await _sqlHelper.ExecuteReaderAsync("sp_GetCarModelById", parameters);

            CarModel carModel = null;

            if (await reader.ReadAsync())
            {
                carModel = new CarModel
                {
                    ModelId = reader.GetInt32(reader.GetOrdinal("ModelId")),
                    BrandId = reader.GetInt32(reader.GetOrdinal("BrandId")),
                    ClassId = reader.GetInt32(reader.GetOrdinal("ClassId")),
                    ModelName = reader.GetString(reader.GetOrdinal("ModelName")),
                    ModelCode = reader.GetString(reader.GetOrdinal("ModelCode")),
                    Description = reader.GetString(reader.GetOrdinal("Description")),
                    Features = reader.GetString(reader.GetOrdinal("Features")),
                    Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                    DateOfManufacturing = reader.GetDateTime(reader.GetOrdinal("DateOfManufacturing")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                    SortOrder = reader.GetInt32(reader.GetOrdinal("SortOrder")),
                    BrandName = reader.GetString(reader.GetOrdinal("BrandName")),
                    ClassName = reader.GetString(reader.GetOrdinal("ClassName")),
                    CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                    ModifiedDate = reader.GetDateTime(reader.GetOrdinal("ModifiedDate")),
                    Images = new List<ModelImage>()
                };
            }

            // If model was found and there's a second result set with images
            if (carModel != null && await reader.NextResultAsync())
            {
                while (await reader.ReadAsync())
                {
                    carModel.Images.Add(new ModelImage
                    {
                        ImageId = reader.GetInt32(reader.GetOrdinal("ImageId")),
                        ModelId = reader.GetInt32(reader.GetOrdinal("ModelId")),
                        ImagePath = reader.GetString(reader.GetOrdinal("ImagePath")),
                        IsDefault = reader.GetBoolean(reader.GetOrdinal("IsDefault")),
                        UploadDate = reader.GetDateTime(reader.GetOrdinal("UploadDate"))
                    });

                    if (reader.GetBoolean(reader.GetOrdinal("IsDefault")))
                    {
                        carModel.DefaultImage = reader.GetString(reader.GetOrdinal("ImagePath"));
                    }
                }
            }

            return carModel;
        }

        public async Task<IEnumerable<CarModel>> GetAllAsync()
        {
            return await SearchAsync(null, null);
        }

        public async Task<IEnumerable<CarModel>> SearchAsync(string searchTerm, string orderBy)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@SearchTerm", SqlDbType.NVarChar, 100) { Value = searchTerm ?? (object)DBNull.Value },
                new SqlParameter("@OrderBy", SqlDbType.NVarChar, 50) { Value = orderBy ?? (object)DBNull.Value }
            };

            var carModels = new List<CarModel>();

            using var reader = await _sqlHelper.ExecuteReaderAsync("sp_GetCarModels", parameters);

            while (await reader.ReadAsync())
            {
                var model = new CarModel
                {
                    ModelId = reader.GetInt32(reader.GetOrdinal("ModelId")),
                    BrandId = reader.GetInt32(reader.GetOrdinal("BrandId")),
                    ClassId = reader.GetInt32(reader.GetOrdinal("ClassId")),
                    ModelName = reader.GetString(reader.GetOrdinal("ModelName")),
                    ModelCode = reader.GetString(reader.GetOrdinal("ModelCode")),
                    Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                    DateOfManufacturing = reader.GetDateTime(reader.GetOrdinal("DateOfManufacturing")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                    SortOrder = reader.GetInt32(reader.GetOrdinal("SortOrder")),
                    BrandName = reader.GetString(reader.GetOrdinal("BrandName")),
                    ClassName = reader.GetString(reader.GetOrdinal("ClassName"))
                };

                // DefaultImage may be null
                var defaultImageOrdinal = reader.GetOrdinal("DefaultImage");
                if (!reader.IsDBNull(defaultImageOrdinal))
                {
                    model.DefaultImage = reader.GetString(defaultImageOrdinal);
                }

                carModels.Add(model);
            }

            return carModels;
        }

        public async Task<int> AddAsync(CarModel entity)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@BrandId", entity.BrandId),
                new SqlParameter("@ClassId", entity.ClassId),
                new SqlParameter("@ModelName", entity.ModelName),
                new SqlParameter("@ModelCode", entity.ModelCode),
                new SqlParameter("@Description", entity.Description),
                new SqlParameter("@Features", entity.Features),
                new SqlParameter("@Price", entity.Price),
                new SqlParameter("@DateOfManufacturing", entity.DateOfManufacturing),
                new SqlParameter("@IsActive", entity.IsActive),
                new SqlParameter("@SortOrder", entity.SortOrder)
            };

            return await _sqlHelper.ExecuteScalarAsync<int>("sp_CreateCarModel", parameters);
        }

        public async Task<bool> UpdateAsync(CarModel entity)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@ModelId", entity.ModelId),
                new SqlParameter("@BrandId", entity.BrandId),
                new SqlParameter("@ClassId", entity.ClassId),
                new SqlParameter("@ModelName", entity.ModelName),
                new SqlParameter("@ModelCode", entity.ModelCode),
                new SqlParameter("@Description", entity.Description),
                new SqlParameter("@Features", entity.Features),
                new SqlParameter("@Price", entity.Price),
                new SqlParameter("@DateOfManufacturing", entity.DateOfManufacturing),
                new SqlParameter("@IsActive", entity.IsActive),
                new SqlParameter("@SortOrder", entity.SortOrder)
            };

            await _sqlHelper.ExecuteNonQueryAsync("sp_UpdateCarModel", parameters);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@ModelId", id)
            };

            await _sqlHelper.ExecuteNonQueryAsync("sp_DeleteCarModel", parameters);
            return true;
        }

        public async Task<int> AddImageAsync(int modelId, string imagePath, bool isDefault)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@ModelId", modelId),
                new SqlParameter("@ImagePath", imagePath),
                new SqlParameter("@IsDefault", isDefault)
            };

            return await _sqlHelper.ExecuteScalarAsync<int>("sp_AddCarModelImage", parameters);
        }

        public async Task<bool> DeleteImageAsync(int imageId)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@ImageId", imageId)
            };

            await _sqlHelper.ExecuteNonQueryAsync("sp_DeleteCarModelImage", parameters);
            return true;
        }

        public async Task<bool> SetDefaultImageAsync(int imageId)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@ImageId", imageId)
            };

            await _sqlHelper.ExecuteNonQueryAsync("sp_SetDefaultCarModelImage", parameters);
            return true;
        }

        public async Task<IEnumerable<Brand>> GetAllBrandsAsync()
        {
            var brands = new List<Brand>();

            using var reader = await _sqlHelper.ExecuteReaderAsync("sp_GetBrands");

            while (await reader.ReadAsync())
            {
                brands.Add(new Brand
                {
                    BrandId = reader.GetInt32(reader.GetOrdinal("BrandId")),
                    Name = reader.GetString(reader.GetOrdinal("Name"))
                });
            }

            return brands;
        }

        public async Task<IEnumerable<Class>> GetAllClassesAsync()
        {
            var classes = new List<Class>();

            using var reader = await _sqlHelper.ExecuteReaderAsync("sp_GetClasses");

            while (await reader.ReadAsync())
            {
                classes.Add(new Class
                {
                    ClassId = reader.GetInt32(reader.GetOrdinal("ClassId")),
                    Name = reader.GetString(reader.GetOrdinal("Name"))
                });
            }

            return classes;
        }
    }
}
