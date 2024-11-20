using BrazosAPI.Extensions;
using BrazosAPI.Models;
using BrazosAPI.Models.DTOs;
using BrazosApp.DataAccess.Repository.IRepository;
using BrazosApp.Models;
using BrazosApp.Models.ViewModels;
using BrazosApp.Utility;
using BrazosApp.Utility.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;

namespace BrazosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SchedulerController : Controller
    {
        private readonly IRepository<Schedule> _schedule;
        private readonly IRepository<Users> _users;
        private readonly IRepository<Establishment> _establishment;
        private readonly IRepository<AgencyStaffReqFields> _agencyStaffReqFields;
        private readonly IRepository<AreaWiseInspectors> _areaWiseInspectors;
        private readonly IRepository<InspectionPurposes> _purposes;
        private readonly IEncrypt _encrypt;
        protected APIResponse _response;
        public SchedulerController(
            IRepository<Schedule> schedule,
            IRepository<Establishment> establishment,
            IRepository<Users> users,
            IRepository<AgencyStaffReqFields> agencyStaffReqFields,
            IRepository<AreaWiseInspectors> areaWiseInspectors,
            IRepository<InspectionPurposes> purposes,
            IEncrypt encrypt)
        {
            _response = new();
            _schedule = schedule;
            _establishment = establishment;
            _users = users;
            _agencyStaffReqFields = agencyStaffReqFields;
            _areaWiseInspectors = areaWiseInspectors;
            _purposes = purposes;
            _encrypt = encrypt;
        }

        [HttpPost("/GetAllScheduledInspections", Name = "GetAllScheduledInspections")]
        public async Task<ActionResult<APIResponse>> GetAllSchedule([FromForm] ScheduleModalDataRequestDTO model, string code, DateTime? SyncDate)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var str = TokenValidator.Validation(token);
            //var role = Request.Headers["Role"].ToString();
            
            //string code = "RF";

            if (str == "Authorized")
            {
                var userId = TokenValidator.GetUserId(token);
                var role = TokenValidator.GetUserRole(token);
                IEnumerable<Schedule> schedules = new List<Schedule>();
                if(role==SD.SuperAdmin|| role == SD.AdminInspector)
                {
                    //if(IsSyncFlag == true)
                    //{
                    //    schedules = await _schedule.GetAllAsync(filter: x => x.Establishment!.PermitNumber!.StartsWith(code) && x.StatusId <= 2 && x.IsAdhoc == false && x.IsSync == false, includeProperties: "Establishment,Purpose");
                    //}
                    //else
                    //{
                    //    schedules = await _schedule.GetAllAsync(filter: x => x.Establishment!.PermitNumber!.StartsWith(code) && x.StatusId <= 2 && x.IsAdhoc == false, includeProperties: "Establishment,Purpose");
                    //}

                    schedules = await _schedule.GetAllAsync(filter: x => x.Establishment!.PermitNumber!.StartsWith(code) && x.StatusId <= 2 && x.IsAdhoc == false && (SyncDate == null || /*x.SyncDate == null ||*/ x.SyncDate > SyncDate), includeProperties: "Establishment,Purpose", orderBy:x=>x.OrderByDescending(x=>x.ScheduledDate));

                    //if (SyncDate != null)
                    //{
                    //    schedules = await _schedule.GetAllAsync(filter: x => x.Establishment!.PermitNumber!.StartsWith(code) && x.StatusId <= 2 && x.IsAdhoc == false && x.SyncDate >= SyncDate, includeProperties: "Establishment,Purpose");
                    //}
                    //else
                    //{
                    //    schedules = await _schedule.GetAllAsync(filter: x => x.Establishment!.PermitNumber!.StartsWith(code) && x.StatusId <= 2 && x.IsAdhoc == false, includeProperties: "Establishment,Purpose");
                    //}

                }
                else if (role == SD.Inspector)
                {
                    //if (IsSyncFlag == true)
                    //{
                    //    schedules = await _schedule.GetAllAsync(filter: x => x.Establishment!.PermitNumber!.StartsWith(code) && x.StatusId <= 2 && x.IsAdhoc == false && x.AssignedTo == Convert.ToInt32(_encrypt.Decrypt256(userId)) && x.IsSync == false, includeProperties: "Establishment,Purpose");
                    //}
                    //else
                    //{
                    //    schedules = await _schedule.GetAllAsync(filter: x => x.Establishment!.PermitNumber!.StartsWith(code) && x.StatusId <= 2 && x.IsAdhoc == false && x.AssignedTo == Convert.ToInt32(_encrypt.Decrypt256(userId)), includeProperties: "Establishment,Purpose");
                    //}

                    schedules = await _schedule.GetAllAsync(filter: x => x.Establishment!.PermitNumber!.StartsWith(code) && x.StatusId <= 2 && x.IsAdhoc == false && x.AssignedTo == Convert.ToInt32(_encrypt.Decrypt256(userId)) && (SyncDate==null || x.SyncDate>SyncDate), includeProperties: "Establishment,Purpose", orderBy: x => x.OrderByDescending(x => x.ScheduledDate));

                    //if (SyncDate != null)
                    //{
                    //    schedules = await _schedule.GetAllAsync(filter: x => x.Establishment!.PermitNumber!.StartsWith(code) && x.StatusId <= 2 && x.IsAdhoc == false && x.AssignedTo == Convert.ToInt32(_encrypt.Decrypt256(userId)) && x.SyncDate >= SyncDate, includeProperties: "Establishment,Purpose");
                    //}
                    //else
                    //{
                    //    schedules = await _schedule.GetAllAsync(filter: x => x.Establishment!.PermitNumber!.StartsWith(code) && x.StatusId <= 2 && x.IsAdhoc == false && x.AssignedTo == Convert.ToInt32(_encrypt.Decrypt256(userId)), includeProperties: "Establishment,Purpose");
                    //}

                }

                //schedules = schedules.ToList().ForEach(x => { x.IsSync = true  });
                //foreach(var schedule in schedules)
                //{
                //    schedule.IsSync = true;
                //    schedule.SyncDate = DateTime.Now;
                //    await _schedule.UpdateAsync(schedule);
                //}

                var SchedulerList = from d in schedules/*await _schedule.GetAllAsync(filter: x => x.Establishment!.PermitNumber!.StartsWith(code) && x.StatusId <= 2 && x.AssignedTo == Convert.ToInt32(_encrypt.Decrypt256(userId)), includeProperties: "Establishment,Purpose")*/
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
                                        EncryptedId = _encrypt.Encrypt256(d.Id.ToString()),
                                        latestUpdatedDate = d.SyncDate
                                        //IsSync = d.IsSync,
                                        //SyncDate = d.SyncDate
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

                DateTime? LatestDt = new DateTime();

                if (SchedulerList.Any())
                {
                    LatestDt = SchedulerList.OrderByDescending(x => x.latestUpdatedDate!.Value).FirstOrDefault()!.latestUpdatedDate;
                }
                else
                {
                    LatestDt = null;
                }

                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Message = "Success";
                _response.Result = new { SchedulerList, LatestDate = LatestDt};
                return Json(new { Success = true, response = _response });
            }

            _response.StatusCode = HttpStatusCode.Unauthorized;
            _response.IsSuccess = false;
            _response.Message = "Access Denied!";
            _response.Result = "";
            //return Ok(_response);
            return Json(new { Success = false, response = _response });
        }

        //[HttpGet("/GetAllScheduledInspections", Name = "GetAllScheduledInspections")]
        //public async Task<ActionResult<APIResponse>> GetAllSchedule(string code, int TerritoryId)
        //{

        //}



        [HttpPost("/ScheduleAutoComplete", Name = "ScheduleAutoComplete")]
        public async Task<ActionResult<APIResponse>> AutoComplete([FromForm] string prefix, [FromForm] string selector)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var str = TokenValidator.Validation(token);
            var code = Request.Query["code"].ToString();
            //var role = Request.Headers["Role"].ToString();
            var sel = selector;

            if (str == "Authorized")
            {
                var userId = TokenValidator.GetUserId(token);
                var role = TokenValidator.GetUserRole(token);
                IEnumerable<AgencyStaffReqFields> agencyReqFieldList = new List<AgencyStaffReqFields>();
                var EstList = new List<AgencyStaffReqFields>();

                if (role == SD.SuperAdmin || role == SD.AdminInspector || role == SD.Inspector) 
                {
                        try
                        {
                              if(selector== "#estName")
                              {
                                     agencyReqFieldList = await _agencyStaffReqFields.GetAllAsync(filter: x => x.Establishment!.Name!.ToLower().StartsWith(prefix.ToLower()) && (x.Establishment!.PermitStatusId == 9 || x.Establishment!.PermitStatusId == 10 || x.Establishment!.PermitStatusId == 13) && x.Establishment!.PermitNumber!.StartsWith(code), includeProperties: "Establishment");
                              }
                              else if(selector == "#estPermit")
                              {
                                    agencyReqFieldList = await _agencyStaffReqFields.GetAllAsync(filter: x => x.Establishment!.PermitNumber!.ToLower().Contains(prefix.ToLower()) && (x.Establishment!.PermitStatusId == 9 || x.Establishment!.PermitStatusId == 10 || x.Establishment!.PermitStatusId == 13) && x.Establishment!.PermitNumber!.StartsWith(code), includeProperties: "Establishment");
                              }
                              else if(selector == "#estAddr")
                              {
                                    agencyReqFieldList = await _agencyStaffReqFields.GetAllAsync(filter: x => x.Establishment!.Address!.ToLower().Contains(prefix.ToLower()) && (x.Establishment!.PermitStatusId == 9 || x.Establishment!.PermitStatusId == 10 || x.Establishment!.PermitStatusId == 13) && x.Establishment!.PermitNumber!.StartsWith(code), includeProperties: "Establishment");
                              }
                        }
                        catch (Exception ex)
                        {

                        }
                        EstList.AddRange(agencyReqFieldList);
                }
                //else if(role == SD.Inspector)
                //{
                //        var AreaIdList = new List<AreaIDList>();
                //        var AreaWiseInspectorList = await _areaWiseInspectors.GetAllAsync(filter: x => x.AssignedUserId == Convert.ToInt32(_encrypt.Decrypt256(userId)));
                //        foreach (var area in AreaWiseInspectorList)
                //        {
                //              AreaIdList.Add(new AreaIDList
                //              {
                //                    Id = area.AreaId,
                //              });
                //        }

                        
                //        foreach (var area in AreaIdList)
                //        {
                //              agencyReqFieldList = new List<AgencyStaffReqFields>();
                //              try
                //              {
                //                    //agencyReqFieldList = await _agencyStaffReqFields.GetAllAsync(filter: x => x.AreaId == area.Id && x.Establishment!.Name!.StartsWith(prefix) && x.Establishment!.PermitStatusId == 9 && x.Establishment!.PermitNumber!.StartsWith(code), includeProperties: "Establishment");
                //                    if (selector == "#estName")
                //                    {
                //                          agencyReqFieldList = await _agencyStaffReqFields.GetAllAsync(filter: x => x.AreaId == area.Id && x.Establishment!.Name!.ToLower().StartsWith(prefix.ToLower()) && (x.Establishment!.PermitStatusId == 9 || x.Establishment!.PermitStatusId == 13) && x.Establishment!.PermitNumber!.StartsWith(code), includeProperties: "Establishment");
                //                    }
                //                    else if (selector == "#estPermit")
                //                    {
                //                          agencyReqFieldList = await _agencyStaffReqFields.GetAllAsync(filter: x => x.AreaId == area.Id && x.Establishment!.PermitNumber!.ToLower().Contains(prefix.ToLower()) && (x.Establishment!.PermitStatusId == 9 || x.Establishment!.PermitStatusId == 13) && x.Establishment!.PermitNumber!.StartsWith(code), includeProperties: "Establishment");
                //                    }
                //                    else if (selector == "#estAddr")
                //                    {
                //                          agencyReqFieldList = await _agencyStaffReqFields.GetAllAsync(filter: x => x.AreaId == area.Id && x.Establishment!.Address!.ToLower().Contains(prefix.ToLower()) && (x.Establishment!.PermitStatusId == 9 || x.Establishment!.PermitStatusId == 13) && x.Establishment!.PermitNumber!.StartsWith(code), includeProperties: "Establishment");
                //                    }

                //              }
                //              catch (Exception ex)
                //              {

                //              }
                //              EstList.AddRange(agencyReqFieldList);
                //              //foreach (var agency in agencyReqFieldList)
                //              //{
                //              //    EstList.Add(new Establishment
                //              //    {
                //              //        Id = agency.Establishment!.Id
                //              //    }); 
                //              //}
                //              //foreach (var agency in agencyReqFieldList)
                //              //{
                //              //      var establishment = await _establishment.GetFirstOrDefaultAsync(e => e.Id == agency.Establishment!.Id);

                //              //      if (establishment != null)
                //              //      {
                //              //            // Add the retrieved establishment to the EstList
                //              //            EstList.Add(establishment);
                //              //      }
                //              //}
                //        }
                //}
                
                IEnumerable<dynamic> estList = new List<dynamic>();

                  if(selector== "#estName")
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
                                   defaultInspectorId = _areaWiseInspectors.GetFirstOrDefault(filter: x => x.Area!.AreaNumber == picker.Establishment.Area, includeProperties: "Area").AssignedUserId
                               }).ToList();      
                  }
                  else if(selector == "#estPermit")
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
                  else if(selector == "#estAddr")
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
                

                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Message = "Success";
                _response.Result = estList;
                return Json(new { Success = true, response = _response });
                //return Json(estList);
            }
            _response.StatusCode = HttpStatusCode.Unauthorized;
            _response.IsSuccess = false;
            _response.Message = "Access Denied!";
            _response.Result = "";
            return Json(new { Success = false, response = _response });
        }


        [HttpPost("/ScheduleUpsert", Name = "ScheduleUpsert")]
        public async Task<ActionResult<APIResponse>> ScheduleUpsert(ScheduleModalDataRequestDTO model)
        {
            if (model == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.Message = string.Empty;
                return BadRequest(_response);
            }
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var str = TokenValidator.Validation(token);
            var code = Request.Query["code"].ToString();
            //var role = Request.Headers["Role"].ToString();

            if (str == "Authorized")
            {
                var userId = TokenValidator.GetUserId(token);
                var role = TokenValidator.GetUserRole(token);
                var msg = "";
                if (model.ScheduleId == 0)
                {
                    Schedule schedule = new Schedule();
                    schedule.EstablishmentId = model.EstId;
                    schedule.PurposeId = model.PurposeId;
                    schedule.ScheduledDate = model.ScheduledDate;
                    if(model.InspectorId == 0)
                    {
                        schedule.AssignedTo = Convert.ToInt32(_encrypt.Decrypt256(userId));
                    }
                    else
                    {
                        schedule.AssignedTo = model.InspectorId;
                    }
                    schedule.StatusId = 2;
                    schedule.CreatedBy = Convert.ToInt32(_encrypt.Decrypt256(userId));
                    schedule.CreatedOn = DateTime.Now;
                    schedule.SyncDate = DateTime.Now;
                    await _schedule.AddAsync(schedule);

                    msg = "Successfully Scheduled";
                }
                else
                {
                    var sch = await _schedule.GetFirstOrDefaultAsync(filter: x => x.Id == model.ScheduleId);
                    sch.ScheduledDate = model.ScheduledDate;
                    sch.PurposeId = model.PurposeId;
                    sch.UpdatedBy = Convert.ToInt32(_encrypt.Decrypt256(userId));
                    sch.UpdatedOn = DateTime.Now;
                    sch.SyncDate = DateTime.Now;
                    await _schedule.UpdateAsync(sch);

                    msg = "Information Successfully Updated";
                }

                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Message = msg;
                _response.Result = "";
                return Json(new { Success = true, response = _response });
            }
            _response.StatusCode = HttpStatusCode.Unauthorized;
            _response.IsSuccess = false;
            _response.Message = "Access Denied!";
            _response.Result = "";
            return Json(new { Success = false, response = _response });
        }


        [HttpGet("/LoadInspectorScheduleData", Name = "LoadInspectorScheduleData")]
        public async Task<ActionResult<APIResponse>> LoadInspectorScheduleData()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            //var role = Request.Headers["Role"].ToString();
            var str = TokenValidator.Validation(token);

            if (str == "Authorized")
            {
                var userId = TokenValidator.GetUserId(token);
                var role = TokenValidator.GetUserRole(token);
                var Purposes = await _purposes.GetAllAsync(filter: x => x.Code == "RF");
                var AllPurposes = await _purposes.GetAllAsync();
                ScheduleModalDataResponseDTO model = new ScheduleModalDataResponseDTO();
                model.Purposes = Purposes.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                }).ToList();
                model.SearchPurposeList = AllPurposes.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Name,
                }).ToList();
                if (role == SD.AdminInspector || role == SD.SuperAdmin)
                {
                    //var AreaIdList = new List<AreaIDList>();
                    //var AreaWiseInspectorList = await _areaWiseInspectors.GetAllAsync(filter: x => x.AssignedUserId == Convert.ToInt32(_encrypt.Decrypt256(userId)));
                    //foreach (var area in AreaWiseInspectorList)
                    //{
                    //    AreaIdList.Add(new AreaIDList
                    //    {
                    //        Id = area.AreaId,
                    //    });
                    //}

                    //var InspectorList = new List<AreaWiseInspectors>();
                    //foreach (var area in AreaIdList)
                    //{
                    //    var Inspectors = await _areaWiseInspectors.GetAllAsync(filter:x=>x.AreaId==area.Id, includeProperties: "AssignedUser");
                    //    if (Inspectors.Any())
                    //    {
                    //        InspectorList.AddRange(Inspectors);
                    //    }
                    //}

                    var InspectorList = await _users.GetAllAsync(filter:x=>x.Id== Convert.ToInt32(_encrypt.Decrypt256(userId)) || (x.Role!.Name!=SD.Clerk && x.Role!.Name!=SD.ViewOnly), includeProperties: "Role");

                    //model.AssignInspectorList = InspectorList.Select(x => new SelectListItem
                    //{
                    //    Text = x.AssignedUser!.FirstName + " " + x.AssignedUser.LastName,
                    //    Value = x.AssignedUserId.ToString(),
                    //}).ToList();
                    model.AssignInspectorList = InspectorList.Select(x => new SelectListItem
                    {
                        Text = x.FirstName + " " + x.LastName,
                        Value = x.Id.ToString(),
                    }).ToList();

                    //model.SearchParamsVM = new InspectionSearchParamsVM();
                    model.SearchAssignInspectorList = InspectorList.Select(x => new SelectListItem
                    {
                        Text = x.FirstName + " " + x.LastName,
                        Value = x.FirstName + " " + x.LastName,
                    }).ToList();

                }

                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Message = "Success";
                _response.Result = model;
                //return Json(new { Success = true, response = _response });
                return Ok(_response);
            }
            else
            {
                _response.StatusCode = HttpStatusCode.Unauthorized;
                _response.IsSuccess = false;
                _response.Message = "Access Denied!";
                _response.Result = "";
                return Ok(_response);
                //return Json(new { Success = false, response = _response });
            }
        }


        [HttpDelete("/DeleteSchedule", Name = "DeleteSchedule")]
        public async Task<ActionResult<APIResponse>> DeleteSchedule(string id)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var str = TokenValidator.Validation(token);
            if (str == "Authorized")
            {
                if (id == null) 
                {
                    //return Json(new { success = false }); 
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.Message = "Id Not Found";
                    _response.Result = "";
                    return Json(new { response = _response });
                }
                var Schedule = await _schedule.GetFirstOrDefaultAsync(filter: x => x.Id == Convert.ToInt32(_encrypt.Decrypt256(id)));
                if (Schedule == null)
                {
                    //return Json(new { success = false });
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    _response.Message = "Record Not Found";
                    _response.Result = "";
                    return Json(new { response = _response });
                }
                Schedule.StatusId = 5;
                Schedule.UpdatedBy = Convert.ToInt32(_encrypt.Decrypt256(TokenValidator.GetUserId(token)));
                Schedule.UpdatedOn = DateTime.Now;
                Schedule.SyncDate = DateTime.Now;
                await _schedule.UpdateAsync(Schedule);

                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Message = "Success";
                _response.Result = "";
                return Json(new { response = _response });
            }
            else
            {
                _response.StatusCode = HttpStatusCode.Unauthorized;
                _response.IsSuccess = false;
                _response.Message = "Unauthorized";
                _response.Result = "";
                return Json(new { response = _response });
            }
        }
    }
}
