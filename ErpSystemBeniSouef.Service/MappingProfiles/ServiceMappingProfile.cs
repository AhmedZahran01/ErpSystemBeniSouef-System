using AutoMapper;
using ErpSystemBeniSouef.Core.DTOs.MainAreaDtos;
using ErpSystemBeniSouef.Core.DTOs.SubAreaDtos;
using ErpSystemBeniSouef.Core.Entities;
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
            CreateMap<CreateSubAreaDto, SubArea>();
            CreateMap<UpdateSubAreaDto, SubArea>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

             




            // Map DTO to Entity (for Create/Update)
            CreateMap<CreateMainAreaDto, MainArea>();
            CreateMap<UpdateMainAreaDto, MainArea>()
             .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null)); // Ignore nulls on update

            // Map Entity to DTO (for Get)
            CreateMap<MainArea, MainAreaResponseDto>();
             
        }


    }
}
