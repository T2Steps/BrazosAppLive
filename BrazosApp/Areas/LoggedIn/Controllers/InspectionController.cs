using BrazosApp.DataAccess.Repository.IRepository;
using BrazosApp.Models;
using BrazosApp.Models.APIDTOs;
using BrazosApp.Models.DTOs;
using BrazosApp.Models.DTOs.ViewModels;
using BrazosApp.Models.ViewModels;
using BrazosApp.Utility;
using BrazosApp.Utility.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using Wkhtmltopdf.NetCore;
using InspectionRequestDTO = BrazosApp.Models.ViewModels.InspectionRequestDTO;

namespace BrazosApp.Areas.LoggedIn.Controllers
{
    [Area("LoggedIn")]
    [Authorize("CommonPolicy")]
    public class InspectionController : Controller
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

        public InspectionController(IRepository<Item> item,
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

        [HttpGet("/Inspections/{code?}")]
        public async Task<IActionResult> Index(string code)
        {
            InspectionSearchParamsVM model = new InspectionSearchParamsVM();
            var purposes = await _purpose.GetAllAsync();
            model.PurposeList = purposes.Select(x => new SelectListItem
            {
                Text = x.Name!.ToString(),
                Value = x.Name.ToString()
            }).ToList();
            var users = await _users.GetAllAsync();
            model.UserList = users.Select(x => new SelectListItem
            {
                  Text = x.FirstName! + " " + x.LastName,
                  Value = x.Id.ToString()
            }).ToList();
            ViewBag.Code = code;
            return View(model);
        }

        [HttpPost("/Inspections/GetAllInspections/{code?}")]
        public async Task<IActionResult> AdminGetAllInspections(InspectionSearchParamsVM model, string code)
        {
            //IEnumerable<dynamic> InspectionList = new List<dynamic>();

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
                                     InspectionDateStr = d.InspectionDate.ToShortDateString(),
                                     InspectedByValue = d.InspectedBy,
                                     InspectedBy = i.FirstName + " " + i.LastName,
                                     Purpose = p.Name,
                                     FollowUp = d.FollowUp,
                                     FollowUpDate = d.FollowUpDate,
                                     FollowUpDateStr = d.FollowUpDate==null? "" : d.FollowUpDate!.Value.ToShortDateString(),
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
                InspectionFinalList = InspectionFinalList.Where(x => x.Purpose != null && x.Purpose.ToLower().Contains(model.Purpose.ToLower()));
            }

            if (model.Inspector != null && model.Inspector != "--Select User--" && model.Inspector != "undefined")
            {
                  InspectionFinalList = InspectionFinalList.Where(x => x.InspectedByValue != 0 && x.InspectedByValue == Convert.ToInt32(model.Inspector));
            }

            if (model.FollowUp != null && model.FollowUp != "--Follow Up--")
            {
                  if (model.FollowUp == "Yes")
                  {
                        InspectionFinalList = InspectionFinalList.Where(x => x.FollowUp ==true);
                  }
                  else if(model.FollowUp == "No")
                  {
                        InspectionFinalList = InspectionFinalList.Where(x => x.FollowUp ==false);
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

            return Json(new { data = InspectionFinalList.ToList() });
        }


        [HttpGet("/EditInspection/{id?}")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return Json(new { success = false, msg = "Not Found" });
            }
            else
            {
                var InspectionId = Convert.ToInt32(_encrypt.Decrypt256(id));
                EditInspectionVM model = new EditInspectionVM();
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
                return View(model);
            }
        }





        [HttpGet("/CreateInspection/{id?}")]
        public async Task<IActionResult> Create(string id)
        {
            InspectionResponseDTO model = new InspectionResponseDTO();

            if (id != "Null" && id != null)
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
                    model.Code = "MF";
                }
            }
            else
            {
                model.Code = Request.Query["code"].ToString();
            }

