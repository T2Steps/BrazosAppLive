using BrazosApp.DataAccess.Repository.IRepository;
using BrazosApp.Models;
using BrazosApp.Utility.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace BrazosApp.Areas.LoggedIn.Controllers
{
    public class LoggedInOpeningInspectionController : Controller
    {
        private readonly IRepository<Item> _item;
        private readonly IRepository<Establishment> _establishments;
        private readonly IRepository<Inspection> _inspection;
        private readonly IRepository<InspectionItemDetails> _inspectionItems;
        private readonly IRepository<OpeningInspectionData> _openinginsData;
        private readonly IRepository<Schedule> _scheduler;
        private readonly IRepository<Users> _users;
        private readonly IRepository<AgencyStaffReqFields> _agencyreqFields;
        private readonly IRepository<AreaWiseInspectors> _areaWiseInspector;
        private readonly IRepository<Payment> _paymentRepo;
        private readonly IRepository<Fees> _fees;
        private readonly IEncrypt _encrypt;

        public LoggedInOpeningInspectionController(IRepository<Item> item,
                IRepository<Establishment> establishments,
                IRepository<Inspection> inspection,
                IRepository<InspectionItemDetails> inspectionItems,
                IRepository<OpeningInspectionData> openinginsData,
                IRepository<Schedule> scheduler,
                IRepository<Users> users,
                IRepository<AgencyStaffReqFields> agencyreqFields,
                IRepository<AreaWiseInspectors> areaWiseInspector,
                IRepository<Payment> paymentRepo,
                IRepository<Fees> fees,
                IEncrypt encrypt)
        {
            _item = item;
            _establishments = establishments;
            _inspectionItems = inspectionItems;
            _openinginsData = openinginsData;
            _inspection = inspection;
            _fees = fees;
            _scheduler = scheduler;
            _users = users;
            _agencyreqFields = agencyreqFields;
            _areaWiseInspector = areaWiseInspector;
            _paymentRepo = paymentRepo;
            _encrypt = encrypt;
        }

            [HttpGet("/Test")]
            public IActionResult Index()
            {
                  return View();
            }
    }
}
