using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models
{
    public class InspectionItemCode
    {
        [Key]
        public byte Id { get; set; }

        public string? Name { get; set; }
    }
}
