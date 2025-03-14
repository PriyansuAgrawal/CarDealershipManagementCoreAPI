using CarDealershipManagement.Core.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealershipManagement.Core.Interfaces.Repositories
{
    public interface ICarModelRepository : IRepository<CarModel>
    {
        Task<IEnumerable<CarModel>> SearchAsync(string searchTerm, string orderBy);
        Task<int> AddImageAsync(int modelId, string imagePath, bool isDefault);
        Task<bool> DeleteImageAsync(int imageId);
        Task<bool> SetDefaultImageAsync(int imageId);
        Task<IEnumerable<Brand>> GetAllBrandsAsync();
        Task<IEnumerable<Class>> GetAllClassesAsync();
    }
}
