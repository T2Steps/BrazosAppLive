using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models.ViewModels
{
    public class UserRegistrationVM
    {
        public Users? user { get; set; }
        public List<SelectListItem>? RoleList { get; set; }
    }
}
