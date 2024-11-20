using BrazosApp.DataAccess.Repository.IRepository;
using BrazosApp.Models;
using BrazosApp.Models.ViewModels;
using BrazosApp.Utility;
using BrazosApp.Utility.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using Wkhtmltopdf.NetCore;

namespace BrazosApp.Areas.LoggedIn.Controllers
{
      [Area("LoggedIn")]
      [Authorize("CommonPolicy")]
      public class InspectionReportController : Controller
      {
            private readonly IRepository<Section> _section;
            private readonly IRepository<SubSection> _subSection;
            private readonly IRepository<Item> _item;
            private readonly IRepository<Establishment> _establishments;
            private readonly IRepository<Inspection> _inspection;
            private readonly IRepository<InspectionItemDetails> _inspectionItems;
            private readonly IRepository<RFMFInspectionData> _rfmfInspectionData;
            private readonly IRepository<Area> _area;
            private readonly IRepository<Schedule> _scheduler;
            private readonly IRepository<Users> _users;
            private readonly IRepository<AreaWiseInspectors> _areaWiseInspector;
            private readonly IRepository<AgencyStaffReqFields> _agencyreqFields;
            private readonly IRepository<InspectionPurposes> _purpose;
            private readonly IRepository<TemperatureObservation> _temperatureObs;
            private readonly IRepository<EstablishmentOwner> _owner;
            private readonly IEncrypt _encrypt;
            private readonly IGeneratePdf _generatePdf;

            public InspectionReportController(IRepository<Item> item,
            IRepository<Section> section,
            IRepository<SubSection> subSection,
            IRepository<Establishment> establishments,
            IRepository<Inspection> inspection,
            IRepository<InspectionItemDetails> inspectionItems,
            IRepository<RFMFInspectionData> rfmfInspectionData,
            //IRepository<OpeningInspectionData> openinginsData,
            IRepository<Schedule> scheduler,
            IRepository<Users> users,
            IRepository<AgencyStaffReqFields> agencyreqFields,
            IRepository<Area> area,
            IRepository<AreaWiseInspectors> areaWiseInspector,
            IRepository<TemperatureObservation> temperatureObs,
            IRepository<InspectionPurposes> purpose,
            IRepository<EstablishmentOwner> owner,
            IEncrypt encrypt,
            IGeneratePdf generatePdf)
            {
                  _item = item;
                  _section = section;
                  _subSection = subSection;
                  _establishments = establishments;
                  _inspectionItems = inspectionItems;
                  _rfmfInspectionData = rfmfInspectionData;
                  _inspection = inspection;
                  _scheduler = scheduler;
                  _users = users;
                  _agencyreqFields = agencyreqFields;
                  _area = area;
                  _areaWiseInspector = areaWiseInspector;
                  _temperatureObs = temperatureObs;
                  _purpose = purpose;
                  _owner = owner;
                  _generatePdf = generatePdf;
                  _encrypt = encrypt;
            }


            [HttpGet("/InspectionReport/{code?}")]
            public async Task<IActionResult> Index(string code)
            {
                  InspectorReportVM model = new InspectorReportVM();
                  var users = await _users.GetAllAsync();
                  model.UserList = users.Select(x => new SelectListItem
                  {
                        Text = x.FirstName! + " " + x.LastName,
                        Value = x.Id.ToString()
                  }).ToList();
                  ViewBag.Code = code;
                  return View(model);
            }

            [HttpPost("/GetAllInspectionReport/{code?}")]
            public async Task<IActionResult> GetAllInspectionReport(InspectorReportVM model, string code)
            {
                  IEnumerable<Inspection> Inspections = new List<Inspection>();

                  if (User.FindFirstValue(ClaimTypes.Role) == SD.Inspector)
                  {
                        Inspections = await _inspection.GetAllAsync(filter: x => x.InspectedBy == Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)), orderBy: x => x.OrderByDescending(x => x.InspectionDate));
                  }

                  else
                  {
                        Inspections = await _inspection.GetAllAsync(orderBy: x => x.OrderByDescending(x => x.InspectionDate));
                  }
                  var InspectionList = from d in Inspections
                                       join u in await _establishments.GetAllAsync(/*filter: x => x.PermitNumber!.StartsWith(code)*/)
                                       //join u in est
                                       on d.EstablishmentId equals u.Id into egroup
                                       join p in await _purpose.GetAllAsync()
                                       on d.PurposeId equals p.Id into mgroup
                                       join i in await _users.GetAllAsync()
                                       on d.InspectedBy equals i.Id into igroup
                                       join insData in await _rfmfInspectionData.GetAllAsync()
                                       on d.Id equals insData.InspectionId into insDatagroup
                                       from u in egroup.DefaultIfEmpty()
                                       from p in mgroup.DefaultIfEmpty()
                                       from i in igroup.DefaultIfEmpty()
                                       from insData in insDatagroup.DefaultIfEmpty()
                                       orderby d.InspectionDate descending
                                       select new
                                       {
                                             Id = d.Id,
                                             Permit = u.PermitNumber,
                                             Name = u.Name,
                                             InspectionDate = d.InspectionDate,
                                             InspectedByValue = d.InspectedBy,
                                             InspectedBy = i.FirstName + " " + i.LastName,
                                             Purpose = p.Name,
                                             FollowUp = d.FollowUp,
                                             FollowUpDate = d.FollowUpDate,
                                             Score = insData?.Score,
                                             EncryptedId = _encrypt.Encrypt256(d.Id.ToString())
                                       };
                  var InspectionFinalList = InspectionList.Where(x => x.Permit.Contains(code));

                  if (model.Inspector != null && model.Inspector != "--Select User--")
                  {
                        InspectionFinalList = InspectionFinalList.Where(x => x.InspectedByValue != 0 && x.InspectedByValue == Convert.ToInt32(model.Inspector));
                  }

                  if (model.SearchBy != null && model.SearchBy != "--Select By--")
                  {
                        if(model.SearchBy== "Inspection Date")
                        {
                              if (model.FromDate != null)
                              {

                                    InspectionFinalList = InspectionFinalList.Where(x => x.InspectionDate >= model.FromDate);
                              }

                              if (model.ToDate != null)
                              {
                                    InspectionFinalList = InspectionFinalList.Where(x => x.InspectionDate <= model.ToDate);
                              }
                        }
                        else if(model.SearchBy == "Follow Up Date")
                        {
                              if (model.FromDate != null)
                              {

                                    InspectionFinalList = InspectionFinalList.Where(x => x.FollowUpDate!=null && x.FollowUpDate! >= model.FromDate);
                              }

                              if (model.ToDate != null)
                              {
                                    InspectionFinalList = InspectionFinalList.Where(x => x.FollowUpDate != null && x.FollowUpDate! <= model.ToDate);
                              }
                        }
                        
                  }

                  var InspectionExportList = from s in InspectionFinalList
                                             select new
                                             {
                                                   Permit = s.Permit,
                                                   Name = s.Name,
                                                   InspectionDate = s.InspectionDate.ToString("MM/dd/yyyy"),
                                                   InspectedBy = s.InspectedBy,
                                                   Purpose = s.Purpose,
                                                   FollowUp = s.FollowUp,
                                                   FollowUpDate = s.FollowUpDate==null? "" : s.FollowUpDate.Value.ToString("MM/dd/yyyy"),
                                                   Score = s.Score,
                                             };

                  return Json(new { data = InspectionExportList.ToList() });
            }
      }
}
