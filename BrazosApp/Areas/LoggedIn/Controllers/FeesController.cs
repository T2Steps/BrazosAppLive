using BrazosApp.DataAccess.Repository.IRepository;
using BrazosApp.Models;
using BrazosApp.Utility.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace BrazosApp.Areas.LoggedIn.Controllers
{
    [Area("LoggedIn")]
    [Authorize("UserManagePolicy")]
    public class FeesController : Controller
    {
        private readonly IRepository<FeePrograms> _feeProgramsRepository;
        private readonly IRepository<JurisdictionAccounts> _jurisdictionRepository;
        private readonly IRepository<EstablishmentTypes> _establishmentTypesRepository;
        private readonly IEncrypt _encrypt;

        public FeesController(IRepository<FeePrograms> feeProgramsRepository, 
            IRepository<JurisdictionAccounts> jurisdictionRepository, 
            IRepository<EstablishmentTypes> establishmentTypesRepository, 
            IEncrypt encrypt)
        {
            _feeProgramsRepository = feeProgramsRepository;
            _jurisdictionRepository = jurisdictionRepository;
            _establishmentTypesRepository = establishmentTypesRepository;
            _encrypt = encrypt;
        }

        [Route("/Fees")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("/GetAllFees")]
        [HttpGet]
        public async Task<IActionResult> GetAllFees()
        {
            var establishmentTypes = await _establishmentTypesRepository.GetAllAsync(filter: x => x.IsActive == true, includeProperties: "Jurisdiction");

            var FeesList = establishmentTypes.Select(async t => new
            {
                Program = _feeProgramsRepository.GetById(t.Jurisdiction.ProgramId).Result.Name!.ToString(),
                Jurisdiction = t.Jurisdiction.Name,
                Description = t.Name,
                AccountCode = t.Jurisdiction.AccountCode,
                AccountDescription = t.Jurisdiction.AccountDescription,
                Fee = t.Q1Fees,
                Q2 = t.Q2Fees,
                Q3 = t.Q3Fees,
                Q4 = t.Q4Fees,
            }).ToList();

            return Json(new { data = FeesList.ToList() });
        }
    }
}
