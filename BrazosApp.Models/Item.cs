using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models
{
    public class Item
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? Points { get; set; }
        public byte CodeId { get; set; }
        [ForeignKey("CodeId")]
        public InspectionItemCode? Code { get; set; }
        public int? SectionId { get; set; }
        public int? SubSectionId { get; set; }
        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsNA { get; set; }
        public bool IsNO { get; set; }

        public byte? ItemNumber { get; set; }

        [NotMapped]
        public string? Status { get; set; }
        [NotMapped]
        public string? IsRepeat { get; set; }
        [NotMapped]
        public string? Score { get; set; }
        [NotMapped]
        public string? CDI { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
        [NotMapped]
        public string? Image { get; set; }
        [NotMapped]
        public string? Comments { get; set; }
    }
}
