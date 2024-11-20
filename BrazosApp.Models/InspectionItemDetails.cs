using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models
{
    public class InspectionItemDetails
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int InspectionId { get; set; }
        [ForeignKey("InspectionId")]
        public Inspection? Inspection { get; set; }

        public int ItemId { get; set; }
        [ForeignKey("ItemId")]
        public Item? Items { get; set; }
        public string? Comment { get; set; }
        public string? Status { get; set; }
        public string? Cos { get; set; }
        public string? R { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
        public string? Image { get; set; }
    }
}
