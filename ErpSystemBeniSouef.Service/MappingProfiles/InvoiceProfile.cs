using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Input;
using ErpSystemBeniSouef.Core.DTOs.InvoiceDtos.Output;
using ErpSystemBeniSouef.Core.Entities;

namespace ErpSystemBeniSouef.Service.MappingProfiles
{
    public class InvoiceProfile : Profile
    {
        public InvoiceProfile()
        {
            CreateMap<AddInvoiceDto, Invoice>()
                .ForMember(dest => dest.SupplierId, opt => opt.MapFrom(src => src.SupplierId));

            CreateMap<Invoice, ReturnInvoiceDto>()
                .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Supplier.Name));
        }
    }
}
