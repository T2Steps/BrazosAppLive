using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models.ViewModels
{
    public class ApplicationVM
    {
        public List<SelectListItem>? ApplicationForList { get; set; }

        public Application? Application { get; set; }
    }
}
