using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models
{
    public class Territory
    {
        public Territory()
        {
            CreatedOn = DateTime.Now;
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "ⓘ Required Field ")]
        [StringLength(100)]
        public string? Name { get; set; }

        [Required(ErrorMessage = "ⓘ Required Field ")]
        [StringLength(30)]
        public string? ColorCode { get; set; }

        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        
        public ICollection<TerritoryWiseInspectors>? Inspectors { get; set; }

        [NotMapped]
        public string? EncryptedId { get; set; }

        
    }
}
