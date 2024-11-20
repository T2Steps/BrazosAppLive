using BrazosApp.DataAccess.Repository.IRepository;
using BrazosApp.Models;
using BrazosApp.Models.ViewModels;
using BrazosApp.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace BrazosApp.Areas.LoggedIn.Controllers
{
      [Area("LoggedIn")]
      [Authorize("CommonPolicy")]
      public class AreaWisePendingInspectionReportController : Controller
      {
            private readonly IRepository<Schedule> _scheduler;
            private readonly IRepository<AreaWiseInspectors> _areaWiseInspector;
            private readonly IRepository<AgencyStaffReqFields> _agencyreqFields;
            private readonly IRepository<Area> _area;
            private readonly IRepository<InspectionPurposes> _purpose;
            private readonly IRepository<Users> _users;

            public AreaWisePendingInspectionReportController(IRepository<Schedule> scheduler,
                IRepository<AgencyStaffReqFields> agencyreqFields,
                IRepository<Area> area,
                IRepository<AreaWiseInspectors> areaWiseInspector,
                IRepository<Users> users,
                IRepository<InspectionPurposes> purpose)
            {
                  _scheduler = scheduler;
                  _areaWiseInspector = areaWiseInspector;
                  _agencyreqFields = agencyreqFields;
                  _area = area;
                  _users = users;
                  _purpose = purpose;

            }

            [HttpGet("/AreaWisePendingInspections/{code?}")]
            public async Task<IActionResult> AreaWisePendingInspections(string code)
            {
                  AreaWisePendingInspectionSearchVM model = new AreaWisePendingInspectionSearchVM();
                  var purposes = await _purpose.GetAllAsync();
                  var Areas = await _area.GetAllAsync();
                  var users = await _users.GetAllAsync();
                  model.PurposeList = purposes.Select(x => new SelectListItem
                  {
                        Text = x.Name!.ToString(),
                        Value = x.Name.ToString()
                  }).ToList();
                  model.AreaList = Areas.Select(x => new SelectListItem
                  {
                        Text = x.AreaNumber.ToString(),
                        Value = x.AreaNumber.ToString()
                  }).ToList();
                  model.AssignInspectorList = users.Select(x => new SelectListItem
                  {
                        Text = x.FirstName! + " " + x.LastName,
                        Value = x.Id.ToString()
                  }).ToList();
                  ViewBag.Code = code;
                  return View(model);
            }

            [HttpPost("/GetAllAreaWisePendingInspections/{code?}")]
            public async Task<IActionResult> GetAllAreaWisePendingInspections(AreaWisePendingInspectionSearchVM model, string code)
            {
                  IEnumerable<Schedule> ScheduleList = new List<Schedule>();

                  if (User.FindFirstValue(ClaimTypes.Role) == SD.Inspector)
                  {
                        ScheduleList = await _scheduler.GetAllAsync(filter: x => x.AssignedTo == Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)), orderBy: x => x.OrderByDescending(x => x.ScheduledDate), includeProperties: "Establishment,Purpose");
                  }

                  else
                  {
                        ScheduleList = await _scheduler.GetAllAsync(includeProperties: "Establishment,Purpose", orderBy: x => x.OrderByDescending(x => x.ScheduledDate));
                  }

                  var AreaWisePendingInspectionList = //from a in await _scheduler.GetAllAsync(filter: x => (x.StatusId == 1 || x.StatusId == 2) && (x.ScheduledDate.Date < DateTime.Now.Date) && (x.Establishment!.PermitNumber!.StartsWith(code)), includeProperties: "Establishment,Purpose")
                                                      from a in ScheduleList.Where(x => (x.StatusId == 1 || x.StatusId == 2) && (x.ScheduledDate.Date < DateTime.Now.Date) && (x.Establishment!.PermitNumber!.StartsWith(code)))
                                                      join u in await _users.GetAllAsync(filter: x => x.IsActive == true && x.IsDelete == false)
                                                      on a.AssignedTo equals u.Id into ugroup
                                                      from u in ugroup.DefaultIfEmpty()
                                                      select new
                                                      {
                                                            area = a.Establishment!.Area,
                                                            name = a.Establishment.Name,
                                                            permitNumber = a.Establishment.PermitNumber,
                                                            address = a.Establishment.Address,
                                                            purpose = a.Purpose!.Name,
                                                            scheduledDate = a.ScheduledDate.Date,
                                                            scheduledDateStr = a.ScheduledDate.Date.ToShortDateString(),
                                                            assignedTo = u?.FirstName + " " + u?.LastName,
                                                            assignedToValue = a.AssignedTo,
                                                            isFollowUpInspection = a.IsFollowUpSchedule
                                                      };

                  if (model.Name != null)
                  {
                        AreaWisePendingInspectionList = AreaWisePendingInspectionList.Where(x => x.name.ToLower().Contains(model.Name.ToLower()));
                  }
                  if (model.Permit != null)
                  {
                        AreaWisePendingInspectionList = AreaWisePendingInspectionList.Where(x => x.permitNumber != null && x.permitNumber.ToLower().Contains(model.Permit.ToLower()));
                  }

                  if (model.Address != null)
                  {
                        AreaWisePendingInspectionList = AreaWisePendingInspectionList.Where(x => x.address != null && x.address.ToLower().Contains(model.Address.ToLower()));
                  }

                  if (model.Purpose != null && model.Purpose != "--Select Purpose--")
                  {
                        AreaWisePendingInspectionList = AreaWisePendingInspectionList.Where(x => x.purpose != null && x.purpose.ToLower().Contains(model.Purpose.ToLower()));
                  }

                  if (model.Area != null)
                  {
                        AreaWisePendingInspectionList = AreaWisePendingInspectionList.Where(x => x.area == model.Area);
                  }

                  if (model.Inspector != null && model.Inspector != "--Select User--" && model.Inspector != "undefined")
                  {
                        AreaWisePendingInspectionList = AreaWisePendingInspectionList.Where(x => x.assignedToValue != 0 && x.assignedToValue == Convert.ToInt32(model.Inspector));
                  }

                  if (model.FromDate != null)
                  {

                        AreaWisePendingInspectionList = AreaWisePendingInspectionList.Where(x => x.scheduledDate.Date >= model.FromDate);
                  }

                  if (model.ToDate != null)
                  {
                        AreaWisePendingInspectionList = AreaWisePendingInspectionList.Where(x => x.scheduledDate.Date <= model.ToDate);
                  }

                  return Json(new { data = AreaWisePendingInspectionList.ToList() });
            }

      }
}
