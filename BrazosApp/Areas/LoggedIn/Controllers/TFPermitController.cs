using BrazosApp.Models.ViewModels;
using BrazosApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;
using System.Security.Claims;
using BrazosApp.Utility.Helpers;

namespace BrazosApp.Areas.LoggedIn.Controllers
{
    public partial class PermitController : Controller
    {

        [HttpGet("/TFNewMPermit")]
        public IActionResult NewTFPermit()
        {
            OnlineTFNewPermitVM model = new OnlineTFNewPermitVM();
            model.Establishment = new Establishment();
            model.Owner = new EstablishmentOwner();
            model.OperationDetails = new TFOperationDetails();
            model.Document = new Document();
            return View(model);
        }

        [HttpPost("/TFNewMPermit")]
        public async Task<IActionResult> NewTFPermit(OnlineTFNewPermitVM model)
        {
            if (ModelState.IsValid)
            {
                Application application = new Application();
                if (model.Owner != null && model.Establishment != null && model.Establishment.Id == 0)
                {
                    application.ApplicationForId = 9;
                    application.ApplicationDate = DateTime.Now;
                    application.OwnerName = model.Owner.Name;
                    application.ContactNumber = model.Owner.ContactNo;
                    application.EmailId = model.Owner.EmailId;
                    application.Status = 3;
                    var ApplicationCount = await _application.GetAllAsync();
                    application.ApplicationNo = "BCHD" + "-" + (ApplicationCount.Count() + 1).ToString("D6");
                    await _application.AddAsync(application);
                }

                if (model.Establishment != null && model.Owner != null && model.OperationDetails != null)
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
                        model.Establishment.ApplicationId = application.Id;
                        model.Establishment.Area = 0;
                        model.Establishment.ApplicationForId = 9;
                        model.Establishment.PermitStatusId = 3;
                        model.Establishment.OldPermitStatusId = 2;
                        model.Establishment.CreatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                        

                        model.Owner.CreatedOn = DateTime.Now;
                        model.Owner.CreatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                        model.Owner.IsActive = true;

                        model.OperationDetails.CreatedOn = DateTime.Now;
                        model.OperationDetails.CreatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                        model.OperationDetails.IsActive = true;

                    }
                    else
                    {
                        var Est = await _establishment.GetFirstOrDefaultAsync(filter: x => x.Id == model.Establishment.Id, isTracking: false);
                        model.Establishment.ApplicationId = Est.ApplicationId;
                        model.Establishment.ApplicationForId = 9;
                        model.Establishment.Area = Est.Area;
                        model.Establishment.PermitStatusId = Est.PermitStatusId;
                        model.Establishment.OldPermitStatusId = Est.OldPermitStatusId;
                        model.Establishment.IsActive = Est.IsActive;
                        model.Establishment.CreatedBy = Est.CreatedBy;
                        model.Establishment.CreatedOn = Est.CreatedOn;
                        model.Establishment.UpdatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                        model.Establishment.UpdatedOn = DateTime.Now;


                        var Owner = await _owner.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == model.Establishment.Id, isTracking: false);
                        model.Owner.Id = Owner.Id;
                        model.Owner.EstablishmentId = Owner.EstablishmentId;
                        model.Owner.CreatedOn = Owner.CreatedOn;
                        model.Owner.CreatedBy = Owner.CreatedBy;
                        model.Owner.IsActive = Owner.IsActive;
                        model.Owner.UpdatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                        model.Owner.UpdatedOn = DateTime.Now;

                        var OperationDetail = await _tfoperationalDetails.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == model.Establishment.Id, isTracking: false);
                        model.OperationDetails.Id = OperationDetail.Id;
                        model.OperationDetails.EstablishmentId = OperationDetail.EstablishmentId;
                        model.OperationDetails.IsActive = OperationDetail.IsActive;
                        model.OperationDetails.CreatedBy = OperationDetail.CreatedBy;
                        model.OperationDetails.CreatedOn = OperationDetail.CreatedOn;
                        model.OperationDetails.UpdatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                        model.OperationDetails.UpdatedOn = DateTime.Now;
                    }

