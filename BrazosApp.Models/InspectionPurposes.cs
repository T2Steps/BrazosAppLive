using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models
{
    public class InspectionPurposes
    {
        public InspectionPurposes()
        {
            IsActive = true;
        }
        [Key]
        public int Id { get; set; }

        [StringLength(100)]
        public string? Name { get; set; }

        [StringLength(7)]
        public string? Code { get; set; }

        public bool IsActive { get; set; }
    }
}
