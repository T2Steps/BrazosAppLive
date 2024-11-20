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
using System.Net;
using System.Security.Claims;

namespace BrazosAPI.Controllers
{
      [Route("api/[controller]")]
      [ApiController]
      [Authorize]
      public class OpeningInspectionController : Controller
      {
            private readonly IRepository<Item> _item;
            private readonly IRepository<Establishment> _establishments;
            private readonly IRepository<Inspection> _inspection;
            private readonly IRepository<InspectionItemDetails> _inspectionItems;
            private readonly IRepository<OpeningInspectionData> _openinginsData;
            private readonly IRepository<Schedule> _scheduler;
            private readonly IRepository<Users> _users;
            private readonly IRepository<AgencyStaffReqFields> _agencyreqFields;
            private readonly IRepository<EstablishmentTypes> _estTypes;
            private readonly IRepository<AreaWiseInspectors> _areaWiseInspector;
            private readonly IRepository<Payment> _paymentRepo;
            private readonly IRepository<PaymentDetailsTable> _paymentDetailsTable;
            private readonly IRepository<Fees> _fees;
            private readonly IRepository<FeesDetailsTable> _feesDetailsTable;
            private readonly IRepository<FoodRenewalHistory> _foodRenewalHistory;
            private readonly IEncrypt _encrypt;
            private readonly APIResponse _response;



            public OpeningInspectionController(IRepository<Item> item,
                IRepository<Establishment> establishments,
                IRepository<Inspection> inspection,
                IRepository<InspectionItemDetails> inspectionItems,
                IRepository<OpeningInspectionData> openinginsData,
                IRepository<Schedule> scheduler,
                IRepository<Users> users,
                IRepository<AgencyStaffReqFields> agencyreqFields,
                IRepository<EstablishmentTypes> estTypes,
                IRepository<AreaWiseInspectors> areaWiseInspector,
                IRepository<Payment> paymentRepo,
                IRepository<PaymentDetailsTable> paymentDetailsTable,
                IRepository<Fees> fees,
                IRepository<FeesDetailsTable> feesDetailsTable,
                IRepository<FoodRenewalHistory> foodRenewalHistory,
                IEncrypt encrypt)
            {
                  _item = item;
                  _establishments = establishments;
                  _inspectionItems = inspectionItems;
                  _openinginsData = openinginsData;
                  _inspection = inspection;
                  _fees = fees;
                  _feesDetailsTable = feesDetailsTable;
                  _scheduler = scheduler;
                  _users = users;
                  _agencyreqFields = agencyreqFields;
                  _estTypes = estTypes;   
                  _areaWiseInspector = areaWiseInspector;
                  _foodRenewalHistory = foodRenewalHistory;
                  _paymentRepo = paymentRepo;
                  _paymentDetailsTable = paymentDetailsTable;
                  _encrypt = encrypt;
                  _response = new APIResponse();

            }


            [HttpGet("/LoadOpeningInspectionEditData", Name = "LoadOpeningInspectionEditData")]
            public async Task<ActionResult<APIResponse>> LoadOpeningInspectionEditData(string id)
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
                        //var userId = TokenValidator.GetUserId(token);
                        EditOpeningInspectionVM model = new EditOpeningInspectionVM();
                        model.Inspection = await _inspection.GetFirstOrDefaultAsync(filter: x => x.Id == Convert.ToInt32(_encrypt.Decrypt256(id)), includeProperties: "Establishment");
                        model.InspectionData = await _openinginsData.GetFirstOrDefaultAsync(filter: x => x.InspectionId == Convert.ToInt32(_encrypt.Decrypt256(id)));
                        model.Items = await _item.GetAllAsync(filter: x => x.IsActive == true && x.IsDeleted == false && x.CodeId == 1);
                        model.Details = await _inspectionItems.GetAllAsync(filter: x => x.InspectionId == Convert.ToInt32(_encrypt.Decrypt256(id)));
                        model.Code = model.Inspection.Establishment!.PermitNumber!.StartsWith("RF") ? "RF" : (model.Inspection.Establishment.PermitNumber.StartsWith("MF") ? "MF" : "");
                        var user = await _users.GetFirstOrDefaultAsync(filter: x => x.Id == model.Inspection.InspectedBy);
                        model.InspectedBy = user.FirstName + " " + user.LastName;
                        TimeZoneInfo centralZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");

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
                        model.FinalItemList = new List<Item>();

