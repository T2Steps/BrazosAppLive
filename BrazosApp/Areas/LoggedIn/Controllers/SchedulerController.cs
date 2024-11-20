using BrazosApp.DataAccess.Repository.IRepository;
using BrazosApp.Models;
using BrazosApp.Models.DTOs;
using BrazosApp.Models.ViewModels;
using BrazosApp.Utility;
using BrazosApp.Utility.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace BrazosApp.Areas.LoggedIn.Controllers
{
    [Area("LoggedIn")]
    [Authorize("CommonPolicy")]
    public class SchedulerController : Controller
    {
        private readonly IRepository<Schedule> _schedule;
        private readonly IRepository<Users> _users;
        private readonly IRepository<Establishment> _establishment;
        private readonly IRepository<AgencyStaffReqFields> _agencyStaffReqFields;
        private readonly IRepository<AreaWiseInspectors> _areaWiseInspectors;
        private readonly IRepository<InspectionPurposes> _purposes;
        private readonly IEncrypt _encrypt;

        public SchedulerController(IRepository<Schedule> schedule,
        IRepository<Establishment> establishment,
        IRepository<Users> users,
        IRepository<AgencyStaffReqFields> agencyStaffReqFields,
        IRepository<AreaWiseInspectors> areaWiseInspectors,
        IRepository<InspectionPurposes> purposes,
        IEncrypt encrypt)
        {
            _schedule = schedule;
            _establishment = establishment;
            _users = users;
            _agencyStaffReqFields = agencyStaffReqFields;
            _areaWiseInspectors = areaWiseInspectors;
            _purposes = purposes;
            _encrypt = encrypt;
        }

        [HttpGet("/Schedules/{code?}")]
        public async Task<IActionResult> Index(string code)
        {
            ViewBag.Code = code;
            IEnumerable<InspectionPurposes> Purposes = new List<InspectionPurposes>();
            Purposes = await _purposes.GetAllAsync(/*filter: x => x.Code == "RF"*/);
            
            ScheduleModalDataRequestDTO model = new ScheduleModalDataRequestDTO();
            if (code == "RF" || code == "MF")
            {
                  model.Purposes = Purposes.Where(x=> x.Code == "RF").Select(x => new SelectListItem
                  {
                      Text = x.Name,
                      Value = x.Id.ToString(),
                  }).ToList(); 
            }
            
            var InspectorList = await _users.GetAllAsync(filter: x => x.Id == Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)) || x.Role!.Name == SD.Inspector || x.Role!.Name == SD.AdminInspector || x.Role!.Name == SD.SuperAdmin, includeProperties: "Role");
            model.AssignInspectorList = InspectorList.Select(x => new SelectListItem
            {
                Text = x.FirstName + " " + x.LastName,
                Value = x.Id.ToString(),
            }).ToList();

            model.SearchParamsVM = new InspectionSearchParamsVM();
            model.SearchParamsVM.PurposeList = Purposes.Select(x => new SelectListItem
            {
                  Text = x.Name,
                  Value = x.Name,
            }).ToList();
            model.SearchParamsVM.AssignInspectorList = InspectorList.Select(x=> new SelectListItem
            {
                  Text = x.FirstName + " " + x.LastName,
                  Value = x.FirstName + " " + x.LastName,
            }).ToList();
            return View(model);
        }

        [HttpPost("/Schedules/GetAllSchedules/{code?}")]
        public async Task<IActionResult> AdminGetAllSchedules(ScheduleModalDataRequestDTO model , string code, DateTime? SyncDate)
        {
            //IEnumerable<Schedule> schedule;
            //if (SyncDate == null)
            //{
            //    schedule = await _schedule.GetAllAsync(filter: x => x.Establishment!.PermitNumber!.StartsWith(code) && x.StatusId <= 2 && x.IsAdhoc == false, includeProperties: "Establishment,Purpose");
            //}
            //else
            //{
            //    schedule = await _schedule.GetAllAsync(filter: x => x.Establishment!.PermitNumber!.StartsWith(code) && x.StatusId <= 2 && x.IsAdhoc == false , includeProperties: "Establishment,Purpose");
            //}
            IEnumerable<Schedule> schedule = new List<Schedule>(); ;
            if (User.FindFirstValue(ClaimTypes.Role) != SD.Inspector)
            {
                schedule = await _schedule.GetAllAsync(filter: x => x.Establishment!.PermitNumber!.StartsWith(code) && x.StatusId <= 2 && x.IsAdhoc == false, includeProperties: "Establishment,Purpose", orderBy: x => x.OrderByDescending(x => x.ScheduledDate));
            }
            else
            {
                schedule = await _schedule.GetAllAsync(filter: x => x.Establishment!.PermitNumber!.StartsWith(code) && x.StatusId <= 2 && x.IsAdhoc == false && x.AssignedTo==Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)), includeProperties: "Establishment,Purpose", orderBy: x => x.OrderByDescending(x => x.ScheduledDate));
            }

            //var schedule = 
            var SchedulerList = from d in schedule
                                join u in await _users.GetAllAsync()
                                on d.AssignedTo equals u.Id into egroup
                                join m in await _users.GetAllAsync()
                                on d.CreatedBy equals m.Id into fgroup
                                from u in egroup.DefaultIfEmpty()
                                from m in fgroup.DefaultIfEmpty()
                                orderby d.ScheduledDate descending
                                select new
                                {
                                    Id = d.Id,
                                    Permit = d.Establishment!.PermitNumber,
                                    Name = d.Establishment!.Name,
                                    Address = d.Establishment!.Address,
                                    ScheduledDate = d.ScheduledDate,
                                    Purpose = d.Purpose!.Name,
                                    AssignedTo = u == null ? "NA" : (u.FirstName + " " + u.LastName),
                                    StatusId = d.StatusId,
                                    EncryptedId = _encrypt.Encrypt256(d.Id.ToString())
                                    //IsInspected = d.StatusId == 4 ? "Yes" : "No",
                                    //CreatedBy = m.FirstName + " " + m.LastName,
                                };

                  if (model.SearchParamsVM!.Name != null)
                  {
                        SchedulerList = SchedulerList.Where(x => x.Name.ToLower().Contains(model.SearchParamsVM!.Name.ToLower()));
                  }
                  if (model.SearchParamsVM!.Permit != null)
                  {
                        SchedulerList = SchedulerList.Where(x => x.Permit != null && x.Permit.ToLower().Contains(model.SearchParamsVM!.Permit.ToLower()));
                  }

                  if (model.SearchParamsVM!.Address != null)
                  {
                    SchedulerList = SchedulerList.Where(x => x.Address != null && x.Address.ToLower().Contains(model.SearchParamsVM!.Address.ToLower()));
                  }

                  if (model.SearchParamsVM!.Purpose != null && model.SearchParamsVM!.Purpose != "--Select Purpose--")
                  {
                        SchedulerList = SchedulerList.Where(x => x.Purpose != null && x.Purpose.ToLower().Contains(model.SearchParamsVM!.Purpose!.ToLower()));
                  }

                  if (model.SearchParamsVM!.AssignedTo != null && model.SearchParamsVM!.AssignedTo != "--Select Assigned To--")
                  {
                        SchedulerList = SchedulerList.Where(x => x.AssignedTo != null && x.AssignedTo.ToLower().Contains(model.SearchParamsVM!.AssignedTo!.ToLower()));
                  }

                  if (model.SearchParamsVM!.FromDate != null)
                  {

                        SchedulerList = SchedulerList.Where(x => x.ScheduledDate >= model.SearchParamsVM!.FromDate);
                  }

                  if (model.SearchParamsVM!.ToDate != null)
                  {
                        SchedulerList = SchedulerList.Where(x => x.ScheduledDate <= model.SearchParamsVM!.ToDate);
                  }
                  return Json(new { data = SchedulerList.ToList() });
        }

        [HttpPost("/Schedules/ScheduleAutoComplete")]
        public async Task<JsonResult> AutoComplete(string? code, string prefix, string selector)
        {

            //var EstList = await _establishment.GetAllAsync(filter: x => (x.PermitStatusId == 9 || x.PermitStatusId == 13) && x.PermitNumber!.StartsWith(code) && (x.Name!.StartsWith(prefix)));
            //var estList = (from picker in EstList
            //               select new
            //               {
            //                   label = picker.Name,
            //                   val = picker.Id,
            //                   permit = picker.PermitNumber,
            //                   address = picker.Address,
            //                   state = picker.State,
            //                   city = picker.City,
            //                   zip = picker.Zip,
            //                   risk = picker.RiskCategory
            //               }).ToList();

            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var role = User.FindFirstValue(ClaimTypes.Role);
            IEnumerable<AgencyStaffReqFields> agencyReqFieldList = new List<AgencyStaffReqFields>();
            var EstList = new List<AgencyStaffReqFields>();
            if (role == SD.SuperAdmin || role == SD.AdminInspector || role == SD.Inspector)
            {
                try
                {
                    if (selector == "#estName")
                    {
                        agencyReqFieldList = await _agencyStaffReqFields.GetAllAsync(filter: x => x.Establishment!.Name!.ToLower().StartsWith(prefix.ToLower()) && (x.Establishment!.PermitStatusId == 9 || x.Establishment!.PermitStatusId == 10 || x.Establishment!.PermitStatusId == 13) && x.Establishment!.PermitNumber!.StartsWith(code), includeProperties: "Establishment");
                    }
                    else if (selector == "#estPermit")
                    {
                        agencyReqFieldList = await _agencyStaffReqFields.GetAllAsync(filter: x => x.Establishment!.PermitNumber!.ToLower().Contains(prefix.ToLower()) && (x.Establishment!.PermitStatusId == 9 || x.Establishment!.PermitStatusId == 10 || x.Establishment!.PermitStatusId == 13) && x.Establishment!.PermitNumber!.StartsWith(code), includeProperties: "Establishment");
                    }
                    else if (selector == "#estAddr")
                    {
                        agencyReqFieldList = await _agencyStaffReqFields.GetAllAsync(filter: x => x.Establishment!.Address!.ToLower().Contains(prefix.ToLower()) && (x.Establishment!.PermitStatusId == 9 || x.Establishment!.PermitStatusId == 10 || x.Establishment!.PermitStatusId == 13) && x.Establishment!.PermitNumber!.StartsWith(code), includeProperties: "Establishment");
                    }
                }
                catch (Exception ex)
                {

                }
                EstList.AddRange(agencyReqFieldList);
            }
            //else if (role == SD.Inspector)
            //{
            //    var AreaIdList = new List<AreaIDList>();
            //    var AreaWiseInspectorList = await _areaWiseInspectors.GetAllAsync(filter: x => x.AssignedUserId == userId);
            //    foreach (var area in AreaWiseInspectorList)
            //    {
            //        AreaIdList.Add(new AreaIDList
            //        {
            //            Id = area.AreaId,
            //        });
            //    }


            //    foreach (var area in AreaIdList)
            //    {
            //        agencyReqFieldList = new List<AgencyStaffReqFields>();
            //        try
            //        {
            //            //agencyReqFieldList = await _agencyStaffReqFields.GetAllAsync(filter: x => x.AreaId == area.Id && x.Establishment!.Name!.StartsWith(prefix) && x.Establishment!.PermitStatusId == 9 && x.Establishment!.PermitNumber!.StartsWith(code), includeProperties: "Establishment");
            //            if (selector == "#estName")
            //            {
            //                agencyReqFieldList = await _agencyStaffReqFields.GetAllAsync(filter: x => x.AreaId == area.Id && x.Establishment!.Name!.ToLower().StartsWith(prefix.ToLower()) && (x.Establishment!.PermitStatusId == 9 || x.Establishment!.PermitStatusId == 13) && x.Establishment!.PermitNumber!.StartsWith(code), includeProperties: "Establishment");
            //            }
            //            else if (selector == "#estPermit")
            //            {
            //                agencyReqFieldList = await _agencyStaffReqFields.GetAllAsync(filter: x => x.AreaId == area.Id && x.Establishment!.PermitNumber!.ToLower().Contains(prefix.ToLower()) && (x.Establishment!.PermitStatusId == 9 || x.Establishment!.PermitStatusId == 13) && x.Establishment!.PermitNumber!.StartsWith(code), includeProperties: "Establishment");
            //            }
            //            else if (selector == "#estAddr")
            //            {
            //                agencyReqFieldList = await _agencyStaffReqFields.GetAllAsync(filter: x => x.AreaId == area.Id && x.Establishment!.Address!.ToLower().Contains(prefix.ToLower()) && (x.Establishment!.PermitStatusId == 9 || x.Establishment!.PermitStatusId == 13) && x.Establishment!.PermitNumber!.StartsWith(code), includeProperties: "Establishment");
            //            }

            //        }
            //        catch (Exception ex)
            //        {

            //        }
            //        EstList.AddRange(agencyReqFieldList);
            //        //foreach (var agency in agencyReqFieldList)
            //        //{
            //        //    EstList.Add(new Establishment
            //        //    {
            //        //        Id = agency.Establishment!.Id
            //        //    }); 
            //        //}
            //        //foreach (var agency in agencyReqFieldList)
            //        //{
            //        //      var establishment = await _establishment.GetFirstOrDefaultAsync(e => e.Id == agency.Establishment!.Id);

            //        //      if (establishment != null)
            //        //      {
            //        //            // Add the retrieved establishment to the EstList
            //        //            EstList.Add(establishment);
            //        //      }
            //        //}
            //    }
            //}

            IEnumerable<dynamic> estList = new List<dynamic>();

            if (selector == "#estName")
            {
                estList = (from picker in EstList
                           select new
                           {
                               label = picker.Establishment!.Name,
                               val = picker.Establishment!.Id,
                               permit = picker.Establishment!.PermitNumber,
                               address = picker.Establishment!.Address,
                               state = picker.Establishment!.State,
                               city = picker.Establishment!.City,
                               zip = picker.Establishment!.Zip,
                               risk = picker.RiskCategoryId,
                               area = picker.Establishment!.Area,
                               defaultInspectorId = _areaWiseInspectors.GetFirstOrDefault(filter:x=>x.Area!.AreaNumber == picker.Establishment.Area, includeProperties: "Area").AssignedUserId
                           }).ToList();
            }
            else if (selector == "#estPermit")
            {
                estList = (from picker in EstList
                           select new
                           {
                               label = picker.Establishment!.PermitNumber,
                               val = picker.Establishment!.Id,
                               //permit = picker.Establishment!.PermitNumber,
                               name = picker.Establishment!.Name,
                               address = picker.Establishment!.Address,
                               state = picker.Establishment!.State,
                               city = picker.Establishment!.City,
                               zip = picker.Establishment!.Zip,
                               risk = picker.RiskCategoryId,
                               area = picker.Establishment!.Area,
                               defaultInspectorId = _areaWiseInspectors.GetFirstOrDefault(filter: x => x.Area!.AreaNumber == picker.Establishment.Area, includeProperties: "Area").AssignedUserId
                           }).ToList();
            }
            else if (selector == "#estAddr")
            {
                estList = (from picker in EstList
                           select new
                           {
                               label = picker.Establishment!.Address,
                               val = picker.Establishment!.Id,
                               name = picker.Establishment!.Name,
                               permit = picker.Establishment!.PermitNumber,
                               //address = picker.Establishment!.Address,
                               state = picker.Establishment!.State,
                               city = picker.Establishment!.City,
                               zip = picker.Establishment!.Zip,
                               risk = picker.RiskCategoryId,
                               area = picker.Establishment!.Area,
                               defaultInspectorId = _areaWiseInspectors.GetFirstOrDefault(filter: x => x.Area!.AreaNumber == picker.Establishment.Area, includeProperties: "Area").AssignedUserId
                           }).ToList();
            }

            return Json(estList);
        }

        [HttpPost("/Schedules/ScheduleUpsert")]
        public async Task<IActionResult> ScheduleUpsert(ScheduleModalDataRequestDTO model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            var msg = "";
            if (model.ScheduleId == 0)
            {
                Schedule schedule = new Schedule();
                schedule.EstablishmentId = model.EstId;
                schedule.PurposeId = model.PurposeId;
                schedule.ScheduledDate = model.ScheduledDate;
                if (model.InspectorId == 0)
                {
                    schedule.AssignedTo = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                }
                else
                {
                    schedule.AssignedTo = model.InspectorId;
                }
                schedule.StatusId = 2;
                schedule.CreatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                schedule.SyncDate = DateTime.Now;
                //try
                //{
                //    //var ScheduleFromDb = await _schedule.GetAllAsync(filter: x => x.EstablishmentId == model.Schedule.EstablishmentId && x.CreatedBy == model.Schedule.CreatedBy && ((model.Schedule.CreatedOn - x.CreatedOn).Minutes) < 1);
                //    var scheduleFromDb = await _schedule.GetAllAsync(filter: x => x.EstablishmentId == schedule.EstablishmentId);
                //    if (!scheduleFromDb.Any())
                //    {
                //        await _schedule.AddAsync(schedule);
                //        return Json(new { success = true, msg = "Successfully Scheduled" });
                //    }
                //    else
                //    {
                //        var scheduleLastFromDb = scheduleFromDb.Where(x => x.CreatedBy == schedule.CreatedBy).ToList().OrderByDescending(x => x.CreatedOn).FirstOrDefault();

                //        if ((schedule.CreatedOn - scheduleLastFromDb!.CreatedOn).Seconds > 9)
                //        {
                //            await _schedule.AddAsync(schedule);
                //            return Json(new { success = true, msg = "Successfully Scheduled" });
                //        }
                //        else
                //        {
                //            return Json(new { success = true });
                //        }
                //    }
                //}
                //catch (Exception ex)
                //{
                //    return Json(new { success = true });
                //}
                await _schedule.AddAsync(schedule);

                msg = "Successfully Scheduled";
            }
            else
            {
                var sch = await _schedule.GetFirstOrDefaultAsync(filter: x => x.Id == model.ScheduleId);
                sch.ScheduledDate = model.ScheduledDate;
                sch.PurposeId = model.PurposeId;
                sch.AssignedTo = model.InspectorId;
                sch.UpdatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                sch.UpdatedOn = DateTime.Now;
                sch.SyncDate = DateTime.Now;
                await _schedule.UpdateAsync(sch);

                msg = "Information Successfully Updated";
            }

            return Json(new {success = true, msg = msg});   
        }

        [HttpGet("/Schedules/GetSchedule/{id?}")]
        public async Task<IActionResult> GetSchedule(string id)
        {
            if (id == null) { return Json(new { success = false, msg = "Unexpected Error Occurred" }); }
            var Schedule = await _schedule.GetFirstOrDefaultAsync(filter: x => x.Id == Convert.ToInt32(_encrypt.Decrypt256(id)), includeProperties: "Establishment");
            if (Schedule == null) { return Json(new { success = false, msg = "Not Found" }); }
            else
            {
                return Json(new { success = true, schedule = Schedule });
            }
        }

        [HttpDelete("/Schedules/DeleteSchedule/{id?}")]
        public async Task<IActionResult> DeleteSchedule(string id)
        {
            if (id == null) { return Json(new { success = false }); }
            var Schedule = await _schedule.GetFirstOrDefaultAsync(filter: x => x.Id == Convert.ToInt32(_encrypt.Decrypt256(id)));
            if (Schedule == null)
            {
                return Json(new { success = false });
            }
            Schedule.StatusId = 5;
            Schedule.UpdatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            Schedule.UpdatedOn = DateTime.Now;
            Schedule.SyncDate = DateTime.Now;
            await _schedule.UpdateAsync(Schedule);
            return Json(new { success = true });
        }
    }
}
