using AutoMapper;
using CarDealershipManagement.Core.Domain.Models;
using CarDealershipManagement.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealershipManagement.Core.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Car Models
            CreateMap<CarModel, CarModelDto>();
            CreateMap<CarModel, CarModelDetailDto>();
            CreateMap<CarModelCreateRequest, CarModel>();
            CreateMap<CarModelUpdateRequest, CarModel>();
            CreateMap<ModelImage, ModelImageDto>();

            // Brands & Classes
            CreateMap<Brand, BrandDto>();
            CreateMap<Class, ClassDto>();

            // Salesmen
            CreateMap<Salesman, SalesmanDto>();
            CreateMap<SalesmanCreateRequest, Salesman>();
            CreateMap<SalesmanUpdateRequest, Salesman>();
            CreateMap<SalesRecord, SalesRecordDto>();
            CreateMap<SalesRecordUpsertRequest, SalesRecord>()
                .ForMember(dest => dest.SaleMonth, opt => opt.MapFrom(src => src.Month))
                .ForMember(dest => dest.SaleYear, opt => opt.MapFrom(src => src.Year));

            // Commission
            CreateMap<CommissionReport, CommissionDetailDto>();
            CreateMap<CommissionSummary, CommissionSummaryDto>();

            // Auth
            CreateMap<User, AuthResponse>();
            CreateMap<RegisterRequest, User>();
            CreateMap<MenuItem, MenuItemDto>();
        }
    }
}
