using CarDealershipManagement.Core.Domain.Models;
using CarDealershipManagement.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealershipManagement.Core.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> LoginAsync(AuthRequest request);
        Task<int> RegisterAsync(RegisterRequest request);
        Task<User> GetUserByIdAsync(int userId);
        Task<IEnumerable<MenuItem>> GetUserMenuAsync(int userId);
    }
}
