using BrazosApp.DataAccess.Repository.IRepository;
using BrazosApp.Models;
using BrazosApp.Models.ViewModels;
using BrazosApp.Models.ViewModels.TemporaryPermitVMs;
using BrazosApp.Services;
using BrazosApp.Utility;
using BrazosApp.Utility.Helpers;
using BrazosApp.Utility.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using Wkhtmltopdf.NetCore;

namespace BrazosApp.Areas.LoggedIn.Controllers
{
    [Area("LoggedIn")]
    [Authorize("CommonPolicy")]
    public partial class PermitController : Controller
    {
        private readonly IRepository<ApplicationType> _applicationtype;
        private readonly IRepository<ApplicationFor> _applicationfor;
        private readonly IRepository<Application> _application;
        private readonly IRepository<Establishment> _establishment;
        private readonly IRepository<EstablishmentOwner> _owner;
        private readonly IRepository<RFOperationDetails> _rfoperationalDetails;
        private readonly IRepository<MFOperationDetails> _mfoperationalDetails;
        private readonly IRepository<MFVehicleInformation> _vehicleinformation;
        private readonly IRepository<MFOperationLocations> _mfoperationalLocations;
        private readonly IRepository<TFOperationDetails> _tfoperationalDetails;
        private readonly IRepository<BusinessType> _businesstype;
        private readonly IRepository<OperationType> _operationtype;
        private readonly IRepository<Document> _document;
        private readonly IRepository<Notes> _notes;
        private readonly IRepository<Users> _users;
        private readonly IRepository<EstablishmentSize> _size;
        private readonly IRepository<RiskCategory> _riskcategory;
        private readonly IRepository<Area> _area;
        private readonly IRepository<JurisdictionAccounts> _jurisdictionaccounts;
        private readonly IRepository<EstablishmentTypes> _estTypes;
        private readonly IRepository<AgencyStaffReqFields> _agencystaffreqfields;
        private readonly IRepository<Fees> _fees;
        private readonly IRepository<FeesDetailsTable> _feesDetailsTable;
        private readonly IRepository<Payment> _paymentRepo;
        private readonly IRepository<PaymentDetailsTable> _paymentDetailsTable;
        private readonly IRepository<PaymentSplit> _paymentSplit;
        private readonly IRepository<WaterSource> _waterSource;
        private readonly IRepository<PublicSewage> _publicSewage;
        private readonly IRepository<PrivateSeptic> _privateSeptic;
        private readonly IRepository<Schedule> _schedule;
        private readonly IRepository<PermitStatus> _permitStatus;
        private readonly IRepository<InspectionPurposes> _insPurposes;
        private readonly IRepository<AreaWiseInspectors> _areawiseIns;
        private readonly IRepository<InspectionItemDetails> _inspectionItems;
        private readonly IRepository<RFMFInspectionData> _rfmfInspectionData;
        private readonly IRepository<TemperatureObservation> _temperatureObs;
        private readonly IRepository<OpeningInspectionData> _openinginsData;
        private readonly IRepository<Inspection> _inspection;
        private readonly IRepository<FoodRenewalHistory> _foodRenewalHistory;
        private readonly IRepository<PermitReferenceCount> _permitReferenceCount;
        private readonly IWebHostEnvironment _environment;
        private readonly IEncrypt _encrypt;
        private readonly IGeneratePdf _generatePdf;
        private readonly string FoodContainer = @"\PermitDocuments\Food\";

        private readonly IEmailSenderService _emailSenderService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;
        private readonly IJetPayService _jetpayService;

        public PermitController(IRepository<ApplicationType> applicationtype,
            IRepository<ApplicationFor> applicationfor,
            IRepository<Application> application,
            IRepository<Establishment> establishment,
            IRepository<EstablishmentOwner> owner,
            IRepository<Document> document,
            IRepository<Notes> notes,
            IRepository<PermitStatus> permitStatus,
            IRepository<BusinessType> businesstype,
            IRepository<OperationType> operationtype,
            IRepository<RFOperationDetails> rfoperationalDetails,
            IRepository<MFOperationDetails> mfoperationalDetails,
            IRepository<TFOperationDetails> tfoperationalDetails,
            IRepository<MFVehicleInformation> vehicleinformation,
            IRepository<MFOperationLocations> mfoperationalLocations,
            IRepository<EstablishmentSize> size,
            IRepository<RiskCategory> riskcategory,
            IRepository<Area> area,
            IRepository<JurisdictionAccounts> jurisdictionaccounts,
            IRepository<EstablishmentTypes> estTypes,
            IRepository<AgencyStaffReqFields> agencystaffreqfields,
            IRepository<Fees> fees,
            IRepository<FeesDetailsTable> feesDetailsTable,
            IRepository<Payment> paymentRepo,
            IRepository<PaymentDetailsTable> paymentDetailsTable,
            IRepository<PaymentSplit> paymentSplit,
            IRepository<Schedule> schedule,
            IRepository<AreaWiseInspectors> areawiseIns,
            IRepository<InspectionPurposes> insPurposes,
            IRepository<WaterSource> waterSource,
            IRepository<PublicSewage> publicSewage,
            IRepository<PrivateSeptic> privateSeptic,
            IWebHostEnvironment environment,
            IEncrypt encrypt,
            IGeneratePdf generatePdf,
            IRepository<Users> users,
            IRepository<Inspection> inspection,
            IRepository<TemperatureObservation> temperatureObs,
            IRepository<InspectionItemDetails> inspectionItems,
            IRepository<RFMFInspectionData> rfmfInspectionData,
            IRepository<OpeningInspectionData> openinginsData,
            IRepository<FoodRenewalHistory> foodRenewalHistory,
            IRepository<PermitReferenceCount> permitReferenceCount,
            IEmailSenderService emailSenderService,
            IWebHostEnvironment webHostEnvironment,
            IConfiguration configuration,
            IJetPayService jetpayService)
        {
            _applicationtype = applicationtype;
            _applicationfor = applicationfor;
            _application = application;
            _businesstype = businesstype;
            _operationtype = operationtype;
            _environment = environment;
            _establishment = establishment;
            _owner = owner;
            _document = document;
            _notes = notes;
            _permitStatus = permitStatus;
            _rfoperationalDetails = rfoperationalDetails;
            _vehicleinformation = vehicleinformation;
            _mfoperationalDetails = mfoperationalDetails;
            _mfoperationalLocations = mfoperationalLocations;
            _tfoperationalDetails = tfoperationalDetails;
            _users = users;
            _size = size;
            _riskcategory = riskcategory;
            _area = area;
            _jurisdictionaccounts = jurisdictionaccounts;
            _agencystaffreqfields = agencystaffreqfields;
            _fees = fees;
            _feesDetailsTable = feesDetailsTable;
            _paymentRepo = paymentRepo;
            _paymentDetailsTable = paymentDetailsTable;
            _paymentSplit = paymentSplit;
            _schedule = schedule;
            _waterSource = waterSource;
            _publicSewage = publicSewage;
            _privateSeptic = privateSeptic;
            _areawiseIns = areawiseIns;
            _inspection = inspection;
            _rfmfInspectionData = rfmfInspectionData;
            _openinginsData = openinginsData;
            _inspectionItems = inspectionItems;
            _estTypes = estTypes;
            _encrypt = encrypt;
            _generatePdf = generatePdf;
            _insPurposes = insPurposes;
            _temperatureObs = temperatureObs;
            _foodRenewalHistory = foodRenewalHistory;
            _permitReferenceCount = permitReferenceCount;
            _emailSenderService = emailSenderService;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
            _jetpayService = jetpayService;
        }

        [Route("/Permits")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Route("/RetailPermits")]
        [HttpGet]
        public async Task<IActionResult> RetailPermits()
        {
            PermitSearchParamsVM model = new PermitSearchParamsVM();
            var Areas = await _area.GetAllAsync();
            var Risks = await _riskcategory.GetAllAsync();
            var PermitStatus = await _permitStatus.GetAllAsync(filter: x => x.Id > 1);
            var EstTypes = await _estTypes.GetAllAsync(filter:x=>x.Jurisdiction!.ProgramId==1, includeProperties: "Jurisdiction");
            model.AreaList = Areas.Select(x => new SelectListItem
            {
                Text = x.AreaNumber.ToString(),
                Value = x.AreaNumber.ToString()
            }).ToList();

            model.RiskList = Risks.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Name
            }).ToList();

