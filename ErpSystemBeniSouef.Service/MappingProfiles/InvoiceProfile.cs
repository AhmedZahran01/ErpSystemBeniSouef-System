using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input.CashInvoiceDto;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input.ReturnSupplier;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Output.CashInvoice;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Output.ReturnSupplierDtos;
using ErpSystemBeniSouef.Core.Entities;

namespace ErpSystemBeniSouef.Service.MappingProfiles
{
    public class InvoiceProfile : Profile
    {
        public InvoiceProfile()
        {
            CreateMap<AddCashInvoiceDto, Invoice>()
                .ForMember(dest => dest.SupplierId, opt => opt.MapFrom(src => src.SupplierId));

            CreateMap<Invoice, ReturnCashInvoiceDto>()
                .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Supplier.Name))
                .ForMember(dest => dest.SupplierId, opt => opt.MapFrom(src => src.Supplier.Id));
            
            CreateMap<InvoiceDetailsDto, Invoice>().ReverseMap();
            CreateMap<ReturnCashInvoiceDto, Invoice>().ReverseMap()
                 .ForMember( d => d.SupplierName ,  o => o.MapFrom(m=> m.Supplier.Name));

        
            
            //supplier return mapping
            
            CreateMap<AddReturnSupplierInvoiceDto, Invoice>()
               .ForMember(dest => dest.SupplierId, opt => opt.MapFrom(src => src.SupplierId));

            CreateMap<Invoice, DtoForReturnSupplierInvoice>()
                .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Supplier.Name));

            CreateMap<ReturnSupplierInvoiceDetailsDto, Invoice>().ReverseMap();
            CreateMap<DtoForReturnSupplierInvoice, Invoice>().ReverseMap()
                 .ForMember(d => d.SupplierName, o => o.MapFrom(m => m.Supplier.Name));
            CreateMap<UpdateInvoiceDto, Invoice>().ReverseMap();


        }
    }
}
