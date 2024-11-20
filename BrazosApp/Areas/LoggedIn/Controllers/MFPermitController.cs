using BrazosApp.Models;
using BrazosApp.Models.ViewModels;
using BrazosApp.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace BrazosApp.Areas.LoggedIn.Controllers
{
    public partial class PermitController : Controller
    {
            

        [Route("/MFNewMPermit")]
        [HttpGet]
        public async Task<IActionResult> NewMFPermit()
        {
            OnlineMFPermitVM model = new OnlineMFPermitVM();
            model.Establishment = new Establishment();
            model.Owner = new EstablishmentOwner();
            model.OperationDetails = new MFOperationDetails();
            model.VehicleInformation = new MFVehicleInformation();
            model.Document = new Document();
            var BusinessTypes = await _businesstype.GetAllAsync(filter: x => x.IsActive == true);
            var OperationTypes = await _operationtype.GetAllAsync(filter: x => x.IsActive == true);
            var WaterSources = await _waterSource.GetAllAsync(filter: x => x.IsActive == true);
            model.PublicWaterSourceList = WaterSources.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString(),
            }).ToList();
            model.BusinessTypesEng = BusinessTypes.Where(x => x.Code == "MF").Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString(),
            }).ToList();
            model.BusinessTypesSp = BusinessTypes.Where(x => x.Code == "MF").Select(x => new SelectListItem
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
            model.MFOperationLocationsList = new List<MFOperationLocations>();
            if (model.MFOperationLocations != null && model.MFOperationLocations.Count() > 0)
            {
                foreach (var item in model.MFOperationLocations)
                {
                    var result = model.MFOperationLocations?.FirstOrDefault();
                    model.MFOperationLocationsList!.Add(new MFOperationLocations
                    {
                        EstablishmentId = 0,
                        Location = item.Location,
                    });
                }
            }
            return View(model);

        }

        [Route("/MFNewMPermit")]
        [HttpPost]
        public async Task<IActionResult> NewMFPermit(OnlineMFPermitVM model)
        {
            if (ModelState.IsValid)
            {
                Application application = new Application();
                if (model.Owner != null && model.Establishment != null && model.Establishment.Id == 0)
                {
                    application.ApplicationForId = 3;
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
                        //model.Establishment!.PermitNumber = "MF" + "-" + (existingEst.Count() + 1).ToString("D6");
                        //model.Establishment.ApplicationId = model.ApplicationId;
                        //model.Establishment.ApplicationForId = model.ApplicationForId;
                        model.Establishment.ApplicationId = application.Id;
                        model.Establishment.Area = 0;
                        model.Establishment.ApplicationForId = 3;
                        model.Establishment.PermitStatusId = 3;
                        model.Establishment.OldPermitStatusId = 2;
                        model.Establishment.CreatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                        model.Establishment.SyncDate = DateTime.Now;
                    }
                    else
                    {
                        var Est = await _establishment.GetFirstOrDefaultAsync(filter: x => x.Id == model.Establishment.Id, isTracking: false);
                        model.Establishment.ApplicationId = Est.ApplicationId;
                        //model.Establishment.ApplicationForId = model.ApplicationForId;
                        model.Establishment.ApplicationForId = 3;
                        //model.Establishment.PermitNumber = Est.PermitNumber;
                        model.Establishment.Area = Est.Area;
                        model.Establishment.PermitStatusId = Est.PermitStatusId;
                        model.Establishment.IsActive = Est.IsActive;
                        model.Establishment.CreatedBy = Est.CreatedBy;
                        model.Establishment.CreatedOn = Est.CreatedOn;
                        model.Establishment.UpdatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                        model.Establishment.UpdatedOn = DateTime.Now;
                        model.Establishment.SyncDate = DateTime.Now;
                    }

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

                    if (model.VehicleInformation != null)
                    {
                        if (flg == 1)
                        {
                            model.VehicleInformation.EstablishmentId = model.Establishment.Id;
                            model.VehicleInformation.CreatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)); 
                            await _vehicleinformation.AddAsync(model.VehicleInformation);
                        }
                        else
                        {
                            var Vehicle = await _vehicleinformation.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == model.Establishment.Id, isTracking: false);
                            model.VehicleInformation.Id = Vehicle.Id;
                            model.VehicleInformation.EstablishmentId = Vehicle.EstablishmentId;
                            model.VehicleInformation.CreatedOn = Vehicle.CreatedOn;
                            model.VehicleInformation.CreatedBy = Vehicle.CreatedBy;
                            model.VehicleInformation.IsActive = Vehicle.IsActive;
                            model.VehicleInformation.UpdatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)); 
                            model.VehicleInformation.UpdatedOn = DateTime.Now;
                            try
                            {
                                await _vehicleinformation.UpdateAsync(model.VehicleInformation);
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
                            await _mfoperationalDetails.AddAsync(model.OperationDetails);
                        }
                        else
                        {
                            var OperationDetail = await _mfoperationalDetails.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == model.Establishment.Id, isTracking: false);
                            model.OperationDetails.Id = OperationDetail.Id;
                            model.OperationDetails.EstablishmentId = OperationDetail.EstablishmentId;
                            model.OperationDetails.IsActive = OperationDetail.IsActive;
                            model.OperationDetails.CreatedBy = OperationDetail.CreatedBy;
                            model.OperationDetails.CreatedOn = OperationDetail.CreatedOn;
                            model.OperationDetails.UpdatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)); 
                            model.OperationDetails.UpdatedOn = DateTime.Now;

                            try
                            {
                                await _mfoperationalDetails.UpdateAsync(model.OperationDetails);
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }

                    if (model.MFOperationLocationsList!=null)
                    {
                        if (model.MFOperationLocationsList.Any())
                        {
                            if (flg == 1)
                            {
                                foreach(var item in model.MFOperationLocationsList)
                                {
                                    var MFOperationLocation = new MFOperationLocations()
                                    {
                                        EstablishmentId = model.Establishment.Id,
                                        Location = item.Location,
                                    };
                                    
                                    await _mfoperationalLocations.AddAsync(MFOperationLocation);

                                }
                            }
                            else
                            {
                                var locations = await _mfoperationalLocations.GetAllAsync(filter:x=>x.EstablishmentId==model.Establishment.Id);
                                foreach (var item in locations)
                                {
                                    await _mfoperationalLocations.RemoveAsync(item);
                                }
                                foreach (var item in model.MFOperationLocationsList)
                                {
                                    var MFOperationLocation = new MFOperationLocations()
                                    {
                                        EstablishmentId = model.Establishment.Id,
                                        Location = item.Location,
                                    };

                                    await _mfoperationalLocations.AddAsync(MFOperationLocation);

                                }

                            }
                        }
                    }

                    if (flg == 1)
                    {
                        //var Application = await _application.GetFirstOrDefaultAsync(filter: x => x.Id == model.Establishment.ApplicationId);
                        //Application.Status = 3;
                        //try
                        //{
                        //    await _application.UpdateAsync(Application);
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



        [Route("/MFEdit/{id?}")]
        [HttpGet]
        public async Task<IActionResult> MFEdit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                var EiD = "";
                try
                {
                    EiD = _encrypt.Decrypt256(id);
                }
                catch (Exception ex)
                {
                    return Redirect("/Not_Found");
                }
                        
                MFPermitEditVM model = new MFPermitEditVM();
                model.EncryptedEstId = id;
                model.Establishment = await _establishment.GetFirstOrDefaultAsync(x => x.Id == Convert.ToInt32(EiD));

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

                model.Owner = await _owner.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == Convert.ToInt32(EiD));
                if (model.Owner == null)
                {
                   model.Owner = new EstablishmentOwner();
                }
                model.OperationDetails = await _mfoperationalDetails.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == Convert.ToInt32(EiD));
                if(model.OperationDetails == null)
                {
                    model.OperationDetails = new MFOperationDetails();
                }
                        
                model.VehicleInformation = await _vehicleinformation.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == Convert.ToInt32(EiD));
                if (model.VehicleInformation == null)
                {
                   model.VehicleInformation = new MFVehicleInformation();
                }
                model.Document = new Document();
                var BusinessTypes = await _businesstype.GetAllAsync(filter: x => x.IsActive == true);
                var OperationTypes = await _operationtype.GetAllAsync(filter: x => x.IsActive == true);
                var WaterSources = await _waterSource.GetAllAsync(filter: x => x.IsActive == true);
                var EstablishmentSizes = await _size.GetAllAsync(filter: x => x.IsActive == true);
                var RiskCategories = await _riskcategory.GetAllAsync();
                //var Areas = await _area.GetAllAsync(filter: x => x.IsActive == true && x.AreaNumber >= 1 && x.AreaNumber <= 7);
                var Areas = await _area.GetAllAsync(filter: x => x.IsActive == true && x.AreaNumber != 51 /*&& x.AreaNumber <= 7*/);
                var Jurisdictions = await _jurisdictionaccounts.GetAllAsync(filter: x => x.ProgramId == 2);

                model.BusinessTypesEng = BusinessTypes.Where(x => x.Code == "MF").Select(x => new SelectListItem
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

                model.planReviewVM = new PlanReviewVM();
                model.planReviewVM.Code = "MF";
                model.AddPaymentDTO = new AddPaymentDTO();
                model.AddPaymentDTO!.FeesList = new List<FeesDetailsTable>();
                model.AddPaymentDTO.EstId = model.Establishment.Id;
                model.AddPaymentDTO.code = "MF";
                //var random = new Random();
                //model.AddPaymentDTO.InvoiceNo = "IN-" + System.DateTime.Now.Year.ToString() + "-" + random.Next(0, 999999).ToString("D6");
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
                    var EstTypes = await _estTypes.GetAllAsync(filter: x => x.JurisdictionId == agencyStaffReqFields.EstablishmentTypes!.JurisdictionId, orderBy: x => x.OrderBy(x => x.SortOrder)/*.ThenBy(x => x.Name)*/);
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

                //var feesList = await _fees.GetAllAsync(filter: x => x.EstablishmentId == Convert.ToInt32(model.Establishment.Id) && x.IsActive == true);
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

                  var InspectionPurposes = await _insPurposes.GetAllAsync(filter: x => x.IsActive == true && (x.Code == "RF" || x.Code == "COM") && x.Name!= "Walk Through");

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

                  model.MFOperationLocations = await _mfoperationalLocations.GetAllAsync(filter:x=>x.EstablishmentId==model.Establishment.Id);
                  model.MFOperationLocationsList = new List<MFOperationLocations>();
                  if (model.MFOperationLocations != null && model.MFOperationLocations.Count() > 0)
                  {
                        foreach (var item in model.MFOperationLocations)
                        {
                              var result = model.MFOperationLocations?.FirstOrDefault();
                              model.MFOperationLocationsList!.Add(new MFOperationLocations
                              {
                                    EstablishmentId = 0,
                                    Location = item.Location,
                              });
                        }
                  }

                  return View(model);
            }
        }

        [Route("/MFEdit")]
        [HttpPost]
        public async Task<IActionResult> MFEdit(MFPermitEditVM model)
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

                        var OperationDetail = await _mfoperationalDetails.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == model.Establishment.Id, isTracking: false);
                        model.OperationDetails.Id = OperationDetail.Id;
                        model.OperationDetails.EstablishmentId = OperationDetail.EstablishmentId;
                        model.OperationDetails.IsActive = OperationDetail.IsActive;
                        model.OperationDetails.CreatedBy = OperationDetail.CreatedBy;
                        model.OperationDetails.CreatedOn = OperationDetail.CreatedOn;
                        model.OperationDetails.UpdatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                        model.OperationDetails.UpdatedOn = DateTime.Now;

                        try
                        {
                                await _mfoperationalDetails.UpdateAsync(model.OperationDetails);
                        }
                        catch (Exception ex)
                        {

                        }
                    }

                    if(model.VehicleInformation != null)
                    {
                        var Vehicle = await _vehicleinformation.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == model.Establishment.Id, isTracking: false);
                        model.VehicleInformation.Id = Vehicle.Id;
                        model.VehicleInformation.EstablishmentId = Vehicle.EstablishmentId;
                        model.VehicleInformation.CreatedOn = Vehicle.CreatedOn;
                        model.VehicleInformation.CreatedBy = Vehicle.CreatedBy;
                        model.VehicleInformation.IsActive = Vehicle.IsActive;
                        model.VehicleInformation.UpdatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                        model.VehicleInformation.UpdatedOn = DateTime.Now;
                        try
                        {
                                await _vehicleinformation.UpdateAsync(model.VehicleInformation);
                        }
                        catch (Exception ex)
                        {

                        }
                    }

                    if (model.MFOperationLocationsList!=null)
                    {
                        if (model.MFOperationLocationsList.Any())
                        {
                            var locations = await _mfoperationalLocations.GetAllAsync(filter:x=>x.EstablishmentId==model.Establishment.Id);
                            if (locations.Any())
                            {
                              foreach (var item in locations)
                              {
                                    await _mfoperationalLocations.RemoveAsync(item);
                              }
                            }
                            
                            foreach (var item in model.MFOperationLocationsList)
                            {
                                var MFOperationLocation = new MFOperationLocations()
                                {
                                     EstablishmentId = model.Establishment.Id,
                                     Location = item.Location,
                                };
                                await _mfoperationalLocations.AddAsync(MFOperationLocation);

                            }
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
    }
}