            model.PermitStatList = PermitStatus.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Name
            }).ToList();
            
            model.EstTypeList = EstTypes.Select(x=> new SelectListItem
            {
                  Text = x.Name,
                  Value = x.Name
            }).ToList();

            return View(model);
        }

        [Route("/MobilePermits")]
        [HttpGet]
        public async Task<IActionResult> MobilePermits()
        {
            PermitSearchParamsVM model = new PermitSearchParamsVM();
            var Areas = await _area.GetAllAsync();
            var Risks = await _riskcategory.GetAllAsync();
            var PermitStatus = await _permitStatus.GetAllAsync(filter: x => x.Id > 1);
            var EstTypes = await _estTypes.GetAllAsync(filter: x => x.Jurisdiction!.ProgramId == 2, includeProperties: "Jurisdiction");
            model.AreaList = Areas.Select(x => new SelectListItem
            {
                Text = x.AreaNumber.ToString(),
                Value = x.AreaNumber.ToString()
            }).ToList();

            model.RiskList = Risks.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Name
            }).ToList();

            model.PermitStatList = PermitStatus.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Name
            }).ToList();

            model.EstTypeList = EstTypes.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Name
            }).ToList();

            return View(model);
        }

        [Route("/TemporaryPermits")]
        [HttpGet]
        public async Task<IActionResult> TempPermits()
        {
            TempPermitSearchParamsVM model = new TempPermitSearchParamsVM();
            var PermitStatus = await _permitStatus.GetAllAsync(filter: x => x.Name == SD.PermitStatName(2) || x.Name == SD.PermitStatName(3) || x.Name == SD.PermitStatName(7) || x.Name == SD.PermitStatName(9) || x.Name == SD.PermitStatName(13));
            model.PermitStatList = PermitStatus.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Name
            }).ToList();
            return View(model);
        }

        [Route("/GetAllPermits")]
        [HttpGet]
        public async Task<IActionResult> GetAllPermits()
        {
            //var PermitList = await _establishment.GetAllAsync(filter: x => x.IsDeleted != true, includeProperties: "PermitStatus,ApplicationFor", orderBy: x => x.OrderByDescending(x => x.CreatedOn));

            var PermitList = from p in await _establishment.GetAllAsync(filter: x => x.IsDeleted != true, includeProperties: "PermitStatus,ApplicationFor", orderBy: x => x.OrderByDescending(x => x.CreatedOn))
                             select new
                             {
                                 id = p.Id,
                                 permitNumber = p.PermitNumber,
                                 name = p.Name,
                                 area = p.Area,
                                 riskCategory = p.RiskCategory,
                                 permitStatus = p.PermitStatus!.Name,
                                 applicationFor = p.ApplicationFor!.Purpose,
                                 code = p.ApplicationFor!.Code,
                                 activationDate = p.ActivationDate,
                                 expiryDate = p.ExpiryDate,
                                 isActive = p.IsActive,
                                 encryptedId = _encrypt.Encrypt256(Convert.ToString(p.Id))
                             };

            return Json(new { data = PermitList.ToList() });
        }

        [Route("/GetAllRetailPermits")]
        [HttpPost]
        public async Task<IActionResult> GetAllRetailPermits(PermitSearchParamsVM model)
        {
            var PermitList = from p in await _establishment.GetAllAsync(filter: x => x.IsDeleted != true && x.ApplicationFor!.Code == "RF", includeProperties: "PermitStatus,ApplicationFor", orderBy: x => x.OrderByDescending(x=>x.CreatedOn))
                             join a in await _application.GetAllAsync()
                             on p.ApplicationId equals a.Id into egroup
                             join o in await _owner.GetAllAsync()
                             on p.Id equals o.EstablishmentId into ogroup
                             join u in await _users.GetAllAsync()
                             on p.CreatedBy equals u.Id into ugroup
                             join agency in await _agencystaffreqfields.GetAllAsync(includeProperties: "EstablishmentTypes")
                             on p.Id equals agency.EstablishmentId into agroup
                             from a in egroup.DefaultIfEmpty()
                             from o in ogroup.DefaultIfEmpty()
                             from u in ugroup.DefaultIfEmpty()
                             from agency in agroup.DefaultIfEmpty()
                             select new
                             {
                                 id = p.Id,
                                 permitNumber = p.PermitNumber,
                                 applicationNumber = a.ApplicationNo,
                                 applicationDate = p.CreatedOn,
                                 name = p.Name,
                                 owner = o.Name,
                                 address = p.Address,
                                 city = p.City,
                                 category = agency?.EstablishmentTypes!.Name,
                                 area = p.Area,
                                 riskCategory = p.RiskCategory,
                                 createdBy = u?.FirstName + " " + u?.LastName,
                                 permitStatus = p.PermitStatus!.Name,
                                 applicationFor = p.ApplicationFor!.Purpose,
                                 code = p.ApplicationFor!.Code,
                                 activationDate = p.ActivationDate,
                                 expiryDate = p.ExpiryDate,
                                 isActive = p.IsActive,
                                 nextinspectionDate = p.NextInspectionDate,
                                 lastinspectionDate = p.LastInspectionDate,
                                 encryptedId = _encrypt.Encrypt256(Convert.ToString(p.Id))
                             };

            //PermitList = PermitList.Where(x =>
            //        ((model.Name == null || model.Name == "") || x.name.Contains(model.Name)) ||
            //        ((model.Permit == null || model.Permit == "") || x.permitNumber.Contains(model.Permit)) ||
            //        ((model.ApplicationNo == null || model.ApplicationNo == "") || x.applicationNumber.Contains(model.ApplicationNo)) ||
            //        ((model.PermitStatus == null || model.PermitStatus == "--Select Permit Status--") || x.permitStatus.Contains(model.PermitStatus)) ||
            //        ((model.Purpose == null || model.Purpose == "") || x.applicationFor == model.Purpose) ||
            //        (model.Area == null || x.area == model.Area) ||
            //        ((model.Risk == null || model.Risk == "--Select Risk--") || x.riskCategory == model.Risk) ||
            //        (model.FromDate == null || x.applicationDate >= model.FromDate && x.activationDate >= model.FromDate) ||
            //        (model.ToDate == null || x.applicationDate <= model.ToDate && x.activationDate <= model.ToDate)
            //    );

            //if (model.Name != null)
            //{
            //    PermitList = PermitList.Where(x => x.name.Contains(model.Name));
            //}
            //if (model.Permit != null)
            //{
            //    PermitList = PermitList.Where(x => x.permitNumber != null && x.permitNumber.Contains(model.Permit));
            //}

            //if (model.FromDate != null)
            //{

            //    PermitList = PermitList.Where(x => x.applicationDate >= model.FromDate || x.activationDate >= model.FromDate);
            //}

            //if (model.ToDate != null)
            //{
            //    PermitList = PermitList.Where(x => x.applicationDate <= model.ToDate || x.activationDate <= model.ToDate);
            //}

            //if (model.ApplicationNo != null)
            //{
            //    PermitList = PermitList.Where(x => x.applicationNumber.Contains(model.ApplicationNo));
            //}
            //if (model.PermitStatus != null && model.PermitStatus != "--Select Permit Status--")
            //{
            //    PermitList = PermitList.Where(x => x.permitStatus.Contains(model.PermitStatus));
            //}
            //if (model.Purpose != null)
            //{
            //    PermitList = PermitList.Where(x => x.applicationFor == model.Purpose);
            //}
            //if (model.Area != null)
            //{
            //    PermitList = PermitList.Where(x => x.area == model.Area);
            //}
            //if (model.Risk != null && model.Risk != "--Select Risk--")
            //{
            //    PermitList = PermitList.Where(x => x.riskCategory != null && x.riskCategory == model.Risk);
            //}

            if (model.Name != null)
            {
                PermitList = PermitList.Where(x => x.name.ToLower().Contains(model.Name.ToLower()));
            }
            if (model.Permit != null)
            {
                PermitList = PermitList.Where(x => x.permitNumber != null && x.permitNumber.ToLower().Contains(model.Permit.ToLower()));
            }

            if (model.Owner != null)
            {
                PermitList = PermitList.Where(x => x.owner.ToLower().Contains(model.Owner.ToLower()));
            }

            if (model.Address != null)
            {
                PermitList = PermitList.Where(x => x.address != null && x.address.ToLower().Contains(model.Address.ToLower()));
            }

            if (model.City != null)
            {
                PermitList = PermitList.Where(x => x.city != null && x.city.ToLower().Contains(model.City.ToLower()));
            }

            if (model.EstType != null && model.EstType != "--Select Establishment Type--")
            {
                PermitList = PermitList.Where(x => x.category != null && x.category.ToLower().Contains(model.EstType.ToLower()));
            }

            if (model.SearchBy== "Application Date")
            {
                if (model.FromDate != null)
                {

                    PermitList = PermitList.Where(x => x.applicationDate.Date >= model.FromDate /*|| x.activationDate >= model.FromDate*/);
                }

                if (model.ToDate != null)
                {
                    PermitList = PermitList.Where(x => x.applicationDate.Date <= model.ToDate /*|| x.activationDate <= model.ToDate*/);
                }
            }

            if (model.SearchBy == "Next Inspection Date")
            {
                if (model.FromDate != null)
                {

                    PermitList = PermitList.Where(x => x.nextinspectionDate!=null && x.nextinspectionDate.Value.Date >= model.FromDate /*|| x.nextinspectionDate >= model.FromDate*/);
                }

                if (model.ToDate != null)
                {
                    PermitList = PermitList.Where(x => x.nextinspectionDate!=null && x.nextinspectionDate.Value.Date <= model.ToDate /*|| x.nextinspectionDate <= model.ToDate*/);
                }
            }

            if (model.ApplicationNo != null)
            {
                PermitList = PermitList.Where(x => x.applicationNumber.ToLower().Contains(model.ApplicationNo.ToLower()));
            }
            if (model.PermitStatus != null && model.PermitStatus != "--Select Permit Status--")
            {
                PermitList = PermitList.Where(x => x.permitStatus/*.Contains(model.PermitStatus)*/ == model.PermitStatus);
            }
            if (model.Purpose != null)
            {
                PermitList = PermitList.Where(x => x.applicationFor.ToLower().Contains(model.Purpose.ToLower()));
            }
            if (model.Area != null)
            {
                PermitList = PermitList.Where(x => x.area == model.Area);
            }
            if (model.Risk != null && model.Risk != "--Select Risk--")
            {
                PermitList = PermitList.Where(x => x.riskCategory != null && x.riskCategory.ToLower() == model.Risk.ToLower());
            }

            return Json(new { data = PermitList.OrderByDescending(x => x.applicationDate).ToList() });
        }

        [Route("/GetAllMobilePermits")]
        [HttpPost]
        public async Task<IActionResult> GetAllMobilePermits(PermitSearchParamsVM model)
        {
            //var PermitList = from p in await _establishment.GetAllAsync(filter: x => x.IsDeleted != true && x.ApplicationFor.Code == "MF", includeProperties: "PermitStatus,ApplicationFor", orderBy: x => x.OrderByDescending(x => x.CreatedOn))
            //                 join a in await _application.GetAllAsync()
            //                 on p.ApplicationId equals a.Id into egroup
            //                 join u in await _users.GetAllAsync()
            //                 on p.CreatedBy equals u.Id into ugroup
            //                 from a in egroup.DefaultIfEmpty()
            //                 from u in ugroup.DefaultIfEmpty()
            //                     //select p;
            //                 select new
            //                 {
            //                     id = p.Id,
            //                     permitNumber = p.PermitNumber,
            //                     applicationNumber = a.ApplicationNo,
            //                     applicationDate = p.CreatedOn,
            //                     name = p.Name,
            //                     area = p.Area,
            //                     riskCategory = p.RiskCategory,
            //                     createdBy = u?.FirstName + " " + u?.LastName,
            //                     permitStatus = p.PermitStatus!.Name,
            //                     applicationFor = p.ApplicationFor!.Purpose,
            //                     code = p.ApplicationFor!.Code,
            //                     activationDate = p.ActivationDate,
            //                     expiryDate = p.ExpiryDate,
            //                     isActive = p.IsActive,
            //                     encryptedId = _encrypt.Encrypt256(Convert.ToString(p.Id))
            //                 };

            var PermitList = from p in await _establishment.GetAllAsync(filter: x => x.IsDeleted != true && x.ApplicationFor!.Code == "MF", includeProperties: "PermitStatus,ApplicationFor", orderBy: x => x.OrderByDescending(x => x.CreatedOn))
                             join a in await _application.GetAllAsync()
                             on p.ApplicationId equals a.Id into egroup
                             join o in await _owner.GetAllAsync()
                             on p.Id equals o.EstablishmentId into ogroup
                             join u in await _users.GetAllAsync()
                             on p.CreatedBy equals u.Id into ugroup
                             join agency in await _agencystaffreqfields.GetAllAsync(includeProperties: "EstablishmentTypes")
                             on p.Id equals agency.EstablishmentId into agroup
                             from a in egroup.DefaultIfEmpty()
                             from o in ogroup.DefaultIfEmpty()
                             from u in ugroup.DefaultIfEmpty()
                             from agency in agroup.DefaultIfEmpty()
                             select new
                             {
                                 id = p.Id,
                                 permitNumber = p.PermitNumber,
                                 applicationNumber = a.ApplicationNo,
                                 applicationDate = p.CreatedOn,
                                 name = p.Name,
                                 owner = o.Name,
                                 address = p.Address,
                                 city = p.City,
                                 category = agency?.EstablishmentTypes!.Name,
                                 area = p.Area,
                                 riskCategory = p.RiskCategory,
                                 createdBy = u?.FirstName + " " + u?.LastName,
                                 permitStatus = p.PermitStatus!.Name,
                                 applicationFor = p.ApplicationFor!.Purpose,
                                 code = p.ApplicationFor!.Code,
                                 activationDate = p.ActivationDate,
                                 expiryDate = p.ExpiryDate,
                                 isActive = p.IsActive,
                                 nextinspectionDate = p.NextInspectionDate,
                                 lastinspectionDate = p.LastInspectionDate,
                                 encryptedId = _encrypt.Encrypt256(Convert.ToString(p.Id))
                             };

            if (model.Name != null)
            {
                PermitList = PermitList.Where(x => x.name.ToLower().Contains(model.Name.ToLower()));
            }
            if (model.Permit != null)
            {
                PermitList = PermitList.Where(x => x.permitNumber != null && x.permitNumber.ToLower().Contains(model.Permit.ToLower()));
            }
            //if (model.ApplicationNo != null)
            //{
            //    PermitList = PermitList.Where(x => x.applicationNumber.ToLower().Contains(model.ApplicationNo.ToLower()));
            //}

            //if (model.EstType != null && model.EstType != "--Select Establishment Type--")
            //{
            //    PermitList = PermitList.Where(x => x.category != null && x.category.ToLower().Contains(model.EstType.ToLower()));
            //}

            //if (model.FromDate != null)
            //{

            //    PermitList = PermitList.Where(x => x.applicationDate >= model.FromDate || x.activationDate >= model.FromDate);
            //}

            //if (model.ToDate != null)
            //{
            //    PermitList = PermitList.Where(x => x.applicationDate <= model.ToDate || x.activationDate <= model.ToDate);
            //}

            //if (model.ApplicationNo != null)
            //{
            //    PermitList = PermitList.Where(x => x.applicationNumber.ToLower().Contains(model.ApplicationNo.ToLower()));
            //}
            //if (model.PermitStatus != null && model.PermitStatus != "--Select Permit Status--")
            //{
            //    PermitList = PermitList.Where(x => x.permitStatus/*.Contains(model.PermitStatus)*/ == model.PermitStatus);
            //}
            //if (model.Purpose != null)
            //{
            //    PermitList = PermitList.Where(x => x.applicationFor.ToLower().Contains(model.Purpose.ToLower()));
            //}
            //if (model.Area != null)
            //{
            //    PermitList = PermitList.Where(x => x.area == model.Area);
            //}
            //if (model.Risk != null && model.Risk != "--Select Risk--")
            //{
            //    PermitList = PermitList.Where(x => x.riskCategory != null && x.riskCategory == model.Risk);
            //}

            if (model.Owner != null)
            {
                PermitList = PermitList.Where(x => x.owner.ToLower().Contains(model.Owner.ToLower()));
            }

            if (model.Address != null)
            {
                PermitList = PermitList.Where(x => x.address != null && x.address.ToLower().Contains(model.Address.ToLower()));
            }

            if (model.City != null)
            {
                PermitList = PermitList.Where(x => x.city != null && x.city.ToLower().Contains(model.City.ToLower()));
            }

            if (model.EstType != null && model.EstType != "--Select Establishment Type--")
            {
                PermitList = PermitList.Where(x => x.category != null && x.category.ToLower().Contains(model.EstType.ToLower()));
            }

            if (model.SearchBy == "Application Date")
            {
                if (model.FromDate != null)
                {

                    PermitList = PermitList.Where(x => x.applicationDate.Date >= model.FromDate /*|| x.activationDate >= model.FromDate*/);
                }

                if (model.ToDate != null)
                {
                    PermitList = PermitList.Where(x => x.applicationDate.Date <= model.ToDate /*|| x.activationDate <= model.ToDate*/);
                }
            }

            if (model.SearchBy == "Next Inspection Date")
            {
                if (model.FromDate != null)
                {

                    PermitList = PermitList.Where(x => x.nextinspectionDate != null && x.nextinspectionDate.Value.Date >= model.FromDate /*|| x.nextinspectionDate >= model.FromDate*/);
                }

                if (model.ToDate != null)
                {
                    PermitList = PermitList.Where(x => x.nextinspectionDate != null && x.nextinspectionDate.Value.Date <= model.ToDate /*|| x.nextinspectionDate <= model.ToDate*/);
                }
            }

            if (model.ApplicationNo != null)
            {
                PermitList = PermitList.Where(x => x.applicationNumber.ToLower().Contains(model.ApplicationNo.ToLower()));
            }
            if (model.PermitStatus != null && model.PermitStatus != "--Select Permit Status--")
            {
                PermitList = PermitList.Where(x => x.permitStatus/*.Contains(model.PermitStatus)*/ == model.PermitStatus);
            }
            if (model.Purpose != null)
            {
                PermitList = PermitList.Where(x => x.applicationFor.ToLower().Contains(model.Purpose.ToLower()));
            }
            if (model.Area != null)
            {
                PermitList = PermitList.Where(x => x.area == model.Area);
            }
            if (model.Risk != null && model.Risk != "--Select Risk--")
            {
                PermitList = PermitList.Where(x => x.riskCategory != null && x.riskCategory.ToLower() == model.Risk.ToLower());
            }

            return Json(new { data = PermitList.OrderByDescending(x=>x.applicationDate).ToList() });
        }

        [Route("/GetAllTempPermits")]
        [HttpPost]
        public async Task<IActionResult> GetAllTempPermits(TempPermitSearchParamsVM model)
        {
            var PermitList = from p in await _tfoperationalDetails.GetAllAsync(filter: x => x.Establishment!.IsDeleted != true /*&& x.ApplicationFor.Code == "TF"*/, includeProperties: "Event,Establishment", orderBy: x => x.OrderByDescending(x => x.CreatedOn))
                             //join permit in await _permitStatus.GetAllAsync()
                             //on p.Establishment!.PermitStatusId equals permit.Id into egroup
                             join a in await _application.GetAllAsync()
                             on p.Establishment!.ApplicationId equals a.Id into mgroup
                             join af in await _applicationfor.GetAllAsync(filter: x => x.Code == "TF")
                             on p.Establishment!.ApplicationForId equals af.Id into ggroup
                             //from permit in egroup.DefaultIfEmpty()
                             from a in mgroup.DefaultIfEmpty()
                             from af in ggroup.DefaultIfEmpty()
                             select new
                             {
                                 id = p.Id,
                                 permitNumber = p.Establishment!.PermitNumber,
                                 applicationNumber = a.ApplicationNo,
                                 applicationDate = p.Establishment.CreatedOn,
                                 events = p.Event!.Name,
                                 location = p.Event!.Location,
                                 name = p.Establishment!.Name,
                                 owner = _owner.GetFirstOrDefault(filter:x=>x.EstablishmentId==p.Establishment.Id).Name,
                                 address = p.Establishment.Address,
                                 city = p.Establishment.City,
                                 //applicationDate = p.Establishment.CreatedOn,
                                 //area = p.Establishment!.Area,
                                 //riskCategory = p.Establishment!.RiskCategory,
                                 //permitStatus = p.Establishment!.PermitStatus!.Name,
                                 permitStatus = _permitStatus.GetFirstOrDefault(filter:x=>x.Id==p.Establishment.PermitStatusId).Name,
                                 applicationFor = af.Purpose,
                                 code = af.Code,
                                 activationDate = p.Establishment!.ActivationDate,
                                 expiryDate = p.Establishment!.ExpiryDate,
                                 isActive = p.Establishment!.IsActive,
                                 encryptedId = _encrypt.Encrypt256(Convert.ToString(p.Id))
                             };

            if (model.Name != null)
            {
                PermitList = PermitList.Where(x => x.name.ToLower().Contains(model.Name.ToLower()));
            }
            if (model.Permit != null)
            {
                PermitList = PermitList.Where(x => x.permitNumber != null && x.permitNumber.ToLower().Contains(model.Permit.ToLower()));
            }

            if (model.Owner != null)
            {
                PermitList = PermitList.Where(x => x.owner.ToLower().Contains(model.Owner.ToLower()));
            }

            if (model.Address != null)
            {
                PermitList = PermitList.Where(x => x.address != null && x.address.ToLower().Contains(model.Address.ToLower()));
            }

            if (model.City != null)
            {
                PermitList = PermitList.Where(x => x.city != null && x.city.ToLower().Contains(model.City.ToLower()));
            }

            if (model.ApplicationNo != null)
            {
                PermitList = PermitList.Where(x => x.applicationNumber.ToLower().Contains(model.ApplicationNo.ToLower()));
            }
            if (model.PermitStatus != null && model.PermitStatus != "--Select Permit Status--")
            {
                PermitList = PermitList.Where(x => x.permitStatus == model.PermitStatus);
            }

            if (model.SearchBy == "Application Date")
            {
                if (model.FromDate != null)
                {

                    PermitList = PermitList.Where(x => x.applicationDate.Date >= model.FromDate /*|| x.activationDate >= model.FromDate*/);
                }

                if (model.ToDate != null)
                {
                    PermitList = PermitList.Where(x => x.applicationDate.Date <= model.ToDate /*|| x.activationDate <= model.ToDate*/);
                }
            }

            return Json(new { data = PermitList.ToList() });
        }

        [Route("/StepupPermit/{id?}")]
        [HttpPost]
        public async Task<IActionResult> ChangePermitStatus(string id)
        {
            //var processStatus = false;
            var renFlag = 0;
            string strBtnText = string.Empty;
            if (id == null)
            {
                return Json(new { success = false, msg = "Failed" });
            }
            var establishmentId = _encrypt.Decrypt256(id);
            var establishment = await _establishment.GetFirstOrDefaultAsync(filter: x => x.Id == Convert.ToInt32(establishmentId), includeProperties: "ApplicationFor");
            if (establishment.ApplicationFor!.Code == "TF")
            {
                if (establishment.PermitStatusId == 2)
                {
                    establishment.OldPermitStatusId = 2;
                    establishment.PermitStatusId = 3;
                    strBtnText = SD.PermitStatName(7);
                }
                else if (establishment.PermitStatusId == 3)
                {
                    establishment.OldPermitStatusId = 3;
                    establishment.PermitStatusId = 7;
                    strBtnText = SD.PermitStatName(9);
                }
                else if (establishment.PermitStatusId == 7)
                {
                    var Fees = await _fees.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == establishment.Id && x.Status == 2);
                    if (Fees != null)
                    {
                        establishment.OldPermitStatusId = 7;
                        establishment.PermitStatusId = 9;
                        strBtnText = SD.PermitStatName(10);
                    }
                    else
                    {
                        return Json(new { success = false, msg = "Cannot Place in Active!", info = "Please Pay the Permit Fees To Continue" });
                    }
                    
                }
            }
            else
            {
                if (establishment.PermitStatusId == 6)
                {
                    //var fees = await _fees.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == Convert.ToInt32(establishmentId) && x.Status == 1);
                    //var paymentList = await _paymentRepo.GetAllAsync();

                    //var random = new Random();

                    //Payment payment = new Payment();
                    //payment.FeesId = fees.Id;
                    ////payment.InvoiceNo = "IN-" + System.DateTime.Now.Year.ToString() + "-" + (paymentList.Count() + 1).ToString("D6");
                    //payment.InvoiceNo = "IN-" + System.DateTime.Now.Year.ToString() + "-" + random.Next(0, 999999).ToString("D6");
                    //payment.Amount = fees.Amount;
                    //payment.CreatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    //await _paymentRepo.AddAsync(payment);

                    //processStatus = await PaymentProcess(payment.Id);

                    establishment.OldPermitStatusId = establishment.PermitStatusId;
                    establishment.PermitStatusId += 1;
                    //await _establishment.UpdateAsync(establishment);

                    strBtnText = SD.PermitStatName(establishment.PermitStatusId + 1);
                    //if (processStatus == true)
                    //{

                    //}
                    //else
                    //{
                    //    strBtnText = SD.PermitStatName(establishment.PermitStatusId);
                    //}
                }
                else
                {
                    if (establishment.PermitStatusId == 8)
                    {
                        //var Fees = await _fees.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == establishment.Id && x.Status == 2);
                        
                        var payment = await _paymentRepo.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == Convert.ToInt32(_encrypt.Decrypt256(id)) && x.RefundVoidPaymentId == null && x.PaymentStatus == 2);

                        if (payment != null)
                        {
                            establishment.ActivationDate = DateTime.Now;
                            var year = DateTime.Now.Year;
                            establishment.OldPermitStatusId = 8;
                            if(DateTime.Now.Date >= new DateTime(year, 10, 15) && DateTime.Now.Date <= new DateTime(year, 12, 15))
                            {
                                renFlag = 1;
                                establishment.PermitStatusId = 10;                               
                                establishment.ExpiryDate = new DateTime(year, 12, 31);

                                if (establishment.ApplicationForId == 1 || establishment.ApplicationForId == 2)
                                {
                                    var agency = await _agencystaffreqfields.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == establishment.Id, includeProperties: "EstablishmentTypes,EstablishmentSize,RiskCategory");

                                    var estTypes = await _estTypes.GetAllAsync(filter: x => x.IsActive == true && (x.JurisdictionId >= 1 && x.JurisdictionId <= 3));

                                    var SelectedEst = new EstablishmentTypes();
                                    var selectedEstName = "";

                                    if (agency.EstablishmentTypes!.JurisdictionId == 1)
                                    {
                                        //agency.EstablishmentTypes!.IsPorated == true ? selectedEstName = "Food Service Permit Renewal" : selectedEstName = agency.EstablishmentTypes!.Name;
                                        selectedEstName = agency.EstablishmentTypes!.IsPorated == true ? "Food Service Permit Renewal" : agency.EstablishmentTypes!.Name;
                                        //SelectedEst = estTypes.Where(x => x.Name == "Food Service Permit Renewal" && x.JurisdictionId == 1).FirstOrDefault();
                                        SelectedEst = estTypes.Where(x => x.Name == selectedEstName && x.JurisdictionId == 1).FirstOrDefault();
                                    }
                                    else
                                    {
                                        var RiskType = "";
                                        var EstSizeType = "";

                                        selectedEstName = agency.EstablishmentTypes!.IsPorated == true ? "Food Service Permit Renewal" : agency.EstablishmentTypes!.Name;

                                        if(agency.EstablishmentTypes!.IsPorated == true)
                                        {
                                            RiskType = agency.RiskCategory!.Name == "Low" ? "1" : (agency.RiskCategory!.Name == "Medium" ? "2" : (agency.RiskCategory!.Name == "High" ? "3" : ""));
                                            EstSizeType = agency.EstablishmentSize!.Name == "Small" ? "A" : (agency.EstablishmentSize!.Name == "Medium" ? "B" : (agency.EstablishmentSize!.Name == "Large" ? "C" : ""));

                                            SelectedEst = estTypes.Where(x => (x.Name!.Contains("Renewal") && x.Name!.Contains(RiskType + EstSizeType)) && x.JurisdictionId == agency.EstablishmentTypes.JurisdictionId).FirstOrDefault();
                                        }
                                        else
                                        {
                                            SelectedEst = estTypes.Where(x => (x.Name == agency.EstablishmentTypes.Name) && x.JurisdictionId == agency.EstablishmentTypes.JurisdictionId).FirstOrDefault();
                                        }
                                    }
                                    Fees renewalFees = new Fees();
                                    renewalFees.EstablishmentId = establishment.Id;
                                    renewalFees.BaseAmount = SelectedEst!.Q1Fees ?? 0;
                                    renewalFees.Amount = SelectedEst!.Q1Fees ?? 0;
                                    renewalFees.InvoiceNo = GenerateInvoiceNumber().Result;
                                    renewalFees.Status = 1;
                                    renewalFees.CreatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                                    renewalFees.CreatedOn = DateTime.Now;
                                    renewalFees.FeesCalculation = 0;
                                    renewalFees.IsPermitFee = true;
                                    await _fees.AddAsync(renewalFees);

                                    FeesDetailsTable renewalFeesDetails = new FeesDetailsTable();
                                    renewalFeesDetails.FeesId = renewalFees.Id;
                                    renewalFeesDetails.EstablishmentTypeId = SelectedEst.Id;
                                    renewalFeesDetails.Amount = renewalFees.Amount;
                                    renewalFeesDetails.Title = SelectedEst.Name;
                                    renewalFeesDetails.Status = 1;
                                    renewalFeesDetails.CreatedOn = DateTime.Now;
                                    renewalFeesDetails.CreatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                                    await _feesDetailsTable.AddAsync(renewalFeesDetails);



                                    var renewalpayment = new Payment();
                                    renewalpayment.EstablishmentId = establishment.Id;
                                    renewalpayment.FeesId = renewalFees.Id;
                                    renewalpayment.InvoiceNo = renewalFees.InvoiceNo;
                                    renewalpayment.Amount = renewalFees.Amount;
                                    renewalpayment.InvoiceBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                                    renewalpayment.InvoiceDate = renewalFees.CreatedOn;
                                    renewalpayment.PaymentStatus = 1;
                                    renewalpayment.IsPermitFee = true;
                                    try
                                    {
                                        await _paymentRepo.AddAsync(renewalpayment);
                                    }
                                    catch (Exception ex)
                                    {

                                    }


                                    var feesDetails = await _feesDetailsTable.GetAllAsync(x => x.FeesId == renewalFees.Id);
                                    if (feesDetails.Any())
                                    {
                                        foreach (var feesDetailsItem in feesDetails)
                                        {
                                            var renewalpaymentDets = new PaymentDetailsTable();
                                            renewalpaymentDets.PaymentId = renewalpayment.Id;
                                            renewalpaymentDets.Amount = feesDetailsItem.Amount;
                                            renewalpaymentDets.Title = feesDetailsItem.Title;
                                            renewalpaymentDets.PaymentStatus = renewalpayment.PaymentStatus;
                                            await _paymentDetailsTable.AddAsync(renewalpaymentDets);
                                        }
                                    }
                                }
                            }
                            else if(DateTime.Now.Date > new DateTime(year, 12, 15))
                            {
                                establishment.PermitStatusId = 9;
                                establishment.ExpiryDate = new DateTime(DateTime.Now.AddYears(1).Year, 12, 31);
                            }
                            else
                            {
                                establishment.PermitStatusId = 9;
                                establishment.ExpiryDate = new DateTime(year, 12, 31);
                            }
                            
                            FoodRenewalHistory history = new FoodRenewalHistory();
                            history.EstablishmentId = establishment.Id;
                            history.ActivationDate = establishment.ActivationDate.Value;
                            history.ExpiryDate = establishment.ExpiryDate.Value;
                            history.IsActive = true;
                            history.IsCurrentYear = true;
                            await _foodRenewalHistory.AddAsync(history);

                            
                            

                            if (establishment.RiskCategory == "High")
                            {
                                establishment.NextInspectionDate = (establishment.ActivationDate ?? DateTime.Now.Date).AddDays(120);
                            }
                            else if (establishment.RiskCategory == "Medium")
                            {
                                establishment.NextInspectionDate = (establishment.ActivationDate ?? DateTime.Now.Date).AddDays(180);
                            }
                            else if (establishment.RiskCategory == "Low")
                            {
                                establishment.NextInspectionDate = (establishment.ActivationDate ?? DateTime.Now.Date).AddDays(360);
                            }
                            var area = await _area.GetFirstOrDefaultAsync(filter:x=>x.AreaNumber == establishment.Area);
                            var assignedTo = 2;
                            if (area != null)
                            {
                              var areawiseIns = await _areawiseIns.GetFirstOrDefaultAsync(filter:x=>x.AreaId==area.Id);
                              if (areawiseIns != null)
                              {
                                    assignedTo = areawiseIns.AssignedUserId;
                              }
                            }
                            Schedule schedule= new Schedule();
                            schedule.EstablishmentId = establishment.Id;
                            schedule.ScheduledDate = establishment.NextInspectionDate!.Value;
                            schedule.PurposeId = 3;
                            schedule.StatusId = 2;
                            schedule.AssignedTo= assignedTo;
                            schedule.CreatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                            schedule.CreatedOn = DateTime.Now;
                            schedule.IsAdhoc = false;
                            schedule.IsAutoSchedule = true;
                            schedule.SyncDate = DateTime.Now;
                            try 
                            {
                                 await _schedule.AddAsync(schedule);               
                            }
                            catch(Exception Ex)
                            {

                            }
                            
                        }
                        else
                        {
                            return Json(new { success = false, msg = "Cannot Place in Active!", info="Please Pay the Permit Fees To Continue" }); 
                        }
                        
                    }
                    if (establishment.PermitStatusId < 8)
                    {
                        establishment.OldPermitStatusId = establishment.PermitStatusId;
                        establishment.PermitStatusId += 1;
                        strBtnText = SD.PermitStatName(establishment.PermitStatusId + 1);
                    }
                    //await _establishment.UpdateAsync(establishment);

                    
                }
            }
            establishment.SyncDate = DateTime.Now;
            await _establishment.UpdateAsync(establishment);
            return Json(new { success = true, msg = "Successful", permitStatusId = establishment.PermitStatusId, oldPermitStatusId = establishment.OldPermitStatusId, btnText = strBtnText, renFlag = renFlag });
        }

        [HttpGet("/GetPaymentOwnerEmail/{id?}")]
        public async Task<IActionResult> GetPayEstOwner(string Id)
        {
            var payment = await _paymentRepo.GetFirstOrDefaultAsync(filter: x => x.Id == Convert.ToInt32(_encrypt.Decrypt256(Id)));
            if (payment == null)
            {
                return Json(new { success = false });
            }
            else
            {
                var Owner = await _owner.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == payment.EstablishmentId);
                return Json(new { success = true, ownerEmail = Owner.EmailId, paymentId = Id, paymentStatus = payment.PaymentStatus });
            }
            
        }

        [HttpPost("/SendPayDetailsViaEmail")]
        public async Task<IActionResult> SendPayDetailsViaEmail(string PaymentId, string toEmail, string ccEmail, string Subject, string Body)
        {
            var payment = await _paymentRepo.GetFirstOrDefaultAsync(filter:x=>x.Id==Convert.ToInt32(_encrypt.Decrypt256(PaymentId)), includeProperties:"Fees");
            var Est = await _establishment.GetFirstOrDefaultAsync(filter: x => x.Id == payment.EstablishmentId);
            var feesDetails = await _feesDetailsTable.GetAllAsync(filter:x=>x.FeesId==payment.FeesId);

            TimeZoneInfo centralZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");

            InvoicePdfVM invoiceModel = new InvoicePdfVM();
            invoiceModel.Payment = payment;
            invoiceModel.Payment.Fees = payment.Fees;
            invoiceModel.Establishment = Est;            
            invoiceModel.InvoiceDate = invoiceModel.Payment.InvoiceDate!.Value.ToShortDateString() + " " + TimeZoneInfo.ConvertTime(invoiceModel.Payment.InvoiceDate.Value, centralZone).ToShortTimeString();
            var user = await _users.GetFirstOrDefaultAsync(filter: x => x.Id == invoiceModel.Payment.InvoiceBy);
            invoiceModel.InvoiceCreatedBy = user.FirstName + " " + user.LastName;
            invoiceModel.FeesDetails = feesDetails;
            invoiceModel.PaymentDetails = await _paymentDetailsTable.GetAllAsync(filter: x => x.PaymentId == invoiceModel.Payment.Id);
            foreach (var item in invoiceModel.FeesDetails)
            {
                if (item.EstablishmentTypeId != null)
                {
                    var EstType = await _estTypes.GetFirstOrDefaultAsync(filter: x => x.Id == item.EstablishmentTypeId, includeProperties: "Jurisdiction");
                    var Jurisdiction = new JurisdictionAccounts();
                    if (EstType != null)
                    {
                        Jurisdiction = await _jurisdictionaccounts.GetFirstOrDefaultAsync(filter: x => x.Id == EstType.JurisdictionId, includeProperties: "Programs");
                    }
                    invoiceModel.Jurisdiction = Jurisdiction.Name;
                    invoiceModel.Program = Jurisdiction.Programs!.Name;
                }
            }
            var estOwner = await _owner.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == Est.Id);

            var generatedPdf = await _generatePdf.GetByteArray("Views/PdfTemplates/InvoicePdf.cshtml", invoiceModel);
            //Byte[][] bytearray = new Byte[2][];
            //string[] pdfName = new string[2];
            //Byte[][] bytearray = new Byte[][] { generatedPdf };
            //bytearray.Append(generatedPdf);
            //string[] pdfName = new string[] { "Invoice.Pdf" };
            //pdfName.Append("Invoice.Pdf");
            //await _emailSenderService.SendEmail(estOwner.EmailId!, "", "Payment Invoice", "Your Payment Invoice", bytearray, pdfName);

            
            List<byte[]> byteArrayList = new List<byte[]>();
            List<string> pdfNameList = new List<string>();

            
            byteArrayList.Add(generatedPdf);
            pdfNameList.Add("Invoice.Pdf");

            if(payment.PaymentStatus!=1 && payment.PaymentStatus!=3 && payment.PaymentStatus != 4)
            {
                InvoicePdfVM receiptModel = new InvoicePdfVM();
                receiptModel.Payment = payment;
                receiptModel.Fees = payment.Fees;
                receiptModel.Fees!.Establishment = await _establishment.GetFirstOrDefaultAsync(filter: x => x.Id == receiptModel.Payment.EstablishmentId);
                //TimeZoneInfo centralZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
                receiptModel.InvoiceDate = receiptModel.Payment.InvoiceDate!.Value.ToShortDateString() + " " + TimeZoneInfo.ConvertTime(receiptModel.Payment.InvoiceDate!.Value, centralZone).ToShortTimeString();
                //var user = await _users.GetFirstOrDefaultAsync(filter: x => x.Id == receiptModel.Payment.InvoiceBy);
                receiptModel.InvoiceCreatedBy = user.FirstName + " " + user.LastName;
                receiptModel.PaymentDate = receiptModel.Payment.PaymentOn ?? DateTime.Now;
                receiptModel.PaymentDate = TimeZoneInfo.ConvertTime(receiptModel.PaymentDate, centralZone);
                receiptModel.PaymentMethod = receiptModel.Payment.PaymentMethod == 0 ? "Credit Card" : (receiptModel.Payment.PaymentMethod == 2 ? "Cash" : (receiptModel.Payment.PaymentMethod == 3 ? "Check" : (receiptModel.Payment.PaymentMethod == 4 ? "Card" : "Money Order")));
                receiptModel.PaymentDetails = await _paymentDetailsTable.GetAllAsync(filter: x => x.PaymentId == receiptModel.Payment.Id);
                receiptModel.PaymentSplits = await _paymentSplit.GetAllAsync(filter: x => x.PaymentId == receiptModel.Payment.Id);
                foreach (var item in receiptModel.PaymentDetails)
                {
                    var EstType = await _estTypes.GetFirstOrDefaultAsync(filter: x => x.Name == item.Title, includeProperties: "Jurisdiction");
                    var Jurisdiction = new JurisdictionAccounts();
                    if (EstType != null)
                    {
                        Jurisdiction = await _jurisdictionaccounts.GetFirstOrDefaultAsync(filter: x => x.Id == EstType.JurisdictionId, includeProperties: "Programs");
                        receiptModel.Jurisdiction = Jurisdiction.Name;
                        receiptModel.Program = Jurisdiction.Programs!.Name;
                    }

                }
                //var estOwner = await _owner.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == receiptModel.Fees!.Establishment.Id);

                generatedPdf = await _generatePdf.GetByteArray("Views/PdfTemplates/ReceiptPdf.cshtml", receiptModel);
                //Byte[][] bytearray = new Byte[][] { generatedPdf };
                //string[] pdfName = new string[] { "Receipt.Pdf" };

                byteArrayList.Add(generatedPdf);
                pdfNameList.Add("Receipt.Pdf");
            }

            Byte[][] byteArray = byteArrayList.ToArray();
            string[] pdfName = pdfNameList.ToArray();
            try
            {
                await _emailSenderService.SendEmail(toEmail, ccEmail, Subject, Body, byteArray, pdfName);
            }
            catch (Exception ex)
            {

            }
            
            //return Ok();
            return Json(new { success = true, msg = "Mail sent successfully" });
        }


        [Route("/LoadEstTypes/{id?}")]
        [HttpGet]
        public async Task<IActionResult> GetEstTypes(int id)
        {
            var EstTypes = await _estTypes.GetAllAsync(filter: x => x.JurisdictionId == id);
            return Json(new { success = true, estTypes = EstTypes });
        }

        [Route("/GetFee/{id?}")]
        [HttpGet]
        public async Task<IActionResult> GetFee(int id)
        {
            var EstType = await _estTypes.GetFirstOrDefaultAsync(filter: x => x.Id == id);
            var strQuater = "";
            var amount = "";
            //float month = DateTime.Now.Month;
            float Quater = DateTime.Now.Month / 3;

            if (Quater <= 1)
            {
                strQuater = "1st";
                amount = EstType.Q1Fees.ToString();
            }
            else if (Quater <= 2)
            {
                strQuater = "2nd";
                amount = EstType.Q2Fees.ToString();
            }
            else if (Quater <= 3)
            {
                strQuater = "3rd";
                amount = EstType.Q3Fees.ToString();
            }
            else if (Quater <= 4)
            {
                strQuater = "4th";
                amount = EstType.Q4Fees.ToString();
            }
            return Json(new { success = true, quater = strQuater, amount = amount });
        }

        //[Route("/SaveAgencyFields")]
        //[HttpPost]
        //public async Task<IActionResult> SaveAgencyFields(PlanReviewVM planReviewVM)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var type = "";

        //        //float month = DateTime.Now.Month;

        //        float quarter = DateTime.Now.Month / 3;

        //        var establishmentTypes = await _estTypes.GetFirstOrDefaultAsync(filter: x => x.Id == planReviewVM.agencyStaffReqFields!.EstablishmentTypeId);
        //        var establishment = await _establishment.GetFirstOrDefaultAsync(filter: x => x.Id == planReviewVM.agencyStaffReqFields!.EstablishmentId, isTracking: false);
        //        var riskCategory = await _riskcategory.GetFirstOrDefaultAsync(filter: x => x.Id == planReviewVM.agencyStaffReqFields!.RiskCategoryId, isTracking: false);
        //        var territory = await _territory.GetFirstOrDefaultAsync(filter: x => x.Id == planReviewVM.agencyStaffReqFields!.TerritoryId, isTracking: false);

        //        decimal? amount = 0;
        //        amount = quarter <= 1 ? establishmentTypes.Q1Fees : quarter <= 2 ? establishmentTypes.Q2Fees : quarter <= 3 ? establishmentTypes.Q3Fees : quarter <= 4 ? establishmentTypes.Q4Fees : 0;

        //        //if (quarter <= 1)
        //        //{
        //        //    amt = establishmentTypes.Q1Fees;
        //        //}
        //        //else if (quarter <= 2)
        //        //{
        //        //    amt = establishmentTypes.Q2Fees;
        //        //}
        //        //else if (quarter <= 3)
        //        //{
        //        //    amt = establishmentTypes.Q3Fees;
        //        //}
        //        //else if (quarter <= 4)
        //        //{
        //        //    amt = establishmentTypes.Q4Fees;
        //        //}

        //        var fees = new Fees();
        //        if (planReviewVM.agencyStaffReqFields!.Id == 0)
        //        {
        //            planReviewVM.agencyStaffReqFields.CreatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
        //            await _agencystaffreqfields.AddAsync(planReviewVM.agencyStaffReqFields);

        //            fees.EstablishmentTypeId = establishmentTypes.Id;
        //            fees.EstablishmentId = planReviewVM.agencyStaffReqFields!.EstablishmentId;
        //            fees.CreatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
        //            fees.BaseAmount = amount!.Value;
        //            fees.FeesCalculation = 0;
        //            fees.Amount = amount!.Value;
        //            await _fees.AddAsync(fees);

        //            //type = "Create";
        //        }
        //        else
        //        {
        //            var agencyField = await _agencystaffreqfields.GetFirstOrDefaultAsync(filter: x => x.Id == planReviewVM.agencyStaffReqFields!.Id, isTracking: false);
        //            planReviewVM.agencyStaffReqFields.CreatedBy = agencyField.CreatedBy;
        //            planReviewVM.agencyStaffReqFields.CreatedOn = agencyField.CreatedOn;
        //            planReviewVM.agencyStaffReqFields.UpdatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
        //            planReviewVM.agencyStaffReqFields.UpdatedOn = DateTime.Now;
        //            planReviewVM.agencyStaffReqFields.IsActive = agencyField.IsActive;
        //            try
        //            {
        //                await _agencystaffreqfields.UpdateAsync(planReviewVM.agencyStaffReqFields);
        //            }
        //            catch (Exception ex) { }

        //            var feesFromDb = await _fees.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == planReviewVM.agencyStaffReqFields.EstablishmentId && x.Status == 1);
        //            if(feesFromDb == null)
        //            {
        //                fees.EstablishmentTypeId = establishmentTypes.Id;
        //                fees.EstablishmentId = planReviewVM.agencyStaffReqFields!.EstablishmentId;
        //                fees.CreatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
        //                fees.BaseAmount = amount!.Value;
        //                fees.FeesCalculation = 0;
        //                fees.Amount = amount!.Value;
        //                await _fees.AddAsync(fees);
        //            } 
        //            else
        //            {
        //                feesFromDb.UpdatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
        //                feesFromDb.UpdatedOn = DateTime.Now;
        //                feesFromDb.BaseAmount = amount!.Value;
        //                feesFromDb.FeesCalculation = 0;
        //                feesFromDb.Amount = amount!.Value;
        //                await _fees.UpdateAsync(feesFromDb);
        //            }                    

        //            //type = "Update";                    
        //        }
        //        var btntxt = "";
        //        var tok = "";
        //        if (planReviewVM.agencyStaffReqFields.IsPlanReview == false)
        //        {
        //            tok = "N";
        //            establishment.OldPermitStatusId = 3;
        //            establishment.PermitStatusId = 5;
        //            btntxt = "Place in Pending Build-Out";
        //        }
        //        else
        //        {
        //            establishment.OldPermitStatusId = 2;
        //            establishment.PermitStatusId = 3;
        //            btntxt = "Place in Pending Plan Review";
        //            tok = "Y";
        //        }

        //        establishment.Territory = territory.Name;
        //        establishment.RiskCategory = riskCategory.Name;
        //        await _establishment.UpdateAsync(establishment);

        //        var feesList = await _fees.GetAllAsync(filter: x => x.EstablishmentId == Convert.ToInt32(planReviewVM.agencyStaffReqFields.EstablishmentId) && x.Status == 1);
        //        var feesCount = feesList.Count();

        //        //return Json(new { success = true, type = type, id = planReviewVM.agencyStaffReqFields!.Id, permit = establishment.PermitStatusId, old = establishment.OldPermitStatusId, btntxt = btntxt, tok = tok, feesCount = feesCount });
        //        return Json(new { success = true, id = planReviewVM.agencyStaffReqFields!.Id, permitStatusId = establishment.PermitStatusId, oldPermitStatusId = establishment.OldPermitStatusId, btntxt = btntxt, tok = tok, feesCount = feesCount });
        //    }
        //    return Json(new { success = false });
        //}


        [HttpGet("/GetPermitStatusInfo/{id?}")]
        public async Task<IActionResult> GetPermitStatusInfo(string id)
        {
            var est = await _establishment.GetFirstOrDefaultAsync(filter:x=>x.Id==Convert.ToInt32(_encrypt.Decrypt256(id)));
            if(est == null)
            {
                return Json(new { success = false });
            }
            else
            {
                return Json(new { success = true, permitstat = est.PermitStatusId, oldstat = est.OldPermitStatusId });
            }
        }


        [Route("/SaveAgencyFields")]
        [HttpPost]
        public async Task<IActionResult> SaveAgencyFields(PlanReviewVM planReviewVM)
        {
            var applicationFor = string.Empty;
            var operationalDetailsId = "";
            //if (planReviewVM.agencyStaffReqFields.AreaId == 1)
            //{
            //    var modelKey = from modelstate in ModelState select new { key = modelstate.Key, validationState = modelstate.Value.ValidationState };

            //    foreach (var item in modelKey)
            //    {
            //        if (item.validationState.ToString().ToLower() == "invalid")
            //        {
            //            ModelState.Remove(item.key.ToString());
            //        }
            //    }
            //}
            if (ModelState.IsValid)
            {
                planReviewVM.agencyStaffReqFields!.EstablishmentTypes = null;

                float quarter = DateTime.Now.Month / 3;

                var establishmentTypes = await _estTypes.GetFirstOrDefaultAsync(filter: x => x.Id == planReviewVM.agencyStaffReqFields!.EstablishmentTypeId);
                var establishment = await _establishment.GetFirstOrDefaultAsync(filter: x => x.Id == planReviewVM.agencyStaffReqFields!.EstablishmentId, includeProperties: "ApplicationFor");
                applicationFor = establishment.ApplicationFor!.Code;

                var riskCategory = await _riskcategory.GetFirstOrDefaultAsync(filter: x => x.Id == planReviewVM.agencyStaffReqFields!.RiskCategoryId, isTracking: false);
                var area = await _area.GetFirstOrDefaultAsync(filter: x => x.Id == planReviewVM.agencyStaffReqFields!.AreaId, isTracking: false);



                if (applicationFor.ToLower() == "tf")
                {
                    var operationalDetails = await _tfoperationalDetails.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == establishment.Id);
                    operationalDetailsId = _encrypt.Encrypt256(operationalDetails.Id.ToString());
                }

                decimal? amount = 0;
                amount = quarter <= 1 ? establishmentTypes.Q1Fees : quarter <= 2 ? establishmentTypes.Q2Fees : quarter <= 3 ? establishmentTypes.Q3Fees : quarter <= 4 ? establishmentTypes.Q4Fees : 0;

                //var fees = new Fees();
                if (planReviewVM.agencyStaffReqFields!.Id == 0)
                {
                    //var agencyFieldFromDb = await _agencystaffreqfields.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == establishment.Id, isTracking: false);
                    //var agencyFieldFromDb = await _agencystaffreqfields.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == establishment.Id, isTracking: false);
                    var agencyFieldFromDb = _agencystaffreqfields.GetFirstOrDefault(filter: x => x.EstablishmentId == establishment.Id, isTracking: false);

                    if (agencyFieldFromDb == null)
                    {
                        planReviewVM.agencyStaffReqFields.CreatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                        await _agencystaffreqfields.AddAsync(planReviewVM.agencyStaffReqFields);

                        var code = "";
                        //var existingEst = await _establishment.GetAllAsync(filter: x => x.IsAgencyApproved == true);
                        if (establishment.ApplicationForId == 1 || establishment.ApplicationForId == 2)
                        {
                            code = "RF";
                        }
                        else if (establishment.ApplicationForId == 3 || establishment.ApplicationForId == 4)
                        {
                            code = "MF";
                        }
                        else if (establishment.ApplicationForId == 9 || establishment.ApplicationForId == 10)
                        {
                            code = "TF";
                        }
                        //var flag = 0;
                        //var PermitNumberCount = existingEst.Count() + 1;



                        //while (establishment.PermitNumber == null)
                        //{
                        //    var checkExistingEst = await _establishment.GetFirstOrDefaultAsync(filter: x => x.PermitNumber == code + "-" + (PermitNumberCount).ToString("D6"));
                        //    if (checkExistingEst == null)
                        //    {
                        //        establishment.PermitNumber = code + "-" + (PermitNumberCount).ToString("D6");
                        //    }
                        //    else
                        //    {
                        //        PermitNumberCount++;
                        //    }
                        //}
                        establishment.IsAgencyApproved = true;
                        var permitReference = await _permitReferenceCount.GetFirstOrDefaultAsync(filter: x => x.EstId == 0);
                        permitReference.EstId = establishment.Id;
                        await _permitReferenceCount.UpdateAsync(permitReference);

                        establishment.PermitNumber = code + "-" + (permitReference.ReferenceNo).ToString("D6");
                    }
                    //fees.EstablishmentTypeId = establishmentTypes.Id;
                    //fees.EstablishmentId = planReviewVM.agencyStaffReqFields!.EstablishmentId;                    
                    //fees.CreatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    //fees.BaseAmount = amount!.Value;
                    //fees.FeesCalculation = 0;
                    //fees.Amount = amount!.Value;
                    //await _fees.AddAsync(fees);                                        
                }
                else
                {
                    var agencyField = await _agencystaffreqfields.GetFirstOrDefaultAsync(filter: x => x.Id == planReviewVM.agencyStaffReqFields!.Id, isTracking: false);
                    planReviewVM.agencyStaffReqFields.CreatedBy = agencyField.CreatedBy;
                    planReviewVM.agencyStaffReqFields.CreatedOn = agencyField.CreatedOn;
                    planReviewVM.agencyStaffReqFields.UpdatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    planReviewVM.agencyStaffReqFields.UpdatedOn = DateTime.Now;
                    planReviewVM.agencyStaffReqFields.IsActive = agencyField.IsActive;
                    try
                    {
                        await _agencystaffreqfields.UpdateAsync(planReviewVM.agencyStaffReqFields);
                    }
                    catch (Exception ex) { }

                    //var feesFromDb = await _fees.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == planReviewVM.agencyStaffReqFields.EstablishmentId && x.Status == 1);
                    //if (feesFromDb == null)
                    //{
                    //    fees.EstablishmentTypeId = establishmentTypes.Id;
                    //    fees.EstablishmentId = planReviewVM.agencyStaffReqFields!.EstablishmentId;                        
                    //    fees.CreatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    //    fees.BaseAmount = amount!.Value;
                    //    fees.FeesCalculation = 0;
                    //    fees.Amount = amount!.Value;
                    //    await _fees.AddAsync(fees);
                    //}
                    //else
                    //{
                    //    feesFromDb.UpdatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    //    feesFromDb.UpdatedOn = DateTime.Now;
                    //    feesFromDb.BaseAmount = amount!.Value;
                    //    feesFromDb.FeesCalculation = 0;
                    //    feesFromDb.Amount = amount!.Value;
                    //    await _fees.UpdateAsync(feesFromDb);
                    //}
                }

                if (applicationFor.ToLower() != "tf")
                {
                    if (establishment.PermitStatusId < 7)
                    {
                        if (planReviewVM.agencyStaffReqFields.IsPlanReview == false)
                        {
                            establishment.OldPermitStatusId = 3;
                            establishment.PermitStatusId = 6;     // Pending Build-Out
                        }
                        else
                        {
                            establishment.OldPermitStatusId = 3;
                            establishment.PermitStatusId = 4;     // Pending Plan Review
                        }
                    }
                    
                    establishment.RiskCategory = riskCategory.Name;
                }


                establishment.Area = area.AreaNumber;
                
                try
                {
                    establishment.SyncDate = DateTime.Now;
                    await _establishment.UpdateAsync(establishment);
                }
                catch (Exception ex)
                {

                }

            }

            if (applicationFor.ToLower() == "rf")
            {
                return Redirect("RFEdit?id=" + _encrypt.Encrypt256(Convert.ToString(planReviewVM.agencyStaffReqFields!.EstablishmentId)));
            }
            else if (applicationFor.ToLower() == "mf")
            {
                return Redirect("MFEdit?id=" + _encrypt.Encrypt256(Convert.ToString(planReviewVM.agencyStaffReqFields!.EstablishmentId)));
            }
            else
            {
                return Redirect("TFEdit?id=" + operationalDetailsId);
            }
        }

        [Route("/PaymentProcess/{id?}")]
        [HttpPost]
        public async Task<IActionResult> PaymentProcess(string id)
        {
            var existingpayment = await _paymentRepo.GetFirstOrDefaultAsync(filter: x => x.FeesId == Convert.ToInt32(_encrypt.Decrypt256(id)) && x.PaymentStatus == 1);
            if (existingpayment == null)
            {
                var status = _jetpayService.PaymentProcess(id);
                return Json(new { success = status });
            }
            return Json(new { success = false });
            //var paymentId = _encrypt.Decrypt256(id);
            //var payment = await _paymentRepo.GetById(Convert.ToInt32(paymentId));
            //if (payment == null)
            //{
            //    return Json(new { success = false });
            //}

            //var fees = await _fees.GetFirstOrDefaultAsync(filter: x => x.Id == payment.FeesId);
            //var owner = await _owner.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == fees.EstablishmentId);

            ////create pending payment
            //RedirectRequestModel redirectRequest = new RedirectRequestModel();

            //redirectRequest.clientKey = _configuration["JetPayStage:APIClientKey"];
            //redirectRequest.transactionIdentifier = payment.InvoiceNo;
            //redirectRequest.collectionMode = Convert.ToInt32(_configuration["JetPayStage:CollectionMode"]);
            //redirectRequest.amount = payment.Amount;

            //redirectRequest.billing = new RedirectRequestModel.Address();
            //redirectRequest.billing.name = owner.Name;
            //redirectRequest.billing.address = owner.MailingAddress;
            //redirectRequest.billing.city = owner.City;
            //redirectRequest.billing.state = owner.State;
            //redirectRequest.billing.zip = owner.Zip;
            //redirectRequest.billing.email = owner.EmailId;
            //redirectRequest.billing.phone = owner.ContactNo;

            //redirectRequest.lineItems = new RedirectRequestModel.LineItem[] {
            //    new RedirectRequestModel.LineItem()
            //    {
            //        identifiers = new string[] { payment.InvoiceNo },
            //        amount = payment.Amount,
            //        paymentType = _configuration["JetPayStage:PaymentType"]
            //    }
            //};

            //redirectRequest.csiUserId = Convert.ToInt32(_configuration["JetPayStage:csiUserId"]);
            //redirectRequest.notes = "NCR Test";
            //redirectRequest.urlReturnPost = _configuration.GetValue<string>("BaseUrl") + "/PaymentStatus?Id=" + _encrypt.Encrypt256(payment.Id.ToString());
            //redirectRequest.urlSilentPost = "";

            //// The payment method that will be enabled for the transaction. 0 = Block(eCheck and Credit Card), 1 = Accept(eChecks Only), 2 = Accept(Credit Cards Only), 3 = Accept(eCheck and Credit Card) 
            //redirectRequest.allowedPaymentMethod = 2;

            ////Serialize the request model into a Json string for the web request
            //string jsonInput = JsonConvert.SerializeObject(redirectRequest);

            ////Setup the WebRequest with the API process method to be used
            //WebRequest request = WebRequest.Create("https://stage.collectorsolutions.com/magic-api/api/transaction/redirect");

            ////Set timeout, content type and http method type
            //request.Timeout = 120000;
            //request.ContentType = "application/json";
            //request.Method = "POST";

            ////Setup a stream writer to push the information to the server
            //using (StreamWriter requestStream = new StreamWriter(request.GetRequestStream()))
            //{
            //    //Write the data
            //    requestStream.Write(jsonInput);
            //}

            ////Create the string for the returning json string object
            //string jsonOutput = string.Empty;

            ////Setup the WebResponse object to accept the returning information
            //using (WebResponse response = request.GetResponse())
            //{
            //    //Setup a StreamReader object to read the inbound information
            //    StreamReader responseStream = new StreamReader(response.GetResponseStream());

            //    // Read it to a string
            //    jsonOutput = responseStream.ReadToEnd();
            //}

            ////Deserialize it to a response model
            //RedirectResponseModel redirectResponse = JsonConvert.DeserializeObject<RedirectResponseModel>(jsonOutput);

            //if (redirectResponse == null)
            //{
            //    return Json(new { success = false });
            //}
            //else
            //{
            //    payment.RedirectApiCallStatus = redirectResponse.status;
            //    payment.RedirectApiCallOn = System.DateTime.Now;

            //    if (redirectResponse.status.ToLower() == "error")
            //    {
            //        payment.RedirectApiMessage = redirectResponse.errors[0].message.ToString();
            //    }
            //    else
            //    {
            //        string emailContent = new EmailFilePathGenerate(_webHostEnvironment).EmailFilePath(@"EmailTemplates\EmailPayment.html");
            //        emailContent = emailContent.Replace("{{OwnerName}}", owner.Name);
            //        emailContent = emailContent.Replace("{{PaymentRedirectUrl}}", "http://stage.collectorsolutions.com/magic-ui/PaymentRedirect/" + _configuration["JetPayStage:WebClientKey"] + "/" + payment.InvoiceNo);
            //        emailContent = emailContent.Replace("{{ServerUrl}}", _configuration.GetValue<string>("ServerUrl"));
            //        await _emailSenderService.SendEmail(owner.EmailId!, null, "Payment", emailContent, null, null);
            //    }
            //}

            //await _paymentRepo.UpdateAsync(payment);

            //return Json(new { success = true });
        }

        [HttpGet("/GeneratePermitCertificatePdf/{id?}")]
        public async Task<IActionResult> GeneratePermitCertificatePdf(string? id)
        {
            var establishmentId = _encrypt.Decrypt256(id);

            PermitCertificateVM permitCertificateVM = new PermitCertificateVM();
            permitCertificateVM.establishment = await _establishment.GetById(Convert.ToInt32(establishmentId));
            //permitCertificateVM.establishmentOwner = await _owner.GetById(Convert.ToInt32(establishmentId));
            permitCertificateVM.establishmentOwner = await _owner.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == Convert.ToInt32(establishmentId));
            permitCertificateVM.agencyStaffReqFields = await _agencystaffreqfields.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == Convert.ToInt32(establishmentId), includeProperties: "EstablishmentTypes");
            
            return await _generatePdf.GetPdf("Views/PdfTemplates/PermitCertificate.cshtml", permitCertificateVM);
        }
            
        [HttpGet("/DownloadPermitCertificatePdf/{id?}")]
        public async Task<IActionResult> DownloadPermitCertificatePdf(string id)
        {
            var establishmentId = _encrypt.Decrypt256(id);

            PermitCertificateVM permitCertificateVM = new PermitCertificateVM();
            permitCertificateVM.establishment = await _establishment.GetById(Convert.ToInt32(establishmentId));
            //permitCertificateVM.establishmentOwner = await _owner.GetById(Convert.ToInt32(establishmentId));
            permitCertificateVM.establishmentOwner = await _owner.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == Convert.ToInt32(establishmentId));
            permitCertificateVM.agencyStaffReqFields = await _agencystaffreqfields.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == Convert.ToInt32(establishmentId), includeProperties: "EstablishmentTypes");
            var generatedPdf = await _generatePdf.GetByteArray("Views/PdfTemplates/PermitCertificate.cshtml", permitCertificateVM);
            Response.ContentType = "application/pdf";
            Response.Headers.Add("Content-Disposition", "attachment; filename=PermitCertificate.pdf");
            return new FileContentResult(generatedPdf, "application/pdf");
            //return await _generatePdf.GetPdf("Views/PdfTemplates/PermitCertificate.cshtml", permitCertificateVM);
        }


        [HttpPost("/SaveFees")]
        public async Task<IActionResult> SaveFees(AddPaymentDTO model)
        {
            decimal amt = 0;
            var existingPayment = await _paymentRepo.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId== model.EstId && x.IsPermitFee == true && x.RefundVoidPaymentId == null && x.PaymentStatus==2);
            var Fees = new Fees();            
            Fees.EstablishmentId = model.EstId;
            Fees.InvoiceNo = model.InvoiceNo;
            Fees.Status = 1;
            Fees.CreatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            Fees.FeesCalculation = 0;
            //var ExistingFee = await _fees.GetFirstOrDefaultAsync(filter: x => x.IsPermitFee == true);
            if (existingPayment == null)
            {
                Fees.IsPermitFee = true;
            }
            else
            {
                Fees.IsPermitFee = false;
            }
            //var DuplicateFees = await _fees.Get
            await _fees.AddAsync(Fees);            

            if (model.FeesList!.Any())
            {
                foreach (var feesdet in model.FeesList!)
                {
                    if (feesdet.IsSelected == true)
                    {
                        amt = amt + feesdet.Amount;
                        feesdet.FeesId = Fees.Id;
                        feesdet.Status = 1;
                        feesdet.CreatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                        await _feesDetailsTable.AddAsync(feesdet);
                    }
                }
            }
            if (model.LateFine != 0)
            {
                FeesDetailsTable feesDetailsTable = new FeesDetailsTable();
                feesDetailsTable.FeesId = Fees.Id;
                feesDetailsTable.Status = 1;
                feesDetailsTable.Title = "Late Fine";
                feesDetailsTable.Amount = (decimal)model.LateFine!;
                feesDetailsTable.CreatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                amt += (decimal)model.LateFine!;
                await _feesDetailsTable.AddAsync(feesDetailsTable);
            }
            if (model.MiscelliniusFees != 0)
            {
                FeesDetailsTable feesDetailsTable = new FeesDetailsTable();
                feesDetailsTable.FeesId = Fees.Id;
                feesDetailsTable.Status = 1;
                feesDetailsTable.Title = model.MiscelliniusFeesTitle;
                feesDetailsTable.Amount = (decimal)model.MiscelliniusFees!;
                feesDetailsTable.CreatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                amt += (decimal)model.MiscelliniusFees!;
                await _feesDetailsTable.AddAsync(feesDetailsTable);
            }
            Fees.BaseAmount = amt;
            Fees.Amount = amt;
            await _fees.UpdateAsync(Fees);

            var payment = new Payment();
            payment.EstablishmentId = model.EstId;
            payment.FeesId = Fees.Id;
            payment.InvoiceNo = Fees.InvoiceNo;
            payment.Amount = Fees.Amount;
            payment.InvoiceBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            payment.InvoiceDate = Fees.CreatedOn;
            payment.PaymentStatus = 1;
            //var existingPayment = await _paymentRepo.GetFirstOrDefaultAsync(filter: x => x.IsPermitFee == true && x.RefundVoidPaymentId == null);
            if (existingPayment == null)
            {
                payment.IsPermitFee = true;
            }
            else
            {
                payment.IsPermitFee = false;
            }
            await _paymentRepo.AddAsync(payment);

            var feesDetails = await _feesDetailsTable.GetAllAsync(x => x.FeesId == Fees.Id);
            if (feesDetails.Any())
            {
                foreach (var feesDetailsItem in feesDetails)
                {
                    var paymentDets = new PaymentDetailsTable();
                    paymentDets.PaymentId = payment.Id;
                    paymentDets.Amount = feesDetailsItem.Amount;
                    paymentDets.Title = feesDetailsItem.Title;
                    paymentDets.PaymentStatus = payment.PaymentStatus;
                    await _paymentDetailsTable.AddAsync(paymentDets);
                }
            }

            var msg = "";
            var Est = await _establishment.GetFirstOrDefaultAsync(filter: x => x.Id == model.EstId);
            if (Est.PermitStatusId >= 9)
            {
                msg = "Multiple";
            }
            else
            {
                msg = "Single";
            }

            //Invoice Email
            InvoicePdfVM invoiceModel = new InvoicePdfVM();
            invoiceModel.Payment = payment;
            invoiceModel.Payment.Fees = Fees;
            invoiceModel.Establishment = Est;
            TimeZoneInfo centralZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
            invoiceModel.InvoiceDate = invoiceModel.Payment.InvoiceDate!.Value.ToShortDateString() + " " + TimeZoneInfo.ConvertTime(invoiceModel.Payment.InvoiceDate.Value, centralZone).ToShortTimeString();
            var user = await _users.GetFirstOrDefaultAsync(filter: x => x.Id == invoiceModel.Payment.InvoiceBy);
            invoiceModel.InvoiceCreatedBy = user.FirstName + " " + user.LastName;
            invoiceModel.FeesDetails = feesDetails;
            invoiceModel.PaymentDetails = await _paymentDetailsTable.GetAllAsync(filter: x => x.PaymentId == invoiceModel.Payment.Id);
            foreach (var item in invoiceModel.FeesDetails)
            {
                if (item.EstablishmentTypeId != null)
                {
                    var EstType = await _estTypes.GetFirstOrDefaultAsync(filter: x => x.Id == item.EstablishmentTypeId, includeProperties: "Jurisdiction");
                    var Jurisdiction = new JurisdictionAccounts();
                    if (EstType != null)
                    {
                        Jurisdiction = await _jurisdictionaccounts.GetFirstOrDefaultAsync(filter: x => x.Id == EstType.JurisdictionId, includeProperties: "Programs");
                    }
                    invoiceModel.Jurisdiction = Jurisdiction.Name;
                    invoiceModel.Program = Jurisdiction.Programs!.Name;
                }
            }
            var estOwner = await _owner.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == Est.Id);

            var generatedPdf = await _generatePdf.GetByteArray("Views/PdfTemplates/InvoicePdf.cshtml", invoiceModel);
            Byte[][] bytearray = new Byte[][] { generatedPdf };
            string[] pdfName = new string[] { "Invoice.Pdf" };
            await _emailSenderService.SendEmail(estOwner.EmailId!, "", "Payment Invoice", "Your Payment Invoice", bytearray, pdfName);
            //return Json(new { success = true, msg = "Mail sent successfully" });
            //return await _generatePdf.GetPdf("Views/PdfTemplates/InvoicePdf.cshtml", model);

            var btn = "";

            if (Est.PermitStatusId == 6)
            {
                Est.OldPermitStatusId = 6;
                Est.PermitStatusId = 7;
                Est.SyncDate = DateTime.Now;
                await _establishment.UpdateAsync(Est);
                btn = "Opening Inspection";
            }

            return Json(new { success = true, msg = msg, permitstatusid = Est.PermitStatusId, oldpermitstatus = Est.OldPermitStatusId, btn = btn });
        }

        [HttpPost("/CancelPayment")]
        public async Task<IActionResult> CancelPayment(string id)
        {
            var paymentId = Convert.ToInt32(_encrypt.Decrypt256(id));
            var payment = await _paymentRepo.GetFirstOrDefaultAsync(filter: x => x.Id == paymentId, includeProperties: "Fees");
            payment.PaymentStatus = 3;
            payment.IsPermitFee = false;
            await _paymentRepo.UpdateAsync(payment);

            var paymentDetails = await _paymentDetailsTable.GetAllAsync(filter: x => x.PaymentId == paymentId);
            foreach (var paymentItem in paymentDetails)
            {
                paymentItem.PaymentStatus = 3;
                paymentItem.RefundVoidBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                paymentItem.RefundVoidDate = DateTime.Now;
                await _paymentDetailsTable.UpdateAsync(paymentItem);
            }

            payment.Fees!.Status = 3;
            payment.Fees!.UpdatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            payment.Fees!.UpdatedOn = DateTime.Now;
            payment.Fees!.IsPermitFee = false;
            await _fees.UpdateAsync(payment.Fees!);

            var feesDetails = await _feesDetailsTable.GetAllAsync(filter: x => x.FeesId == payment.FeesId);
            foreach (var feesDetail in feesDetails)
            {
                feesDetail.Status = 3;
                feesDetail.UpdatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                feesDetail.UpdatedOn = DateTime.Now;
                await _feesDetailsTable.UpdateAsync(feesDetail);
            }
            return Json(new { success = true });

            //var feeId = Convert.ToInt32(_encrypt.Decrypt256(id));
            //var Fee = await _fees.GetFirstOrDefaultAsync(filter: x => x.Id == feeId);
            //Fee.Status = 3;
            //Fee.UpdatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            //Fee.UpdatedOn = DateTime.Now;
            //await _fees.UpdateAsync(Fee);

            //var FeesDets = await _feesDetailsTable.GetAllAsync(filter: x => x.FeesId == Fee.Id);
            //foreach (var feeDets in FeesDets)
            //{
            //    feeDets.Status = 3;
            //    feeDets.UpdatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            //    feeDets.UpdatedOn = DateTime.Now;
            //    await _feesDetailsTable.UpdateAsync(feeDets);
            //}

            //return Json(new { success = true });
        }

        [HttpPost("/VoidPayment")]
        public async Task<IActionResult> VoidPayment(string id, string? value)
        {
            //var feeId = Convert.ToInt32(_encrypt.Decrypt256(id));
            //var Fee = await _fees.GetFirstOrDefaultAsync(filter: x => x.Id == feeId);
            //Fee.Status = 9;
            //Fee.UpdatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            //Fee.UpdatedOn = DateTime.Now;
            //Fee.ReasonForVoiding = value;
            //Fee.VoidedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            //Fee.VoidedDate = DateTime.Now;
            //Fee.VoidedTransactionNo = "V-" + Fee.InvoiceNo;
            //await _fees.UpdateAsync(Fee);

            //var FeesDets = await _feesDetailsTable.GetAllAsync(filter: x => x.FeesId == Fee.Id);
            //foreach (var feeDets in FeesDets)
            //{
            //    feeDets.Status = 9;
            //    feeDets.UpdatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            //    feeDets.UpdatedOn = DateTime.Now;
            //    await _feesDetailsTable.UpdateAsync(feeDets);
            //}

            //return Json(new { success = true });

            var paymentId = Convert.ToInt32(_encrypt.Decrypt256(id));
            var payment = await _paymentRepo.GetFirstOrDefaultAsync(filter: x => x.Id == paymentId, includeProperties:"Fees");
            payment.PaymentStatus = 6;
            payment.ReasonForRefundVoid = value;
            payment.RefundVoidBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            payment.RefundVoidDate = DateTime.Now;
            payment.VoidedTransactionNo = "V-" + payment.InvoiceNo;
            payment.IsPermitFee = false;
            await _paymentRepo.UpdateAsync(payment);

            var paymentDetails = await _paymentDetailsTable.GetAllAsync(filter: x => x.PaymentId == paymentId);
            foreach (var paymentItem in paymentDetails)
            {
                paymentItem.PaymentStatus = 6;
                paymentItem.RefundVoidBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                paymentItem.RefundVoidDate = DateTime.Now;
                await _paymentDetailsTable.UpdateAsync(paymentItem);
            }

            payment.Fees!.Status = 9;
            payment.Fees!.UpdatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            payment.Fees!.UpdatedOn = DateTime.Now;
            payment.Fees!.IsPermitFee = false;
            await _fees.UpdateAsync(payment.Fees!);

            var feesDetails = await _feesDetailsTable.GetAllAsync(filter:x=>x.FeesId==payment.FeesId);
            foreach(var feesDetail in feesDetails)
            {
                feesDetail.Status = 9;
                feesDetail.UpdatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                feesDetail.UpdatedOn = DateTime.Now;
                await _feesDetailsTable.UpdateAsync(feesDetail);
            }
            return Json(new { success = true });
        }

        [HttpGet("/GetFeesDetails/{id?}")]
        public async Task<IActionResult> GetFeesDetails(string? id)
        {
            if (id == null)
            {
                return Json(new { success = false });
            }
            var payment = await _paymentRepo.GetFirstOrDefaultAsync(filter: x => x.Id == Convert.ToInt32(_encrypt.Decrypt256(id)));
            if (payment == null)
            {
                return Json(new { success = false });
            }
            return Json(new { success = true, payment = payment });
        }

        [HttpPost("/OfflinePayment")]
        public async Task<IActionResult> OfflinePayment(OfflinePaymentVM model)
        {
            //if (model == null)
            //{
            //    return Json(new { success = false });
            //}
            //else
            //{
            //    var Fees = await _fees.GetFirstOrDefaultAsync(filter: x => x.Id == model.PaymentId, includeProperties: "Establishment");
            //    if (Fees == null)
            //    {
            //        return Json(new { success = false });
            //    }
            //    var Pay = await _paymentRepo.GetFirstOrDefaultAsync(filter: x => x.FeesId == Fees.Id && x.PaymentStatus == 1);
            //    if (Pay != null)
            //    {
            //        return Json(new { success = false, errormsg = "Cannot Pay Offline!.. An Online Payment Link Already Send to the registered Email. Try Canceling the Payment and create again to proceed.." });
            //    }
            //    var FeesDetails = await _feesDetailsTable.GetAllAsync(filter: x => x.FeesId == Fees.Id);
            //    Payment payment = new Payment();
            //    payment.InvoiceNo = Fees!.InvoiceNo!;
            //    payment.ReceiptNo = GenerateReceiptNumber().Result;
            //    payment.FeesId = Fees.Id;
            //    payment.Amount = Fees!.Amount;
            //    payment.PaymentMethod = Convert.ToByte(model.PaymentMethodId);
            //    //payment.PaymentOn = model.CollectionDate;
            //    payment.PaymentOn = DateTime.Now;
            //    if (model.PaymentMethodId == 3 || model.PaymentMethodId == 4 || model.PaymentMethodId == 5)
            //    {
            //        //payment.BankName = model.BankName;
            //        payment.ReferenceNumber = model.ReferenceNumber;
            //    }
            //    //else if(model.PaymentMethodId == 4)
            //    //{
            //    //    payment.ReferenceNumber = model.CardNumber;
            //    //}
            //    payment.PaymentBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            //    payment.PaymentStatus = 2;
            //    payment.PaymentType = 2;
            //    await _paymentRepo.AddAsync(payment);

            //    foreach (var feesDets in FeesDetails)
            //    {
            //        var paymentDets = new PaymentDetailsTable();
            //        paymentDets.PaymentId = payment.Id;
            //        paymentDets.Amount = feesDets.Amount;
            //        paymentDets.Title = feesDets.Title;
            //        paymentDets.PaymentStatus = payment.PaymentStatus;
            //        paymentDets.PaymentDate = payment.PaymentOn;
            //        paymentDets.PaymentBy = payment.PaymentBy;
            //        await _paymentDetailsTable.AddAsync(paymentDets);
            //    }

            //    var PaymentDetails = await _paymentDetailsTable.GetAllAsync(filter: x => x.PaymentId == payment.Id);

            //    if (Fees.Establishment!.PermitNumber!.StartsWith("TF"))
            //    {
            //        if (Fees.Establishment!.PermitStatusId == 7)
            //        {
            //            Fees.Establishment!.OldPermitStatusId = 7;
            //            Fees.Establishment.PermitStatusId = 9;
            //            Fees.Establishment.SyncDate = DateTime.Now;
            //            await _establishment.UpdateAsync(Fees.Establishment);
            //        }
            //    }
            //    else
            //    {
            //        if (Fees.Establishment!.PermitStatusId == 7)
            //        {
            //            Fees.Establishment!.OldPermitStatusId = Fees.Establishment.PermitStatusId;
            //            Fees.Establishment.PermitStatusId += 1;
            //            Fees.Establishment.SyncDate = DateTime.Now;
            //            await _establishment.UpdateAsync(Fees.Establishment);
            //        }
            //    }


            //    Fees.Status = 2;
            //    Fees.UpdatedOn = DateTime.Now;
            //    Fees.UpdatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            //    await _fees.UpdateAsync(Fees);
            //    foreach (var fees in FeesDetails)
            //    {
            //        fees.Status = Fees.Status;
            //        fees.UpdatedOn = DateTime.Now;
            //        fees.UpdatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            //        await _feesDetailsTable.UpdateAsync(fees);
            //    }
            //    return Json(new { success = true });
            //}

            if (model == null)
            {
                return Json(new { success = false });
            }
            else
            {
                var activateflg = "";
                var payment = await _paymentRepo.GetFirstOrDefaultAsync(filter: x => x.Id == model.PaymentId, includeProperties: "Fees");
                if (payment == null)
                {
                    return Json(new { success = false });
                }
                var Est = await _establishment.GetFirstOrDefaultAsync(filter: x => x.Id == payment.EstablishmentId);
                if (Est == null)
                {
                    return Json(new { success = false });
                }
                payment.ReceiptNo = GenerateReceiptNumber().Result;
                //payment.PaymentMethod = Convert.ToByte(model.PaymentMethodId);
                payment.PaymentOn = DateTime.Now;
                //if (model.PaymentMethodId == 3 || model.PaymentMethodId == 4 || model.PaymentMethodId == 5)
                //{
                //    payment.ReferenceNumber = model.ReferenceNumber;
                //}
                payment.PaymentBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                payment.PaymentStatus = 2;
                payment.PaymentType = 2;
                payment.IsVoidEnabled = true;
                //await _paymentRepo.UpdateAsync(payment);
                if (model.PaymentSplit != null) 
                {
                    
                        if (model.PaymentSplit!.Any())
                        {
                            foreach (var paysplit in model.PaymentSplit)
                            {
                                if(payment.PayMethodType == null)
                                {
                                    payment.PayMethodType = paysplit.PaymentMethod == 2 ? "Cash" : (paysplit.PaymentMethod == 3 ? "Check" : (paysplit.PaymentMethod == 4 ? "Card" : paysplit.PaymentMethod == 5 ? "Money Order" : ""));
                                }
                                else
                                {
                                    payment.PayMethodType = payment.PayMethodType + "/" + (paysplit.PaymentMethod == 2 ? "Cash" : (paysplit.PaymentMethod == 3 ? "Check" : (paysplit.PaymentMethod == 4 ? "Card" : paysplit.PaymentMethod == 5 ? "Money Order" : "")));
                                }

                                //payment.PayMethodType = (string?)(payment.PayMethodType==null? "" : payment.PayMethodType + "/").Concat(paysplit.PaymentMethod==2? "Cash": (paysplit.PaymentMethod==3? "Check":(paysplit.PaymentMethod==4?"Card":paysplit.PaymentMethod==5? "Money Order":"")));
                                paysplit.PaymentId = model.PaymentId;
                                await _paymentSplit.AddAsync(paysplit);
                            }
                        }
                }
                await _paymentRepo.UpdateAsync(payment);

                var paymentDetails = await _paymentDetailsTable.GetAllAsync(filter: x => x.PaymentId == payment.Id);
                if (paymentDetails.Any())
                {
                    foreach (var paymentDetail in paymentDetails)
                    {
                        paymentDetail.PaymentStatus = payment.PaymentStatus;
                        paymentDetail.PaymentBy = payment.PaymentBy;
                        paymentDetail.PaymentDate = payment.PaymentOn;
                        await _paymentDetailsTable.UpdateAsync(paymentDetail);
                    }
                }


                if (payment.Fees!.Establishment!.PermitNumber!.StartsWith("TF"))
                {
                    if (payment.Fees!.Establishment!.PermitStatusId == 7)
                    {
                        payment.Fees!.Establishment!.OldPermitStatusId = 7;
                        payment.Fees!.Establishment.PermitStatusId = 9;
                        payment.Fees!.Establishment.SyncDate = DateTime.Now;
                        await _establishment.UpdateAsync(payment.Fees!.Establishment);
                    }
                }
                else
                {
                    if (payment.Fees!.Establishment!.PermitStatusId == 7)
                    {
                        payment.Fees!.Establishment!.OldPermitStatusId = payment.Fees!.Establishment.PermitStatusId;
                        payment.Fees!.Establishment.PermitStatusId += 1;
                        payment.Fees!.Establishment.SyncDate = DateTime.Now;
                        await _establishment.UpdateAsync(payment.Fees!.Establishment);
                    }
                }


                payment.Fees!.Status = 2;
                payment.Fees!.UpdatedOn = DateTime.Now;
                payment.Fees!.UpdatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                payment.Fees!.IsVoidEnabled = true;
                await _fees.UpdateAsync(payment.Fees!);
                var feesDetails = await _feesDetailsTable.GetAllAsync(filter: x => x.FeesId == payment.Fees!.Id);
                foreach (var fees in feesDetails)
                {
                    fees.Status = payment.Fees!.Status;
                    fees.UpdatedOn = DateTime.Now;
                    fees.UpdatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    await _feesDetailsTable.UpdateAsync(fees);
                }
                //Email
                InvoicePdfVM receiptModel = new InvoicePdfVM();
                receiptModel.Payment = payment;
                receiptModel.Fees = payment.Fees;
                receiptModel.Fees!.Establishment = await _establishment.GetFirstOrDefaultAsync(filter: x => x.Id == receiptModel.Payment.EstablishmentId);
                TimeZoneInfo centralZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
                receiptModel.InvoiceDate = receiptModel.Payment.InvoiceDate!.Value.ToShortDateString() + " " + TimeZoneInfo.ConvertTime(receiptModel.Payment.InvoiceDate!.Value, centralZone).ToShortTimeString();
                var user = await _users.GetFirstOrDefaultAsync(filter: x => x.Id == receiptModel.Payment.InvoiceBy);
                receiptModel.InvoiceCreatedBy = user.FirstName + " " + user.LastName;
                receiptModel.PaymentDate = receiptModel.Payment.PaymentOn ?? DateTime.Now;
                receiptModel.PaymentDate = TimeZoneInfo.ConvertTime(receiptModel.PaymentDate, centralZone);
                receiptModel.PaymentMethod = receiptModel.Payment.PaymentMethod == 0 ? "Credit Card" : (receiptModel.Payment.PaymentMethod == 2 ? "Cash" : (receiptModel.Payment.PaymentMethod == 3 ? "Check" : (receiptModel.Payment.PaymentMethod == 4 ? "Card" : "Money Order")));
                receiptModel.PaymentDetails = await _paymentDetailsTable.GetAllAsync(filter: x => x.PaymentId == receiptModel.Payment.Id);
                receiptModel.PaymentSplits = await _paymentSplit.GetAllAsync(filter: x => x.PaymentId == receiptModel.Payment.Id);
                foreach (var item in receiptModel.PaymentDetails)
                {
                    var EstType = await _estTypes.GetFirstOrDefaultAsync(filter: x => x.Name == item.Title, includeProperties: "Jurisdiction");
                    var Jurisdiction = new JurisdictionAccounts();
                    if (EstType != null)
                    {
                        Jurisdiction = await _jurisdictionaccounts.GetFirstOrDefaultAsync(filter: x => x.Id == EstType.JurisdictionId, includeProperties: "Programs");
                        receiptModel.Jurisdiction = Jurisdiction.Name;
                        receiptModel.Program = Jurisdiction.Programs!.Name;
                    }

                }
                var estOwner = await _owner.GetFirstOrDefaultAsync(filter:x=>x.EstablishmentId== receiptModel.Fees!.Establishment.Id);

                var generatedPdf = await _generatePdf.GetByteArray("Views/PdfTemplates/ReceiptPdf.cshtml", receiptModel);
                Byte[][] bytearray = new Byte[][] { generatedPdf };
                string[] pdfName = new string[] { "Receipt.Pdf" };
                await _emailSenderService.SendEmail(estOwner.EmailId!,"", "Payment Receipt", "Thank you for payment. Best wishes from Brazos County, TX Health District (BCHD), 201 N Texas Ave Bryan, TX 77803", bytearray, pdfName);
                //return Json(new { success = true, msg = "Mail sent successfully" });

                var inspections = await _inspection.GetAllAsync(filter: x => x.PurposeId == 1 && x.EstablishmentId == payment.EstablishmentId);
                if (inspections.Any())
                {
                    var flg = 0;
                    foreach (var inspection in inspections)
                    {
                        var openingChkData = await _openinginsData.GetFirstOrDefaultAsync(filter: x => x.InspectionId == inspection.Id);
                        if (openingChkData.PermitApproval == true)
                        {
                            flg = 1;
                            break;
                        }
                    }
                    if (flg == 1)
                    {
                        if (payment.IsPermitFee==true && payment.Fees!.Establishment!.PermitStatusId == 8)
                        {
                            activateflg = "Active";

                            payment.Fees!.Establishment!.ActivationDate = DateTime.Now;
                            var year = DateTime.Now.Year;
                            //payment.Fees!.Establishment!.ExpiryDate = new DateTime(year, 12, 31);

                            payment.Fees!.Establishment!.OldPermitStatusId = 8;
                            if (DateTime.Now.Date >= new DateTime(year, 10, 15) && DateTime.Now.Date <= new DateTime(year, 12, 15))
                            {
                                payment.Fees!.Establishment!.PermitStatusId = 10;
                                payment.Fees!.Establishment!.ExpiryDate = new DateTime(year, 12, 31);

                                if (payment.Fees!.Establishment!.ApplicationForId == 1 || payment.Fees!.Establishment!.ApplicationForId == 2)
                                {
                                    var agency = await _agencystaffreqfields.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == payment.Fees!.Establishment!.Id, includeProperties: "EstablishmentTypes,EstablishmentSize,RiskCategory");

                                    var estTypes = await _estTypes.GetAllAsync(filter: x => x.IsActive == true && (x.JurisdictionId >= 1 && x.JurisdictionId <= 3));

                                    var SelectedEst = new EstablishmentTypes();
                                    var selectedEstName = "";

                                    if (agency.EstablishmentTypes!.JurisdictionId == 1)
                                    {
                                        //agency.EstablishmentTypes!.IsPorated == true ? selectedEstName = "Food Service Permit Renewal" : selectedEstName = agency.EstablishmentTypes!.Name;
                                        selectedEstName = agency.EstablishmentTypes!.IsPorated == true ? "Food Service Permit Renewal" : agency.EstablishmentTypes!.Name;
                                        //SelectedEst = estTypes.Where(x => x.Name == "Food Service Permit Renewal" && x.JurisdictionId == 1).FirstOrDefault();
                                        SelectedEst = estTypes.Where(x => x.Name == selectedEstName && x.JurisdictionId == 1).FirstOrDefault();
                                    }
                                    else
                                    {
                                        var RiskType = "";
                                        var EstSizeType = "";

                                        selectedEstName = agency.EstablishmentTypes!.IsPorated == true ? "Food Service Permit Renewal" : agency.EstablishmentTypes!.Name;

                                        if (agency.EstablishmentTypes!.IsPorated == true)
                                        {
                                            RiskType = agency.RiskCategory!.Name == "Low" ? "1" : (agency.RiskCategory!.Name == "Medium" ? "2" : (agency.RiskCategory!.Name == "High" ? "3" : ""));
                                            EstSizeType = agency.EstablishmentSize!.Name == "Small" ? "A" : (agency.EstablishmentSize!.Name == "Medium" ? "B" : (agency.EstablishmentSize!.Name == "Large" ? "C" : ""));

                                            SelectedEst = estTypes.Where(x => (x.Name!.Contains("Renewal") && x.Name!.Contains(RiskType + EstSizeType)) && x.JurisdictionId == agency.EstablishmentTypes.JurisdictionId).FirstOrDefault();
                                        }
                                        else
                                        {
                                            SelectedEst = estTypes.Where(x => (x.Name == agency.EstablishmentTypes.Name) && x.JurisdictionId == agency.EstablishmentTypes.JurisdictionId).FirstOrDefault();
                                        }
                                    }

                                    //if (agency.EstablishmentTypes!.JurisdictionId == 1)
                                    //{
                                    //    SelectedEst = estTypes.Where(x => x.Name == "Food Service Permit Renewal" && x.JurisdictionId == 1).FirstOrDefault();
                                    //}
                                    //else
                                    //{
                                    //    var RiskType = "";
                                    //    var EstSizeType = "";

                                    //    RiskType = agency.RiskCategory!.Name == "Low" ? "1" : (agency.RiskCategory!.Name == "Medium" ? "2" : (agency.RiskCategory!.Name == "High" ? "3" : ""));
                                    //    EstSizeType = agency.EstablishmentSize!.Name == "Small" ? "A" : (agency.EstablishmentSize!.Name == "Medium" ? "B" : (agency.EstablishmentSize!.Name == "Large" ? "C" : ""));

                                    //    SelectedEst = estTypes.Where(x => (x.Name!.Contains("Renewal") && x.Name!.Contains(RiskType + EstSizeType)) && x.JurisdictionId == agency.EstablishmentTypes.JurisdictionId).FirstOrDefault();

                                    //}
                                    Fees renewalFees = new Fees();
                                    renewalFees.EstablishmentId = payment.Fees!.Establishment!.Id;
                                    renewalFees.BaseAmount = SelectedEst!.Q1Fees ?? 0;
                                    renewalFees.Amount = SelectedEst!.Q1Fees ?? 0;
                                    renewalFees.InvoiceNo = GenerateInvoiceNumber().Result;
                                    renewalFees.Status = 1;
                                    renewalFees.CreatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                                    renewalFees.CreatedOn = DateTime.Now;
                                    renewalFees.FeesCalculation = 0;
                                    renewalFees.IsPermitFee = true;
                                    await _fees.AddAsync(renewalFees);

                                    FeesDetailsTable renewalFeesDetails = new FeesDetailsTable();
                                    renewalFeesDetails.FeesId = renewalFees.Id;
                                    renewalFeesDetails.EstablishmentTypeId = SelectedEst.Id;
                                    renewalFeesDetails.Amount = renewalFees.Amount;
                                    renewalFeesDetails.Title = SelectedEst.Name;
                                    renewalFeesDetails.Status = 1;
                                    renewalFeesDetails.CreatedOn = DateTime.Now;
                                    renewalFeesDetails.CreatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                                    await _feesDetailsTable.AddAsync(renewalFeesDetails);



                                    var renewalpayment = new Payment();
                                    renewalpayment.EstablishmentId = payment.Fees!.Establishment!.Id;
                                    renewalpayment.FeesId = renewalFees.Id;
                                    renewalpayment.InvoiceNo = renewalFees.InvoiceNo;
                                    renewalpayment.Amount = renewalFees.Amount;
                                    renewalpayment.InvoiceBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                                    renewalpayment.InvoiceDate = renewalFees.CreatedOn;
                                    renewalpayment.PaymentStatus = 1;
                                    renewalpayment.IsPermitFee = true;
                                    try
                                    {
                                        await _paymentRepo.AddAsync(renewalpayment);
                                    }
                                    catch (Exception ex)
                                    {

                                    }


                                    var renewalfeesDetails = await _feesDetailsTable.GetAllAsync(x => x.FeesId == renewalFees.Id);
                                    if (renewalfeesDetails.Any())
                                    {
                                        foreach (var feesDetailsItem in renewalfeesDetails)
                                        {
                                            var renewalpaymentDets = new PaymentDetailsTable();
                                            renewalpaymentDets.PaymentId = renewalpayment.Id;
                                            renewalpaymentDets.Amount = feesDetailsItem.Amount;
                                            renewalpaymentDets.Title = feesDetailsItem.Title;
                                            renewalpaymentDets.PaymentStatus = renewalpayment.PaymentStatus;
                                            await _paymentDetailsTable.AddAsync(renewalpaymentDets);
                                        }
                                    }
                                }
                            }
                            else if (DateTime.Now.Date > new DateTime(year, 12, 15))
                            {
                                payment.Fees!.Establishment!.PermitStatusId = 9;
                                payment.Fees!.Establishment!.ExpiryDate = new DateTime(DateTime.Now.AddYears(1).Year, 12, 31);
                            }
                            else
                            {
                                payment.Fees!.Establishment!.PermitStatusId = 9;
                                payment.Fees!.Establishment!.ExpiryDate = new DateTime(year, 12, 31);
                            }

                            //FoodRenewalHistory history = new FoodRenewalHistory();
                            //history.EstablishmentId = establishment.Id;
                            //history.ActivationDate = establishment.ActivationDate.Value;
                            //history.ExpiryDate = establishment.ExpiryDate.Value;
                            //history.IsActive = true;
                            //history.IsCurrentYear = true;
                            //await _foodRenewalHistory.AddAsync(history);

                            if (payment.Fees!.Establishment!.RiskCategory == "High")
                            {
                                payment.Fees!.Establishment!.NextInspectionDate = (payment.Fees!.Establishment!.ActivationDate ?? DateTime.Now.Date).AddDays(120);
                            }
                            else if (payment.Fees!.Establishment!.RiskCategory == "Medium")
                            {
                                payment.Fees!.Establishment!.NextInspectionDate = (payment.Fees!.Establishment!.ActivationDate ?? DateTime.Now.Date).AddDays(180);
                            }
                            else if (payment.Fees!.Establishment!.RiskCategory == "Low")
                            {
                                payment.Fees!.Establishment!.NextInspectionDate = (payment.Fees!.Establishment!.ActivationDate ?? DateTime.Now.Date).AddDays(360);
                            }
                            payment.Fees!.Establishment!.OldPermitStatusId = 8;
                            payment.Fees!.Establishment!.PermitStatusId = 9;

                            var area = await _area.GetFirstOrDefaultAsync(filter: x => x.AreaNumber == payment.Fees!.Establishment!.Area);
                            var assignedTo = 2;
                            if (area != null)
                            {
                                var areawiseIns = await _areawiseIns.GetFirstOrDefaultAsync(filter: x => x.AreaId == area.Id);
                                if (areawiseIns != null)
                                {
                                    assignedTo = areawiseIns.AssignedUserId;
                                }
                            }
                            Schedule schedule = new Schedule();
                            schedule.EstablishmentId = payment.Fees!.Establishment!.Id;
                            schedule.ScheduledDate = payment.Fees!.Establishment!.NextInspectionDate!.Value;
                            schedule.PurposeId = 3;
                            schedule.StatusId = 2;
                            schedule.AssignedTo = assignedTo;
                            schedule.CreatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                            schedule.CreatedOn = DateTime.Now;
                            schedule.IsAdhoc = false;
                            schedule.IsAutoSchedule = true;
                            schedule.SyncDate = DateTime.Now;
                            try
                            {
                                await _schedule.AddAsync(schedule);
                            }
                            catch (Exception Ex)
                            {

                            }
                            payment.Fees!.Establishment!.SyncDate = DateTime.Now;
                            await _establishment.UpdateAsync(payment.Fees!.Establishment!);

                            FoodRenewalHistory history = new FoodRenewalHistory();
                            history.EstablishmentId = payment.Fees!.Establishment!.Id;
                            history.ActivationDate = payment.Fees!.Establishment!.ActivationDate!.Value;
                            history.ExpiryDate = payment.Fees!.Establishment!.ExpiryDate!.Value;
                            history.IsCurrentYear = true;
                            history.IsActive = true;
                            await _foodRenewalHistory.AddAsync(history);
                            return Json(new { success = true, activateflg = activateflg, permitStatusId = payment.Fees!.Establishment!.PermitStatusId, oldPermitStatusId = payment.Fees!.Establishment!.OldPermitStatusId });
                        }
                    }
                }

                if (payment.IsPermitFee == true && payment.Fees!.Establishment!.PermitStatusId == 10)
                {
                    activateflg = "Active";
                    payment.Fees!.Establishment!.ExpiryDate = new DateTime(DateTime.Now.AddYears(1).Year, 12, 31);
                    payment.Fees!.Establishment!.OldPermitStatusId = 10;
                    payment.Fees!.Establishment!.PermitStatusId = 9;

                    var existingHistory = await _foodRenewalHistory.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == payment.Fees!.Establishment!.Id && x.IsCurrentYear==true, isTracking: false);
                    if (existingHistory != null)
                    {
                        existingHistory.IsCurrentYear = false;
                        existingHistory.IsActive = false;
                        await _foodRenewalHistory.UpdateAsync(existingHistory);
                    }
                    payment.Fees!.Establishment!.SyncDate = DateTime.Now;
                    await _establishment.UpdateAsync(payment.Fees!.Establishment!);

                    FoodRenewalHistory history = new FoodRenewalHistory();
                    history.EstablishmentId = payment.Fees!.Establishment!.Id;
                    history.ActivationDate = payment.Fees!.Establishment!.ActivationDate!.Value;
                    history.ExpiryDate = payment.Fees!.Establishment!.ExpiryDate!.Value;
                    history.IsCurrentYear = true;
                    history.IsActive = true;
                    await _foodRenewalHistory.AddAsync(history);
                    return Json(new { success = true, activateflg = activateflg, permitStatusId = payment.Fees!.Establishment!.PermitStatusId, oldPermitStatusId = payment.Fees!.Establishment!.OldPermitStatusId });
                }

                return Json(new { success = true, activateflg = activateflg, permitStatusId = payment.Fees!.Establishment!.PermitStatusId, oldPermitStatusId = payment.Fees!.Establishment!.OldPermitStatusId });
            }
        }

        [HttpPost("/RefundOfflinePayment")]
        public async Task<IActionResult> RefundOfflinePayment(string id, string? pay_action, string? value)
        {
            //if (id == null)
            //{
            //    return Json(new { success = false, msg = "Not Found" });
            //}
            //else
            //{
            //    var Fees = await _fees.GetFirstOrDefaultAsync(filter: x => x.Id == Convert.ToInt32(_encrypt.Decrypt256(id)), includeProperties: "Establishment");
            //    if (Fees == null)
            //    {
            //        return Json(new { success = false });
            //    }
            //    var Pay = await _paymentRepo.GetFirstOrDefaultAsync(filter: x => x.FeesId == Fees.Id && x.PaymentStatus == 2);
            //    if (Pay == null)
            //    {
            //        return Json(new { success = false, msg = "Not Found" });
            //    }
            //    var FeesDetails = await _feesDetailsTable.GetAllAsync(filter: x => x.FeesId == Fees.Id);

            //    Payment payment = new Payment();
            //    payment.InvoiceNo = Fees!.InvoiceNo!;
            //    payment.ReceiptNo = GenerateReceiptNumber().Result;
            //    payment.FeesId = Fees.Id;
            //    payment.Amount = -System.Math.Abs(Fees!.Amount);
            //    payment.PaymentMethod = 2;
            //    //payment.PaymentOn = model.CollectionDate;
            //    payment.PaymentOn = Pay.PaymentOn;
            //    payment.PaymentBy = Pay.PaymentBy;
            //    payment.OldPaymentId = Pay.Id;
            //    payment.RefundVoidDate = DateTime.Now;
            //    payment.RefundVoidBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            //    payment.ReasonForRefundVoid = value;
            //    payment.PaymentStatus = 5;
            //    payment.PaymentType = 2;
            //    await _paymentRepo.AddAsync(payment);

            //    foreach (var feesDets in FeesDetails)
            //    {
            //        var paymentDets = new PaymentDetailsTable();
            //        paymentDets.PaymentId = payment.Id;
            //        paymentDets.Amount = -System.Math.Abs(feesDets.Amount);
            //        paymentDets.Title = feesDets.Title;
            //        paymentDets.PaymentStatus = payment.PaymentStatus;
            //        paymentDets.PaymentDate = payment.PaymentOn;
            //        paymentDets.PaymentBy = payment.PaymentBy;
            //        await _paymentDetailsTable.AddAsync(paymentDets);
            //    }

            //    //var PaymentDetails = await _paymentDetailsTable.GetAllAsync(filter: x => x.PaymentId == payment.Id);
            //    Fees.Status = 11;
            //    Fees.UpdatedOn = DateTime.Now;
            //    Fees.UpdatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            //    await _fees.UpdateAsync(Fees);
            //    foreach (var fees in FeesDetails)
            //    {
            //        fees.Status = Fees.Status;
            //        fees.UpdatedOn = DateTime.Now;
            //        fees.UpdatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            //        await _feesDetailsTable.UpdateAsync(fees);
            //    }
            //    return Json(new { success = true, msg = "Successfully Refunded" });
            //}

            if (id == null)
            {
                return Json(new { success = false, msg = "Not Found" });
            }
            else
            {
                var payment = await _paymentRepo.GetFirstOrDefaultAsync(filter: x => x.Id == Convert.ToInt32(_encrypt.Decrypt256(id)), includeProperties: "Fees", isTracking:false);
                if (payment == null)
                {
                    return Json(new { success = false });
                }
                
                var FeesDetails = await _feesDetailsTable.GetAllAsync(filter: x => x.FeesId == payment.Fees!.Id);


                Payment refundPayment = new Payment();
                refundPayment.InvoiceNo = payment!.InvoiceNo!;
                refundPayment.InvoiceDate = payment!.InvoiceDate!;
                refundPayment.InvoiceBy = payment.InvoiceBy;
                refundPayment.EstablishmentId = payment.EstablishmentId;
                refundPayment.ReceiptNo = GenerateReceiptNumber().Result;
                refundPayment.FeesId = payment.Fees!.Id;
                refundPayment.Amount = -System.Math.Abs(payment.Fees!.Amount);
                refundPayment.PayMethodType = payment.PayMethodType;
                //refundPayment.PaymentMethod = payment.PaymentMethod;
                //refundPayment.ReferenceNumber = payment.ReferenceNumber;
                //payment.PaymentOn = model.CollectionDate;
                refundPayment.PaymentOn = payment.PaymentOn;
                refundPayment.PaymentBy = payment.PaymentBy;
                refundPayment.OldPaymentId = payment.Id;
                refundPayment.RefundVoidDate = DateTime.Now;
                refundPayment.RefundVoidBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                refundPayment.ReasonForRefundVoid = value;
                if(pay_action == "\"OVmJOZsWsU\"")
                {
                    refundPayment.VoidedTransactionNo = "V-" + payment.InvoiceNo;
                    refundPayment.PaymentStatus = 6;
                }
                else if(pay_action == "\"vUZbE7i6A2\"")
                {
                    refundPayment.PaymentStatus = 5;
                }
                refundPayment.PaymentType = payment.PaymentType;
                await _paymentRepo.AddAsync(refundPayment);

                payment.RefundVoidPaymentId = refundPayment.Id;
                await _paymentRepo.UpdateAsync(payment);

                var PaymentSplits = await _paymentSplit.GetAllAsync(filter: x => x.PaymentId == payment.Id);
                if (PaymentSplits.Any())
                {
                    foreach(var paysplit in PaymentSplits)
                    {
                        var refundPaySplits = new PaymentSplit();
                        refundPaySplits.PaymentId = refundPayment.Id;
                        refundPaySplits.PaymentMethod = paysplit.PaymentMethod;
                        refundPaySplits.Amount = -System.Math.Abs(paysplit!.Amount);
                        refundPayment.ReferenceNumber = paysplit.ReferenceNumber;
                        await _paymentSplit.AddAsync(refundPaySplits);
                    }
                }

                foreach (var feesDets in FeesDetails)
                {
                    var paymentDets = new PaymentDetailsTable();
                    paymentDets.PaymentId = refundPayment.Id;
                    paymentDets.Amount = -System.Math.Abs(feesDets.Amount);
                    paymentDets.Title = feesDets.Title;
                    paymentDets.PaymentStatus = refundPayment.PaymentStatus;
                    paymentDets.PaymentDate = refundPayment.PaymentOn;
                    paymentDets.PaymentBy = refundPayment.PaymentBy;
                    paymentDets.RefundVoidBy = refundPayment.RefundVoidBy;
                    paymentDets.RefundVoidDate = refundPayment.RefundVoidDate;
                    await _paymentDetailsTable.AddAsync(paymentDets);
                }

                //var PaymentDetails = await _paymentDetailsTable.GetAllAsync(filter: x => x.PaymentId == payment.Id);
                if (pay_action == "\"OVmJOZsWsU\"")
                {
                    payment.Fees.Status = 9;
                }
                else if (pay_action == "\"vUZbE7i6A2\"")
                {
                    payment.Fees.Status = 11;
                }
                //payment.Fees.Status = 11;
                payment.Fees.UpdatedOn = DateTime.Now;
                payment.Fees.UpdatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                await _fees.UpdateAsync(payment.Fees);
                foreach (var fees in FeesDetails)
                {
                    fees.Status = payment.Fees.Status;
                    fees.UpdatedOn = DateTime.Now;
                    fees.UpdatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    await _feesDetailsTable.UpdateAsync(fees);
                }
                return Json(new { success = true, msg = "Successfully Refunded" });
            }
        }


        [HttpGet("/CheckPendingPayment/{id?}")]
        public async Task<IActionResult> CheckPendingPayment(string id)
        {
            var estId = Convert.ToInt32(_encrypt.Decrypt256(id));
            //var fees = await _fees.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == Convert.ToInt32(_encrypt.Decrypt256(id)) && x.Status == 1, includeProperties:"Establishment");
            //if (fees == null || fees.Establishment!.PermitStatusId>=9)
            //{
            //    return Json(new { success = false });
            //}
            var IsAnyExistingPayment = await _paymentRepo.GetAllAsync(filter: x => x.EstablishmentId == Convert.ToInt32(_encrypt.Decrypt256(id)) && x.IsPermitFee == true);
            var payment = await _paymentRepo.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == Convert.ToInt32(_encrypt.Decrypt256(id)) && x.RefundVoidPaymentId==null && x.IsPermitFee==true && (x.PaymentStatus == 2 || x.PaymentStatus == 1));
            var Est = await _establishment.GetFirstOrDefaultAsync(filter:x=>x.Id==estId);

            //if (!IsAnyExistingPayment.Any() || (IsAnyExistingPayment.Any() && payment == null && Est!.PermitStatusId != 9))
            //{
            //    return Json(new { success = true });
            //}

            if (!IsAnyExistingPayment.Any())
            {
                return Json(new { success = false });
            }
            else if(IsAnyExistingPayment.Any() && payment==null && Est!.PermitStatusId <= 9){
                return Json(new { success = false });
            }
            else if(Est!.PermitStatusId == 9 || Est!.PermitStatusId == 10)
            {
                  return Json(new { success = false });
            }
            return Json(new { success = true });
        
        }

        [HttpGet("/GetInvoicePdf")]
        public async Task<IActionResult> GetInvoicePdf(string id)
        {
            //if (id == null)
            //{
            //    return BadRequest();
            //}
            //InvoicePdfVM model = new InvoicePdfVM();
            //model.Fees = await _fees.GetFirstOrDefaultAsync(filter: x => x.Id == Convert.ToInt32(_encrypt.Decrypt256(id)), includeProperties: "Establishment");
            //if (model.Fees == null)
            //{
            //    return NotFound();
            //}
            //TimeZoneInfo centralZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
            //model.InvoiceDate = model.Fees.CreatedOn.ToShortDateString() + " " + TimeZoneInfo.ConvertTime(model.Fees.CreatedOn, centralZone).ToShortTimeString(); 
            //var user = await _users.GetFirstOrDefaultAsync(filter: x => x.Id == model.Fees.CreatedBy);
            //model.InvoiceCreatedBy = user.FirstName + " " + user.LastName;
            //model.FeesDetails = await _feesDetailsTable.GetAllAsync(filter: x => x.FeesId == model.Fees.Id);
            //foreach (var item in model.FeesDetails)
            //{
            //    if (item.EstablishmentTypeId != null)
            //    {
            //        var EstType = await _estTypes.GetFirstOrDefaultAsync(filter: x => x.Id == item.EstablishmentTypeId, includeProperties: "Jurisdiction");
            //        var Jurisdiction = new JurisdictionAccounts();
            //        if (EstType != null)
            //        {
            //            Jurisdiction = await _jurisdictionaccounts.GetFirstOrDefaultAsync(filter: x => x.Id == EstType.JurisdictionId, includeProperties: "Programs");
            //        }
            //        model.Jurisdiction = Jurisdiction.Name;
            //        model.Program = Jurisdiction.Programs!.Name;
            //    }
            //}
            //return await _generatePdf.GetPdf("Views/PdfTemplates/InvoicePdf.cshtml", model);

            if (id == null)
            {
                return BadRequest();
            }
            InvoicePdfVM model = new InvoicePdfVM();
            model.Payment = await _paymentRepo.GetFirstOrDefaultAsync(filter: x => x.Id == Convert.ToInt32(_encrypt.Decrypt256(id)), includeProperties: "Fees");
            if (model.Payment == null)
            {
                return NotFound();
            }
            model.Establishment = await _establishment.GetFirstOrDefaultAsync(filter: x => x.Id == model.Payment.EstablishmentId);
            TimeZoneInfo centralZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
            model.InvoiceDate = model.Payment.InvoiceDate!.Value.ToShortDateString() + " " + TimeZoneInfo.ConvertTime(model.Payment.InvoiceDate.Value, centralZone).ToShortTimeString();
            var user = await _users.GetFirstOrDefaultAsync(filter: x => x.Id == model.Payment.InvoiceBy);
            model.InvoiceCreatedBy = user.FirstName + " " + user.LastName;
            model.FeesDetails = await _feesDetailsTable.GetAllAsync(filter: x => x.FeesId == model.Payment.Fees!.Id);
            model.PaymentDetails = await _paymentDetailsTable.GetAllAsync(filter: x => x.PaymentId == model.Payment.Id);
            foreach (var item in model.FeesDetails)
            {
                if (item.EstablishmentTypeId != null)
                {
                    var EstType = await _estTypes.GetFirstOrDefaultAsync(filter: x => x.Id == item.EstablishmentTypeId, includeProperties: "Jurisdiction");
                    var Jurisdiction = new JurisdictionAccounts();
                    if (EstType != null)
                    {
                        Jurisdiction = await _jurisdictionaccounts.GetFirstOrDefaultAsync(filter: x => x.Id == EstType.JurisdictionId, includeProperties: "Programs");
                    }
                    model.Jurisdiction = Jurisdiction.Name;
                    model.Program = Jurisdiction.Programs!.Name;
                }
            }
            return await _generatePdf.GetPdf("Views/PdfTemplates/InvoicePdf.cshtml", model);
        }

        [HttpGet("/GetReceiptPdf")]
        public async Task<IActionResult> GetReceiptPdf(string id)
        {
            //if (id == null)
            //{
            //    return BadRequest();
            //}
            //InvoicePdfVM model = new InvoicePdfVM();
            //model.Fees = await _fees.GetFirstOrDefaultAsync(filter: x => x.Id == Convert.ToInt32(_encrypt.Decrypt256(id)), includeProperties: "Establishment");
            //if (model.Fees == null)
            //{
            //    return NotFound();
            //}
            //TimeZoneInfo centralZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
            //model.InvoiceDate = model.Fees.CreatedOn.ToShortDateString() + " " + TimeZoneInfo.ConvertTime(model.Fees.CreatedOn, centralZone).ToShortTimeString();
            //var user = await _users.GetFirstOrDefaultAsync(filter: x => x.Id == model.Fees.CreatedBy);
            //model.InvoiceCreatedBy = user.FirstName + " " + user.LastName;

            //if(flg == "Refunded")
            //{
            //    model.Payment = await _paymentRepo.GetFirstOrDefaultAsync(filter: x => x.FeesId == model.Fees.Id && x.PaymentStatus == 5);
            //}
            //else
            //{
            //    model.Payment = await _paymentRepo.GetFirstOrDefaultAsync(filter: x => x.FeesId == model.Fees.Id);
            //}

            //model.PaymentDate = model.Payment.PaymentOn??DateTime.Now;
            //model.PaymentDate = TimeZoneInfo.ConvertTime(model.PaymentDate, centralZone);
            //if (model.Payment.PaymentMethod == 0)
            //{
            //    model.PaymentMethod = "Credit Card";
            //}
            //else if (model.Payment.PaymentMethod == 2)
            //{
            //    model.PaymentMethod = "Cash";
            //}
            //else if (model.Payment.PaymentMethod == 3)
            //{
            //    model.PaymentMethod = "Check";
            //}
            //else if (model.Payment.PaymentMethod == 4)
            //{
            //    model.PaymentMethod = "Card";
            //}
            //else if (model.Payment.PaymentMethod == 5)
            //{
            //    model.PaymentMethod = "Money Order";
            //}
            //model.PaymentDetails = await _paymentDetailsTable.GetAllAsync(filter: x => x.PaymentId == model.Payment.Id);
            //foreach (var item in model.PaymentDetails)
            //{
            //    //if (item.EstablishmentTypeId != null)
            //    //{

            //    //}
            //    var EstType = await _estTypes.GetFirstOrDefaultAsync(filter: x => x.Name == item.Title, includeProperties: "Jurisdiction");
            //    var Jurisdiction = new JurisdictionAccounts();
            //    if (EstType != null)
            //    {
            //        Jurisdiction = await _jurisdictionaccounts.GetFirstOrDefaultAsync(filter: x => x.Id == EstType.JurisdictionId, includeProperties: "Programs");
            //    }
            //    model.Jurisdiction = Jurisdiction.Name;
            //    model.Program = Jurisdiction.Programs!.Name;
            //}
            //return await _generatePdf.GetPdf("Views/PdfTemplates/ReceiptPdf.cshtml", model);
            ////return await _generatePdf.GetPdf("Views/PdfTemplates/ReceiptPdf.cshtml", model);
            ///
            if (id == null)
            {
                return BadRequest();
            }
            InvoicePdfVM model = new InvoicePdfVM();
            model.Payment = await _paymentRepo.GetFirstOrDefaultAsync(filter: x => x.Id == Convert.ToInt32(_encrypt.Decrypt256(id)), includeProperties: "Fees");
            if (model.Payment == null)
            {
                return NotFound();
            }


            model.Fees = model.Payment.Fees;
            model.Fees!.Establishment = await _establishment.GetFirstOrDefaultAsync(filter:x=>x.Id == model.Payment.EstablishmentId);

            TimeZoneInfo centralZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
            model.InvoiceDate = model.Payment.InvoiceDate!.Value.ToShortDateString() + " " + TimeZoneInfo.ConvertTime(model.Payment.InvoiceDate!.Value, centralZone).ToShortTimeString();
            var user = await _users.GetFirstOrDefaultAsync(filter: x => x.Id == model.Payment.InvoiceBy);
            model.InvoiceCreatedBy = user.FirstName + " " + user.LastName;

            //if (flg == "Refunded")
            //{
            //    model.Payment = await _paymentRepo.GetFirstOrDefaultAsync(filter: x => x.FeesId == model.Fees.Id && x.PaymentStatus == 5);
            //}
            //else
            //{
            //    model.Payment = await _paymentRepo.GetFirstOrDefaultAsync(filter: x => x.FeesId == model.Fees.Id);
            //}

            model.PaymentDate = model.Payment.PaymentOn ?? DateTime.Now;
            model.PaymentDate = TimeZoneInfo.ConvertTime(model.PaymentDate, centralZone);
            model.PaymentMethod = model.Payment.PaymentMethod==0? "Credit Card":(model.Payment.PaymentMethod == 2 ? "Cash" : (model.Payment.PaymentMethod == 3 ? "Check" : (model.Payment.PaymentMethod == 4 ? "Card" : "Money Order")));
            //if (model.Payment.PaymentMethod == 0)
            //{
            //    model.PaymentMethod = "Credit Card";
            //}
            //else if (model.Payment.PaymentMethod == 2)
            //{
            //    model.PaymentMethod = "Cash";
            //}
            //else if (model.Payment.PaymentMethod == 3)
            //{
            //    model.PaymentMethod = "Check";
            //}
            //else if (model.Payment.PaymentMethod == 4)
            //{
            //    model.PaymentMethod = "Card";
            //}
            //else if (model.Payment.PaymentMethod == 5)
            //{
            //    model.PaymentMethod = "Money Order";
            //}
            model.PaymentDetails = await _paymentDetailsTable.GetAllAsync(filter: x => x.PaymentId == model.Payment.Id);
            model.PaymentSplits = await _paymentSplit.GetAllAsync(filter: x => x.PaymentId == model.Payment.Id);
            model.FeesDetails = await _feesDetailsTable.GetAllAsync(filter: x => x.FeesId == model.Payment.Fees!.Id);
            
            foreach (var item in model.FeesDetails)
            {
                if (item.EstablishmentTypeId != null)
                {
                    var EstType = await _estTypes.GetFirstOrDefaultAsync(filter: x => x.Id == item.EstablishmentTypeId, includeProperties: "Jurisdiction");
                    var Jurisdiction = new JurisdictionAccounts();
                    if (EstType != null)
                    {
                        Jurisdiction = await _jurisdictionaccounts.GetFirstOrDefaultAsync(filter: x => x.Id == EstType.JurisdictionId, includeProperties: "Programs");
                    }
                    model.Jurisdiction = Jurisdiction.Name;
                    model.Program = Jurisdiction.Programs!.Name;
                }
            }
            //foreach (var item in model.PaymentDetails)
            //{
            //    var EstType = await _estTypes.GetFirstOrDefaultAsync(filter: x => x.Name == item.Title, includeProperties: "Jurisdiction");
            //    var Jurisdiction = new JurisdictionAccounts();
            //    if (EstType != null)
            //    {
            //        Jurisdiction = await _jurisdictionaccounts.GetFirstOrDefaultAsync(filter: x => x.Id == EstType.JurisdictionId, includeProperties: "Programs");
            //        model.Jurisdiction = Jurisdiction.Name;
            //        model.Program = Jurisdiction.Programs!.Name;
            //    }

            //}
            return await _generatePdf.GetPdf("Views/PdfTemplates/ReceiptPdf.cshtml", model);
        }


        [HttpPost("/ActiveInactivePermit/{id?}")]
        public async Task<IActionResult> ActiveInactivePermit(string id)
        {
            if (id == null)
            {
                return Json(new { success = false, msg = "Not Found" });
            }
            else
            {
                var est = await _establishment.GetFirstOrDefaultAsync(filter: x => x.Id == Convert.ToInt32(_encrypt.Decrypt256(id)));
                if (est == null)
                {
                    return Json(new { success = false, msg = "Not Found" });
                }
                else
                {
                    
                    if (est.IsActive == true)
                    {
                        est.OldPermitStatusId = est.PermitStatusId;
                        est.PermitStatusId = 13;
                        est.IsActive = false;
                        
                    }
                    else
                    {
                        //est.PermitStatusId = est.OldPermitStatusId??9;
                        est.PermitStatusId = est.OldPermitStatusId == 10 ? 10 : 9;
                        //if (est.OldPermitStatusId == 10)
                        //{
                        //    est.PermitStatusId = 10;
                        //}
                        //else if(est.OldPermitStatusId == 9)
                        //{
                        //    est.PermitStatusId = 9;
                        //}
                        
                        est.OldPermitStatusId = 13;
                        est.IsActive = true;
                        //foreach (var schedule in scheduleList)
                        //{
                        //    schedule.StatusId = 2;
                        //    await _schedule.UpdateAsync(schedule);
                        //}
                    }
                    est.SyncDate = DateTime.Now;
                    await _establishment.UpdateAsync(est);
                }
            }
            return Json(new { success = true, msg = "Success" });
        }

        [HttpPost("/ClosePermit/{id?}")]
        public async Task<IActionResult> ClosePermit(string id)
        {
            if (id == null)
            {
                return Json(new { success = false, msg = "Not Found" });
            }
            else
            {
                var est = await _establishment.GetFirstOrDefaultAsync(filter: x => x.Id == Convert.ToInt32(_encrypt.Decrypt256(id)));
                if (est == null)
                {
                    return Json(new { success = false, msg = "Not Found" });
                }
                else
                {
                    est.OldPermitStatusId = est.PermitStatusId;
                    est.PermitStatusId = 14;
                    est.IsActive = false;
                    var scheduleList = await _schedule.GetAllAsync(filter: x => x.EstablishmentId == est.Id && x.StatusId == 2);
                     if (scheduleList.Any())
                     {
                        foreach (var schedule in scheduleList)
                        {
                              schedule.StatusId = 6;
                              await _schedule.UpdateAsync(schedule);
                        }
                     }
                    est.SyncDate = DateTime.Now;
                    await _establishment.UpdateAsync(est);
                }
            }
            return Json(new { success = true, msg = "Success" });
        }


        public async Task<string> GenerateInvoiceNumber()
        {
            var InvoiceNo = "";

            while (InvoiceNo == "")
            {
                  var random = new Random();
                  var month = System.DateTime.Now.Month;
                  var monthStr = "";
                  if (month < 10)
                  {
                        monthStr = 0.ToString() + month.ToString();
                  }
                  else
                  {
                        monthStr = month.ToString();
                  }
                  var tempInvoice = System.DateTime.Now.Year.ToString() + "-" + monthStr + "-" + random.Next(0, 9999).ToString("D4");
                  var fees = await _fees.GetFirstOrDefaultAsync(filter: x => x.InvoiceNo == tempInvoice);
                  if (fees == null)
                  {
                        InvoiceNo = tempInvoice;
                  }
            }

            return InvoiceNo;
        }

        public async Task<string> GenerateReceiptNumber()
        {
            var ReceiptNo = "";
            var cnt = 0;
            var payment = await _paymentRepo.GetAllAsync();
            if (payment.Any()) 
            {
                cnt = payment.Count()+1;
            }
            
            while (ReceiptNo == "")
            {
                  var month = System.DateTime.Now.Month;
                  var monthStr = "";
                  if (month < 10)
                  {
                        monthStr = 0.ToString() + month.ToString();
                  }
                  else
                  {
                        monthStr = month.ToString();
                  }
                  
                  var tempReceiptNo = System.DateTime.Now.Year.ToString() + "-" + monthStr + "-" + cnt.ToString("D6"); 
                  var Existingpayment = await _paymentRepo.GetFirstOrDefaultAsync(filter: x => x.ReceiptNo == tempReceiptNo);
                  if (Existingpayment == null)
                  {
                        ReceiptNo = tempReceiptNo;
                  }
                  else
                  {
                        cnt++;
                  }
            }
            return ReceiptNo;
        }

        [HttpGet("/GetInvoiceNumber")]
        public IActionResult GetInvoiceNumber()
        {
            var InvoiceNo = GenerateInvoiceNumber().Result;

            //while (InvoiceNo == "")
            //{
            //    var random = new Random();
            //    var month = System.DateTime.Now.Month;
            //    var monthStr = "";
            //    if (month < 10)
            //    {
            //        monthStr = 0.ToString() + month.ToString();
            //    }
            //    else
            //    {
            //        monthStr = month.ToString();
            //    }
            //    var tempInvoice = System.DateTime.Now.Year.ToString() + "-" + monthStr + "-" + random.Next(0, 9999).ToString("D4");
            //    var fees = await _fees.GetFirstOrDefaultAsync(filter: x => x.InvoiceNo == tempInvoice);
            //    if (fees == null)
            //    {
            //        InvoiceNo = tempInvoice;
            //    }
            //}
            return Json(new { success = true, invoice = InvoiceNo });
        }

        [HttpGet("/GetCertificateEstOwnerEmail/{EstId?}")]
        public async Task<IActionResult> GetCertificateEstOwnerEmail(string EstId)
        {
            var EstOwner = new EstablishmentOwner();
            try
            {
                EstOwner = await _owner.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == Convert.ToInt32(_encrypt.Decrypt256(EstId)));
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }

            return Json(new { success = true, email = EstOwner.EmailId, id= EstId });
        }

        [HttpPost("/SendPermitCertificateViaEmail")]
        public async Task<IActionResult> SendCertificateEmail(string CertificateMailEstId, string toEmail, string ccEmail, string Subject, string Body)
        {
            var establishmentId = _encrypt.Decrypt256(CertificateMailEstId);
            PermitCertificateVM permitCertificateVM = new PermitCertificateVM();
            permitCertificateVM.establishment = await _establishment.GetById(Convert.ToInt32(establishmentId));
            permitCertificateVM.establishmentOwner = await _owner.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == Convert.ToInt32(establishmentId));
            permitCertificateVM.agencyStaffReqFields = await _agencystaffreqfields.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == Convert.ToInt32(establishmentId), includeProperties: "EstablishmentTypes");
            var generatedPdf = await _generatePdf.GetByteArray("Views/PdfTemplates/PermitCertificate.cshtml", permitCertificateVM);
            Byte[][] byteArray = new Byte[][] { generatedPdf };            
            string[] pdfName = new string[] { "PermitCertificate.Pdf" };
            try
            {
                await _emailSenderService.SendEmail(toEmail, ccEmail, Subject, Body, byteArray, pdfName);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, msg = ex.Message });
            }
            return Json(new { success = true, msg = "Mail sent successfully" });
        }
    }
}
