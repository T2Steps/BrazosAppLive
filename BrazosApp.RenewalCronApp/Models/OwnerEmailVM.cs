using BrazosApp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.RenewalCronApp.Models
{
    public class OwnerEmailVM
    {
        public string? ToMail { get; set; }
        public string? CCMail { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }

        public string? htmlCode { get; set; }
    }
}
