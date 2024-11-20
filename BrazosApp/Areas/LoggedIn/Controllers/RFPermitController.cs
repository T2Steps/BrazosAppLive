using BrazosApp.Models.ViewModels;
using BrazosApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using BrazosApp.Utility;
using Microsoft.Data.SqlClient;
using System.Data;
using ClosedXML.Excel;

namespace BrazosApp.Areas.LoggedIn.Controllers
{
    public partial class PermitController : Controller
    {

        [HttpGet("/RFPermitView")]
        public async Task<IActionResult> ViewPermit(string id)
        {
            RFPermitViewVM model = new RFPermitViewVM();
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                var EstId = "";
                try
                {
                    EstId = _encrypt.Decrypt256(id);
                }
                catch (Exception ex)
                {
                    return Redirect("/Not_Found");
                }

                model.Establishment = await _establishment.GetFirstOrDefaultAsync(x => x.Id == Convert.ToInt32(EstId));

                if (model.Establishment.CreatedBy == 0)
                {
                    model.CreatedBy = "Applicant";
                }
                else
                {
                    var user = await _users.GetFirstOrDefaultAsync(filter: x => x.Id == model.Establishment.CreatedBy);
                    if (user != null)
                    {
                        model.CreatedBy = user.FirstName + " " + user.LastName;
                    }
                    else
                    {
                        model.CreatedBy = string.Empty;
                    }
                }

                var application = await _application.GetById(model.Establishment.ApplicationId.GetValueOrDefault());
                model.ApplicationNo = application.ApplicationNo;
                model.ApplicationDate = application.ApplicationDate;

                model.EncryptedEstId = id;
                model.Owner = await _owner.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == Convert.ToInt32(EstId));
                if (model.Owner == null)
                {
                    model.Owner = new EstablishmentOwner();
                }
                model.OperationDetails = await _rfoperationalDetails.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == Convert.ToInt32(EstId));
                if (model.OperationDetails == null)
                {
                    model.OperationDetails = new RFOperationDetails();
                }
                
                var BusinessTypes = await _businesstype.GetAllAsync(filter: x => x.IsActive == true);
                var OperationTypes = await _operationtype.GetAllAsync(filter: x => x.IsActive == true);
                var EstablishmentSizes = await _size.GetAllAsync(filter: x => x.IsActive == true);
                var RiskCategories = await _riskcategory.GetAllAsync();
                var Areas = await _area.GetAllAsync(filter: x => x.IsActive == true && x.AreaNumber != 51 /*&& x.AreaNumber <= 7*/);
                var Jurisdictions = await _jurisdictionaccounts.GetAllAsync(filter: x => x.ProgramId == 1);

                var WaterSources = await _waterSource.GetAllAsync(filter: x => x.IsActive == true);
                var PublicSewage = await _publicSewage.GetAllAsync(filter: x => x.IsActive == true);
                var PrivateSewage = await _privateSeptic.GetAllAsync(filter: x => x.IsActive == true);

