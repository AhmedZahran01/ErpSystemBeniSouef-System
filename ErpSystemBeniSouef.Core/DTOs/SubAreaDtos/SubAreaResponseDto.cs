using ErpSystemBeniSouef.Core.Entities;
using ErpSystemBeniSouef.Dtos.MainAreaDto;
using System.ComponentModel.DataAnnotations.Schema;   

namespace ErpSystemBeniSouef.Core.DTOs.MainAreaDtos
{
    public class SubAreaDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string MainAreaName { get; set; } = string.Empty;

        [ForeignKey("mainRegions")]
        public int MainAreaId { get; set; }
        public virtual MainAreaDto mainRegions { get; set; } = new MainAreaDto();

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
