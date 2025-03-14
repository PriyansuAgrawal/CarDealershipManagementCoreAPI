using AutoMapper;
using CarDealershipManagement.Core.DTOs;
using CarDealershipManagement.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarDealershipManagement.API.Controllers
{
    public class AuthController : ApiControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public AuthController(IAuthService authService, IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<AuthResponse>>> Login([FromBody] AuthRequest request)
        {
            var response = await _authService.LoginAsync(request);

            if (response == null)
                return Error<AuthResponse>("Invalid username or password");

            return Success(response);
        }

        [HttpPost("register")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<int>>> Register([FromBody] RegisterRequest request)
        {
            try
            {
                var userId = await _authService.RegisterAsync(request);
                return Success(userId, "User registered successfully");
            }
            catch (InvalidOperationException ex)
            {
                return Error<int>(ex.Message);
            }
        }

        [HttpGet("menu")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<IEnumerable<MenuItemDto>>>> GetMenu()
        {
            var currentUserService = HttpContext.RequestServices.GetRequiredService<ICurrentUserService>();
            var userId = currentUserService.UserId;

            if (!userId.HasValue)
                return Error<IEnumerable<MenuItemDto>>("User not authenticated");

            var menu = await _authService.GetUserMenuAsync(userId.Value);
            var menuDtos = _mapper.Map<IEnumerable<MenuItemDto>>(menu);

            return Success(menuDtos);
        }
        [HttpGet("menu/{id}")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<IEnumerable<MenuItemDto>>>> GetMenu([FromQuery] int roleId)
        {
            var currentUserService = HttpContext.RequestServices.GetRequiredService<ICurrentUserService>();
            var userId = currentUserService.UserId;

            if (!userId.HasValue)
                return Error<IEnumerable<MenuItemDto>>("User not authenticated");

            var menu = await _authService.GetUserMenuAsync(userId.Value);
            var menuDtos = _mapper.Map<IEnumerable<MenuItemDto>>(menu);

            return Success(menuDtos);
        }
    }
}
