using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models
{
    public class ApplicationType
    {
        public ApplicationType()
        {
            IsActive = true;
        }
        [Key]
        public int Id { get; set; }

        [StringLength(150)]
        public string? Name { get; set; }

        public bool IsActive { get; set; }


    }
}
