using BrazosApp.DataAccess.Repository.IRepository;
using BrazosApp.Models;
using BrazosApp.Models.ViewModels;
using BrazosApp.Utility.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BrazosApp.Controllers
{
    public class MFApplicationController : Controller
      {
            private readonly IRepository<ApplicationType> _applicationtype;
            private readonly IRepository<ApplicationFor> _applicationfor;
            private readonly IRepository<Application> _application;
            private readonly IRepository<Establishment> _establishment;
            private readonly IRepository<EstablishmentOwner> _owner;
            private readonly IRepository<MFOperationDetails> _operationdetails;
            private readonly IRepository<MFVehicleInformation> _vehicleinformation;
            private readonly IRepository<MFOperationLocations> _mfoperationalLocations;
            private readonly IRepository<Document> _document;
            private readonly IRepository<BusinessType> _businesstype;
            private readonly IRepository<OperationType> _operationtype;
            private readonly IRepository<WaterSource> _waterSource;
            private readonly IWebHostEnvironment _environment;
            private readonly IEncrypt _encrypt;
            private readonly string Container = @"\PermitDocuments\Food\";

            public MFApplicationController(IRepository<ApplicationType> applicationtype,
                IRepository<ApplicationFor> applicationfor,
                IRepository<Application> application,
                IRepository<Establishment> establishment,
                IRepository<EstablishmentOwner> owner,
                IRepository<MFOperationDetails> operationdetails,
                IRepository<MFVehicleInformation> vehicleinformation,
                IRepository<MFOperationLocations> mfoperationalLocations,
                IRepository<Document> document,
                IRepository<BusinessType> businesstype,
                IRepository<OperationType> operationtype,
                IRepository<WaterSource> waterSource,
                IEncrypt encrypt,
                IWebHostEnvironment environment)
            {
                  _applicationtype = applicationtype;
                  _applicationfor = applicationfor;
                  _application = application;
                  _establishment = establishment;
                  _owner = owner;
                  _vehicleinformation= vehicleinformation;
                  _operationdetails = operationdetails;
                  _mfoperationalLocations = mfoperationalLocations;
                  _document = document;
                  _businesstype = businesstype;
                  _operationtype = operationtype;
                  _waterSource = waterSource;
                  _environment = environment;
                  _encrypt = encrypt;
            }

            [HttpPost("/MFAutoComplete")]
            public async Task<JsonResult> AutoComplete(string prefix)
            {

                  var est = await _establishment.GetAllAsync(filter: x => x.PermitStatusId == 9  && x.PermitNumber!.StartsWith("RF") && (x.Name!.StartsWith(prefix)|| x.Address!.StartsWith(prefix)));
                  var estlist = (from picker in est
                                 select new
                                 {
                                       label = picker.Name + ", " + picker.Address + "," + picker.City + "," + picker.State + "," + picker.Zip,
                                       val = picker.Id,
                                 }).ToList();

                  return Json(estlist);
                  //return null;
            }


            [Route("/MFNewPermit/{id?}")]
            public async Task<IActionResult> NewPermit(string id)
            {
                  var AppId = "";
                  
                  try
                  {
                    AppId = _encrypt.Decrypt256(id);
                  }
                  catch (Exception ex)
                  { 
                    return Redirect("/Not_Found");
                  }
                  var Application = await _application.GetFirstOrDefaultAsync(filter: x => x.Id == Convert.ToInt32(AppId), includeProperties: "ApplicationFor");
                  if (Application.Status == 3)
                  {
                        return Redirect("/Verification?id=" + Application.Id);
                  }
                  ViewBag.Lang = Application.ApplicationFor!.LanguageTypeId;
                  ViewBag.Owner = Application.OwnerName;
                  ViewBag.Email = Application.EmailId;
                  ViewBag.Phone = Application.ContactNumber;
                  OnlineMFPermitVM model = new OnlineMFPermitVM();
                  model.EncryptedApplicationId = id;
                  model.ApplicationId = Convert.ToInt32(AppId);
                  model.ApplicationForId = Application.ApplicationForId;
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

            [Route("/MFNewPermit")]
            [HttpPost]
            public async Task<IActionResult> NewPermit(OnlineMFPermitVM model)
            {
                  if (ModelState.IsValid)
                  {
                        var application = await _application.GetFirstOrDefaultAsync(filter: x => x.Id == model.ApplicationId);
                        if (model.Establishment != null)
                        {
                              var flg = 0;
                              var ExistingEst = await _establishment.GetFirstOrDefaultAsync(filter: x => x.ApplicationId == model.ApplicationId);
                              if (model.Establishment.Id == 0 && ExistingEst == null)
                              {
                                    flg = 1;
                                    //var existingEst = await _establishment.GetAllAsync();
                                    //model.Establishment!.PermitNumber = "MF" + "-" + (existingEst.Count() + 1).ToString("D6");
                                    model.Establishment.ApplicationId = model.ApplicationId;
                                    model.Establishment.ApplicationForId = model.ApplicationForId;
                                    model.Establishment.Area = 0;
                                    //model.Establishment.PermitStatusId = 1;
                                    model.Establishment.PermitStatusId = 2;
                                    model.Establishment.CreatedBy = 0;
                                    model.Establishment.SyncDate = DateTime.Now;
                              }
                              else
                              {
                                    var Est = await _establishment.GetFirstOrDefaultAsync(filter: x => x.Id == model.Establishment.Id, isTracking: false);
                                    model.Establishment.ApplicationId = model.ApplicationId;
                                    model.Establishment.ApplicationForId = model.ApplicationForId;
                                    model.Establishment.Area = Est.Area;    
                                    //model.Establishment.PermitNumber = Est.PermitNumber;
                                    model.Establishment.PermitStatusId = Est.PermitStatusId;
                                    model.Establishment.IsActive = Est.IsActive;
                                    model.Establishment.CreatedBy = Est.CreatedBy;
                                    model.Establishment.CreatedOn = Est.CreatedOn;
                                    model.Establishment.UpdatedBy = 0;
                                    model.Establishment.UpdatedOn = DateTime.Now;
                                    model.Establishment.SyncDate = DateTime.Now;
                              }

                              //var str = model.ApplicationDt!.Split(" ");
                              //var str1 = str[0] + " " + str[1] + " " + str[2] + " " + str[3];
                              //model.Establishment!.ApplicantSignDate = DateTime.ParseExact(str1!, "ddd MMM dd yyyy", CultureInfo.InvariantCulture);
                              var str = model.ApplicationDt!.Split("-");
                              model.Establishment!.ApplicantSignDate = new DateTime(Convert.ToInt32(str[2]), Convert.ToInt32(str[0]), Convert.ToInt32(str[1]));

                              if (flg == 1)
                              {
                                    await _establishment.AddAsync(model.Establishment);
                                    application.Status = 3;
                                    try
                                    {
                                          await _application.UpdateAsync(application);
                                    }
                                    catch (Exception ex) { }
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
                                          model.Owner.CreatedBy = 0;
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
                                          model.Owner.UpdatedBy = 0;
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

                              if(model.VehicleInformation != null)
                              {
                                    if (flg == 1)
                                    {
                                          model.VehicleInformation.EstablishmentId = model.Establishment.Id;
                                          model.VehicleInformation.CreatedBy = 0;
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
                                          model.VehicleInformation.UpdatedBy = 0;
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
                                          //str = model.CertExpiryDt!.Split(" ");
                                          //str1 = str[0] + " " + str[1] + " " + str[2] + " " + str[3];
                                          //model.OperationDetails!.CertificateExpirationDt = DateTime.ParseExact(str1!, "ddd MMM dd yyyy", CultureInfo.InvariantCulture);
                                          str = model.CertExpiryDt!.Split("-");
                                          model.OperationDetails!.CertificateExpirationDt = new DateTime(Convert.ToInt32(str[2]), Convert.ToInt32(str[0]), Convert.ToInt32(str[1]));
                                    }


                                    if (flg == 1)
                                    {
                                          model.OperationDetails.EstablishmentId = model.Establishment.Id;
                                          model.OperationDetails.CreatedBy = 0;
                                          await _operationdetails.AddAsync(model.OperationDetails);
                                    }
                                    else
                                    {
                                          var OperationDetail = await _operationdetails.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == model.Establishment.Id, isTracking: false);
                                          model.OperationDetails.Id = OperationDetail.Id;
                                          model.OperationDetails.EstablishmentId = OperationDetail.EstablishmentId;
                                          model.OperationDetails.IsActive = OperationDetail.IsActive;
                                          model.OperationDetails.CreatedBy = OperationDetail.CreatedBy;
                                          model.OperationDetails.CreatedOn = OperationDetail.CreatedOn;
                                          model.OperationDetails.UpdatedBy = 0;
                                          model.OperationDetails.UpdatedOn = DateTime.Now;

                                          try
                                          {
                                                await _operationdetails.UpdateAsync(model.OperationDetails);
                                          }
                                          catch (Exception ex)
                                          {

                                          }
                                    }
                              }

                              if (model.MFOperationLocationsList != null)
                              {
                                    if (model.MFOperationLocationsList.Any())
                                    {
                                          if (flg == 1)
                                          {
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
                                          else
                                          {
                                                var locations = await _mfoperationalLocations.GetAllAsync(filter: x => x.EstablishmentId == model.Establishment.Id);
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
      }
}
