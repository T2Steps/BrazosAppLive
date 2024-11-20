using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models
{
    public class EstablishmentTypes
    {
        public EstablishmentTypes()
        {
            IsActive = true;
            SortOrder = 0;
        }

        [Key]
        public int Id { get; set; }
        [Required]
        public int JurisdictionId { get; set; }
        [ForeignKey("JurisdictionId")]
        public JurisdictionAccounts? Jurisdiction { get; set; }

        public string? Name { get; set; }

        [Precision(18, 2)]
        public decimal? Q1Fees { get; set; }
        [Precision(18, 2)]
        public decimal? Q2Fees { get; set; }
        [Precision(18, 2)]
        public decimal? Q3Fees { get; set; }
        [Precision(18, 2)]
        public decimal? Q4Fees { get; set; }

        public string? FeeCalculation { get; set; }
        public string? Message { get; set; }

        public bool IsPorated { get; set; }
        public bool IsActive { get; set; }

        public byte SortOrder { get; set; }
    }
}
