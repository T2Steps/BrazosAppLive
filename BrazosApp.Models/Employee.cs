using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models
{
      public class Employee
      {
            public int Id { get; set; }

            public string Name { get; set; }

            public DateTime JoiningDate { get; set; }

            public int Salary { get; set; }

            public bool IsActive { get; set; }

            public string? EncryptedId { get; set; }

            public Establishment Establishment { get; set; }
      }
}
