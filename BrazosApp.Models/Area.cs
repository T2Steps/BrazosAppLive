using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models
{
      public class Area
      {
            [Key]
            public int Id { get; set; }

            public int AreaNumber { get; set; }

            public string? Description { get; set; }

            public bool IsActive { get; set; }

            public ICollection<AreaWiseInspectors>? Inspectors { get; set; }

            [NotMapped]
            public string? EncryptedId { get; set; }
      }
}