                    var str = model.StartDate!.Split("-");
                    model.Establishment!.ActivationDate = new DateTime(Convert.ToInt32(str[2]), Convert.ToInt32(str[0]), Convert.ToInt32(str[1]));

                    str = model.EndDate!.Split("-");
                    model.Establishment!.ExpiryDate = new DateTime(Convert.ToInt32(str[2]), Convert.ToInt32(str[0]), Convert.ToInt32(str[1]));

                    model.Establishment.SyncDate = DateTime.Now;

                    if (flg == 1)
                    {
                        try
                        {
                            await _establishment.AddAsync(model.Establishment);
                            try
                            {
                                model.Owner.EstablishmentId = model.Establishment.Id;
                                await _owner.AddAsync(model.Owner);
                                try
                                {
                                    model.OperationDetails.EstablishmentId = model.Establishment.Id;
                                    await _tfoperationalDetails.AddAsync(model.OperationDetails);
                                }
                                catch(Exception ex)
                                {
                                    await _owner.RemoveAsync(model.Owner);
                                    await _establishment.RemoveAsync(model.Establishment);
                                }
                            }
                            catch(Exception ex)
                            {
                                await _establishment.RemoveAsync(model.Establishment);
                            }
                        }
                        catch(Exception ex)
                        {
                            await _establishment.RemoveAsync(model.Establishment);
                        }
                    }
                    else
                    {
                        try
                        {
                            await _establishment.UpdateAsync(model.Establishment);
                            await _owner.UpdateAsync(model.Owner);
                            await _tfoperationalDetails.UpdateAsync(model.OperationDetails);
                        }
                        catch (Exception ex)
                        {

                        }
                    }

