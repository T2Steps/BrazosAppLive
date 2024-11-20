using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models
{
      public class BusinessType
      {
            public BusinessType()
            {
                  IsActive= true;
            }
            [Key]
            public int Id { get; set; }

            [Required]
            public string? Name { get; set; }
            [Required]
            public string? SpName { get; set; }
            [StringLength(6)]
            public string? Code { get; set; }

            public bool IsActive { get; set; }
      }
}
