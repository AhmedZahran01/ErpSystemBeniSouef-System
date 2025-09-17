using AutoMapper;
using ErpSystemBeniSouef.Core.DTOs.MainAreaDtos;
using ErpSystemBeniSouef.Core.DTOs.ProductDtos;
using ErpSystemBeniSouef.Core.DTOs.ProductsDto;
using ErpSystemBeniSouef.Core.DTOs.SubAreaDtos;
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
                

        }


    }
}