                        if (model.Items != null && model.Items.Count() > 0)
                        {
                              foreach (var item in model.Items)
                              {
                                    var result = model.Details?.FirstOrDefault(rel => rel.ItemId == item.Id);
                                    model.FinalItemList.Add(new Item
                                    {
                                          Id = item.Id,
                                          Name = item.Name,
                                          IsNA = item.IsNA,
                                          Status = result?.Status,
                                    });
                              }
                        }

                        model.InspectionEditRequestDTO = new InspectionEditRequestDTO();
                        model.InspectionEditRequestDTO.Inspection = new Inspection();
                        model.InspectionEditRequestDTO.InspectionData = new OpeningInspectionData();
                        
                        //return View(model);

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

            [HttpGet("/LoadOpeningInspectionData", Name = "LoadOpeningInspectionData")]
            public async Task<ActionResult<APIResponse>> LoadOpeningInspectionData(string id)
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
                        OpeningInspectionResponseDTO model = new OpeningInspectionResponseDTO();
                        var schedule = await _scheduler.GetFirstOrDefaultAsync(filter: x => x.Id == Convert.ToInt32(_encrypt.Decrypt256(id)), includeProperties: "Establishment");
                        var agencyReq = await _agencyreqFields.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == schedule.EstablishmentId);
                        var user = await _users.GetFirstOrDefaultAsync(filter: x => x.Id == Convert.ToInt32(_encrypt.Decrypt256(userId)));

                        model.SId = schedule.Id;
                        model.EstablishmentId = schedule.EstablishmentId;
                        model.PurposeId = 1;
                        model.RiskId = agencyReq.RiskCategoryId;
                        model.EstName = schedule.Establishment!.Name;
                        model.Permit = schedule.Establishment!.PermitNumber;
                        model.Address = schedule.Establishment!.Address + ", " + schedule.Establishment!.State + ", " + schedule.Establishment!.City + ", " + schedule.Establishment!.Zip;
                        model.InspectedBy = user.FirstName + " " + user.LastName;
                        model.InspectedBySignFileName = user.SignFileName;
                        if (schedule.Establishment!.PermitNumber!.StartsWith("RF"))
                        {
                              model.Items = await _item.GetAllAsync(filter: x => x.IsActive == true && x.IsDeleted == false && x.CodeId == 1);
                              model.Code = "RF";
                        }
                        else if (schedule.Establishment!.PermitNumber!.StartsWith("MF"))
                        {
                              model.Items = await _item.GetAllAsync(filter: x => x.IsActive == true && x.IsDeleted == false && x.CodeId == 2);
                              model.Code = "MF";
                        }

                        model.InTime = DateTime.Now;
                        model.FinalItemList = new List<Item>();

