using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealershipManagement.Core.DTOs
{
    public class AuthRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
    public class AuthResponse
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
        public string RoleName { get; set; }
        public int RoleId { get; set; } 
    }
    public class RegisterRequest
    {
        [Required]
        [MinLength(3)]
        public string Username { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public int RoleId { get; set; }
    }
    public class MenuItemDto
    {
        public int MenuId { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Icon { get; set; }
        public int? ParentId { get; set; }
        public int SortOrder { get; set; }
        public List<MenuItemDto> Children { get; set; } = new List<MenuItemDto>();
    }
}
