using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models
{
      public class WaterSource
      {
            public WaterSource()
            {
                  IsActive = true;
            }
            [Key]
            public int Id { get; set; }
            [StringLength(100)]
            public string? Name { get; set; }

            public bool IsActive { get; set; }
      }

    public class PublicSewage
    {
        public PublicSewage()
        {
            IsActive = true;
        }
        [Key]
        public int Id { get; set; }
        [StringLength(100)]
        public string? Name { get; set; }

        public bool IsActive { get; set; }
    }

    public class PrivateSeptic
    {
        public PrivateSeptic()
        {
            IsActive = true;
        }
        [Key]
        public int Id { get; set; }
        [StringLength(100)]
        public string? Name { get; set; }

        public bool IsActive { get; set; }
    }
}
