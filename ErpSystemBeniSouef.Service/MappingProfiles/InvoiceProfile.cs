using AutoMapper;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input.CashInvoiceDto;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input.DueInvoiceDto;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input.ReturnSupplier;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input.SupplierCash;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Output.CashInvoice;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Output.DueInvoiceDtos;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Output.ReturnSupplierDtos;
using ErpSystemBeniSouef.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            
            CreateMap<CashInvoiceDetailsDto, Invoice>().ReverseMap();
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
            CreateMap<UpdateCashInvoiceDto, Invoice>().ReverseMap();
            
            //cash Region
            CreateMap<CashInvoiceItemDto, CashInvoiceItemDetailsDto>().ReverseMap();

            //Due Region

            CreateMap<DueInvoiceDetailsDto, Invoice>().ReverseMap()
                 .ForMember(d => d.SupplierName, o => o.MapFrom(m => m.Supplier.Name));

            CreateMap<AddDueInvoiceDto, Invoice>().ReverseMap();
            CreateMap<UpdateDueInvoiceDto, Invoice>().ReverseMap();

            CreateMap<DueInvoiceItemDto, DueInvoiceItemDetailsDto>().ReverseMap();

            //Return Supplier Accoint Mapping

            #region MyRegion


            CreateMap<ReturnSupplierInvoiceDetailsDto, Invoice>().ReverseMap();
            CreateMap<DtoForReturnSupplierInvoice, Invoice>().ReverseMap()
                 .ForMember(d => d.SupplierName, o => o.MapFrom(m => m.Supplier.Name));
            CreateMap<UpdateInvoiceDto, Invoice>().ReverseMap();


            CreateMap<InvoiceItem, ReturnSupplierInvoiceItemDetailsDto>()
    .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName))
    .ForMember(dest => dest.ProductType, opt => opt.MapFrom(src => src.ProductType))
    .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes));


            CreateMap<ReturnSupplierInvoiceItemDetailsDto, ReturnSupplierInvoiceItemDto>().ReverseMap();

            CreateMap<Invoice, ReturnSupplierInvoiceDetailsDto>()
    .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Supplier.Name))
    .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

            CreateMap<InvoiceItem, ReturnSupplierInvoiceItemDetailsDto>();

            CreateMap<SupplierAccount, UpdateSupplierCashDto>().ReverseMap();


            #endregion








        }
    }
}
