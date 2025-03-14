using AutoMapper;
using CarDealershipManagement.Core.DTOs;
using CarDealershipManagement.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarDealershipManagement.API.Controllers
{
    public class ReportController : ApiControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReportController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("commission")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<CommissionReportDto>>> GetCommissionReport([FromQuery] CommissionReportRequest request)
        {
            var result = await _unitOfWork.Commissions.GenerateReportAsync(request.Month, request.Year);

            var reportDto = new CommissionReportDto
            {
                Details = _mapper.Map<List<CommissionDetailDto>>(result.Details),
                Summary = _mapper.Map<List<CommissionSummaryDto>>(result.Summary)
            };

            return Success(reportDto);
        }
    }
}
