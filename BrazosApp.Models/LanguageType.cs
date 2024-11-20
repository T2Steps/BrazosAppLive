using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models
{
    public class LanguageType
    {
        [Key]
        public int Id { get; set; }

        [StringLength(40)]
        public string? Name { get; set; }

        [StringLength(10)]
        public string? Code { get; set; }
        public bool IsActive { get; set; }
    }
}
