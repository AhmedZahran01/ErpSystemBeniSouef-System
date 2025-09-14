using AutoMapper;
using ErpSystemBeniSouef.Core.DTOs.MainAreaDtos;
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
            CreateMap<UpdateSubAreaDto, SubArea>()
                .ForAllMembers(opts =>
                    opts.Condition((src, dest, srcMember) => srcMember != null));



            // MainArea
            // Map DTO to Entity (for Create/Update)
            CreateMap<CreateMainAreaDto, MainArea>().ReverseMap();
            CreateMap<UpdateMainAreaDto, MainArea>()
                .ForAllMembers(opts =>
                    opts.Condition((src, dest, srcMember) => srcMember != null));// Ignore nulls on update
            CreateMap<MainArea, MainAreaDto>().ReverseMap();

                

        }


    }
}
