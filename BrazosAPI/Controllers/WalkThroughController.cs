using BrazosAPI.Extensions;
using BrazosAPI.Models;
using BrazosAPI.Models.DTOs;
using BrazosApp.DataAccess.Repository.IRepository;
using BrazosApp.Models;
using BrazosApp.Utility.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BrazosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WalkThroughController : Controller
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
        private readonly APIResponse _response;

        public WalkThroughController(IRepository<Item> item,
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
            _response = new APIResponse();

        }
        [HttpPost("/SaveWalkThroughInspection", Name = "Save_Walk_Through_Inspection")]
        public async Task<ActionResult<APIResponse>> SaveWalkThroughInspection([FromForm] OpeningInspectionRequestDTO model)
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
                inspection.EstablishmentId = model.EstablishmentId;
                inspection.ScheduleId = model.ScheduleId;
                inspection.RiskId = model.RiskId;
                inspection.PurposeId = model.PurposeId;
                inspection.TimeIn = model.TimeIn;
                inspection.TimeOut = DateTime.Now;
                inspection.InspectionDate = model.InspectionDate;
                //inspection.FollowUp = model.FollowUp;
                //inspection.FollowUpDate = model.FollowUpDate;
                inspection.InspectedBy = userId;
                inspection.InspectorSignFile = model.InspectorSignFile;
                inspection.InspectedBySign = model.InspectedBySign;
                inspection.Comment = model.Comment;
                await _inspection.AddAsync(inspection);

                var openingData = new OpeningInspectionData();
                openingData.InspectionId = inspection.Id;
                openingData.PersonInCharge = model.PersonInCharge;
                openingData.PersonInChargeSign = model.PersonInChargeSign;
                //openingData.PermitApproval = model.PermitApproval;
                //openingData.InspectedBy = userId;
                //openingData.InspectorSignFile = model.InspectorSignFile;
                //openingData.InspectedBySign = model.InspectedBySign;
                var agencyReqField = await _agencyreqFields.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == inspection.EstablishmentId, includeProperties: "Establishment");
                //if (inspection.FollowUp == true)
                //{
                //    //openingData.PermitApproval = false;
                //    Schedule sch = new Schedule();
                //    sch.EstablishmentId = inspection.EstablishmentId;
                //    sch.PurposeId = model.PurposeId;
                //    sch.ScheduledDate = model.FollowUpDate ?? DateTime.Now.AddDays(7);

                //    var territoryWiseDefaultIns = await _areaWiseInspector.GetFirstOrDefaultAsync(filter: x => x.AreaId == agencyReqField.AreaId);
                //    sch.AssignedTo = territoryWiseDefaultIns.AssignedUserId;
                //    sch.StatusId = 2;
                //    sch.CreatedBy = userId;
                //    sch.CreatedOn = DateTime.Now;
                //    await _scheduler.AddAsync(sch);
                //}
                //else
                //{
                //    //var est = await _establishments.GetFirstOrDefaultAsync(filter:x=>x.Id==model.EstablishmentId);


                //    if (agencyReqField.Establishment!.PermitStatusId == 8)
                //    {
                //        var Fees = await _fees.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == model.EstablishmentId && x.Status == 2);
                //        if (Fees != null)
                //        {
                //              agencyReqField.Establishment!.OldPermitStatusId = 8;
                //              agencyReqField.Establishment!.PermitStatusId = 9;
                //              agencyReqField.Establishment!.ActivationDate = DateTime.Now;
                //              var year = DateTime.Now.Year;
                //              agencyReqField.Establishment!.ExpiryDate = new DateTime(year, 12, 31);
                //        }


                //    }
                //    await _establishments.UpdateAsync(agencyReqField.Establishment!);
                //    //openingData.PermitApproval = true;
                //}
                //if (agencyReqField.Establishment!.PermitStatusId == 8)
                //{
                //    if (openingData.PermitApproval == true)
                //    {
                //        agencyReqField.Establishment!.OldPermitStatusId = 8;
                //        agencyReqField.Establishment!.PermitStatusId = 9;
                //        agencyReqField.Establishment!.ActivationDate = DateTime.Now;
                //        var year = DateTime.Now.Year;
                //        agencyReqField.Establishment!.ExpiryDate = new DateTime(year, 12, 31);
                //        await _establishments.UpdateAsync(agencyReqField.Establishment!);
                //    }
                //}

                await _openinginsData.AddAsync(openingData);
                var schedule = await _scheduler.GetFirstOrDefaultAsync(filter: x => x.Id == inspection.ScheduleId);
                schedule.StatusId = 4;
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


        [HttpPost("/SaveWalkThroughInspectionItems", Name = "Save_WalkThrough_Inspection_Items")]
        public async Task<ActionResult<APIResponse>> SaveWalkThroughInspectionItems([FromForm] OpeningInspectionItemDetailsRequestDTO model)
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
            if (str == "Authorized")
            {
                var inspectionDetail = new InspectionItemDetails();
                inspectionDetail.InspectionId = model.InspectionId;
                inspectionDetail.ItemId = model.ItemId;
                inspectionDetail.Status = model.Status;
                try
                {
                    await _inspectionItems.AddAsync(inspectionDetail);
                }
                catch (Exception ex)
                {

                }

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

            //return Ok();
        }

        [HttpGet("/GetWalkThroughCheckDataPdf", Name = "GetWalkThroughCheckData")]
        public async Task<ActionResult<APIResponse>> GetWalkThroughCheckDataPdf(string id)
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
                OpeningCheckListPdfDTO model = new OpeningCheckListPdfDTO();
                //model.
                var inspectionId = Convert.ToInt32(_encrypt.Decrypt256(id));
                var inspection = await _inspection.GetFirstOrDefaultAsync(filter: x => x.Id == inspectionId, includeProperties: "Establishment");
                var openingInspectionDatas = await _openinginsData.GetFirstOrDefaultAsync(filter: x => x.InspectionId == inspectionId);
                var inspectionItems = await _inspectionItems.GetAllAsync(filter: x => x.InspectionId == inspectionId, includeProperties: "Items");
                var user = await _users.GetFirstOrDefaultAsync(filter: x => x.Id == inspection.InspectedBy);

                model.InspectionDetails = new OpeningInspectionDetails();
                TimeZoneInfo centralZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
                model.InspectionDetails.Id = inspectionId;
                model.InspectionDetails.EstId = inspection.EstablishmentId;
                model.InspectionDetails.EstName = inspection.Establishment!.Name;
                model.InspectionDetails.Permit = inspection.Establishment!.PermitNumber;
                model.InspectionDetails.Address = inspection.Establishment!.Address + ", " + inspection.Establishment!.State + ", " + inspection.Establishment!.City + ", " + inspection.Establishment!.Zip;
                model.InspectionDetails.Date = inspection.InspectionDate.ToShortDateString() + "/" + TimeZoneInfo.ConvertTime(inspection.TimeIn, centralZone).ToShortTimeString() + "-" + TimeZoneInfo.ConvertTime(inspection.TimeOut, centralZone).ToShortTimeString();
                model.InspectionDetails.InspectedBy = user.FirstName + " " + user.LastName;
                model.InspectionDetails.ReceivedBy = openingInspectionDatas.PersonInCharge;
                model.InspectionDetails.ReceivedBySign = openingInspectionDatas.PersonInChargeSign;
                model.InspectionDetails.InspectedBySign = inspection.InspectedBySign;
                model.InspectionDetails.InspectedBySignFile = inspection.InspectorSignFile;
                model.InspectionDetails.PermitApproval = openingInspectionDatas.PermitApproval == true ? "Yes" : "No";
                model.InspectionDetails.Comment = inspection.Comment;

                if (inspection.Establishment.PermitNumber!.StartsWith("RF"))
                {
                    model.Code = "RF";
                }
                else if (inspection.Establishment.PermitNumber!.StartsWith("MF"))
                {
                    model.Code = "MF";
                }

                model.InspectionItemList = new List<OpeningInspectionItems>();
                foreach (var items in inspectionItems)
                {
                    model.InspectionItemList!.Add(new OpeningInspectionItems
                    {
                        Id = items.Id,
                        ItemId = items.ItemId,
                        Name = items.Items!.Name,
                        Status = items.Status,
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
    }
}
