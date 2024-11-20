using BrazosApp.DataAccess.Repository.IRepository;
using BrazosApp.Models;
using BrazosApp.Models.ViewModels;
using BrazosApp.Utility.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace BrazosApp.Controllers
{
    public class TFApplicationController : Controller
    {
        private readonly IRepository<ApplicationType> _applicationtype;
        private readonly IRepository<ApplicationFor> _applicationfor;
        private readonly IRepository<Application> _application;
        private readonly IRepository<Events> _events;
        private readonly IRepository<Establishment> _establishment;
        private readonly IRepository<EstablishmentOwner> _owner;
        private readonly IRepository<TFOperationDetails> _operationdetails;        
        private readonly IRepository<Document> _document;
        private readonly IRepository<BusinessType> _businesstype;
        private readonly IRepository<OperationType> _operationtype;
        private readonly IRepository<WaterSource> _waterSource;
        private readonly IWebHostEnvironment _environment;
        private readonly IEncrypt _encrypt;
        private readonly string Container = @"\PermitDocuments\Food\";

        public TFApplicationController(IRepository<ApplicationType> applicationtype,
                IRepository<ApplicationFor> applicationfor,
                IRepository<Application> application,
                IRepository<Events> events,
                IRepository<Establishment> establishment,
                IRepository<EstablishmentOwner> owner,
                IRepository<TFOperationDetails> operationdetails,
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
            _events = events;
            _establishment = establishment;
            _owner = owner;
            _operationdetails = operationdetails;
            _document = document;
            _businesstype = businesstype;
            _operationtype = operationtype;
            _waterSource = waterSource;
            _environment = environment;
            _encrypt = encrypt;
        }
        [Route("/TFNewPermit/{id?}")]
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
            OnlineTFNewPermitVM model = new OnlineTFNewPermitVM();
            model.EncryptedApplicationId = id;
            model.ApplicationId = Convert.ToInt32(AppId);
            model.ApplicationForId = Application.ApplicationForId;
            model.Establishment = new Establishment();
            model.Owner = new EstablishmentOwner();
            model.OperationDetails = new TFOperationDetails();
            model.Document = new Document();
            return View(model);
                  
        }

        [HttpPost("/EventAutoComplete")]
        public async Task<IActionResult> AutoComplete(string prefix)
        {
            var Events = await _events.GetAllAsync(filter:x=>x.IsActive==true && x.Name!.StartsWith(prefix) && x.EndDate.Date>DateTime.Now.Date);
            var EventsList = (from picker in Events
                           select new
                           {
                               label = picker.Name,
                               val = picker.Id,
                               location = picker.Location,
                               startDt = picker.StartDate,
                               endDt = picker.EndDate,
                           }).ToList();
            return Json(new { Success = true, response = EventsList });
        }

        [HttpPost("/TFNewPermit")]
        public async Task<IActionResult> NewPermit(OnlineTFNewPermitVM model)
        {
            if (ModelState.IsValid)
            {
                var application = await _application.GetFirstOrDefaultAsync(filter: x => x.Id == model.ApplicationId);
                if (model.Establishment != null)
                {
                    var flg = 0;
                    var ExistingEst = await _establishment.GetFirstOrDefaultAsync(filter:x=>x.ApplicationId== model.ApplicationId);
                    if (model.Establishment.Id == 0 && ExistingEst==null)
                    {
                        flg = 1;
                        //var existingEst = await _establishment.GetAllAsync();
                        //model.Establishment!.PermitNumber = "RF" + "-" + (existingEst.Count() + 1).ToString("D6");
                        model.Establishment.ApplicationId = model.ApplicationId;
                        model.Establishment.ApplicationForId = model.ApplicationForId;
                        model.Establishment.Area = 0;
                        //model.Establishment.ActivationDate = Convert.ToDateTime(model.StartDate);
                        //model.Establishment.ExpiryDate = Convert.ToDateTime(model.EndDate);
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
                        //model.Establishment.PermitNumber = Est.PermitNumber;
                        //model.Establishment.ActivationDate = Convert.ToDateTime(model.StartDate);
                        //model.Establishment.ExpiryDate = Convert.ToDateTime(model.EndDate);
                        model.Establishment.PermitStatusId = Est.PermitStatusId;
                        model.Establishment.Area = Est.Area;
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

                    str = model.StartDate!.Split("-");
                    model.Establishment!.ActivationDate = new DateTime(Convert.ToInt32(str[2]), Convert.ToInt32(str[0]), Convert.ToInt32(str[1]));
                    //str1 = str[0] + " " + str[1] + " " + str[2] + " " + str[3];
                    //model.Establishment!.ActivationDate = DateTime.ParseExact(str1!, "ddd MMM dd yyyy", CultureInfo.InvariantCulture);

                    str = model.EndDate!.Split("-");
                    model.Establishment!.ExpiryDate = new DateTime(Convert.ToInt32(str[2]), Convert.ToInt32(str[0]), Convert.ToInt32(str[1]));
                    //str1 = str[0] + " " + str[1] + " " + str[2] + " " + str[3];
                    //model.Establishment!.ExpiryDate = DateTime.ParseExact(str1!, "ddd MMM dd yyyy", CultureInfo.InvariantCulture);

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

                    if (model.OperationDetails != null)
                    {

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
                            model.OperationDetails.EventId = OperationDetail.EventId;
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
    }
}
