using BrazosAPI.Extensions;
using BrazosAPI.Models;
using BrazosAPI.Models.DTOs;
using BrazosApp.DataAccess.Repository.IRepository;
using BrazosApp.Models;
using BrazosApp.Models.APIDTOs;
using BrazosApp.Models.ViewModels;
using BrazosApp.Utility;
using BrazosApp.Utility.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;
using System.Security.Claims;
using static System.Collections.Specialized.BitVector32;
using Section = BrazosApp.Models.Section;

namespace BrazosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InspectionController : Controller
    {
        private readonly IRepository<Section> _section;
        private readonly IRepository<SubSection> _subSection;
        private readonly IRepository<Item> _item;
        private readonly IRepository<Establishment> _establishments;
        private readonly IRepository<Inspection> _inspection;
        private readonly IRepository<InspectionItemDetails> _inspectionItems;
        private readonly IRepository<RFMFInspectionData> _rfmfInspectionData;
        //private readonly IRepository<OpeningInspectionData> _openinginsData;
        private readonly IRepository<Schedule> _scheduler;
        private readonly IRepository<Users> _users;
        private readonly IRepository<Area> _area;
        private readonly IRepository<AreaWiseInspectors> _areaWiseInspector;
        private readonly IRepository<AgencyStaffReqFields> _agencyreqFields;
        private readonly IRepository<InspectionPurposes> _purpose;
        private readonly IRepository<TemperatureObservation> _temperatureObs;
        private readonly IRepository<EstablishmentOwner> _owner;
        private readonly IEncrypt _encrypt;
        private readonly APIResponse _response;

        public InspectionController(IRepository<Item> item,
                IRepository<Section> section,
                IRepository<SubSection> subSection,
                IRepository<Establishment> establishments,
                IRepository<EstablishmentOwner> owner,
                IRepository<Inspection> inspection,
                IRepository<InspectionItemDetails> inspectionItems,
                IRepository<RFMFInspectionData> rfmfInspectionData,
                //IRepository<OpeningInspectionData> openinginsData,
                IRepository<Schedule> scheduler,
                IRepository<Users> users,
                IRepository<AgencyStaffReqFields> agencyreqFields,
                IRepository<AreaWiseInspectors> areaWiseInspector,
                IRepository<TemperatureObservation> temperatureObs,
                IRepository<InspectionPurposes> purpose,
                IEncrypt encrypt,
                IRepository<Area> area)
            {
                  _item = item;
                  _section = section;
                  _subSection = subSection;
                  _establishments = establishments;
                  _owner = owner;
                  _inspectionItems = inspectionItems;
                  _rfmfInspectionData = rfmfInspectionData;
                  _inspection = inspection;
                  _scheduler = scheduler;
                  _users = users;
                  _agencyreqFields = agencyreqFields;
                  _areaWiseInspector = areaWiseInspector;
                  _temperatureObs = temperatureObs;
                  _purpose = purpose;
                  _encrypt = encrypt;
                  _response = new APIResponse();
                  _area = area;
        }

        [HttpGet("/GetSecondInsSignFile", Name = "GetSecondInsSignFile")]
        public async Task<ActionResult<APIResponse>> GetSecondInsSignFile(string id)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var str = TokenValidator.Validation(token);
            if (str == "Authorized")
            {
                var user = await _users.GetFirstOrDefaultAsync(filter: x => x.Id == Convert.ToInt32(id));
                if (user == null)
                {
                    return NotFound();
                }
                else
                {
                    var isSignFilePresent = user.SignFileName != null ? "Present" : "Absent";
                    //return Json(new { success = true, isSignFilePresent = isSignFilePresent, signFile = user.SignFileName });

                    _response.StatusCode = HttpStatusCode.OK;
                    _response.IsSuccess = true;
                    _response.Message = "Success";
                    _response.Result = new { isSignFilePresent = isSignFilePresent, signFile = user.SignFileName };
                    return Json(new { response = _response });
                }
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

        [HttpPost("/GetAllInspections", Name = "GetAllInspections")]
        public async Task<ActionResult<APIResponse>> GetAllInspections([FromForm] InspectionSearchParamsVM model, string code)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            //var role = Request.Headers["Role"].ToString();
            
            var str = TokenValidator.Validation(token);

            if (str == "Authorized")
            {
                var userId = TokenValidator.GetUserId(token);
                var role = TokenValidator.GetUserRole(token);
                //var est = await _establishments.GetAllAsync(filter: x => x.PermitNumber!.StartsWith(code));
                //var est = await _establishments.GetAllAsync(filter: x => x.ApplicationFor!.Code == code, includeProperties: "ApplicationFor");

                IEnumerable<Inspection> InsList = new List<Inspection>();

                if (role==SD.SuperAdmin|| role==SD.AdminInspector)
                {
                    InsList = await _inspection.GetAllAsync(orderBy:x=>x.OrderByDescending(x=>x.InspectionDate));
                }
                else if(role == SD.Inspector)
                {
                    InsList = await _inspection.GetAllAsync(filter: x => x.InspectedBy == Convert.ToInt32(_encrypt.Decrypt256(userId)), orderBy: x => x.OrderByDescending(x => x.InspectionDate));
                }

                var InspectionList = from d in InsList
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
                                         InspectedBy = i.FirstName + " " + i.LastName,
                                         Purpose = p.Name,
                                         FollowUp = d.FollowUp,
                                         FollowUpDate = d.FollowUpDate,
                                         Score = insData?.Score,
                                         isPermitSuspended = d.IsPermitSuspended,
                                         EncryptedId = _encrypt.Encrypt256(d.Id.ToString())
                                     };

                var InspectionFinalList = InspectionList.Where(x => x.Permit.Contains(code));

                if (model.Name != null)
                {
                    InspectionFinalList = InspectionFinalList.Where(x => x.Name.ToLower().Contains(model.Name.ToLower()));
                }
                if (model.Permit != null)
                {
                    InspectionFinalList = InspectionFinalList.Where(x => x.Permit != null && x.Permit.ToLower().Contains(model.Permit.ToLower()));
                }

                if (model.Purpose != null && model.Purpose != "--Select Purpose--")
                {
                    InspectionFinalList = InspectionFinalList.Where(x => x.Purpose != null && x.Purpose.ToLower()/*Contains(model.Purpose.ToLower())*/ == model.Purpose.ToLower());
                }

                if (model.AssignedTo != null && model.AssignedTo != "--Select Inspected By--" && model.AssignedTo!="undefined")
                {
                    InspectionFinalList = InspectionFinalList.Where(x => x.InspectedBy != null && x.InspectedBy.ToLower()/*Contains(model.AssignedTo.ToLower())*/ == model.AssignedTo.ToLower());
                }

                if (model.FollowUp != null && model.FollowUp != "--Follow Up--")
                {
                  if (model.FollowUp == "Yes")
                  {
                        InspectionFinalList = InspectionFinalList.Where(x => x.FollowUp == true);
                  }
                  else if (model.FollowUp == "No")
                  {
                        InspectionFinalList = InspectionFinalList.Where(x => x.FollowUp == false);
                  }
                }

                if (model.FromDate != null)
                {

                    InspectionFinalList = InspectionFinalList.Where(x => x.InspectionDate >= model.FromDate);
                }

                if (model.ToDate != null)
                {
                    InspectionFinalList = InspectionFinalList.Where(x => x.InspectionDate <= model.ToDate);
                }
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Message = "Success";
                _response.Result = InspectionFinalList;
                return Json(new { Success = true, response = _response });
            }

            _response.StatusCode = HttpStatusCode.Unauthorized;
            _response.IsSuccess = false;
            _response.Message = "Access Denied!";
            _response.Result = "";
            //return Ok(_response);
            return Json(new { Success = false, response = _response });
        }

        [HttpGet("/LoadInspectionIndexSearchData", Name = "LoadInspectioIndexSearchnData")]
        public async Task<ActionResult<APIResponse>> LoadInspectionIndexSearchData()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var str = TokenValidator.Validation(token);

            if (str == "Authorized")
            {
                var userId = TokenValidator.GetUserId(token);
                InspectionSearchParamsDTO model = new InspectionSearchParamsDTO();
                var purposes = await _purpose.GetAllAsync();
                model.PurposeList = purposes.Select(x => new SelectListItem
                {
                    Text = x.Name!.ToString(),
                    Value = x.Name.ToString()
                }).ToList();

                var inspectors = await _users.GetAllAsync(filter:x=>x.Role!.Name==SD.SuperAdmin|| x.Role.Name==SD.Inspector|| x.Role.Name==SD.AdminInspector, includeProperties:"Role");
                model.AssignInspectorList = inspectors.Select(x => new SelectListItem
                {
                    Text = x.FirstName + " " + x.LastName,
                    Value = x.FirstName + " " + x.LastName,
                }).ToList();

                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Message = "Success";
                _response.Result = model;
                return Ok(_response);
            }
            _response.StatusCode = HttpStatusCode.Unauthorized;
            _response.IsSuccess = false;
            _response.Message = "Access Denied!";
            _response.Result = "";
            return Ok(_response);

        }


        [HttpGet("/LoadInspectionData", Name = "LoadInspectionData")]
        public async Task<ActionResult<APIResponse>> LoadInspectionData(string id)
        {
            if (id == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.Message = string.Empty;
                return BadRequest(_response);
            }
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var str = TokenValidator.Validation(token);

            if (str == "Authorized")
            {
                var userId = TokenValidator.GetUserId(token);
                InspectionResponseDTO model = new InspectionResponseDTO();
                if (id != "Null")
                {
                    var schedule = await _scheduler.GetFirstOrDefaultAsync(filter: x => x.Id == Convert.ToInt32(_encrypt.Decrypt256(id)), includeProperties: "Establishment");
                    var agencyReq = await _agencyreqFields.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == schedule.EstablishmentId);
                    model.SId = schedule.Id;
                    model.EstablishmentId = schedule.EstablishmentId;
                    model.PurposeId = schedule.PurposeId;
                    model.RiskId = agencyReq.RiskCategoryId;
                    model.EstName = schedule.Establishment!.Name;
                    model.Permit = schedule.Establishment!.PermitNumber;
                    model.Address = schedule.Establishment!.Address + ", " + schedule.Establishment!.State + ", " + schedule.Establishment!.City + ", " + schedule.Establishment!.Zip;
                    if (schedule.Establishment!.PermitNumber!.StartsWith("RF"))
                    {

                        model.Code = "RF";
                    }
                    else if (schedule.Establishment!.PermitNumber!.StartsWith("MF"))
                    {
                        //model.Items = await _item.GetAllAsync(filter: x => x.IsActive == true && x.IsDeleted == false && x.CodeId == 3);
                        model.Code = "MF";
                    }
                }

                var user = await _users.GetFirstOrDefaultAsync(filter: x => x.Id == Convert.ToInt32(_encrypt.Decrypt256(userId)));


                model.InspectedBy = user.FirstName + " " + user.LastName;
                model.InspectedBySignFileName = user.SignFileName;
                model.InspectorEmailId = user.EmailId;
                model.Items = await _item.GetAllAsync(filter: x => x.IsActive == true && x.IsDeleted == false && x.CodeId == 3);
                model.Sections = await _section.GetAllAsync(filter: x => x.IsActive == true && x.IsDeleted == false);
                model.SubSections = await _subSection.GetAllAsync(filter: x => x.IsActive == true && x.IsDeleted == false);


                model.InTime = DateTime.Now;
                TimeZoneInfo centralZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
                model.TimeIn = TimeZoneInfo.ConvertTime(model.InTime, centralZone).ToShortTimeString();
                model.FinalItemList = new List<Item>();
                model.FinalTemperatureList = new List<TemperatureObservation>();

                if (model.Items != null && model.Items.Count() > 0)
                {
                    foreach (var item in model.Items)
                    {
                        var result = model.Details?.FirstOrDefault(rel => rel.ItemId == item.Id);
                        model.FinalItemList.Add(new Item
                        {
                            Id = item.Id,
                            Name = item.Name,
                            Points = item.Points,
                            IsNA = item.IsNA,
                            IsNO = item.IsNO,
                            SubSectionId = item.SubSectionId,
                            CDI = result?.Cos,
                            IsRepeat = result?.R,
                            Status = result?.Status,
                            Image = result?.Image,
                            Comments = result?.Comment
                        });
                    }
                }
                if (model.TemperatureObs != null && model.TemperatureObs.Count() > 0)
                {
                    foreach (var item in model.TemperatureObs)
                    {
                        var result = model.TemperatureObs?.FirstOrDefault();
                        model.FinalTemperatureList!.Add(new TemperatureObservation
                        {
                            InspectionId = item.InspectionId,
                            Temperature = item.Temperature,
                            ItemName = item.ItemName
                        });
                    }
                }

                var users = await _users.GetAllAsync(filter: x => x.IsActive == true && x.IsDelete == false && (x.Role!.Name == SD.SuperAdmin || x.Role.Name == SD.AdminInspector || x.Role.Name == SD.Inspector) && x.Id != Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)), includeProperties: "Role");
                model.UserList = users.Select(x => new SelectListItem
                {
                    Text = (x.FirstName + " " + x.LastName).ToString(),
                    Value = x.Id.ToString()
                }).ToList();

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

        [HttpGet("/LoadInspectionEditData", Name = "LoadInspectionEditData")]
        public async Task<ActionResult<APIResponse>> LoadInspectionEditData(string id)
        {
            if (id == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.Message = string.Empty;
                return BadRequest(_response);
            }
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var str = TokenValidator.Validation(token);

            if (str == "Authorized")
            {
                var userId = TokenValidator.GetUserId(token);
                InspectionEditDTO model = new InspectionEditDTO();
                var InspectionId = Convert.ToInt32(_encrypt.Decrypt256(id));
                model.Inspection = await _inspection.GetFirstOrDefaultAsync(filter: x => x.Id == InspectionId, includeProperties: "Establishment");
                var user = await _users.GetFirstOrDefaultAsync(filter: x => x.Id == model.Inspection.InspectedBy);
                model.InspectedBy = user.FirstName + " " + user.LastName;
                model.InspectionData = await _rfmfInspectionData.GetFirstOrDefaultAsync(filter: x => x.InspectionId == InspectionId);
                if (model.InspectionData.CFMExpiryDate == null)
                {
                    model.InspectionData.CFMExpiryDate = Convert.ToDateTime("01/01/0001");
                }
                if (model.InspectionData == null)
                {
                    model.InspectionData = new RFMFInspectionData();
                }
                if (model.Inspection.Establishment!.PermitNumber!.StartsWith("RF"))
                {
                    model.Code = "RF";
                }
                else if (model.Inspection.Establishment!.PermitNumber!.StartsWith("MF"))
                {
                    model.Code = "MF";
                }
                TimeZoneInfo centralZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
                //model.TimeIn = TimeZoneInfo.ConvertTime(model.Inspection.TimeIn, centralZone).ToShortTimeString();
                //model.TimeOut = TimeZoneInfo.ConvertTime(model.Inspection.TimeOut, centralZone).ToShortTimeString();
                if (model.Inspection.UpdatedOn == null)
                {
                    model.TimeIn = TimeZoneInfo.ConvertTime(model.Inspection.TimeIn, centralZone).ToString("HH:mm");
                    model.TimeOut = TimeZoneInfo.ConvertTime(model.Inspection.TimeOut, centralZone).ToString("HH:mm");
                }
                else
                {
                    model.TimeIn = model.Inspection.TimeIn.ToString("HH:mm");
                    model.TimeOut = model.Inspection.TimeOut.ToString("HH:mm");
                }
                
                var purposes = await _purpose.GetAllAsync(filter: x => x.Code == "RF");
                model.Purposes = purposes.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                }).ToList();
                model.Items = await _item.GetAllAsync(filter: x => x.IsActive == true && x.IsDeleted == false && x.CodeId == 3);
                model.Sections = await _section.GetAllAsync(filter: x => x.IsActive == true && x.IsDeleted == false);
                model.Subsections = await _subSection.GetAllAsync(filter: x => x.IsActive == true && x.IsDeleted == false);
                model.Details = await _inspectionItems.GetAllAsync(filter: x => x.InspectionId == InspectionId);
                model.FinalItemList = new List<Item>();
                //model.FinalTemperatureList = new List<TemperatureObservation>();

                if (model.Items != null && model.Items.Count() > 0)
                {
                    foreach (var item in model.Items)
                    {
                        var result = model.Details?.FirstOrDefault(rel => rel.ItemId == item.Id);
                        model.FinalItemList.Add(new Item
                        {
                            Id = item.Id,
                            Name = item.Name,
                            Points = item.Points,
                            IsNA = item.IsNA,
                            IsNO = item.IsNO,
                            SubSectionId = item.SubSectionId,
                            CDI = result?.Cos,
                            IsRepeat = result?.R,
                            Status = result?.Status,
                            Image = result?.Image,
                            Comments = result?.Comment
                        });
                    }
                }
                model.FinalTemperatureList = new List<TemperatureObservation>();
                model.TemperatureObservations = await _temperatureObs.GetAllAsync(filter: x => x.InspectionId == InspectionId);
                if (model.TemperatureObservations != null && model.TemperatureObservations.Count() > 0)
                {
                    foreach (var item in model.TemperatureObservations)
                    {
                        var result = model.TemperatureObservations?.FirstOrDefault();
                        model.FinalTemperatureList!.Add(new TemperatureObservation
                        {
                            InspectionId = item.InspectionId,
                            Temperature = item.Temperature,
                            ItemName = item.ItemName
                        });
                    }
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

        //[HttpGet("/LoadNewInspectionData", Name = "LoadNewInspectionData")]
        //public async Task<ActionResult<APIResponse>> LoadNewInspectionData()
        //{
        //      var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        //      var str = TokenValidator.Validation(token);

        //      if (str == "Authorized")
        //      {
        //            var userId = TokenValidator.GetUserId(token);
        //            InspectionResponseDTO model = new InspectionResponseDTO();
        //            //var schedule = await _scheduler.GetFirstOrDefaultAsync(filter: x => x.Id == Convert.ToInt32(_encrypt.Decrypt256(id)), includeProperties: "Establishment");
        //            //var agencyReq = await _agencyreqFields.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == schedule.EstablishmentId);
        //            var user = await _users.GetFirstOrDefaultAsync(filter: x => x.Id == Convert.ToInt32(_encrypt.Decrypt256(userId)));

        //            //model.SId = schedule.Id;
        //            //model.EstablishmentId = schedule.EstablishmentId;
        //            //model.PurposeId = schedule.PurposeId;
        //            //model.RiskId = agencyReq.RiskCategoryId;
        //            //model.EstName = schedule.Establishment!.Name;
        //            //model.Permit = schedule.Establishment!.PermitNumber;
        //            //model.Address = schedule.Establishment!.Address + ", " + schedule.Establishment!.State + ", " + schedule.Establishment!.City + ", " + schedule.Establishment!.Zip;
        //            model.InspectedBy = user.FirstName + " " + user.LastName;
        //            model.InspectedBySignFileName = user.SignFileName;
        //            model.InspectorEmailId = user.EmailId;
        //            model.Items = await _item.GetAllAsync(filter: x => x.IsActive == true && x.IsDeleted == false && x.CodeId == 3);
        //            model.Sections = await _section.GetAllAsync(filter: x => x.IsActive == true && x.IsDeleted == false);
        //            model.SubSections = await _subSection.GetAllAsync(filter: x => x.IsActive == true && x.IsDeleted == false);
        //            //if (schedule.Establishment!.PermitNumber!.StartsWith("RF"))
        //            //{

        //            //      model.Code = "RF";
        //            //}
        //            //else if (schedule.Establishment!.PermitNumber!.StartsWith("MF"))
        //            //{
        //            //      //model.Items = await _item.GetAllAsync(filter: x => x.IsActive == true && x.IsDeleted == false && x.CodeId == 3);
        //            //      model.Code = "MF";
        //            //}

        //            model.InTime = DateTime.Now;
        //            model.FinalItemList = new List<Item>();
        //            model.FinalTemperatureList = new List<TemperatureObservation>();

        //            if (model.Items != null && model.Items.Count() > 0)
        //            {
        //                  foreach (var item in model.Items)
        //                  {
        //                        var result = model.Details?.FirstOrDefault(rel => rel.ItemId == item.Id);
        //                        model.FinalItemList.Add(new Item
        //                        {
        //                              Id = item.Id,
        //                              Name = item.Name,
        //                              Points = item.Points,
        //                              IsNA = item.IsNA,
        //                              IsNO = item.IsNO,
        //                              SubSectionId = item.SubSectionId,
        //                              CDI = result?.Cos,
        //                              IsRepeat = result?.R,
        //                              Status = result?.Status,
        //                              Image = result?.Image,
        //                              Comments = result?.Comment
        //                        });
        //                  }
        //            }
        //            if (model.TemperatureObs != null && model.TemperatureObs.Count() > 0)
        //            {
        //                  foreach (var item in model.TemperatureObs)
        //                  {
        //                        var result = model.TemperatureObs?.FirstOrDefault();
        //                        model.FinalTemperatureList!.Add(new TemperatureObservation
        //                        {
        //                              InspectionId = item.InspectionId,
        //                              Temperature = item.Temperature,
        //                              ItemName = item.ItemName
        //                        });
        //                  }
        //            }

        //            _response.StatusCode = HttpStatusCode.OK;
        //            _response.IsSuccess = true;
        //            _response.Message = "Success";
        //            _response.Result = model;
        //            //return Json(new { Success = true, response = _response });
        //            return Ok(_response);
        //      }
        //      else
        //      {
        //            _response.StatusCode = HttpStatusCode.Unauthorized;
        //            _response.IsSuccess = false;
        //            _response.Message = "Access Denied!";
        //            _response.Result = "";
        //            return Ok(_response);
        //            //return Json(new { Success = false, response = _response });
        //      }


        //}


        [HttpPost("/SaveInspectionEdit", Name = "Save_Inspection_Edit_Data")]
        public async Task<ActionResult<APIResponse>> SaveInspectionEdit([FromForm] BrazosApp.Models.ViewModels.InspectionRequestDTO model)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var str = TokenValidator.Validation(token);
            if (str == "Authorized")
            {
                if (model == null)
                {
                    return BadRequest();
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        var inspection = await _inspection.GetFirstOrDefaultAsync(filter: x => x.Id == model.InspectionId);
                        var isOldInspectionPermitSuspended = inspection.IsPermitSuspended;
                        inspection.InspectionDate = model.InspectionDate;
                        inspection.PurposeId = model.PurposeId;
                        inspection.TimeIn = model.TimeIn;
                        inspection.TimeOut = model.TimeOut;
                        
                        inspection.IsPermitSuspended = model.PermitSuspend;
                        
                        if (inspection.FollowUp != true && model.FollowUp == true)
                        {
                            inspection.FollowUpDate = model.FollowUpDate ?? DateTime.Now.AddDays(7);
                            Schedule sch = new Schedule();
                            sch.EstablishmentId = inspection.EstablishmentId;
                            sch.PurposeId = model.PurposeId;
                            sch.ScheduledDate = model.FollowUpDate ?? DateTime.Now.AddDays(7);
                            sch.IsFollowUpSchedule = true;
                            sch.ParentInspectionId = inspection.Id;
                            //var areaWiseDefaultIns = await _areaWiseInspector.GetFirstOrDefaultAsync(filter: x => x.AreaId == agencyReqField.AreaId);
                            //sch.AssignedTo = areaWiseDefaultIns.AssignedUserId;
                            sch.AssignedTo = inspection.InspectedBy;
                            sch.StatusId = 2;
                            sch.CreatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                            sch.CreatedOn = DateTime.Now;
                            sch.SyncDate = DateTime.Now;
                            await _scheduler.AddAsync(sch);
                        }
                        inspection.FollowUp = model.FollowUp;
                        var agencyReqField = await _agencyreqFields.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == inspection.EstablishmentId, includeProperties: "Establishment");
                        if (model.PermitSuspend == true)
                        {
                            if(agencyReqField.Establishment!.PermitStatusId >= 9 && agencyReqField.Establishment!.PermitStatusId!=13 && agencyReqField.Establishment!.IsActive != false)
                            {
                                agencyReqField.Establishment!.OldPermitStatusId = agencyReqField.Establishment!.PermitStatusId == 10 ? 10 : 9;
                                agencyReqField.Establishment!.PermitStatusId = 13;
                                agencyReqField.Establishment!.IsActive = false;
                                agencyReqField.Establishment.SyncDate = DateTime.Now;
                                await _establishments.UpdateAsync(agencyReqField.Establishment!);
                            }                            
                        }
                        else
                        {
                            if (agencyReqField.Establishment!.PermitStatusId >= 9 && isOldInspectionPermitSuspended == true)
                            {
                                agencyReqField.Establishment!.OldPermitStatusId = 13;
                                agencyReqField.Establishment!.PermitStatusId = agencyReqField.Establishment!.OldPermitStatusId == 10 ? 10 : 9;
                                agencyReqField.Establishment!.IsActive = true;
                                agencyReqField.Establishment.SyncDate = DateTime.Now;
                                await _establishments.UpdateAsync(agencyReqField.Establishment!);
                            }                           
                        }
                        //inspection.InspectedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                        //inspection.InspectorSignFile = model.InspectorSignFile;
                        //inspection.InspectedBySign = model.InspectedBySign;
                        inspection.Comment = model.Comment;
                        inspection.UpdatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                        inspection.UpdatedOn = DateTime.Now;
                        await _inspection.UpdateAsync(inspection);

                        var inspectionData = await _rfmfInspectionData.GetFirstOrDefaultAsync(filter: x => x.InspectionId == inspection.Id);
                        //inspectionData.PersonInCharge = model.PersonInCharge;
                        //inspectionData.PersonInChargeSign = model.PersonInChargeSign;
                        inspectionData.Score = model.Score;
                        inspectionData.NumberOfVioCos = model.NumberOfVioCos;
                        inspectionData.NumberOfRepeatedVio = model.NumberOfRepeatedVio;
                        inspectionData.SampleCollected = model.SampleCollected;
                        inspectionData.CFM = model.CFM;
                        inspectionData.FHC = model.FHC;
                        inspectionData.CFMExpiryDate = model.CFMExpDate;
                        inspectionData.NumberOfEmployees = model.NumberOfEmployees;
                        //inspectionData.SecondaryInspector = model.SecondInspector;
                        //inspectionData.SecondaryInspectorSign = model.SecondInspectorSigns;
                        await _rfmfInspectionData.UpdateAsync(inspectionData);
                        _response.StatusCode = HttpStatusCode.OK;
                        _response.IsSuccess = true;
                        _response.Message = "Successfully Updated";
                        _response.Result = model.InspectionId!;
                        return Json(new { response = _response });
                        //return Json(new { success = true, msg = "Successfully Updated", inspectionId = model.InspectionId });
                    }
                    _response.StatusCode = HttpStatusCode.InternalServerError;
                    _response.IsSuccess = false;
                    _response.Message = "Unexpected Error Occurred";
                    _response.Result = model.InspectionId!;
                    return Json(new { response = _response });
                }
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

        [HttpPost("/RemoveAllItems", Name = "Remove_All_InspectionItems")]
        public async Task<ActionResult<APIResponse>> RemoveAllItems([FromForm] int InspectionId)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var str = TokenValidator.Validation(token);
            if (str == "Authorized")
            {
                var InspectionItems = await _inspectionItems.GetAllAsync(filter: x => x.InspectionId == InspectionId);
                if (InspectionItems.Any())
                {
                    foreach (var item in InspectionItems)
                    {
                        await _inspectionItems.RemoveAsync(item);
                    }
                }
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

        [HttpPost("/RemoveAllTemps", Name = "Remove_All_TemperatureData")]
        public async Task<ActionResult<APIResponse>> RemoveAllTemps([FromForm] int InspectionId)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var str = TokenValidator.Validation(token);
            if (str == "Authorized")
            {
                var TempObs = await _temperatureObs.GetAllAsync(filter: x => x.InspectionId == InspectionId);
                if (TempObs.Any())
                {
                    foreach (var item in TempObs)
                    {
                        await _temperatureObs.RemoveAsync(item);
                    }
                }
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

        [HttpPost("/SaveEditInspectionItems", Name = "Save_InspectionEdit_Items")]
        public async Task<ActionResult<APIResponse>> SaveInspectionItems([FromForm] InspectionItemDetails model)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var str = TokenValidator.Validation(token);
            if (str == "Authorized")
            {
                var inspectionDetail = new InspectionItemDetails
                {
                    InspectionId = model.InspectionId,
                    ItemId = model.ItemId,
                    Status = model.Status,
                    Cos = model.Cos,
                    R = model.R,
                    Comment = model.Comment,
                    Image = model.Image
                };

                await _inspectionItems.AddAsync(inspectionDetail);
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


        [HttpPost("/SaveInspection", Name = "Save_Inspection")]
        public async Task<ActionResult<APIResponse>> SaveInspection([FromForm] BrazosApp.Models.ViewModels.InspectionRequestDTO model)
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
            var userId = Convert.ToInt32(_encrypt.Decrypt256(TokenValidator.GetUserId(token)));
            if (str == "Authorized")
            {
                var inspection = new Inspection();
                var schedule = new Schedule();
                if (model.ScheduleId == 0) 
                {
                    Schedule autoSchedule = new Schedule();
                    autoSchedule.EstablishmentId = model.EstablishmentId;
                    autoSchedule.PurposeId = model.PurposeId;
                    autoSchedule.StatusId = 2;
                    autoSchedule.AssignedTo = userId;
                    autoSchedule.ScheduledDate = DateTime.Now;
                    autoSchedule.CreatedBy = userId;
                    autoSchedule.IsAdhoc = true;
                    autoSchedule.CreatedOn = DateTime.Now;
                    autoSchedule.SyncDate = DateTime.Now;
                    autoSchedule.IsFollowUpSchedule = false;
                    await _scheduler.AddAsync(autoSchedule);
                    schedule = autoSchedule;
                    inspection.ScheduleId = autoSchedule.Id;
                }
                else
                {
                    schedule = await _scheduler.GetFirstOrDefaultAsync(filter: x => x.Id == model.ScheduleId);
                    inspection.ScheduleId = model.ScheduleId;
                    if (schedule.IsFollowUpSchedule == true)
                    {
                        inspection.ParentInspectionId = schedule.ParentInspectionId;
                    }
                }
                schedule.Establishment = await _establishments.GetFirstOrDefaultAsync(filter: x => x.Id == schedule.EstablishmentId);

                inspection.EstablishmentId = model.EstablishmentId;
                if(model.ScheduleId != 0)
                {
                    inspection.ScheduleId = model.ScheduleId;
                }
                
                inspection.RiskId = model.RiskId;
                inspection.PurposeId = model.PurposeId;
                inspection.TimeIn = model.TimeIn;
                inspection.TimeOut = DateTime.Now;
                inspection.InspectionDate = model.InspectionDate;
                inspection.FollowUp = model.FollowUp;
                inspection.IsPermitSuspended = model.PermitSuspend;
                if (inspection.FollowUp == true) 
                { 
                     inspection.FollowUpDate = model.FollowUpDate ?? DateTime.Now.AddDays(7);   
                }
                inspection.InspectedBy = userId;
                inspection.InspectorSignFile = model.InspectorSignFile;
                inspection.InspectedBySign = model.InspectedBySign;
                inspection.Comment = model.Comment;
                inspection.IsPermitSuspended = model.PermitSuspend;
                await _inspection.AddAsync(inspection);

                var inspectionData = new RFMFInspectionData();
                inspectionData.InspectionId = inspection.Id;
                inspectionData.PersonInCharge = model.PersonInCharge;
                inspectionData.PersonInChargeSign = model.PersonInChargeSign;
                inspectionData.Score = model.Score;
                inspectionData.NumberOfVioCos = model.NumberOfVioCos;
                inspectionData.NumberOfRepeatedVio = model.NumberOfRepeatedVio;
                inspectionData.SampleCollected = model.SampleCollected;
                inspectionData.CFM = model.CFM;
                inspectionData.FHC = model.FHC;
                inspectionData.CFMExpiryDate = model.CFMExpDate;
                inspectionData.NumberOfEmployees = model.NumberOfEmployees;
                inspectionData.SecondaryInspector = model.SecondInspector;
                inspectionData.SecondaryInspectorSign = model.SecondInspectorSigns;
                inspectionData.SecondaryInspectorSignFile = model.SecondInspectorSignFile;
                //inspectionData.InspectedBy = userId;
                //inspectionData.InspectorSignFile = model.InspectorSignFile;
                //inspectionData.InspectedBySign = model.InspectedBySign;
                
                if (model.PermitSuspend == true)
                {
                    var agencyReqField = await _agencyreqFields.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == inspection.EstablishmentId, includeProperties: "Establishment");
                    if (agencyReqField.Establishment!.PermitStatusId >= 9 && agencyReqField.Establishment!.PermitStatusId != 13 && agencyReqField.Establishment!.IsActive != false)
                    {
                        //agencyReqField.Establishment!.OldPermitStatusId = agencyReqField.Establishment!.PermitStatusId;
                        agencyReqField.Establishment!.OldPermitStatusId = agencyReqField.Establishment!.PermitStatusId == 10 ? 10 : 9;
                        agencyReqField.Establishment!.PermitStatusId = 13;
                        agencyReqField.Establishment!.IsActive = false;
                        agencyReqField.Establishment.SyncDate = DateTime.Now;
                        await _establishments.UpdateAsync(agencyReqField.Establishment!);
                    }   
                }
                        
                if (model.FollowUp == true)
                {
                    //inspectionData.PermitApproval = false;
                    Schedule sch = new Schedule();
                    sch.EstablishmentId = inspection.EstablishmentId;
                    sch.PurposeId = model.PurposeId;
                    sch.ScheduledDate = model.FollowUpDate ?? DateTime.Now.AddDays(7);
                    sch.IsFollowUpSchedule = true;
                    sch.ParentInspectionId = inspection.Id;
                    //var areaWiseDefaultIns = await _areaWiseInspector.GetFirstOrDefaultAsync(filter: x => x.AreaId == agencyReqField.AreaId);
                    //sch.AssignedTo = areaWiseDefaultIns.AssignedUserId;
                    sch.AssignedTo = userId;
                    sch.StatusId = 2;
                    sch.CreatedBy = userId;
                    sch.CreatedOn = DateTime.Now;
                    sch.SyncDate = DateTime.Now;
                    await _scheduler.AddAsync(sch);
                }
                //else
                //{
                    //var est = await _establishments.GetFirstOrDefaultAsync(filter:x=>x.Id==model.EstablishmentId);
                    //if (agencyReqField.Establishment!.PermitStatusId == 8)
                    //{
                    //    agencyReqField.Establishment!.OldPermitStatusId = 8;
                    //    agencyReqField.Establishment!.PermitStatusId = 9;
                    //}
                    //await _establishments.UpdateAsync(agencyReqField.Establishment!);
                    //openingData.PermitApproval = true;
                //}
                await _rfmfInspectionData.AddAsync(inspectionData);
                //var schedule = await _scheduler.GetFirstOrDefaultAsync(filter: x => x.Id == inspection.ScheduleId, includeProperties:"Establishment");
                
                //if (schedule.IsAutoSchedule)
                //{
                //        var nextInsDate = DateTime.Now;
                //        if (schedule.Establishment!.RiskCategory == "High")
                //        {
                //              schedule.Establishment!.NextInspectionDate = schedule.Establishment!.NextInspectionDate!.Value.AddDays(120);
                //        }
                //        else if (schedule.Establishment!.RiskCategory == "Medium")
                //        {
                //              schedule.Establishment!.NextInspectionDate = schedule.Establishment!.NextInspectionDate!.Value.AddDays(180);
                //        }
                //        else if (schedule.Establishment!.RiskCategory == "Low")
                //        {
                //              schedule.Establishment!.NextInspectionDate = schedule.Establishment!.NextInspectionDate!.Value.AddDays(360);
                //        }
                //        schedule.Establishment!.LastInspectionDate = DateTime.Now;
                //        var area = await _area.GetFirstOrDefaultAsync(filter: x => x.AreaNumber == schedule.Establishment!.Area);
                //        var assignedTo = 2;
                //        if (area != null)
                //        {
                //              var areawiseIns = await _areaWiseInspector.GetFirstOrDefaultAsync(filter: x => x.AreaId == area.Id);
                //              if (areawiseIns != null)
                //              {
                //                    assignedTo = areawiseIns.AssignedUserId;
                //              }
                //        }
                //        Schedule autosch = new Schedule();
                //        autosch.EstablishmentId = schedule.EstablishmentId;
                //        autosch.ScheduledDate = schedule.Establishment!.NextInspectionDate!.Value;
                //        autosch.PurposeId = 3;
                //        autosch.StatusId = 2;
                //        autosch.AssignedTo = assignedTo;
                //        autosch.CreatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                //        autosch.CreatedOn = DateTime.Now;
                //        autosch.IsAdhoc = false;
                //        autosch.IsAutoSchedule = true;
                //        autosch.SyncDate = DateTime.Now;
                //        await _scheduler.AddAsync(autosch);
                //        schedule.Establishment.SyncDate = DateTime.Now;
                //        await _establishments.UpdateAsync(schedule.Establishment!);
                //}
                //var nextInsDate = DateTime.Now;
                if (inspection.PurposeId == 3 && inspection.ParentInspectionId == null)
                {
                    if (schedule.Establishment!.RiskCategory == "High")
                    {
                        //schedule.Establishment!.NextInspectionDate = schedule.Establishment!.NextInspectionDate!.Value.AddDays(120);
                        schedule.Establishment!.NextInspectionDate = inspection.InspectionDate.AddDays(120);
                    }
                    else if (schedule.Establishment!.RiskCategory == "Medium")
                    {
                        //schedule.Establishment!.NextInspectionDate = schedule.Establishment!.NextInspectionDate!.Value.AddDays(180);
                        schedule.Establishment!.NextInspectionDate = inspection.InspectionDate.AddDays(180);
                    }
                    else if (schedule.Establishment!.RiskCategory == "Low")
                    {
                        //schedule.Establishment!.NextInspectionDate = schedule.Establishment!.NextInspectionDate!.Value.AddDays(360);
                        schedule.Establishment!.NextInspectionDate = inspection.InspectionDate.AddDays(360);
                    }
                    schedule.Establishment!.LastInspectionDate = DateTime.Now;
                    //var area = await _area.GetFirstOrDefaultAsync(filter: x => x.AreaNumber == schedule.Establishment!.Area);
                    //var assignedTo = 2;
                    //if (area != null)
                    //{
                    //    var areawiseIns = await _areaWiseInspector.GetFirstOrDefaultAsync(filter: x => x.AreaId == area.Id);
                    //    if (areawiseIns != null)
                    //    {
                    //        assignedTo = areawiseIns.AssignedUserId;
                    //    }
                    //}
                    Schedule autosch = new Schedule();
                    autosch.EstablishmentId = schedule.EstablishmentId;
                    autosch.ScheduledDate = schedule.Establishment!.NextInspectionDate!.Value;
                    autosch.PurposeId = schedule.PurposeId;
                    autosch.StatusId = 2;
                    autosch.AssignedTo = inspection.InspectedBy;
                    autosch.CreatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    autosch.CreatedOn = DateTime.Now;
                    autosch.IsAdhoc = false;
                    autosch.IsAutoSchedule = true;
                    autosch.IsFollowUpSchedule = false;
                    autosch.SyncDate = DateTime.Now;
                    await _scheduler.AddAsync(autosch);
                    schedule.Establishment.SyncDate = DateTime.Now;
                    await _establishments.UpdateAsync(schedule.Establishment!);
                }


                schedule.StatusId = 4;
                schedule.SyncDate = DateTime.Now;
                await _scheduler.UpdateAsync(schedule);
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Message = "Success";
                _response.Result = inspection.Id;
                //return Json(new { Success = true, response = _response });
                return Json(new { success = true, response = _response });
            }
            else
            {
                _response.StatusCode = HttpStatusCode.Unauthorized;
                _response.IsSuccess = false;
                _response.Message = "Access Denied!";
                _response.Result = "";
                //return Ok(_response);
                return Json(new { Success = false, response = _response });
            }

            //return Ok();
        }


        //[HttpPost("/SaveInspectionItemsAPI", Name = "Save_Inspection_Items")]
        //public async Task<ActionResult<APIResponse>> SaveInspectionItems([FromBody] InspectionItemDetails model)
        //{
        //    if (model == null)
        //    {
        //        _response.StatusCode = HttpStatusCode.BadRequest;
        //        _response.IsSuccess = false;
        //        _response.Message = string.Empty;
        //        return BadRequest(_response);
        //    }
        //    var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        //    var str = TokenValidator.Validation(token);
        //    if (str == "Authorized")
        //    {
        //        var inspectionDetail = new InspectionItemDetails();
        //        inspectionDetail.InspectionId = model.InspectionId;
        //        inspectionDetail.ItemId = model.ItemId;
        //        inspectionDetail.Status = model.Status;
        //        inspectionDetail.Cos = model.Cos;
        //        inspectionDetail.R = model.R;
        //        try
        //        {
        //            await _inspectionItems.AddAsync(inspectionDetail);
        //        }
        //        catch (Exception ex)
        //        {

        //        }

        //        _response.StatusCode = HttpStatusCode.OK;
        //        _response.IsSuccess = true;
        //        _response.Message = "Success";
        //        _response.Result = "";
        //        return Ok(_response);
        //        //return Json(new { Success = true, response = _response });
        //        //return Json(new { success = true, response = _response });
        //    }
        //    else
        //    {
        //        _response.StatusCode = HttpStatusCode.Unauthorized;
        //        _response.IsSuccess = false;
        //        _response.Message = "Access Denied!";
        //        _response.Result = "";
        //        //return Ok(_response);
        //        //return Json(new { Success = false, response = _response });
        //        return Ok(_response);
        //    }

        //    //return Ok();
        //}

        [HttpPost("/SaveInspectionItemsAPI", Name = "Save_Inspection_Items")]
        public async Task<ActionResult<APIResponse>> SaveInspectionItems([FromBody] InspectionItemDetailsRequestDTO model)
        {
            if (model == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.Message = "Invalid model";
                return BadRequest(_response);
            }

            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var str = TokenValidator.Validation(token);

            if (str == "Authorized")
            {
                var inspectionDetail = new InspectionItemDetails
                {
                    InspectionId = model.InspectionId,
                    ItemId = model.ItemId,
                    Status = model.Status,
                    Cos = model.Cos,
                    R = model.R,
                    Comment = model.Comment,
                    Image = model.ImageFile
                };

                try
                {
                    await _inspectionItems.AddAsync(inspectionDetail);
                }
                catch (Exception ex)
                {
                    // Handle exception (logging, etc.)
                }

                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Message = "Success";
                _response.Result = "";
                return Ok(_response);
            }
            else
            {
                _response.StatusCode = HttpStatusCode.Unauthorized;
                _response.IsSuccess = false;
                _response.Message = "Access Denied!";
                _response.Result = "";
                return Ok(_response);
            }
        }


        [HttpPost("/SaveTemperatureObs", Name = "SaveTemperatureObs")]
        public async Task<IActionResult> SaveTemperatureObs([FromForm] TemperatureObservation model)
        {
            //TemperatureObservation model = new TemperatureObservation();
            if (model == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.Message = string.Empty;
                return BadRequest(_response);
            }
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var str = TokenValidator.Validation(token);
            if (str == "Authorized")
            {
                await _temperatureObs.AddAsync(model);
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Message = "Success";
                _response.Result = "";
                //return Json(new { Success = true, response = _response });
                return Json(new { success = true, response = _response });
            }
            else
            {
                _response.StatusCode = HttpStatusCode.Unauthorized;
                _response.IsSuccess = false;
                _response.Message = "Access Denied!";
                _response.Result = "";
                //return Ok(_response);
                return Json(new { Success = false, response = _response });
            }

            //    if (ModelState.IsValid)
            //{

            //    return Json(new { success = true });
            //}
            //return Json(new { success = false });
        }



        [HttpGet("/GetInspectionDataPdf", Name = "GetInspectionDataPdf")]
        public async Task<ActionResult<APIResponse>> GetInspectionDataPdf(string id)
        {
            if (id == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.Message = string.Empty;
                return BadRequest(_response);
            }

            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var str = TokenValidator.Validation(token);

            if (str == "Authorized")
            {
                FoodInspectionPdfDTO model = new FoodInspectionPdfDTO();
                //model.
                var inspectionId = Convert.ToInt32(_encrypt.Decrypt256(id));
                var inspection = await _inspection.GetFirstOrDefaultAsync(filter: x => x.Id == inspectionId, includeProperties: "Establishment");
                var InspectionDatas = await _rfmfInspectionData.GetFirstOrDefaultAsync(filter: x => x.InspectionId == inspectionId);
                var inspectionItems = await _inspectionItems.GetAllAsync(filter: x => x.InspectionId == inspectionId, includeProperties: "Items");
                var TemperatureObs = await _temperatureObs.GetAllAsync(filter: x => x.InspectionId == inspectionId);
                var user = await _users.GetFirstOrDefaultAsync(filter: x => x.Id == inspection.InspectedBy);

                TimeZoneInfo centralZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
                model.InspectionDetails = new InspectionDetails();
                model.InspectionDetails.Id = inspectionId;
                model.InspectionDetails.EstId = inspection.EstablishmentId;
                model.InspectionDetails.EstName = inspection.Establishment!.Name;
                model.InspectionDetails.Permit = inspection.Establishment!.PermitNumber;
                model.InspectionDetails.Risk = inspection.Establishment!.RiskCategory;
                model.InspectionDetails.PurposeId = inspection.PurposeId;
                model.InspectionDetails.Address = inspection.Establishment!.Address + ", " + inspection.Establishment!.State + ", " + inspection.Establishment!.City + ", " + inspection.Establishment!.Zip;
                model.InspectionDetails.Date = inspection.InspectionDate.ToShortDateString();
                if (inspection.UpdatedOn == null)
                {
                    model.InspectionDetails.TimeIn = TimeZoneInfo.ConvertTime(inspection.TimeIn, centralZone).ToShortTimeString();
                    model.InspectionDetails.TimeOut = TimeZoneInfo.ConvertTime(inspection.TimeOut, centralZone).ToShortTimeString();
                }
                else
                {
                    model.InspectionDetails.TimeIn = inspection.TimeIn.ToShortTimeString();
                    model.InspectionDetails.TimeOut = inspection.TimeOut.ToShortTimeString();
                }
                
                model.InspectionDetails.Score = InspectionDatas.Score;
                model.InspectionDetails.SampleCollected = InspectionDatas.SampleCollected;
                model.InspectionDetails.CFM = InspectionDatas.CFM;
                model.InspectionDetails.CFMExpDate = InspectionDatas.CFMExpiryDate;
                model.InspectionDetails.FHC = InspectionDatas.FHC;
                model.InspectionDetails.NumberOfEmployees = InspectionDatas.NumberOfEmployees;
                model.InspectionDetails.PermitSuspend = inspection.IsPermitSuspended;
                model.InspectionDetails.InspectedBy = user.FirstName + " " + user.LastName;
                model.InspectionDetails.InspectedByEmail = user.EmailId;
                model.InspectionDetails.ReceivedBy = InspectionDatas.PersonInCharge;
                model.InspectionDetails.ReceivedBySign = InspectionDatas.PersonInChargeSign;
                model.InspectionDetails.SecondaryInspector = InspectionDatas.SecondaryInspector;
                model.InspectionDetails.SecondaryInspectorSign = InspectionDatas.SecondaryInspectorSign;
                model.InspectionDetails.SecondaryInspectorSignFile = InspectionDatas.SecondaryInspectorSignFile;
                model.InspectionDetails.InspectedBySign = inspection.InspectedBySign;
                model.InspectionDetails.InspectedBySignFile = inspection.InspectorSignFile;
                model.InspectionDetails.FollowUp = inspection.FollowUp;
                model.InspectionDetails.Comment = inspection.Comment;
                //model.InspectionDetails.PermitApproval = openingInspectionDatas.PermitApproval == true ? "Yes" : "No";

                if (inspection.Establishment.PermitNumber!.StartsWith("RF"))
                {
                    model.Code = "RF";
                }
                else if (inspection.Establishment.PermitNumber!.StartsWith("MF"))
                {
                    model.Code = "MF";
                }

                model.InspectionItemList = new List<InspectionItems>();
                foreach (var items in inspectionItems)
                {
                    model.InspectionItemList!.Add(new InspectionItems
                    {
                        Id = items.Id,
                        ItemId = items.ItemId,
                        ItemNumber = items.Items!.ItemNumber,
                        SubCategoryId = items.Items!.SubSectionId,
                        IsNA = items.Items!.IsNA,
                        IsNO = items.Items!.IsNO,
                        Name = items.Items!.Name,
                        Point = items.Items!.Points,
                        Status = items.Status,
                        CDI = items.Cos,
                        R = items.R,
                        Image = items.Image,
                        Comment = items.Comment,
                    });
                }

                model.TemperatureObservations = new List<TemperatureObservation>();
                foreach (var temperatureObservation in TemperatureObs)
                {
                    model.TemperatureObservations.Add(new TemperatureObservation
                    {
                        Id = temperatureObservation.Id,
                        InspectionId = temperatureObservation.InspectionId,
                        ItemName = temperatureObservation.ItemName,
                        Temperature = temperatureObservation.Temperature,
                    });
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

        [HttpGet("/GetInspectionCommentSectionAPIDataPdf", Name = "GetInspectionCommentSectionAPIDataPdf")]
        public async Task<ActionResult<APIResponse>> GetInspectionCommentSectionAPIDataPdf(string id)
        {
            if (id == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.Message = string.Empty;
                return BadRequest(_response);
            }

            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var str = TokenValidator.Validation(token);

            if (str == "Authorized")
            {
                var inspectionId = Convert.ToInt32(_encrypt.Decrypt256(id));
                InspectionCommentPdfDTO model = new InspectionCommentPdfDTO();
                model.Inspection = await _inspection.GetFirstOrDefaultAsync(x => x.Id == inspectionId, includeProperties: "Establishment");
                if (model.Inspection == null) { return NotFound(); }
                model.InspectionData = await _rfmfInspectionData.GetFirstOrDefaultAsync(filter: x => x.InspectionId == inspectionId);
                var user = await _users.GetAllAsync();
                model.InspectedBy = (from u in user where u.Id == model.Inspection.InspectedBy select (u.FirstName + " " + u.LastName)).FirstOrDefault();
                model.SecondaryInspectedBy = (from u in user where u.Id == model.Inspection.InspectedBy select (u.FirstName + " " + u.LastName)).FirstOrDefault();
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


        [HttpGet("/GetEstOwnerEmail", Name = "GetEstOwnerEmail")]
        public async Task<IActionResult> GetEstOwnerEmail(string InspectionId)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var str = TokenValidator.Validation(token);
            if (str == "Authorized")
            {
                var InsId = Convert.ToInt32(_encrypt.Decrypt256(InspectionId));
                var inspection = await _inspection.GetFirstOrDefaultAsync(filter: x => x.Id == InsId, includeProperties: "Purpose");
                if (inspection == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    _response.Message = "Inspection Data Not Found!";
                    _response.Result = "";
                    //return Ok(_response);
                    return Json(new { response = _response });
                    //return Json(new { success = false });
                }
                else
                {
                    var owner = await _owner.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == inspection.EstablishmentId);
                    if (owner == null)
                    {
                        _response.StatusCode = HttpStatusCode.NotFound;
                        _response.IsSuccess = false;
                        _response.Message = "Owner Data Not Found!";
                        _response.Result = "";
                        //return Ok(_response);
                        return Json(new { response = _response });
                        //return Json(new { success = false });
                    }
                    else
                    {
                        _response.StatusCode = HttpStatusCode.OK;
                        _response.IsSuccess = true;
                        _response.Message = "Success";
                        _response.Result = new { email = owner.EmailId, id = InspectionId, purpose = inspection.Purpose!.Name};
                        //return Ok(_response);
                        return Json(new { response = _response });
                        //return Json(new { success = true, email = owner.EmailId, id = InspectionId, purpose = inspection.Purpose!.Name });
                    }
                }
            }
            else
            {
                _response.StatusCode = HttpStatusCode.Unauthorized;
                _response.IsSuccess = false;
                _response.Message = "Access Denied!";
                _response.Result = "";
                //return Ok(_response);
                return Json(new { response = _response });
            }
        }
        //[HttpGet("/GetInspectionCheckDataPdf", Name = "GetInspectionCheckData")]
        //public async Task<ActionResult<APIResponse>> GetInspectionCheckDataPdf(string id)
        //{
        //      if (id == null)
        //      {
        //            _response.StatusCode = HttpStatusCode.BadRequest;
        //            _response.IsSuccess = false;
        //            _response.Message = string.Empty;
        //            return BadRequest(_response);
        //      }

        //      var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        //      var str = TokenValidator.Validation(token);

        //      if (str == "Authorized")
        //      {
        //            OpeningCheckListPdfDTO model = new OpeningCheckListPdfDTO();
        //            //model.
        //            var inspectionId = Convert.ToInt32(_encrypt.Decrypt256(id));
        //            var inspection = await _inspection.GetFirstOrDefaultAsync(filter: x => x.Id == inspectionId, includeProperties: "Establishment");
        //            var openingInspectionDatas = await _rfmfInspectionData.GetFirstOrDefaultAsync(filter: x => x.InspectionId == inspectionId);
        //            var inspectionItems = await _inspectionItems.GetAllAsync(filter: x => x.InspectionId == inspectionId, includeProperties: "Items");
        //            var user = await _users.GetFirstOrDefaultAsync(filter: x => x.Id == inspection.InspectedBy);

        //            model.InspectionDetails = new OpeningInspectionDetails();
        //            model.InspectionDetails.Id = inspectionId;
        //            model.InspectionDetails.EstId = inspection.EstablishmentId;
        //            model.InspectionDetails.EstName = inspection.Establishment!.Name;
        //            model.InspectionDetails.Permit = inspection.Establishment!.PermitNumber;
        //            model.InspectionDetails.Address = inspection.Establishment!.Address + ", " + inspection.Establishment!.State + ", " + inspection.Establishment!.City + ", " + inspection.Establishment!.Zip;
        //            model.InspectionDetails.Date = inspection.InspectionDate.ToShortDateString() + "/" + inspection.TimeIn.ToShortTimeString() + "-" + inspection.TimeOut.ToShortTimeString();
        //            model.InspectionDetails.InspectedBy = user.FirstName + " " + user.LastName;
        //            model.InspectionDetails.ReceivedBy = openingInspectionDatas.PersonInCharge;
        //            model.InspectionDetails.ReceivedBySign = openingInspectionDatas.PersonInChargeSign;
        //            model.InspectionDetails.InspectedBySign = inspection.InspectedBySign;
        //            model.InspectionDetails.InspectedBySignFile = inspection.InspectorSignFile;
        //            model.InspectionDetails.PermitApproval = openingInspectionDatas.SampleCollected == true ? "Yes" : "No";

        //            if (inspection.Establishment.PermitNumber!.StartsWith("RF"))
        //            {
        //                  model.Code = "RF";
        //            }
        //            else if (inspection.Establishment.PermitNumber!.StartsWith("MF"))
        //            {
        //                  model.Code = "MF";
        //            }

        //            model.InspectionItemList = new List<OpeningInspectionItems>();
        //            foreach (var items in inspectionItems)
        //            {
        //                  model.InspectionItemList!.Add(new OpeningInspectionItems
        //                  {
        //                        Id = items.Id,
        //                        ItemId = items.ItemId,
        //                        Name = items.Items!.Name,
        //                        Status = items.Status,
        //                  });
        //            }
        //            _response.StatusCode = HttpStatusCode.OK;
        //            _response.IsSuccess = true;
        //            _response.Message = "Success";
        //            _response.Result = model;
        //            //return Json(new { Success = true, response = _response });
        //            return Ok(_response);
        //      }
        //      else
        //      {
        //            _response.StatusCode = HttpStatusCode.Unauthorized;
        //            _response.IsSuccess = false;
        //            _response.Message = "Access Denied!";
        //            _response.Result = "";
        //            return Ok(_response);
        //            //return Json(new { Success = false, response = _response });
        //      }
        //}

    }
}