                    if (flg == 1)
                    {
                        return Json(new { success = true, id = model.Establishment.Id, encryptid = _encrypt.Encrypt256(Convert.ToString(model.Establishment.Id)), encryptOperationId = _encrypt.Encrypt256(Convert.ToString(model.OperationDetails.Id)), type = "Create" });
                    }
                    else
                    {
                        return Json(new { success = true, type = "Update" });
                    }
                }
            }
            return Json(new { success = false, id = 0 });
        }

        //[HttpPost("/TFNewMPermit")]
        //public async Task<IActionResult> NewTFPermit(OnlineTFNewPermitVM model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        Application application = new Application();
        //        if (model.Owner != null && model.Establishment != null && model.Establishment.Id == 0)
        //        {
        //            application.ApplicationForId = 9;
        //            application.ApplicationDate = DateTime.Now;
        //            //application.ApplicationDate = DateTime.Now;
        //            //application.ApplicationDate = model.ApplicationDate ?? DateTime.Now;
        //            application.OwnerName = model.Owner.Name;
        //            application.ContactNumber = model.Owner.ContactNo;
        //            application.EmailId = model.Owner.EmailId;
        //            application.Status = 3;
        //            var ApplicationCount = await _application.GetAllAsync();
        //            application.ApplicationNo = "BCHD" + "-" + (ApplicationCount.Count() + 1).ToString("D6");
        //            await _application.AddAsync(application);
        //        }

        //        if (model.Establishment != null)
        //        {
        //            var flg = 0;
        //            if (model.Establishment.Id == 0)
        //            {
        //                flg = 1;
        //                //var existingEst = await _establishment.GetAllAsync();
        //                //model.Establishment!.PermitNumber = "RF" + "-" + (existingEst.Count() + 1).ToString("D6");
        //                model.Establishment.ApplicationId = application.Id;
        //                model.Establishment.Area = 0;
        //                model.Establishment.ApplicationForId = 9;
        //                model.Establishment.PermitStatusId = 3;
        //                model.Establishment.OldPermitStatusId = 2;
        //                model.Establishment.CreatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
        //            }
        //            else
        //            {
        //                var Est = await _establishment.GetFirstOrDefaultAsync(filter: x => x.Id == model.Establishment.Id, isTracking: false);
        //                //model.Establishment.ApplicationId = model.ApplicationId;
        //                //model.Establishment.ApplicationForId = model.ApplicationForId;
        //                model.Establishment.ApplicationId = Est.ApplicationId;
        //                model.Establishment.ApplicationForId = 9;
        //                model.Establishment.Area = Est.Area;
        //                //model.Establishment.PermitNumber = Est.PermitNumber;
        //                model.Establishment.PermitStatusId = Est.PermitStatusId;
        //                model.Establishment.IsActive = Est.IsActive;
        //                model.Establishment.CreatedBy = Est.CreatedBy;
        //                model.Establishment.CreatedOn = Est.CreatedOn;
        //                model.Establishment.UpdatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
        //                model.Establishment.UpdatedOn = DateTime.Now;
        //            }

        //            var str = model.StartDate!.Split("-");
        //            model.Establishment!.ActivationDate = new DateTime(Convert.ToInt32(str[2]), Convert.ToInt32(str[0]), Convert.ToInt32(str[1]));

        //            str = model.EndDate!.Split("-");
        //            model.Establishment!.ExpiryDate = new DateTime(Convert.ToInt32(str[2]), Convert.ToInt32(str[0]), Convert.ToInt32(str[1]));


        //            if (flg == 1)
        //            {
        //                model.Establishment.SyncDate = DateTime.Now;
        //                await _establishment.AddAsync(model.Establishment);
        //            }
        //            else
        //            {
        //                try
        //                {
        //                    await _establishment.UpdateAsync(model.Establishment);
        //                }
        //                catch (Exception ex)
        //                {

        //                }
        //            }

        //            if (model.Owner != null)
        //            {
        //                if (flg == 1)
        //                {
        //                    model.Owner.EstablishmentId = model.Establishment.Id;
        //                    model.Owner.CreatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
        //                    await _owner.AddAsync(model.Owner);
        //                }
        //                else
        //                {
        //                    var Owner = await _owner.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == model.Establishment.Id, isTracking: false);
        //                    model.Owner.Id = Owner.Id;
        //                    model.Owner.EstablishmentId = Owner.EstablishmentId;
        //                    model.Owner.CreatedOn = Owner.CreatedOn;
        //                    model.Owner.CreatedBy = Owner.CreatedBy;
        //                    model.Owner.IsActive = Owner.IsActive;
        //                    model.Owner.UpdatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
        //                    model.Owner.UpdatedOn = DateTime.Now;
        //                    try
        //                    {
        //                        await _owner.UpdateAsync(model.Owner);
        //                    }
        //                    catch (Exception ex)
        //                    {

        //                    }

        //                }
        //            }

        //            if (model.OperationDetails != null)
        //            {
        //                //if (model.CertExpiryDt != null)
        //                //{
        //                //      var str = model.CertExpiryDt!.Split(" ");
        //                //      var str1 = str[0] + " " + str[1] + " " + str[2] + " " + str[3];
        //                //      model.OperationDetails!.CertificateExpirationDt = DateTime.ParseExact(str1!, "ddd MMM dd yyyy", CultureInfo.InvariantCulture);
        //                //}


        //                if (flg == 1)
        //                {
        //                    model.OperationDetails.EstablishmentId = model.Establishment.Id;
        //                    model.OperationDetails.CreatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
        //                    await _tfoperationalDetails.AddAsync(model.OperationDetails);
        //                }
        //                else
        //                {
        //                    var OperationDetail = await _tfoperationalDetails.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == model.Establishment.Id, isTracking: false);
        //                    model.OperationDetails.Id = OperationDetail.Id;
        //                    model.OperationDetails.EstablishmentId = OperationDetail.EstablishmentId;
        //                    model.OperationDetails.IsActive = OperationDetail.IsActive;
        //                    model.OperationDetails.CreatedBy = OperationDetail.CreatedBy;
        //                    model.OperationDetails.CreatedOn = OperationDetail.CreatedOn;
        //                    model.OperationDetails.UpdatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
        //                    model.OperationDetails.UpdatedOn = DateTime.Now;

        //                    try
        //                    {
        //                        await _tfoperationalDetails.UpdateAsync(model.OperationDetails);
        //                    }
        //                    catch (Exception ex)
        //                    {

        //                    }
        //                }
        //            }

        //            if (flg == 1)
        //            {
        //                return Json(new { success = true, id = model.Establishment.Id, encryptid = _encrypt.Encrypt256(Convert.ToString(model.Establishment.Id)), type = "Create" });
        //            }
        //            else
        //            {
        //                return Json(new { success = true, type = "Update" });
        //            }
        //        }
        //    }
        //    return Json(new { success = false, id = 0 });
        //    //return Ok();
        //}


        [HttpGet("/TFEdit/{id?}")]
        public async Task<IActionResult> TFEdit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                var OperationalId = "";
                var EstId = 0;
                try
                {
                    OperationalId = _encrypt.Decrypt256(id);
                }
                catch (Exception ex)
                {
                    return Redirect("/Not_Found");
                }

                TFPermitEditVM model = new TFPermitEditVM();
                model.OperationDetails = await _tfoperationalDetails.GetFirstOrDefaultAsync(filter:x=>x.Id== Convert.ToInt32(OperationalId), includeProperties: "Event");
                if (model.OperationDetails==null) { return NotFound(); }
                else
                {
                    EstId = model.OperationDetails.EstablishmentId;
                }

                model.Establishment = await _establishment.GetFirstOrDefaultAsync(x => x.Id == Convert.ToInt32(EstId));

                var application = await _application.GetById(model.Establishment.ApplicationId.GetValueOrDefault());
                model.ApplicationNo = application.ApplicationNo;

                model.EncryptedEstId = _encrypt.Encrypt256(model.Establishment.Id.ToString());
                model.Owner = await _owner.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == Convert.ToInt32(EstId));
                model.Document = new Document();
                //var EstablishmentSizes = await _size.GetAllAsync(filter: x => x.IsActive == true);
                //var RiskCategories = await _riskcategory.GetAllAsync();
                //var Areas = await _area.GetAllAsync(filter: x => x.IsActive == true && x.AreaNumber >= 1 && x.AreaNumber <= 7);
                var Jurisdictions = await _jurisdictionaccounts.GetAllAsync(filter: x => x.ProgramId == 3);

                

                model.planReviewVM = new PlanReviewVM();
                model.planReviewVM.Code = "TF";
                model.AddPaymentDTO = new AddPaymentDTO();
                model.AddPaymentDTO!.FeesList = new List<FeesDetailsTable>();
                model.AddPaymentDTO.EstId = model.Establishment.Id;
                model.AddPaymentDTO.code = "TF";
                var random = new Random();
                //model.AddPaymentDTO.InvoiceNo = "IN-" + System.DateTime.Now.Year.ToString() + "-" + random.Next(0, 999999).ToString("D6");
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
                model.AddPaymentDTO.InvoiceNo = System.DateTime.Now.Year.ToString() + "-" + monthStr + "-" + random.Next(0, 9999).ToString("D4");

                IEnumerable<AreaWiseInspectors> Inspectors = Enumerable.Empty<AreaWiseInspectors>();
                var agencyStaffReqFields = await _agencystaffreqfields.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == model.Establishment.Id, includeProperties: "EstablishmentTypes");
                if (agencyStaffReqFields == null)
                {
                    model.planReviewVM.agencyStaffReqFields = new AgencyStaffReqFields();
                    model.planReviewVM.agencyStaffReqFields.EstablishmentId = model.Establishment.Id;
                }
                else
                {
                    model.planReviewVM.agencyStaffReqFields = agencyStaffReqFields;
                    Inspectors = await _areawiseIns.GetAllAsync(/*filter: x => x.AreaId == agencyStaffReqFields.AreaId,*/ includeProperties: "AssignedUser");
                    var EstTypes = await _estTypes.GetAllAsync(filter: x => x.JurisdictionId == agencyStaffReqFields.EstablishmentTypes!.JurisdictionId);
                    model.AddPaymentDTO!.FeesList = new List<FeesDetailsTable>();

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

                    if (model.Establishment.ActivationDate.HasValue && (model.Establishment.ActivationDate.Value - DateTime.Now).TotalHours < 72)
                    {
                        model.AddPaymentDTO.LateFine = 60;
                    }
                    else
                    {
                        model.AddPaymentDTO.LateFine = 0;
                    }

                }

                //var feesList = await _fees.GetAllAsync(filter: x => x.EstablishmentId == Convert.ToInt32(model.Establishment.Id) && x.PaymentStatus == 1);
                //model.planReviewVM.feesCount = feesList.Count();

                //model.planReviewVM.EstablishmentSizeList = EstablishmentSizes.Select(x => new SelectListItem
                //{
                //    Text = x.Name,
                //    Value = x.Id.ToString(),
                //}).ToList();

                //model.planReviewVM.RiskCategoryList = RiskCategories.Select(x => new SelectListItem
                //{
                //    Text = x.Name,
                //    Value = x.Id.ToString(),
                //}).ToList();

                //model.planReviewVM.AreaList = Areas.Select(x => new SelectListItem
                //{
                //    Text = x.AreaNumber.ToString(),
                //    Value = x.Id.ToString(),
                //}).ToList();

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
                        model.ScheduleVM.InspectorList = Inspectors.Select(x => new SelectListItem
                        {
                              Text = x.AssignedUser!.FirstName + " " + x.AssignedUser.LastName,
                              Value = x.AssignedUserId.ToString(),
                        }).ToList();
                        model.ScheduleVM.Schedule.EstablishmentId = agencyStaffReqFields.EstablishmentId;
                  }

                  return View(model);
            }
        }


        
        [HttpPost("/TFEdit")]
        public async Task<IActionResult> TFEdit(TFPermitEditVM model)
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
                    model.Establishment.UpdatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    model.Establishment.UpdatedOn = DateTime.Now;
                    model.Establishment.ApplicantSign = establishment.ApplicantSign;
                    model.Establishment.ApplicantSignDate = establishment.ApplicantSignDate;
                    model.Establishment.OldEstId = establishment.OldEstId;
                    model.Establishment.OldPermitStatusId = establishment.OldPermitStatusId;
                    model.Establishment.RiskCategory = establishment.RiskCategory;
                    model.Establishment.Area = establishment.Area;
                    //model.Establishment.ActivationDate = establishment.ActivationDate;
                    //model.Establishment.ExpiryDate = establishment.ExpiryDate;

                    var str = model.StartDate!.Split(" ");
                    var str1 = str[0] + " " + str[1] + " " + str[2] + " " + str[3];
                    model.Establishment!.ActivationDate = DateTime.ParseExact(str1!, "ddd MMM dd yyyy", CultureInfo.InvariantCulture);

                    str = model.EndDate!.Split(" ");
                    str1 = str[0] + " " + str[1] + " " + str[2] + " " + str[3];
                    model.Establishment!.ExpiryDate = DateTime.ParseExact(str1!, "ddd MMM dd yyyy", CultureInfo.InvariantCulture);
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
                        //if (model.CertExpiryDt != null)
                        //{
                        //    var str = model.CertExpiryDt!.Split(" ");
                        //    var str1 = str[0] + " " + str[1] + " " + str[2] + " " + str[3];
                        //    model.OperationDetails!.CertificateExpirationDt = DateTime.ParseExact(str1!, "ddd MMM dd yyyy", CultureInfo.InvariantCulture);
                        //}

                        var OperationDetail = await _tfoperationalDetails.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == model.Establishment.Id, isTracking: false);
                        model.OperationDetails.Id = OperationDetail.Id;
                        model.OperationDetails.EstablishmentId = OperationDetail.EstablishmentId;
                        model.OperationDetails!.EventId = model.OperationDetails.EventId;
                        model.OperationDetails.IsActive = OperationDetail.IsActive;
                        model.OperationDetails.CreatedBy = OperationDetail.CreatedBy;
                        model.OperationDetails.CreatedOn = OperationDetail.CreatedOn;
                        model.OperationDetails.UpdatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                        model.OperationDetails.UpdatedOn = DateTime.Now;

                        try
                        {
                            await _tfoperationalDetails.UpdateAsync(model.OperationDetails);
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
    }
}