                model.BusinessTypesEng = BusinessTypes.Where(x => x.Code == "RF").Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                }).ToList();

                model.OperationTypesEng = OperationTypes.Where(x => x.Code == "RF").Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                }).ToList();

                model.PublicWaterSourceList = WaterSources.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                }).ToList();
                model.PublicSewageList = PublicSewage.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                }).ToList();
                model.PrivateSeptic = PrivateSewage.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                }).ToList();

                model.planReviewVM = new PlanReviewVM();
                model.planReviewVM.Code = "RF";
                
                

                var agencyStaffReqFields = await _agencystaffreqfields.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == model.Establishment.Id, includeProperties: "EstablishmentTypes");
                if (agencyStaffReqFields == null)
                {
                    model.planReviewVM.agencyStaffReqFields = new AgencyStaffReqFields();
                    model.planReviewVM.agencyStaffReqFields.EstablishmentId = model.Establishment.Id;
                }
                else
                {
                    model.planReviewVM.agencyStaffReqFields = agencyStaffReqFields;
                    //Inspectors = await _areawiseIns.GetAllAsync(filter: x => x.AreaId == agencyStaffReqFields.AreaId, includeProperties: "AssignedUser");
                    var EstTypes = await _estTypes.GetAllAsync(filter: x => x.JurisdictionId == agencyStaffReqFields.EstablishmentTypes!.JurisdictionId, orderBy: x => x.OrderBy(x => x.SortOrder)/*.ThenBy(x=>x.Name)*/);
                }

                //var feesList = await _fees.GetAllAsync(filter: x => x.EstablishmentId == Convert.ToInt32(model.Establishment.Id) && x.PaymentStatus == 1);
                //model.planReviewVM.feesCount = feesList.Count();

                model.planReviewVM.EstablishmentSizeList = EstablishmentSizes.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                }).ToList();

                model.planReviewVM.RiskCategoryList = RiskCategories.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                }).ToList();

                model.planReviewVM.AreaList = Areas.Select(x => new SelectListItem
                {
                    Text = x.AreaNumber.ToString(),
                    Value = x.Id.ToString(),
                }).ToList();

                model.planReviewVM.JurisdictionList = Jurisdictions.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                }).ToList();
            }
            return View(model);
        }


        [Route("/RFNewMPermit")]
        [HttpGet]
        public async Task<IActionResult> NewRFPermit()
        {
            OnlineRFNewPermitVM model = new OnlineRFNewPermitVM();
            model.Establishment = new Establishment();
            model.Owner = new EstablishmentOwner();
            model.OperationDetails = new RFOperationDetails();
            model.Document = new BrazosApp.Models.Document();
            var BusinessTypes = await _businesstype.GetAllAsync(filter: x => x.IsActive == true);
            var OperationTypes = await _operationtype.GetAllAsync(filter: x => x.IsActive == true);
            var WaterSources = await _waterSource.GetAllAsync(filter: x => x.IsActive == true);
            var PublicSewage = await _publicSewage.GetAllAsync(filter: x => x.IsActive == true);
            var PrivateSewage = await _privateSeptic.GetAllAsync(filter: x => x.IsActive == true);
            model.PublicWaterSourceList = WaterSources.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString(),
            }).ToList();
            model.PublicSewageList = PublicSewage.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString(),
            }).ToList();
            model.PrivateSeptic = PrivateSewage.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString(),
            }).ToList();
            model.BusinessTypesEng = BusinessTypes.Where(x => x.Code == "RF").Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString(),
            }).ToList();
            model.BusinessTypesSp = BusinessTypes.Where(x => x.Code == "RF").Select(x => new SelectListItem
            {
                Text = x.SpName,
                Value = x.Id.ToString(),
            }).ToList();
            model.OperationTypesEng = OperationTypes.Where(x => x.Code == "RF").Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString(),
            }).ToList();
            model.OperationTypesSp = OperationTypes.Where(x => x.Code == "RF").Select(x => new SelectListItem
            {
                Text = x.SpName,
                Value = x.Id.ToString(),
            }).ToList();
            return View(model);
        }

        [Route("/RFNewMPermit")]
        [HttpPost]
        public async Task<IActionResult> NewRFPermit(OnlineRFNewPermitVM model)
        {
            if (ModelState.IsValid)
            {
                Application application = new Application();
                if (model.Owner != null && model.Establishment != null && model.Establishment.Id == 0)
                {
                    application.ApplicationForId = 1;
                    application.ApplicationDate = model.ApplicationDate ?? DateTime.Now;
                    application.OwnerName = model.Owner.Name;
                    application.ContactNumber = model.Owner.ContactNo;
                    application.EmailId = model.Owner.EmailId;
                    application.Status = 3;
                    var ApplicationCount = await _application.GetAllAsync();
                    application.ApplicationNo = "BCHD" + "-" + (ApplicationCount.Count() + 1).ToString("D6");
                    await _application.AddAsync(application);
                }

                if (model.Establishment != null)
                {
                    var flg = 0;
                    var ExistingEst = new Establishment();
                    if (application.Id != 0)
                    {
                        ExistingEst = await _establishment.GetFirstOrDefaultAsync(filter: x => x.ApplicationId == application.Id);
                    }

                    if (model.Establishment.Id == 0 && ExistingEst == null)
                    {
                        flg = 1;
                        //var existingEst = await _establishment.GetAllAsync();
                        //model.Establishment!.PermitNumber = "RF" + "-" + (existingEst.Count() + 1).ToString("D6");
                        model.Establishment.ApplicationId = application.Id;
                        model.Establishment.Area = 0;
                        model.Establishment.ApplicationForId = 1;
                        model.Establishment.PermitStatusId = 3;
                        model.Establishment.OldPermitStatusId = 2;
                        model.Establishment.CreatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                        model.Establishment.SyncDate = DateTime.Now;
                    }
                    else
                    {
                        var Est = await _establishment.GetFirstOrDefaultAsync(filter: x => x.Id == model.Establishment.Id, isTracking: false);
                        //model.Establishment.ApplicationId = model.ApplicationId;
                        //model.Establishment.ApplicationForId = model.ApplicationForId;
                        model.Establishment.ApplicationId = Est.ApplicationId;
                        model.Establishment.ApplicationForId = 1;
                        model.Establishment.Area = Est.Area;
                        //model.Establishment.PermitNumber = Est.PermitNumber;
                        model.Establishment.PermitStatusId = Est.PermitStatusId;
                        model.Establishment.IsActive = Est.IsActive;
                        model.Establishment.CreatedBy = Est.CreatedBy;
                        model.Establishment.CreatedOn = Est.CreatedOn;
                        model.Establishment.UpdatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                        model.Establishment.UpdatedOn = DateTime.Now;
                        model.Establishment.SyncDate = DateTime.Now;
                    }

                    //var str = model.ApplicationDt!.Split(" ");
                    //var str1 = str[0] + " " + str[1] + " " + str[2] + " " + str[3];
                    //model.Establishment!.ApplicantSignDate = DateTime.ParseExact(str1!, "ddd MMM dd yyyy", CultureInfo.InvariantCulture);

                    if (flg == 1)
                    {
                        await _establishment.AddAsync(model.Establishment);
                    }
                    else
                    {
                        try
                        {
                            await _establishment.UpdateAsync(model.Establishment);
                        }
                        catch (Exception ex)
                        {

                        }
                    }

                    if (model.Owner != null)
                    {
                        if (flg == 1)
                        {
                            model.Owner.EstablishmentId = model.Establishment.Id;
                            model.Owner.CreatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                            await _owner.AddAsync(model.Owner);
                        }
                        else
                        {
                            var Owner = await _owner.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == model.Establishment.Id, isTracking: false);
                            model.Owner.Id = Owner.Id;
                            model.Owner.EstablishmentId = Owner.EstablishmentId;
                            model.Owner.CreatedOn = Owner.CreatedOn;
                            model.Owner.CreatedBy = Owner.CreatedBy;
                            model.Owner.IsActive = Owner.IsActive;
                            model.Owner.UpdatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                            model.Owner.UpdatedOn = DateTime.Now;
                            try
                            {
                                await _owner.UpdateAsync(model.Owner);
                            }
                            catch (Exception ex)
                            {

                            }

                        }
                    }

                    if (model.OperationDetails != null)
                    {
                        if (model.CertExpiryDt != null)
                        {
                            //var str = model.CertExpiryDt!.Split(" ");
                            //var str1 = str[0] + " " + str[1] + " " + str[2] + " " + str[3];
                            //model.OperationDetails!.CertificateExpirationDt = DateTime.ParseExact(str1!, "ddd MMM dd yyyy", CultureInfo.InvariantCulture);
                            var str = model.CertExpiryDt!.Split("-");
                            model.OperationDetails!.CertificateExpirationDt = new DateTime(Convert.ToInt32(str[2]), Convert.ToInt32(str[0]), Convert.ToInt32(str[1]));
                        }


                        if (flg == 1)
                        {
                            model.OperationDetails.EstablishmentId = model.Establishment.Id;
                            model.OperationDetails.CreatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                            await _rfoperationalDetails.AddAsync(model.OperationDetails);
                        }
                        else
                        {
                            var OperationDetail = await _rfoperationalDetails.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == model.Establishment.Id, isTracking: false);
                            model.OperationDetails.Id = OperationDetail.Id;
                            model.OperationDetails.EstablishmentId = OperationDetail.EstablishmentId;
                            model.OperationDetails.IsActive = OperationDetail.IsActive;
                            model.OperationDetails.CreatedBy = OperationDetail.CreatedBy;
                            model.OperationDetails.CreatedOn = OperationDetail.CreatedOn;
                            model.OperationDetails.UpdatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                            model.OperationDetails.UpdatedOn = DateTime.Now;

                            try
                            {
                                await _rfoperationalDetails.UpdateAsync(model.OperationDetails);
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }

                    if (flg == 1)
                    {
                        //var Application = await _application.GetFirstOrDefaultAsync(filter: x => x.Id == model.Establishment.ApplicationId);
                        //Application.Status = 3;
                        //try
                        //{
                        //      await _application.UpdateAsync(Application);
                        //}
                        //catch (Exception ex) { }
                        return Json(new { success = true, id = model.Establishment.Id, encryptid = _encrypt.Encrypt256(Convert.ToString(model.Establishment.Id)), type = "Create" });
                    }
                    else
                    {
                        return Json(new { success = true, type = "Update" });
                    }
                }
            }
            return Json(new { success = false, id = 0 });

        }


        [Route("/RFEdit/{id?}")]
        [HttpGet]
        public async Task<IActionResult> RFEdit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                var EstId = "";
                try
                {
                    EstId = _encrypt.Decrypt256(id);
                }
                catch (Exception ex)
                {
                    return Redirect("/Not_Found");
                }

                RFPermitEditVM model = new RFPermitEditVM();
                model.Establishment = await _establishment.GetFirstOrDefaultAsync(x => x.Id == Convert.ToInt32(EstId));

                if (model.Establishment.CreatedBy == 0) 
                {
                    model.CreatedBy = "Applicant";
                }
                else 
                {
                    var user = await _users.GetFirstOrDefaultAsync(filter:x=>x.Id==model.Establishment.CreatedBy);
                    if (user != null)
                    {
                        model.CreatedBy = user.FirstName + " " + user.LastName;
                    }
                    else 
                    {
                        model.CreatedBy = string.Empty;
                    }
                }

                var application = await _application.GetById(model.Establishment.ApplicationId.GetValueOrDefault());
                model.ApplicationNo = application.ApplicationNo;
                model.ApplicationDate = application.ApplicationDate;

                model.EncryptedEstId = id;
                model.Owner = await _owner.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == Convert.ToInt32(EstId));
                if (model.Owner == null)
                {
                   model.Owner = new EstablishmentOwner();
                }
                model.OperationDetails = await _rfoperationalDetails.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == Convert.ToInt32(EstId));
                if(model.OperationDetails == null)
                {
                    model.OperationDetails = new RFOperationDetails();
                }
                model.Document = new BrazosApp.Models.Document();
                var BusinessTypes = await _businesstype.GetAllAsync(filter: x => x.IsActive == true);
                var OperationTypes = await _operationtype.GetAllAsync(filter: x => x.IsActive == true);
                var EstablishmentSizes = await _size.GetAllAsync(filter: x => x.IsActive == true);
                var RiskCategories = await _riskcategory.GetAllAsync();
                var Areas = await _area.GetAllAsync(filter: x => x.IsActive == true && x.AreaNumber != 51 /*&& x.AreaNumber <= 7*/);
                var Jurisdictions = await _jurisdictionaccounts.GetAllAsync(filter: x => x.ProgramId == 1);

                var WaterSources = await _waterSource.GetAllAsync(filter: x => x.IsActive == true);
                var PublicSewage = await _publicSewage.GetAllAsync(filter: x => x.IsActive == true);
                var PrivateSewage = await _privateSeptic.GetAllAsync(filter: x => x.IsActive == true);

                model.BusinessTypesEng = BusinessTypes.Where(x => x.Code == "RF").Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                }).ToList();

                model.OperationTypesEng = OperationTypes.Where(x => x.Code == "RF").Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                }).ToList();

                model.PublicWaterSourceList = WaterSources.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                }).ToList();
                model.PublicSewageList = PublicSewage.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                }).ToList();
                model.PrivateSeptic = PrivateSewage.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                }).ToList();

                model.planReviewVM = new PlanReviewVM();
                model.planReviewVM.Code = "RF";
                model.AddPaymentDTO = new AddPaymentDTO();
                model.AddPaymentDTO!.FeesList = new List<FeesDetailsTable>();
                model.AddPaymentDTO.EstId = model.Establishment.Id;
                model.AddPaymentDTO.code = "RF";
                //var random = new Random();
                //model.AddPaymentDTO.InvoiceNo = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month + "-" + random.Next(0, 9999).ToString("D4");
                //var month = System.DateTime.Now.Month;
                //var monthStr = "";
                //if (month < 10)
                //{
                //    monthStr = 0.ToString() + month.ToString();
                //}
                //else
                //{
                //   monthStr = month.ToString();
                //}
                //model.AddPaymentDTO.InvoiceNo = System.DateTime.Now.Year.ToString() + "-" + monthStr + "-" + random.Next(0, 9999).ToString("D4");
                
                //IEnumerable<AreaWiseInspectors> Inspectors = Enumerable.Empty<AreaWiseInspectors>();

                var agencyStaffReqFields = await _agencystaffreqfields.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == model.Establishment.Id, includeProperties: "EstablishmentTypes");
                if (agencyStaffReqFields == null)
                {
                    model.planReviewVM.agencyStaffReqFields = new AgencyStaffReqFields();
                    model.planReviewVM.agencyStaffReqFields.EstablishmentId = model.Establishment.Id;
                }
                else
                {
                    model.planReviewVM.agencyStaffReqFields = agencyStaffReqFields;
                    //Inspectors = await _areawiseIns.GetAllAsync(filter: x => x.AreaId == agencyStaffReqFields.AreaId, includeProperties: "AssignedUser");
                    var EstTypes = await _estTypes.GetAllAsync(filter:x=>x.JurisdictionId==agencyStaffReqFields.EstablishmentTypes!.JurisdictionId, orderBy:x=>x.OrderBy(x=>x.SortOrder)/*.ThenBy(x=>x.Name)*/);
                    model.AddPaymentDTO!.FeesList = new List<FeesDetailsTable>();
                    model.AddPaymentDTO.LateFine = 0;
                    
                    decimal quarter = (decimal)DateTime.Now.Month/3;
                    foreach (var estType in EstTypes)
                    {
                        model.AddPaymentDTO!.FeesList!.Add(new FeesDetailsTable
                        {
                            Amount = (decimal)(quarter <= 1 ? estType.Q1Fees : quarter <= 2 ? estType.Q2Fees : quarter <= 3 ? estType.Q3Fees : quarter <= 4 ? estType.Q4Fees : 0)!,
                            Title = estType.Name,
                            EstablishmentTypeId = estType.Id,
                            IsSelected = false
                        });
                    }
                    
                    model.OfflinePaymentVM = new OfflinePaymentVM();
                    model.OfflinePaymentVM.PaymentSplit = new List<PaymentSplit>();

                }

                //var feesList = await _fees.GetAllAsync(filter: x => x.EstablishmentId == Convert.ToInt32(model.Establishment.Id) && x.PaymentStatus == 1);
                //model.planReviewVM.feesCount = feesList.Count();

                model.planReviewVM.EstablishmentSizeList = EstablishmentSizes.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                }).ToList();

                model.planReviewVM.RiskCategoryList = RiskCategories.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                }).ToList();

                model.planReviewVM.AreaList = Areas.Select(x => new SelectListItem
                {
                    Text = x.AreaNumber.ToString(),
                    Value = x.Id.ToString(),
                }).ToList();

                model.planReviewVM.JurisdictionList = Jurisdictions.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                }).ToList();


                var InspectionPurposes = await _insPurposes.GetAllAsync(filter: x => x.IsActive == true && (x.Code == "RF" || x.Code == "COM"));

                model.ScheduleVM = new ScheduleVM();
                model.ScheduleVM.Schedule = new Schedule();
                model.ScheduleVM.InspectionPurposes = InspectionPurposes.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                }).ToList();
                if (agencyStaffReqFields != null)
                {
                    //model.ScheduleVM.InspectorList = Inspectors.Select(x => new SelectListItem
                    //{
                    //    Text = x.AssignedUser!.FirstName + " " + x.AssignedUser.LastName,
                    //    Value = x.AssignedUserId.ToString(),
                    //}).ToList();
                    var GetInspectorList = await _users.GetAllAsync(filter: x => x.Id == Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)) || x.Role!.Name == SD.Inspector || x.Role!.Name == SD.AdminInspector || x.Role!.Name == SD.SuperAdmin, includeProperties: "Role");
                    model.ScheduleVM.InspectorList = GetInspectorList.Select(x => new SelectListItem
                    {
                        Text = x.FirstName + " " + x.LastName,
                        Value = x.Id.ToString(),
                    }).ToList();
                    model.ScheduleVM.Schedule.EstablishmentId = agencyStaffReqFields.EstablishmentId;
                }

                return View(model);
            }
        }

        [Route("/RFEdit")]
        [HttpPost]
        public async Task<IActionResult> RFEdit(RFPermitEditVM model)
        {
            if (ModelState.IsValid)
            {
                if (model.Establishment != null)
                {
                    var establishment = await _establishment.GetFirstOrDefaultAsync(filter: x => x.Id == model.Establishment.Id, isTracking: false);
                    model.Establishment.ApplicationId = establishment.ApplicationId;
                    model.Establishment.ApplicationForId = establishment.ApplicationForId;
                    model.Establishment.PermitNumber = establishment.PermitNumber;
                    model.Establishment.PermitStatusId = establishment.PermitStatusId;
                    model.Establishment.IsActive = establishment.IsActive;
                    model.Establishment.CreatedBy = establishment.CreatedBy;
                    model.Establishment.CreatedOn = establishment.CreatedOn;
                    model.Establishment.NextInspectionDate = establishment.NextInspectionDate;
                    model.Establishment.LastInspectionDate = establishment.LastInspectionDate;
                    model.Establishment.OldPermitNumber = establishment.OldPermitNumber;
                    model.Establishment.UpdatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    model.Establishment.UpdatedOn = DateTime.Now;
                    model.Establishment.ApplicantSign = establishment.ApplicantSign;
                    model.Establishment.ApplicantSignDate = establishment.ApplicantSignDate;
                    model.Establishment.OldEstId = establishment.OldEstId;
                    model.Establishment.OldPermitStatusId = establishment.OldPermitStatusId;
                    model.Establishment.RiskCategory = establishment.RiskCategory;
                    model.Establishment.Area = establishment.Area;
                    model.Establishment.ActivationDate = establishment.ActivationDate;
                    //model.Establishment.ExpiryDate = establishment.ExpiryDate;
                    model.Establishment.SyncDate = DateTime.Now;
                    model.Establishment.IsAgencyApproved = establishment.IsAgencyApproved;

                    try
                    {
                        await _establishment.UpdateAsync(model.Establishment);
                    }
                    catch (Exception ex)
                    {

                    }

                    if (model.Owner != null)
                    {
                        var Owner = await _owner.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == model.Establishment.Id, isTracking: false);
                        model.Owner.Id = Owner.Id;
                        model.Owner.EstablishmentId = Owner.EstablishmentId;
                        model.Owner.CreatedOn = Owner.CreatedOn;
                        model.Owner.CreatedBy = Owner.CreatedBy;
                        model.Owner.IsActive = Owner.IsActive;
                        model.Owner.UpdatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                        model.Owner.UpdatedOn = DateTime.Now;
                        try
                        {
                            await _owner.UpdateAsync(model.Owner);
                        }
                        catch (Exception ex)
                        {

                        }
                    }

                    if (model.OperationDetails != null)
                    {
                        if (model.CertExpiryDt != null)
                        {
                            //var str = model.CertExpiryDt!.Split(" ");
                            //var str1 = str[0] + " " + str[1] + " " + str[2] + " " + str[3];
                            //model.OperationDetails!.CertificateExpirationDt = DateTime.ParseExact(str1!, "ddd MMM dd yyyy", CultureInfo.InvariantCulture);
                            var str = model.CertExpiryDt!.Split("-");
                            model.OperationDetails!.CertificateExpirationDt = new DateTime(Convert.ToInt32(str[2]), Convert.ToInt32(str[0]), Convert.ToInt32(str[1]));
                        }

                        var OperationDetail = await _rfoperationalDetails.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == model.Establishment.Id, isTracking: false);
                        model.OperationDetails.Id = OperationDetail.Id;
                        model.OperationDetails.EstablishmentId = OperationDetail.EstablishmentId;
                        model.OperationDetails.IsActive = OperationDetail.IsActive;
                        model.OperationDetails.CreatedBy = OperationDetail.CreatedBy;
                        model.OperationDetails.CreatedOn = OperationDetail.CreatedOn;
                        model.OperationDetails.UpdatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                        model.OperationDetails.UpdatedOn = DateTime.Now;

                        try
                        {
                            await _rfoperationalDetails.UpdateAsync(model.OperationDetails);
                        }
                        catch (Exception ex)
                        {

                        }
                    }

                    if (model.Establishment.PermitStatusId == 1)
                    {
                        model.Establishment.PermitStatusId = 2;
                        model.Establishment.OldPermitStatusId = 1;
                        await _establishment.UpdateAsync(model.Establishment);
                        return Json(new { success = true, type = 1 });
                    }
                    else
                    {
                        return Json(new { success = true, type = 2 });
                    }
                }
                return Json(new { success = false });
            }
            else
            {
                return Json(new { success = false });
            }
        }


        [HttpGet("/Renewal/{id?}")]
        public async Task<IActionResult> RFRenewal(string id)
        {
            if(id == null)
            {
                return Json(new { success = false, msg = "Not Found" });
            }
            else
            {
                var EstId = Convert.ToInt32(_encrypt.Decrypt256(id));
                var Est = await _establishment.GetFirstOrDefaultAsync(filter: x => x.Id == EstId, isTracking:false);
                if (Est == null)
                {
                    return Json(new { success = false, msg = "Record Not Found" });
                }
                else
                {
                    
                }
            }

            return Json(new {success = false});
        }


        //[HttpPost("/ExportToExcelReport")]
        //public async Task<IActionResult> ExportToExcelReport([FromBody] List<Dictionary<string, object>> tableData)
        //{
        //    DataTable dataTable = new DataTable("datatable");

        //    // Define columns
        //    dataTable.Columns.Add("EstName", typeof(string));
        //    dataTable.Columns.Add("PermitNumber", typeof(string));
        //    dataTable.Columns.Add("EstAddress", typeof(string));
        //    dataTable.Columns.Add("EstState", typeof(string));
        //    dataTable.Columns.Add("EstCity", typeof(string));
        //    dataTable.Columns.Add("EstZip", typeof(string));
        //    dataTable.Columns.Add("ContactNo", typeof(string));
        //    dataTable.Columns.Add("ActivationDate", typeof(string));
        //    dataTable.Columns.Add("ExpiryDate", typeof(string));
        //    dataTable.Columns.Add("PermitStatus", typeof(string));
        //    dataTable.Columns.Add("RiskCategory", typeof(string));
        //    dataTable.Columns.Add("Area", typeof(string));
        //    dataTable.Columns.Add("NextInspectionDate", typeof(string));
        //    dataTable.Columns.Add("LastInspectionDate", typeof(string));
        //    dataTable.Columns.Add("OwnerName", typeof(string));
        //    dataTable.Columns.Add("OwnerAddress", typeof(string));
        //    dataTable.Columns.Add("OwnerState", typeof(string));
        //    dataTable.Columns.Add("OwnerCity", typeof(string));
        //    dataTable.Columns.Add("OwnerZip", typeof(string));
        //    dataTable.Columns.Add("OwnerContactNo", typeof(string));
        //    dataTable.Columns.Add("Email", typeof(string));
        //    dataTable.Columns.Add("BusinessType", typeof(string));
        //    dataTable.Columns.Add("OperationType", typeof(string));
        //    dataTable.Columns.Add("WithinCityLimit", typeof(string));
        //    dataTable.Columns.Add("NumberOfEmployees", typeof(string));
        //    dataTable.Columns.Add("CertifiedFoodManager", typeof(string));
        //    dataTable.Columns.Add("CertificateExpDt", typeof(string));
        //    dataTable.Columns.Add("PublicSewage", typeof(string));
        //    dataTable.Columns.Add("WaterSource", typeof(string));
        //    dataTable.Columns.Add("PrivateSeptic", typeof(string));
        //    dataTable.Columns.Add("EstablishmentSize", typeof(string));
        //    dataTable.Columns.Add("EstablishmentType", typeof(string));

        //    // Populate DataTable with data from tableData
        //    foreach (var row in tableData)
        //    {
        //        var dataRow = dataTable.NewRow();
        //        foreach (var column in row.Keys)
        //        {
        //            dataRow[column] = row[column]?.ToString();
        //        }
        //        dataTable.Rows.Add(dataRow);
        //    }

        //    // Create Excel file
        //    using (var workbook = new XLWorkbook())
        //    {
        //        workbook.Worksheets.Add(dataTable);

        //        using (var stream = new MemoryStream())
        //        {
        //            workbook.SaveAs(stream);
        //            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Retail_Permits.xlsx");
        //        }
        //    }
        //}


        [HttpPost("/ExportToExcelReport")]
        public async Task<IActionResult> ExportToExcelReport(PermitSearchParamsVM model)
        {
            SqlConnection myConnection = new SqlConnection(_configuration.GetConnectionString("dbConString"));
            DataSet ds = new DataSet();

            using (var workbook = new XLWorkbook())
            {
                //var worksheet = workbook.Worksheets.Add("FilteredData");

                DataTable dataTable = new DataTable();

                dataTable.TableName = "datatable";
                dataTable.Columns.Add("EstName", typeof(string));
                dataTable.Columns.Add("PermitNumber", typeof(string));
                dataTable.Columns.Add("EstAddress", typeof(string));
                dataTable.Columns.Add("EstState", typeof(string));
                dataTable.Columns.Add("EstCity", typeof(string));
                dataTable.Columns.Add("EstZip", typeof(string));
                dataTable.Columns.Add("ContactNo", typeof(string));
                dataTable.Columns.Add("ActivationDate", typeof(string));
                dataTable.Columns.Add("ExpiryDate", typeof(string));
                dataTable.Columns.Add("PermitStatus", typeof(string));
                dataTable.Columns.Add("RiskCategory", typeof(string));
                dataTable.Columns.Add("Area", typeof(string));
                dataTable.Columns.Add("NextInspectionDate", typeof(string));
                dataTable.Columns.Add("LastInspectionDate", typeof(string));
                dataTable.Columns.Add("OwnerName", typeof(string));
                dataTable.Columns.Add("OwnerAddress", typeof(string));
                dataTable.Columns.Add("OwnerState", typeof(string));
                dataTable.Columns.Add("OwnerCity", typeof(string));
                dataTable.Columns.Add("OwnerZip", typeof(string));
                dataTable.Columns.Add("OwnerContactNo", typeof(string));
                dataTable.Columns.Add("Email", typeof(string));
                dataTable.Columns.Add("BusinessType", typeof(string));
                dataTable.Columns.Add("OperationType", typeof(string));
                dataTable.Columns.Add("WithinCityLimit", typeof(string));
                dataTable.Columns.Add("NumberOfEmployees", typeof(string));
                dataTable.Columns.Add("CertifiedFoodManager", typeof(string));
                dataTable.Columns.Add("CertificateExpDt", typeof(string));
                dataTable.Columns.Add("PublicSewage", typeof(string));
                dataTable.Columns.Add("WaterSource", typeof(string));
                dataTable.Columns.Add("PrivateSeptic", typeof(string));
                dataTable.Columns.Add("EstablishmentSize", typeof(string));
                dataTable.Columns.Add("EstablishmentType", typeof(string));


                if (myConnection.State == ConnectionState.Closed)
                    myConnection.Open();

                var myCommand = new SqlCommand("EstablishmentExcelReport", myConnection);
                myCommand.CommandType = CommandType.StoredProcedure;

                myCommand.Parameters.AddWithValue("@Name", model.Name);
                myCommand.Parameters.AddWithValue("@Permit", model.Permit);
                myCommand.Parameters.AddWithValue("@Owner", model.Owner);
                myCommand.Parameters.AddWithValue("@Address", model.Address);
                myCommand.Parameters.AddWithValue("@City", model.City);                
                myCommand.Parameters.AddWithValue("@ApplicationNo", model.City);
                

                if (model.SearchBy != null && model.SearchBy != "--Search By--")
                {
                    myCommand.Parameters.AddWithValue("@SearchBy", model.SearchBy);
                }

                if (model.EstType != null && model.EstType != "--Select Establishment Type--")
                {
                    myCommand.Parameters.AddWithValue("@EstType", model.EstType);
                }

                if (model.FromDate != null)
                {
                    myCommand.Parameters.AddWithValue("@FromDate", model.FromDate);
                }

                if (model.ToDate != null)
                {
                    myCommand.Parameters.AddWithValue("@ToDate", model.ToDate);
                }
                
                if (model.PermitStatus != null && model.PermitStatus != "--Select Permit Status--")
                {
                    myCommand.Parameters.AddWithValue("@PermitStatus", model.PermitStatus);
                }
                
                if (model.Area != null)
                {
                    myCommand.Parameters.AddWithValue("@Area", model.Area);
                }
                if (model.Risk != null && model.Risk != "--Select Risk--")
                {
                    myCommand.Parameters.AddWithValue("@Risk", model.Risk);
                }

                //myCommand.Parameters.AddWithValue("@CustomerID", model.Name);
                //myCommand.ExecuteNonQuery();
                SqlDataAdapter myDataAdapter = new SqlDataAdapter(myCommand);
                myDataAdapter.Fill(ds);

                if (ds.Tables[0].Rows.Count != 0)
                {
                    foreach (DataRow dRow in ds.Tables[0].Rows)
                    {
                        dataTable.Rows.Add(
                                dRow["EstName"].ToString()
                                , dRow["PermitNumber"].ToString()
                                , dRow["EstAddress"].ToString()
                                , dRow["EstState"].ToString()
                                , dRow["EstCity"].ToString()
                                , dRow["EstZip"].ToString()
                                , dRow["ContactNo"].ToString()
                                , dRow["ActivationDate"].ToString()
                                , dRow["ExpiryDate"].ToString()
                                , dRow["PermitStatus"].ToString()
                                , dRow["RiskCategory"].ToString()
                                , dRow["Area"].ToString()
                                , dRow["NextInspectionDate"].ToString()
                                , dRow["LastInspectionDate"].ToString()
                                , dRow["OwnerName"].ToString()
                                , dRow["OwnerAddress"].ToString()
                                , dRow["OwnerState"].ToString()
                                , dRow["OwnerCity"].ToString()
                                , dRow["OwnerZip"].ToString()
                                , dRow["OwnerContactNo"].ToString()
                                , dRow["Email"].ToString()
                                , dRow["BusinessType"].ToString()
                                , dRow["OperationType"].ToString()
                                , dRow["WithinCityLimit"].ToString()
                                , dRow["NumberOfEmployees"].ToString()
                                , dRow["CertifiedFoodManager"].ToString()
                                , dRow["CertificateExpDt"].ToString()
                                , dRow["PublicSewage"].ToString()
                                , dRow["WaterSource"].ToString()
                                , dRow["PrivateSeptic"].ToString()
                                , dRow["EstablishmentSize"].ToString()
                                , dRow["EstablishmentType"].ToString()
                            );
                    }

                    //worksheet.Cell(1, 1).InsertTable(dataTable);
                    workbook.Worksheets.Add(dataTable);
                }
                var stream = new MemoryStream();
                workbook.SaveAs(stream);

                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Retail_Permits.xlsx");
            }
        }


        //[HttpPost]
        //public async Task<IActionResult> ExportToExcelReport()
        //{
        //    SqlConnection myConnection = new SqlConnection(_configuration.GetConnectionString("dbConString"));
        //    DataSet ds = new DataSet();

        //    using (var workbook = new XLWorkbook())
        //    {
        //        //var worksheet = workbook.Worksheets.Add("FilteredData");

        //        DataTable dataTable = new DataTable();

        //        dataTable.TableName = "datatable";
        //        dataTable.Columns.Add("EstName", typeof(string));
        //        dataTable.Columns.Add("PermitNumber", typeof(string));
        //        dataTable.Columns.Add("EstAddress", typeof(string));
        //        dataTable.Columns.Add("EstState", typeof(string));
        //        dataTable.Columns.Add("EstCity", typeof(string));
        //        dataTable.Columns.Add("EstZip", typeof(string));
        //        dataTable.Columns.Add("ContactNo", typeof(string));
        //        dataTable.Columns.Add("ActivationDate", typeof(string));
        //        dataTable.Columns.Add("ExpiryDate", typeof(string));
        //        dataTable.Columns.Add("PermitStatus", typeof(string));
        //        dataTable.Columns.Add("RiskCategory", typeof(string));
        //        dataTable.Columns.Add("Area", typeof(string));
        //        dataTable.Columns.Add("NextInspectionDate", typeof(string));
        //        dataTable.Columns.Add("LastInspectionDate", typeof(string));
        //        dataTable.Columns.Add("OwnerName", typeof(string));
        //        dataTable.Columns.Add("OwnerAddress", typeof(string));
        //        dataTable.Columns.Add("OwnerState", typeof(string));
        //        dataTable.Columns.Add("OwnerCity", typeof(string));
        //        dataTable.Columns.Add("OwnerZip", typeof(string));
        //        dataTable.Columns.Add("OwnerContactNo", typeof(string));
        //        dataTable.Columns.Add("Email", typeof(string));
        //        dataTable.Columns.Add("BusinessType", typeof(string));
        //        dataTable.Columns.Add("OperationType", typeof(string));
        //        dataTable.Columns.Add("WithinCityLimit", typeof(string));
        //        dataTable.Columns.Add("NumberOfEmployees", typeof(string));
        //        dataTable.Columns.Add("CertifiedFoodManager", typeof(string));
        //        dataTable.Columns.Add("CertificateExpDt", typeof(string));
        //        dataTable.Columns.Add("PublicSewage", typeof(string));
        //        dataTable.Columns.Add("WaterSource", typeof(string));
        //        dataTable.Columns.Add("PrivateSeptic", typeof(string));
        //        dataTable.Columns.Add("EstablishmentSize", typeof(string));
        //        dataTable.Columns.Add("EstablishmentType", typeof(string));


        //        if (myConnection.State == ConnectionState.Closed)
        //            myConnection.Open();

        //        var myCommand = new SqlCommand("EstablishmentExcelReport", myConnection);
        //        myCommand.CommandType = CommandType.StoredProcedure;
        //        SqlDataAdapter myDataAdapter = new SqlDataAdapter(myCommand);
        //        myDataAdapter.Fill(ds);

        //        if (ds.Tables[0].Rows.Count != 0)
        //        {
        //            foreach (DataRow dRow in ds.Tables[0].Rows)
        //            {
        //                dataTable.Rows.Add(
        //                        dRow["EstName"].ToString()
        //                        , dRow["PermitNumber"].ToString()
        //                        , dRow["EstAddress"].ToString()
        //                        , dRow["EstState"].ToString()
        //                        , dRow["EstCity"].ToString()
        //                        , dRow["EstZip"].ToString()
        //                        , dRow["ContactNo"].ToString()
        //                        , dRow["ActivationDate"].ToString()
        //                        , dRow["ExpiryDate"].ToString()
        //                        , dRow["PermitStatus"].ToString()
        //                        , dRow["RiskCategory"].ToString()
        //                        , dRow["Area"].ToString()
        //                        , dRow["NextInspectionDate"].ToString()
        //                        , dRow["LastInspectionDate"].ToString()
        //                        , dRow["OwnerName"].ToString()
        //                        , dRow["OwnerAddress"].ToString()
        //                        , dRow["OwnerState"].ToString()
        //                        , dRow["OwnerCity"].ToString()
        //                        , dRow["OwnerZip"].ToString()
        //                        , dRow["OwnerContactNo"].ToString()
        //                        , dRow["Email"].ToString()
        //                        , dRow["BusinessType"].ToString()
        //                        , dRow["OperationType"].ToString()
        //                        , dRow["WithinCityLimit"].ToString()
        //                        , dRow["NumberOfEmployees"].ToString()
        //                        , dRow["CertifiedFoodManager"].ToString()
        //                        , dRow["CertificateExpDt"].ToString()
        //                        , dRow["PublicSewage"].ToString()
        //                        , dRow["WaterSource"].ToString()
        //                        , dRow["PrivateSeptic"].ToString()
        //                        , dRow["EstablishmentSize"].ToString()
        //                        , dRow["EstablishmentType"].ToString()
        //                    );
        //            }

        //            //worksheet.Cell(1, 1).InsertTable(dataTable);
        //            workbook.Worksheets.Add(dataTable);
        //        }
        //        var stream = new MemoryStream();
        //        workbook.SaveAs(stream);

        //        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Retail_Permits.xlsx");
        //    }
        //}
    }
}
