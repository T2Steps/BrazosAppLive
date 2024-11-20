using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models
{
    public class JurisdictionAccounts
    {
        public JurisdictionAccounts()
        {
            IsActive = true;
        }

        [Key]
        public int Id { get; set; }
        [Required]
        public int ProgramId { get; set; }
        [ForeignKey("ProgramId")]
        public FeePrograms? Programs { get; set; }

        public string? Name { get; set; }

        public string? AccountCode { get; set; }

        public string? AccountDescription { get; set; }

        public bool IsActive { get; set; }
    }
}
