using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models.ViewModels
{
    public class PermitCertificateVM
    {
        public Establishment? establishment { get; set; }

        public EstablishmentOwner? establishmentOwner { get; set; }

        public AgencyStaffReqFields? agencyStaffReqFields { get; set; }
    }
}