            var user = await _users.GetFirstOrDefaultAsync(filter: x => x.Id == Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)));


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
                    var result = model.details?.FirstOrDefault(rel => rel.ItemId == item.Id);
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

            var users = await _users.GetAllAsync(filter:x=>x.IsActive==true && x.IsDelete==false && (x.Role!.Name==SD.SuperAdmin || x.Role.Name == SD.AdminInspector || x.Role.Name == SD.Inspector) && x.Id!=Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)), includeProperties:"Role");
            model.UserList = users.Select(x => new SelectListItem
            {
                Text = (x.FirstName + " " + x.LastName).ToString(),
                Value = x.Id.ToString()
            }).ToList();
            InspectionCreateVM viewModel = new InspectionCreateVM();
            viewModel.Response = model;

            return View(viewModel);
        }

        [HttpGet("/GetSecondInsSignFile/{id?}")]
        public async Task<IActionResult> GetSecondInsSignFile(string? id)
        {
            var user = await _users.GetFirstOrDefaultAsync(filter: x => x.Id == Convert.ToInt32(id));
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                var isSignFilePresent = user.SignFileName != null ? "Present" : "Absent";
                return Json(new {success=true, isSignFilePresent = isSignFilePresent, signFile =  user.SignFileName});
            }
        }


        [HttpPost("/SaveInspection")]

        public async Task<IActionResult> SaveInspection(InspectionRequestDTO model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var inspection = new Inspection();
                    var schedule = new Schedule();
                    if (model.ScheduleId == 0)
                    {
                        Schedule autoSchedule = new Schedule();
                        autoSchedule.EstablishmentId = model.EstablishmentId;
                        autoSchedule.PurposeId = model.PurposeId;
                        autoSchedule.StatusId = 2;
                        autoSchedule.AssignedTo = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                        autoSchedule.ScheduledDate = DateTime.Now;
                        autoSchedule.CreatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                        autoSchedule.IsAdhoc = true;
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
                    schedule.Establishment = await _establishments.GetFirstOrDefaultAsync(filter:x=>x.Id== schedule.EstablishmentId);

                    inspection.EstablishmentId = model.EstablishmentId;
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
                    inspection.InspectedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    inspection.InspectorSignFile = model.InspectorSignFile;
                    inspection.InspectedBySign = model.InspectedBySign;
                    inspection.Comment = model.Comment;
                    inspection.IsPermitSuspended = model.PermitSuspend;
                    try
                    {
                        await _inspection.AddAsync(inspection);
                    }
                    catch(Exception ex)
                    {

                    }
                    

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
                        sch.AssignedTo = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                        sch.StatusId = 2;
                        sch.CreatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                        sch.CreatedOn = DateTime.Now;
                        sch.SyncDate = DateTime.Now;
                        await _scheduler.AddAsync(sch);
                    }

                    if (model.PermitSuspend == true)
                    {
                        var agencyReqField = await _agencyreqFields.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == inspection.EstablishmentId, includeProperties: "Establishment");
                        if(agencyReqField.Establishment!.PermitStatusId >= 9 && agencyReqField.Establishment!.PermitStatusId!=13 && agencyReqField.Establishment!.IsActive != false)
                        {
                            agencyReqField.Establishment!.OldPermitStatusId = agencyReqField.Establishment!.PermitStatusId;
                            agencyReqField.Establishment!.PermitStatusId = 13;
                            agencyReqField.Establishment!.IsActive = false;
                            agencyReqField.Establishment.SyncDate = DateTime.Now;
                            await _establishments.UpdateAsync(agencyReqField.Establishment!);
                        }                        
                        
                    }
                    //else
                    //{
                    //    //var est = await _establishments.GetFirstOrDefaultAsync(filter:x=>x.Id==model.EstablishmentId);
                    //    if (agencyReqField.Establishment!.PermitStatusId == 8)
                    //    {
                    //        agencyReqField.Establishment!.OldPermitStatusId = 8;
                    //        agencyReqField.Establishment!.PermitStatusId = 9;
                    //    }
                    //    await _establishments.UpdateAsync(agencyReqField.Establishment!);
                    //    //openingData.PermitApproval = true;
                    //}
                    await _rfmfInspectionData.AddAsync(inspectionData);

                    //if (schedule.IsAutoSchedule)
                    //{
                    //      var nextInsDate = DateTime.Now;
                    //      if (schedule.Establishment!.RiskCategory == "High")
                    //      {
                    //            schedule.Establishment!.NextInspectionDate = schedule.Establishment!.NextInspectionDate!.Value.AddDays(120);
                    //      }
                    //      else if (schedule.Establishment!.RiskCategory == "Medium")
                    //      {
                    //            schedule.Establishment!.NextInspectionDate = schedule.Establishment!.NextInspectionDate!.Value.AddDays(180);
                    //      }
                    //      else if (schedule.Establishment!.RiskCategory == "Low")
                    //      {
                    //            schedule.Establishment!.NextInspectionDate = schedule.Establishment!.NextInspectionDate!.Value.AddDays(360);
                    //      }
                    //      schedule.Establishment!.LastInspectionDate = DateTime.Now;
                    //      var area = await _area.GetFirstOrDefaultAsync(filter: x => x.AreaNumber == schedule.Establishment!.Area);
                    //      var assignedTo = 2;
                    //      if (area != null)
                    //      {
                    //            var areawiseIns = await _areaWiseInspector.GetFirstOrDefaultAsync(filter: x => x.AreaId == area.Id);
                    //            if (areawiseIns != null)
                    //            {
                    //                  assignedTo = areawiseIns.AssignedUserId;
                    //            }
                    //      }
                    //      Schedule autosch = new Schedule();
                    //      autosch.EstablishmentId = schedule.EstablishmentId;
                    //      autosch.ScheduledDate = schedule.Establishment!.NextInspectionDate!.Value;
                    //      autosch.PurposeId = 3;
                    //      autosch.StatusId = 2;
                    //      autosch.AssignedTo = assignedTo;
                    //      autosch.CreatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    //      autosch.CreatedOn = DateTime.Now;
                    //      autosch.IsAdhoc = false;
                    //      autosch.IsAutoSchedule = true;
                    //      autosch.SyncDate = DateTime.Now;
                    //      await _scheduler.AddAsync(autosch);

                    //      schedule.Establishment.SyncDate = DateTime.Now;
                    //      await _establishments.UpdateAsync(schedule.Establishment!);
                    //}

                    //var nextInsDate = DateTime.Now;
                    //var schedule = 
                    
                    if (inspection.PurposeId == 3 && inspection.ParentInspectionId==null)
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

                    return Json(new { success = true, msg = "Successfully Updated", inspectionId = inspection.Id });
                }
            }

            return Json(new { success = false, msg = "Unexpected Error Occurred" });
        }



        [HttpPost("/SaveInspectionEdit")]
        public async Task<IActionResult> SaveInspectionEdit(InspectionRequestDTO model)
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
                    
                    if (inspection.FollowUp != true && model.FollowUp==true)
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
                        //inspection.FollowUpDate = model.FollowUpDate ?? DateTime.Now.AddDays(7);
                    }
                    inspection.FollowUp = model.FollowUp;
                    var agencyReqField = await _agencyreqFields.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == inspection.EstablishmentId, includeProperties: "Establishment");
                    if (model.PermitSuspend == true)
                    {
                        if(agencyReqField.Establishment!.PermitStatusId >= 9 && agencyReqField.Establishment!.PermitStatusId!=13 && agencyReqField.Establishment!.IsActive != false)
                        {
                            agencyReqField.Establishment!.OldPermitStatusId = agencyReqField.Establishment!.PermitStatusId==10?10:9;
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
                            agencyReqField.Establishment!.PermitStatusId = agencyReqField.Establishment!.OldPermitStatusId == 10? 10: 9;                            
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

                    return Json(new { success = true, msg = "Successfully Updated", inspectionId = model.InspectionId });
                }
            }

            return Json(new { success = false, msg = "Unexpected Error Occurred" });
        }

        [HttpPost("/RemoveAllItems")]
        public async Task<IActionResult> RemoveAllItems(int InspectionId)
        {
            var InspectionItems = await _inspectionItems.GetAllAsync(filter: x => x.InspectionId == InspectionId);
            if (InspectionItems.Any())
            {
                foreach (var item in InspectionItems)
                {
                    await _inspectionItems.RemoveAsync(item);
                }
            }
            return Json(new { success = true });
        }

        [HttpPost("/RemoveAllTemps")]
        public async Task<IActionResult> RemoveAllTemps(int InspectionId)
        {
            var TempObs = await _temperatureObs.GetAllAsync(filter: x => x.InspectionId == InspectionId);
            if (TempObs.Any())
            {
                foreach (var item in TempObs)
                {
                    await _temperatureObs.RemoveAsync(item);
                }
            }
            return Json(new { success = true });
        }

        [HttpPost("/AdminPanelSaveInspectionItems")]
        public async Task<IActionResult> SaveInspectionItems(InspectionItemDetails model)
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
            return Json(new { success = true });
        }

        [HttpPost("/AdminPanelSaveTemperatureObs")]
        public async Task<IActionResult> SaveTemperatureObs(TemperatureObservation model)
        {
            if (ModelState.IsValid)
            {
                await _temperatureObs.AddAsync(model);
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpGet("/GetEstOwnerEmail/{InspectionId?}")]
        public async Task<IActionResult> GetEstOwnerEmail(string InspectionId)
        {
            var InsId = Convert.ToInt32(_encrypt.Decrypt256(InspectionId));
            var inspection = await _inspection.GetFirstOrDefaultAsync(filter: x => x.Id == InsId, includeProperties: "Purpose");
            if (inspection == null)
            {
                return Json(new { success = false });
            }
            else
            {
                var owner = await _owner.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == inspection.EstablishmentId);
                if (owner == null)
                {
                    return Json(new { success = false });
                }
                else
                {
                    return Json(new { success = true, email = owner.EmailId, id = InspectionId, purpose = inspection.Purpose!.Name });
                }
            }
        }

        [HttpGet("/DownloadInspectionCommentCertificatePdf/{id?}")]
        public async Task<IActionResult> DownloadPermitCertificatePdf(string id)
        {
            var InspectionID = _encrypt.Decrypt256(id);
            InspectionCommentPdfDTO model = new InspectionCommentPdfDTO();
            model.Inspection = await _inspection.GetFirstOrDefaultAsync(x => x.Id == Convert.ToInt32(InspectionID), includeProperties: "Establishment");
            if (model.Inspection == null) { return NotFound(); }
            model.InspectionData = await _rfmfInspectionData.GetFirstOrDefaultAsync(filter: x => x.InspectionId == Convert.ToInt32(InspectionID));
            var user = await _users.GetAllAsync();
            model.InspectedBy = (from u in user where u.Id == model.Inspection.InspectedBy select (u.FirstName + " " + u.LastName)).FirstOrDefault();
            model.SecondaryInspectedBy = (from u in user where u.Id == model.Inspection.InspectedBy select (u.FirstName + " " + u.LastName)).FirstOrDefault();
            var generatedPdf = await _generatePdf.GetByteArray("Views/PdfTemplates/InspectionCommentPagePdf.cshtml", model);
            Response.ContentType = "application/pdf";
            Response.Headers.Add("Content-Disposition", "attachment; filename=InspectionCommentSection.pdf");
            return new FileContentResult(generatedPdf, "application/pdf");
        }

    }
}
