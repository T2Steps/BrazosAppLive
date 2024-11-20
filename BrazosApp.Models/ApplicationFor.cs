using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models
{
    public class ApplicationFor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ApplicationTypeId { get; set; }
        [ForeignKey("ApplicationTypeId")]
        public ApplicationType? ApplicationType { get; set; }

        [Required]
        public int LanguageTypeId { get; set; }
        [ForeignKey("LanguageTypeId")]
        public LanguageType? LanguageType { get; set; }
        
        [Required]
        public string? Name { get; set; }

        
        [StringLength(6)]
        public string? Code { get; set; }

        [StringLength(20)]
        public string? Purpose { get; set; }

        public bool IsActive { get; set; }
    }
}
