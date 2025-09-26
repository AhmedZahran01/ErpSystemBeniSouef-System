using AutoMapper;
using ErpSystemBeniSouef.Core.DTOs.Collector;
using ErpSystemBeniSouef.Core.DTOs.MainAreaDtos;
using ErpSystemBeniSouef.Core.DTOs.ProductDtos;
using ErpSystemBeniSouef.Core.DTOs.ProductsDto;
using ErpSystemBeniSouef.Core.DTOs.RepresentativeDto;
using ErpSystemBeniSouef.Core.DTOs.StorekeeperResponseDto;
using ErpSystemBeniSouef.Core.DTOs.SubAreaDtos;
using ErpSystemBeniSouef.Core.DTOs.SupplierDto;
using ErpSystemBeniSouef.Core.Entities;
using ErpSystemBeniSouef.Dtos.MainAreaDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Service.MappingProfiles
{
    public class ServiceMappingProfile : Profile
    {
        public ServiceMappingProfile()
        {     
            // SubArea
            CreateMap<CreateSubAreaDto, SubArea>().ReverseMap();
            CreateMap<SubAreaDto, SubArea>().ReverseMap();
            CreateMap<UpdateSubAreaDto, SubArea>()
                .ForAllMembers(opts =>
                    opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<CreateSubAreaDto, SubAreaDto>().ReverseMap();



            // MainArea
            // Map DTO to Entity (for Create/Update)
            CreateMap<CreateMainAreaDto, MainArea>().ReverseMap();
            CreateMap<UpdateMainAreaDto, MainArea>()
                .ForAllMembers(opts =>
                    opts.Condition((src, dest, srcMember) => srcMember != null));// Ignore nulls on update
            CreateMap<MainArea, MainAreaDto>().ReverseMap();

            //Product Mapp
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<CreateProductDto, ProductDto>().ReverseMap();
            CreateMap<CreateProductDto, Product>().ReverseMap();
            CreateMap<UpdateProductDto, Product>().ReverseMap();
                

            //Category Mapp
            CreateMap<Category, CategoryDto>().ReverseMap();


            //Suppliers Mapp
            CreateMap<Supplier, SupplierRDto>().ReverseMap();
            CreateMap<Supplier, CreateSupplierDto>().ReverseMap();
            CreateMap<Supplier, UpdateSupplierDto>().ReverseMap();


            //Collector Mapp
            CreateMap<Collector, CollectorDto>().ReverseMap();
            CreateMap<Collector, CreateCollectorDto>().ReverseMap();
            CreateMap<Collector, UpdateCollectorDto>().ReverseMap();


            //Representative Mapp
            CreateMap<Representative, RepresentativeDto>().ReverseMap();
            CreateMap<Representative, CreateRepresentativeDto>().ReverseMap();
            CreateMap<Representative, UpdateRepresentativeDto>().ReverseMap();
                

            //Representative Mapp
            CreateMap<Storekeeper, StorekeeperResponseDto>().ReverseMap();
            CreateMap<Storekeeper, CreateStorekeeperDto>().ReverseMap();
            CreateMap<Storekeeper, UpdateStorekeeperDto>().ReverseMap();
                

        }


    }
}
