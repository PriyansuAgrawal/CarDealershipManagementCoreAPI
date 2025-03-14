using AutoMapper;
using CarDealershipManagement.Core.Domain.Models;
using CarDealershipManagement.Core.DTOs;
using CarDealershipManagement.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarDealershipManagement.API.Controllers
{
    public class SalesmanController : ApiControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SalesmanController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<SalesmanDto>>>> GetAll()
        {
            var salesmen = await _unitOfWork.Salesmen.GetAllAsync();
            var salesmenDtos = _mapper.Map<IEnumerable<SalesmanDto>>(salesmen);

            return Success(salesmenDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<SalesmanDto>>> GetById(int id)
        {
            var salesman = await _unitOfWork.Salesmen.GetByIdAsync(id);

            if (salesman == null)
                return Error<SalesmanDto>($"Salesman with ID {id} not found");

            var salesmanDto = _mapper.Map<SalesmanDto>(salesman);

            return Success(salesmanDto);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<ApiResponse<int>>> Create([FromBody] SalesmanCreateRequest request)
        {
            var salesman = _mapper.Map<Salesman>(request);
            salesman.CreatedDate = DateTime.Now;

            var salesmanId = await _unitOfWork.Salesmen.AddAsync(salesman);

            return Success(salesmanId, "Salesman created successfully");
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<ApiResponse<bool>>> Update(int id, [FromBody] SalesmanUpdateRequest request)
        {
            if (id != request.SalesmanId)
                return Error<bool>("ID mismatch");

            var existingSalesman = await _unitOfWork.Salesmen.GetByIdAsync(id);

            if (existingSalesman == null)
                return Error<bool>($"Salesman with ID {id} not found");

            var salesman = _mapper.Map<Salesman>(request);
            salesman.CreatedDate = existingSalesman.CreatedDate;

            var result = await _unitOfWork.Salesmen.UpdateAsync(salesman);

            return Success(result, "Salesman updated successfully");
        }

        [HttpGet("sales")]
        public async Task<ActionResult<ApiResponse<IEnumerable<SalesRecordDto>>>> GetSalesRecords([FromQuery] int? month, [FromQuery] int? year)
        {
            var salesRecords = await _unitOfWork.Salesmen.GetSalesRecordsAsync(month, year);
            var salesRecordDtos = _mapper.Map<IEnumerable<SalesRecordDto>>(salesRecords);

            return Success(salesRecordDtos);
        }

        [HttpPost("sales")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<ApiResponse<int>>> UpsertSalesRecord([FromBody] SalesRecordUpsertRequest request)
        {
            var salesRecord = _mapper.Map<SalesRecord>(request);

            var recordId = await _unitOfWork.Salesmen.UpsertSalesRecordAsync(salesRecord);

            return Success(recordId, "Sales record saved successfully");
        }

        [HttpPost("populateSampleData")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<ApiResponse<bool>>> PopulateSampleData([FromQuery] int month, [FromQuery] int year)
        {
            var result = await _unitOfWork.Salesmen.PopulateSampleDataAsync(month, year);

            return Success(result, "Sample data populated successfully");
        }
    }
}
