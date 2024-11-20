using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models
{
    public class FeePrograms
    {
        public FeePrograms()
        {
            IsActive = true;
        }

        [Key]
        public int Id { get; set; }

        public string? Name { get; set; }

        public bool IsActive { get; set; }
    }
}
