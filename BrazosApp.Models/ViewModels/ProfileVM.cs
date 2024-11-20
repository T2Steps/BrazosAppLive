using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models.ViewModels
{
    public class ProfileVM
    {
        public Users? User { get; set; }
        public ResetPasswordVM? ResetPassword { get; set; }
    }
}
