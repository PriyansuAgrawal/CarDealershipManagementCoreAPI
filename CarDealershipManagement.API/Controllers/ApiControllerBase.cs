using CarDealershipManagement.Core.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CarDealershipManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class ApiControllerBase : ControllerBase
    {
        protected ActionResult<ApiResponse<T>> Success<T>(T data, string message = "Operation completed successfully")
        {
            return Ok(ApiResponse<T>.CreateSuccess(data, message));
        }

        protected ActionResult<ApiResponse<T>> Error<T>(string message, T data = default)
        {
            return BadRequest(ApiResponse<T>.CreateError(message, data));
        }
    }
}
