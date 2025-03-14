using AutoMapper;
using CarDealershipManagement.Core.Domain.Models;
using CarDealershipManagement.Core.DTOs;
using CarDealershipManagement.Core.Interfaces.Repositories;
using CarDealershipManagement.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BC = BCrypt.Net.BCrypt;

namespace CarDealershipManagement.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public AuthService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<AuthResponse> LoginAsync(AuthRequest request)
        {
            var user = await _unitOfWork.Users.GetByUsernameAsync(request.Username);

            if (user == null || !BC.Verify(request.Password, user.PasswordHash))
                return null;

            var token = GenerateJwtToken(user);

            var response = _mapper.Map<AuthResponse>(user);
            response.Token = token;

            return response;
        }

        public async Task<int> RegisterAsync(RegisterRequest request)
        {
            // Check if username already exists
            var existingUser = await _unitOfWork.Users.GetByUsernameAsync(request.Username);
            if (existingUser != null)
                throw new InvalidOperationException("Username already exists");

            // Create user entity
            var user = _mapper.Map<User>(request);
            user.PasswordHash = BC.HashPassword(request.Password);
            user.CreatedDate = DateTime.Now;
            user.ModifiedDate = DateTime.Now;

            return await _unitOfWork.Users.AddAsync(user);
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _unitOfWork.Users.GetByIdAsync(userId);
        }

        public async Task<IEnumerable<MenuItem>> GetUserMenuAsync(int userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
                return Enumerable.Empty<MenuItem>();

            var menuItems = await _unitOfWork.Users.GetMenuByRoleIdAsync(user.RoleId);
            return BuildMenuTree(menuItems.ToList());
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:Secret"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.RoleName)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        private IEnumerable<MenuItem> BuildMenuTree(List<MenuItem> allItems)
        {
            var rootItems = allItems.Where(x => x.ParentId == null).ToList();
            var dict = allItems.ToDictionary(x => x.MenuId);

            foreach (var item in allItems.Where(x => x.ParentId != null))
            {
                if (dict.TryGetValue(item.ParentId.Value, out var parent))
                {
                    parent.Children.Add(item);
                }
            }

            return rootItems;
        }
    }
}