                        if (model.Items != null && model.Items.Count() > 0)
                        {
                              foreach (var item in model.Items)
                              {
                                    var result = model.details?.FirstOrDefault(rel => rel.ItemId == item.Id);
                                    model.FinalItemList.Add(new Item
                                    {
                                          Id = item.Id,
                                          Name = item.Name,
                                          //Points = item.Points,
                                          IsNA = item.IsNA,
                                          //IsNO = item.IsNO,
                                          //SubSectionId = item.SubSectionId,
                                          //CDI = result?.Cos,
                                          //IsRepeat = result?.R,
                                          Status = result?.Status,
                                          //Image = result?.Image,
                                          //Comments = result?.Comment
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

            [HttpPost("/SaveOpeningInspectionEdit", Name = "Opening_Inspection_Edit")]
            public async Task<ActionResult<APIResponse>> SaveOpeningInspectionEdit([FromForm] InspectionEditRequestDTO model)
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
                        var inspection = await _inspection.GetFirstOrDefaultAsync(filter: x => x.Id == model.Inspection!.Id, includeProperties: "Establishment");
                        var inspectionData = await _openinginsData.GetFirstOrDefaultAsync(filter: x => x.InspectionId == model.Inspection!.Id);
                        inspection.InspectionDate = model.Inspection!.InspectionDate;
                        inspection.TimeIn = model.Inspection!.TimeIn;
                        inspection.TimeOut = model.Inspection!.TimeOut;
                        inspection.Comment = model.Inspection!.Comment;
                        inspection.FollowUp = model.Inspection!.FollowUp;
                        inspection.UpdatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                        inspection.UpdatedOn = DateTime.Now;
                        if (inspection.FollowUp == true)
                        {
                              inspection.FollowUpDate = model.Inspection.FollowUpDate ?? DateTime.Now.AddDays(7);
                              Schedule sch = new Schedule();
                              sch.EstablishmentId = inspection.EstablishmentId;
                              sch.PurposeId = 1;
                              sch.ScheduledDate = (DateTime)inspection.FollowUpDate;
                              sch.AssignedTo = inspection.InspectedBy;
                              sch.StatusId = 2;
                              sch.CreatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                              sch.CreatedOn = DateTime.Now;
                              sch.SyncDate = DateTime.Now;
                              await _scheduler.AddAsync(sch);
                        }
                        else
                        {
                              inspection.FollowUpDate = null;
                        }
                        inspectionData.PermitApproval = model.InspectionData!.PermitApproval;
                        try
                        {
                              await _inspection.UpdateAsync(inspection);
                        }
                        catch (Exception ex)
                        {

                        }

                        try
                        {
                              await _openinginsData.UpdateAsync(inspectionData);
                        }
                        catch (Exception ex)
                        {

                        }


                        var payment = await _paymentRepo.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == model.Inspection.EstablishmentId && x.IsPermitFee == true && x.RefundVoidPaymentId == null && x.PaymentStatus == 2);
                        if (payment != null && inspection.Establishment!.PermitStatusId == 8)
                        {
                              if (inspectionData.PermitApproval == true)
                              {

                                    inspection.Establishment!.OldPermitStatusId = 8;
                                    //inspection.Establishment!.PermitStatusId = 9;
                                    inspection.Establishment!.ActivationDate = DateTime.Now;
                                    var year = DateTime.Now.Year;
                                    //inspection.Establishment!.ExpiryDate = new DateTime(year, 12, 31);

                                    if (DateTime.Now.Date >= new DateTime(year, 10, 15) && DateTime.Now.Date <= new DateTime(year, 12, 15))
                                    {
                                        inspection.Establishment!.PermitStatusId = 10;
                                        inspection.Establishment!.ExpiryDate = new DateTime(year, 12, 31);

                                        if (inspection.Establishment!.ApplicationForId == 1 || inspection.Establishment!.ApplicationForId == 2)
                                        {
                                            var renagency = await _agencyreqFields.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == inspection.Establishment!.Id, includeProperties: "EstablishmentTypes,EstablishmentSize,RiskCategory");

                                            var estTypes = await _estTypes.GetAllAsync(filter: x => x.IsActive == true && (x.JurisdictionId >= 1 && x.JurisdictionId <= 3));

                                            var SelectedEst = new EstablishmentTypes();
                                            var selectedEstName = "";

                                            if (renagency.EstablishmentTypes!.JurisdictionId == 1)
                                            {
                                                //agency.EstablishmentTypes!.IsPorated == true ? selectedEstName = "Food Service Permit Renewal" : selectedEstName = agency.EstablishmentTypes!.Name;
                                                selectedEstName = renagency.EstablishmentTypes!.IsPorated == true ? "Food Service Permit Renewal" : renagency.EstablishmentTypes!.Name;
                                                //SelectedEst = estTypes.Where(x => x.Name == "Food Service Permit Renewal" && x.JurisdictionId == 1).FirstOrDefault();
                                                SelectedEst = estTypes.Where(x => x.Name == selectedEstName && x.JurisdictionId == 1).FirstOrDefault();
                                            }
                                            else
                                            {
                                                var RiskType = "";
                                                var EstSizeType = "";

                                                selectedEstName = renagency.EstablishmentTypes!.IsPorated == true ? "Food Service Permit Renewal" : renagency.EstablishmentTypes!.Name;

                                                if (renagency.EstablishmentTypes!.IsPorated == true)
                                                {
                                                    RiskType = renagency.RiskCategory!.Name == "Low" ? "1" : (renagency.RiskCategory!.Name == "Medium" ? "2" : (renagency.RiskCategory!.Name == "High" ? "3" : ""));
                                                    EstSizeType = renagency.EstablishmentSize!.Name == "Small" ? "A" : (renagency.EstablishmentSize!.Name == "Medium" ? "B" : (renagency.EstablishmentSize!.Name == "Large" ? "C" : ""));

                                                    SelectedEst = estTypes.Where(x => (x.Name!.Contains("Renewal") && x.Name!.Contains(RiskType + EstSizeType)) && x.JurisdictionId == renagency.EstablishmentTypes.JurisdictionId).FirstOrDefault();
                                                }
                                                else
                                                {
                                                    SelectedEst = estTypes.Where(x => (x.Name == renagency.EstablishmentTypes.Name) && x.JurisdictionId == renagency.EstablishmentTypes.JurisdictionId).FirstOrDefault();
                                                }
                                            }
                                            //if (renagency.EstablishmentTypes!.JurisdictionId == 1)
                                            //{
                                            //    SelectedEst = estTypes.Where(x => x.Name == "Food Service Permit Renewal" && x.JurisdictionId == 1).FirstOrDefault();
                                            //}
                                            //else
                                            //{
                                            //    var RiskType = "";
                                            //    var EstSizeType = "";

                                            //    RiskType = renagency.RiskCategory!.Name == "Low" ? "1" : (renagency.RiskCategory!.Name == "Medium" ? "2" : (renagency.RiskCategory!.Name == "High" ? "3" : ""));
                                            //    EstSizeType = renagency.EstablishmentSize!.Name == "Small" ? "A" : (renagency.EstablishmentSize!.Name == "Medium" ? "B" : (renagency.EstablishmentSize!.Name == "Large" ? "C" : ""));

                                            //    SelectedEst = estTypes.Where(x => (x.Name!.Contains("Renewal") && x.Name!.Contains(RiskType + EstSizeType)) && x.JurisdictionId == renagency.EstablishmentTypes.JurisdictionId).FirstOrDefault();

                                            //}
                                            Fees renewalFees = new Fees();
                                            renewalFees.EstablishmentId = inspection.Establishment!.Id;
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
                                            renewalpayment.EstablishmentId = inspection.Establishment!.Id;
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
                                        inspection.Establishment!.PermitStatusId = 9;
                                        inspection.Establishment!.ExpiryDate = new DateTime(DateTime.Now.AddYears(1).Year, 12, 31);
                                    }
                                    else
                                    {
                                        inspection.Establishment!.PermitStatusId = 9;
                                        inspection.Establishment!.ExpiryDate = new DateTime(year, 12, 31);
                                    }
                                    FoodRenewalHistory history = new FoodRenewalHistory();
                                    history.EstablishmentId = inspection.Establishment!.Id;
                                    history.ActivationDate = inspection.Establishment!.ActivationDate!.Value;
                                    history.ExpiryDate = inspection.Establishment!.ExpiryDate!.Value;
                                    history.IsCurrentYear = true;
                                    history.IsActive = true;
                                    await _foodRenewalHistory.AddAsync(history);
                                    //inspection.Establishment!.OldPermitStatusId = 8;
                                    //inspection.Establishment!.PermitStatusId = 9;
                                    //inspection.Establishment!.ActivationDate = DateTime.Now;
                                    //var year = DateTime.Now.Year;
                                    //inspection.Establishment!.ExpiryDate = new DateTime(year, 12, 31);

                                    //FoodRenewalHistory history = new FoodRenewalHistory();
                                    //history.EstablishmentId = agencyReqField.Establishment!.Id;
                                    //history.ActivationDate = agencyReqField.Establishment!.ActivationDate.Value;
                                    //history.ExpiryDate = agencyReqField.Establishment!.ExpiryDate.Value;
                                    //history.IsActive = true;
                                    //history.IsCurrentYear = true;
                                    //await _foodRenewalHistory.AddAsync(history);

                                    if (inspection.Establishment!.RiskCategory == "High")
                                    {
                                          inspection.Establishment!.NextInspectionDate = (inspection.Establishment!.ActivationDate ?? DateTime.Now.Date).AddDays(120);
                                    }
                                    else if (inspection.Establishment!.RiskCategory == "Medium")
                                    {
                                          inspection.Establishment!.NextInspectionDate = (inspection.Establishment!.ActivationDate ?? DateTime.Now.Date).AddDays(180);
                                    }
                                    else if (inspection.Establishment!.RiskCategory == "Low")
                                    {
                                          inspection.Establishment!.NextInspectionDate = (inspection.Establishment!.ActivationDate ?? DateTime.Now.Date).AddDays(360);
                                    }
                                    //var area = await _area.GetFirstOrDefaultAsync(filter: x => x.AreaNumber == establishment.Area);
                                    var assignedTo = 2;
                                    var agency = await _agencyreqFields.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == inspection.EstablishmentId);
                                    if (agency.Area != null)
                                    {
                                          var areawiseIns = await _areaWiseInspector.GetFirstOrDefaultAsync(filter: x => x.AreaId == agency.Area.Id);
                                          if (areawiseIns != null)
                                          {
                                                assignedTo = areawiseIns.AssignedUserId;
                                          }
                                    }
                                    Schedule autoschedule = new Schedule();
                                    autoschedule.EstablishmentId = inspection.Establishment.Id;
                                    autoschedule.ScheduledDate = inspection.Establishment.NextInspectionDate!.Value;
                                    autoschedule.PurposeId = 3;
                                    autoschedule.StatusId = 2;
                                    autoschedule.AssignedTo = assignedTo;
                                    autoschedule.CreatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                                    autoschedule.CreatedOn = DateTime.Now;
                                    autoschedule.IsAdhoc = false;
                                    autoschedule.IsAutoSchedule = true;
                                    autoschedule.SyncDate = DateTime.Now;
                                    await _scheduler.AddAsync(autoschedule);
                                    inspection.Establishment.SyncDate = DateTime.Now;
                                    await _establishments.UpdateAsync(inspection.Establishment!);
                              }
                        }
                        //return Json(new { success = true, inspectionid = inspection.Id });
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



            [HttpPost("/SaveOpeningInspectionItemsEdit", Name = "Opening_Inspection_Items_Edit")]
            public async Task<ActionResult<APIResponse>> SaveInspectionItemsEdit([FromForm] InspectionItemDetails model)
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

                        //if(inspectionDetail.Status== "OUT")
                        //{
                        //    var openingData = await _openinginsData.GetFirstOrDefaultAsync(filter:x=>x.InspectionId==model.InspectionId);
                        //}

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


            [HttpPost("/SaveOpeningInspection", Name = "Opening_Inspection")]
            public async Task<ActionResult<APIResponse>> SaveOpeningInspection([FromForm] OpeningInspectionRequestDTO model)
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
                        inspection.FollowUp = model.FollowUp;
                        if (inspection.FollowUp == true)
                        {
                            inspection.FollowUpDate = model.FollowUpDate ?? DateTime.Now.AddDays(7);
                        }
                        
                        inspection.InspectedBy = userId;
                        inspection.InspectorSignFile = model.InspectorSignFile;
                        inspection.InspectedBySign = model.InspectedBySign;
                        inspection.Comment = model.Comment;
                        await _inspection.AddAsync(inspection);

                        var openingData = new OpeningInspectionData();
                        openingData.InspectionId = inspection.Id;
                        openingData.PersonInCharge = model.PersonInCharge;
                        openingData.PersonInChargeSign = model.PersonInChargeSign;
                        openingData.PermitApproval = model.PermitApproval;
                        openingData.SecondaryInspector = model.SecondInspector;
                        openingData.SecondaryInspectorSign = model.SecondInspectorSigns;
                        openingData.SecondaryInspectorSignFile = model.SecondInspectorSignFile;
                        //openingData.InspectedBy = userId;
                        //openingData.InspectorSignFile = model.InspectorSignFile;
                        //openingData.InspectedBySign = model.InspectedBySign;
                        var agencyReqField = await _agencyreqFields.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == inspection.EstablishmentId, includeProperties: "Establishment,Area");
                        if (inspection.FollowUp == true)
                        {
                              //openingData.PermitApproval = false;
                              Schedule sch = new Schedule();
                              sch.EstablishmentId = inspection.EstablishmentId;
                              sch.PurposeId = model.PurposeId;
                              sch.ScheduledDate = model.FollowUpDate ?? DateTime.Now.AddDays(7);

                              //var territoryWiseDefaultIns = await _areaWiseInspector.GetFirstOrDefaultAsync(filter: x => x.AreaId == agencyReqField.AreaId);
                              //sch.AssignedTo = territoryWiseDefaultIns.AssignedUserId;
                              sch.AssignedTo = userId;
                              sch.StatusId = 2;
                              sch.CreatedBy = userId;
                              sch.CreatedOn = DateTime.Now;
                              sch.SyncDate = DateTime.Now;
                              await _scheduler.AddAsync(sch);
                        }
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

                        var payment = await _paymentRepo.GetFirstOrDefaultAsync(filter:x=> x.EstablishmentId == model.EstablishmentId && x.IsPermitFee==true && x.RefundVoidPaymentId==null && x.PaymentStatus==2);
                        if (payment != null &&  agencyReqField.Establishment!.PermitStatusId == 8)
                        {
                              if (openingData.PermitApproval == true)
                              {
                                    agencyReqField.Establishment!.OldPermitStatusId = 8;
                                    //agencyReqField.Establishment!.PermitStatusId = 9;
                                    agencyReqField.Establishment!.ActivationDate = DateTime.Now;
                                    var year = DateTime.Now.Year;
                                    //agencyReqField.Establishment!.ExpiryDate = new DateTime(year, 12, 31);

                                    if (DateTime.Now.Date >= new DateTime(year, 10, 15) && DateTime.Now.Date <= new DateTime(year, 12, 15))
                                    {
                                        agencyReqField.Establishment!.PermitStatusId = 10;
                                        agencyReqField.Establishment!.ExpiryDate = new DateTime(year, 12, 31);

                                        if (DateTime.Now.Date >= new DateTime(year, 10, 15) && DateTime.Now.Date <= new DateTime(year, 12, 15))
                                        {
                                            inspection.Establishment!.PermitStatusId = 10;
                                            inspection.Establishment!.ExpiryDate = new DateTime(year, 12, 31);

                                            if (inspection.Establishment!.ApplicationForId == 1 || inspection.Establishment!.ApplicationForId == 2)
                                            {
                                                var renagency = await _agencyreqFields.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == inspection.Establishment!.Id, includeProperties: "EstablishmentTypes,EstablishmentSize,RiskCategory");

                                                var estTypes = await _estTypes.GetAllAsync(filter: x => x.IsActive == true && (x.JurisdictionId >= 1 && x.JurisdictionId <= 3));

                                                var SelectedEst = new EstablishmentTypes();
                                                var selectedEstName = "";

                                                if (renagency.EstablishmentTypes!.JurisdictionId == 1)
                                                {
                                                    //agency.EstablishmentTypes!.IsPorated == true ? selectedEstName = "Food Service Permit Renewal" : selectedEstName = agency.EstablishmentTypes!.Name;
                                                    selectedEstName = renagency.EstablishmentTypes!.IsPorated == true ? "Food Service Permit Renewal" : renagency.EstablishmentTypes!.Name;
                                                    //SelectedEst = estTypes.Where(x => x.Name == "Food Service Permit Renewal" && x.JurisdictionId == 1).FirstOrDefault();
                                                    SelectedEst = estTypes.Where(x => x.Name == selectedEstName && x.JurisdictionId == 1).FirstOrDefault();
                                                }
                                                else
                                                {
                                                    var RiskType = "";
                                                    var EstSizeType = "";

                                                    selectedEstName = renagency.EstablishmentTypes!.IsPorated == true ? "Food Service Permit Renewal" : renagency.EstablishmentTypes!.Name;

                                                    if (renagency.EstablishmentTypes!.IsPorated == true)
                                                    {
                                                        RiskType = renagency.RiskCategory!.Name == "Low" ? "1" : (renagency.RiskCategory!.Name == "Medium" ? "2" : (renagency.RiskCategory!.Name == "High" ? "3" : ""));
                                                        EstSizeType = renagency.EstablishmentSize!.Name == "Small" ? "A" : (renagency.EstablishmentSize!.Name == "Medium" ? "B" : (renagency.EstablishmentSize!.Name == "Large" ? "C" : ""));

                                                        SelectedEst = estTypes.Where(x => (x.Name!.Contains("Renewal") && x.Name!.Contains(RiskType + EstSizeType)) && x.JurisdictionId == renagency.EstablishmentTypes.JurisdictionId).FirstOrDefault();
                                                    }
                                                    else
                                                    {
                                                        SelectedEst = estTypes.Where(x => (x.Name == renagency.EstablishmentTypes.Name) && x.JurisdictionId == renagency.EstablishmentTypes.JurisdictionId).FirstOrDefault();
                                                    }
                                                }

                                                //if (renagency.EstablishmentTypes!.JurisdictionId == 1)
                                                //{
                                                //    SelectedEst = estTypes.Where(x => x.Name == "Food Service Permit Renewal" && x.JurisdictionId == 1).FirstOrDefault();
                                                //}
                                                //else
                                                //{
                                                //    var RiskType = "";
                                                //    var EstSizeType = "";

                                                //    RiskType = renagency.RiskCategory!.Name == "Low" ? "1" : (renagency.RiskCategory!.Name == "Medium" ? "2" : (renagency.RiskCategory!.Name == "High" ? "3" : ""));
                                                //    EstSizeType = renagency.EstablishmentSize!.Name == "Small" ? "A" : (renagency.EstablishmentSize!.Name == "Medium" ? "B" : (renagency.EstablishmentSize!.Name == "Large" ? "C" : ""));

                                                //    SelectedEst = estTypes.Where(x => (x.Name!.Contains("Renewal") && x.Name!.Contains(RiskType + EstSizeType)) && x.JurisdictionId == renagency.EstablishmentTypes.JurisdictionId).FirstOrDefault();

                                                //}
                                                Fees renewalFees = new Fees();
                                                renewalFees.EstablishmentId = inspection.Establishment!.Id;
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
                                                renewalpayment.EstablishmentId = inspection.Establishment!.Id;
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
                                    }
                                    else if (DateTime.Now.Date > new DateTime(year, 12, 15))
                                    {
                                        agencyReqField.Establishment!.PermitStatusId = 9;
                                        agencyReqField.Establishment!.ExpiryDate = new DateTime(DateTime.Now.AddYears(1).Year, 12, 31);
                                    }
                                    else
                                    {
                                        agencyReqField.Establishment!.PermitStatusId = 9;
                                        agencyReqField.Establishment!.ExpiryDate = new DateTime(year, 12, 31);
                                    }
                                    

                                    FoodRenewalHistory history = new FoodRenewalHistory();
                                    history.EstablishmentId = agencyReqField.Establishment!.Id;
                                    history.ActivationDate = agencyReqField.Establishment!.ActivationDate.Value;
                                    history.ExpiryDate = agencyReqField.Establishment!.ExpiryDate.Value;
                                    history.IsActive = true;
                                    history.IsCurrentYear = true;
                                    await _foodRenewalHistory.AddAsync(history);
                                    
                                    if (agencyReqField.Establishment!.RiskCategory == "High")
                                    {
                                        agencyReqField.Establishment!.NextInspectionDate = (agencyReqField.Establishment!.ActivationDate ?? DateTime.Now.Date).AddDays(120);
                                    }
                                    else if (agencyReqField.Establishment!.RiskCategory == "Medium")
                                    {
                                        agencyReqField.Establishment!.NextInspectionDate = (agencyReqField.Establishment!.ActivationDate ?? DateTime.Now.Date).AddDays(180);
                                    }
                                    else if (agencyReqField.Establishment!.RiskCategory == "Low")
                                    {
                                        agencyReqField.Establishment!.NextInspectionDate = (agencyReqField.Establishment!.ActivationDate ?? DateTime.Now.Date).AddDays(360);
                                    }
                                    var assignedTo = 2;
                                    if (agencyReqField.Area != null)
                                    {
                                        var areawiseIns = await _areaWiseInspector.GetFirstOrDefaultAsync(filter: x => x.AreaId == agencyReqField.Area.Id);
                                        if (areawiseIns != null)
                                        {
                                            assignedTo = areawiseIns.AssignedUserId;
                                        }
                                    }
                                    Schedule autoschedule = new Schedule();
                                    autoschedule.EstablishmentId = agencyReqField.Establishment.Id;
                                    autoschedule.ScheduledDate = agencyReqField.Establishment.NextInspectionDate!.Value;
                                    autoschedule.PurposeId = 3;
                                    autoschedule.StatusId = 2;
                                    autoschedule.AssignedTo = assignedTo;
                                    autoschedule.CreatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                                    autoschedule.CreatedOn = DateTime.Now;
                                    autoschedule.SyncDate = DateTime.Now;
                                    autoschedule.IsAdhoc = false;
                                    autoschedule.IsAutoSchedule = true;
                                    await _scheduler.AddAsync(autoschedule);
                                    agencyReqField.Establishment.SyncDate = DateTime.Now;
                                    await _establishments.UpdateAsync(agencyReqField.Establishment!);
                              }
                        }
                        
                        await _openinginsData.AddAsync(openingData);
                        var schedule = await _scheduler.GetFirstOrDefaultAsync(filter: x => x.Id == inspection.ScheduleId);
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


            [HttpPost("/SaveOpeningInspectionItems", Name = "Opening_Inspection_Items")]
            public async Task<ActionResult<APIResponse>> SaveInspectionItems([FromForm] OpeningInspectionItemDetailsRequestDTO model)
            {
                  if (model == null)
                  {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        _response.Message = string.Empty;
                        return BadRequest(_response);
                  }
                //_response.StatusCode = HttpStatusCode.OK;
                //_response.IsSuccess = true;
                //_response.Message = "Success";
                //_response.Result = model;
                ////return Json(new { Success = true, response = _response });
                //return Json(new { success = true, response = _response });

                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                  var str = TokenValidator.Validation(token);
                  if (str == "Authorized")
                  {
                        var inspectionDetail = new InspectionItemDetails();
                        inspectionDetail.InspectionId = model.InspectionId;
                        inspectionDetail.ItemId = model.ItemId;
                        inspectionDetail.Status = model.Status;

                        //_response.StatusCode = HttpStatusCode.OK;
                        //_response.IsSuccess = true;
                        //_response.Message = "Success";
                        //_response.Result = inspectionDetail;
                        ////return Json(new { Success = true, response = _response });
                        //return Json(new { success = true, response = _response });

                        try
                        {
                              await _inspectionItems.AddAsync(inspectionDetail);
                        }
                        catch (Exception ex)
                        {
                            _response.StatusCode = HttpStatusCode.InternalServerError;
                            _response.IsSuccess = false;
                            _response.Message = ex.StackTrace!.ToString();
                            _response.Result = "";
                            //return Ok(_response);
                            return Json(new { Success = false, response = _response });
                        }

                        //if(inspectionDetail.Status== "OUT")
                        //{
                        //    var openingData = await _openinginsData.GetFirstOrDefaultAsync(filter:x=>x.InspectionId==model.InspectionId);
                        //}

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

            [HttpGet("/GetOpeningCheckDataPdf", Name = "GetOpeningCheckData")]
            public async Task<ActionResult<APIResponse>> GetOpeningCheckDataPdf(string id)
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

                        if (inspection.UpdatedOn == null)
                        {
                              model.InspectionDetails.Date = inspection.InspectionDate.ToShortDateString() + "/" + TimeZoneInfo.ConvertTime(inspection.TimeIn, centralZone).ToShortTimeString() + "-" + TimeZoneInfo.ConvertTime(inspection.TimeOut, centralZone).ToShortTimeString();
                              //model.InspectionDetails.TimeIn = TimeZoneInfo.ConvertTime(inspection.TimeIn, centralZone).ToShortTimeString();
                              //model.InspectionDetails.TimeOut = TimeZoneInfo.ConvertTime(inspection.TimeOut, centralZone).ToShortTimeString();
                        }
                        else
                        {
                              model.InspectionDetails.Date = inspection.InspectionDate.ToShortDateString() + "/" + inspection.TimeIn.ToShortTimeString() + "-" + inspection.TimeOut.ToShortTimeString();
                              //model.InspectionDetails.TimeIn = inspection.TimeIn.ToShortTimeString();
                              //model.InspectionDetails.TimeOut = inspection.TimeOut.ToShortTimeString();
                        }

                        
                        
                        model.InspectionDetails.InspectedBy = user.FirstName + " " + user.LastName;
                        model.InspectionDetails.ReceivedBy = openingInspectionDatas.PersonInCharge;
                        model.InspectionDetails.ReceivedBySign = openingInspectionDatas.PersonInChargeSign;
                        model.InspectionDetails.InspectedBySign = inspection.InspectedBySign;
                        model.InspectionDetails.InspectedBySignFile = inspection.InspectorSignFile;
                        model.InspectionDetails.SecondInspector = openingInspectionDatas.SecondaryInspector;
                        model.InspectionDetails.SecondInspectorSigns = openingInspectionDatas.SecondaryInspectorSign;
                        model.InspectionDetails.SecondInspectorSignFile = openingInspectionDatas.SecondaryInspectorSignFile;
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


            [NonAction]
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
      }
}
