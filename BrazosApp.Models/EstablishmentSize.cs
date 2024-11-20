using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models
{
    public class EstablishmentSize
    {
        public EstablishmentSize()
        {
            IsActive = true;
        }
        public int Id { get; set; }

        public string? Name { get; set; }

        public bool IsActive { get; set; }
    }
}
