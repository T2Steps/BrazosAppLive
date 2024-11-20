using BrazosApp.DataAccess.Repository.IRepository;
using BrazosApp.Models;
using BrazosApp.Models.ViewModels;
using BrazosApp.Utility.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace BrazosApp.Controllers
{
    public class ApplicationController : Controller
    {
        private readonly IRepository<ApplicationType> _applicationtype;
        private readonly IRepository<ApplicationFor> _applicationfor;
        private readonly IRepository<Application> _application;
        private readonly IRepository<Establishment> _establishment;
        private readonly IRepository<EstablishmentOwner> _owner;
        private readonly IRepository<RFOperationDetails> _operationdetails;
        private readonly IRepository<Document> _document;
        private readonly IRepository<BusinessType> _businesstype;
        private readonly IRepository<OperationType> _operationtype;
        private readonly IRepository<Users> _users;
        private readonly IRepository<WaterSource> _waterSource;
        private readonly IRepository<PublicSewage> _publicSewage;
        private readonly IRepository<PrivateSeptic> _privateSeptic;

        private readonly IWebHostEnvironment _environment;
        private readonly IEncrypt _encrypt;
        private readonly string Container = @"\PermitDocuments\Food\";

        public ApplicationController(IRepository<ApplicationType> applicationtype,
            IRepository<ApplicationFor> applicationfor,
            IRepository<Application> application,
            IRepository<Establishment> establishment,
            IRepository<EstablishmentOwner> owner,
            IRepository<RFOperationDetails> operationdetails,
            IRepository<Document> document,
            IRepository<BusinessType> businesstype,
            IRepository<OperationType> operationtype,
            IRepository<Users> users,
            IRepository<WaterSource> waterSource,
            IRepository<PublicSewage> publicSewage,
            IRepository<PrivateSeptic> privateSeptic,
            IEncrypt encrypt,
            IWebHostEnvironment environment)
        {
            _applicationtype = applicationtype;
            _applicationfor = applicationfor;
            _application = application;
            _establishment = establishment;
            _owner = owner;
            _operationdetails = operationdetails;
            _document = document;
            _businesstype = businesstype;
            _operationtype = operationtype;
            _users = users;
            _waterSource = waterSource;
            _publicSewage = publicSewage;
            _privateSeptic = privateSeptic;
            _environment = environment;
            _encrypt = encrypt;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Route("/RFNewPermit/{id?}")]
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
            OnlineRFNewPermitVM model = new OnlineRFNewPermitVM();
            model.EncryptedApplicationId = id;
            model.ApplicationId = Convert.ToInt32(AppId);
            model.ApplicationForId = Application.ApplicationForId;
            model.Establishment = new Establishment();
            model.Owner = new EstablishmentOwner();
            model.OperationDetails = new RFOperationDetails();
            model.Document = new Document();
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

        [Route("/RFNewPermit")]
        [HttpPost]
        public async Task<IActionResult> NewPermit(OnlineRFNewPermitVM model)
        {
            if (ModelState.IsValid)
            {
                var application = await _application.GetFirstOrDefaultAsync(filter:x=>x.Id== model.ApplicationId);
                
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

                    if (flg == 1)
                    {
                        //var Application = await _application.GetFirstOrDefaultAsync(filter: x => x.Id == model.Establishment.ApplicationId);
                        
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

        [Route("/GetAllDocs/{id?}")]
        [HttpGet]
        public async Task<IActionResult> GetAllDocs(string id)
        {
            //var documents = await _document.GetAllAsync(filter: x => x.EstablishmentId == id);
            var EstId = _encrypt.Decrypt256(id);
            TimeZoneInfo centralZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
            var documents = from d in await _document.GetAllAsync(filter: x => x.EstablishmentId == Convert.ToInt32(EstId))
                            join u in await _users.GetAllAsync()
                            on d.UploadedBy equals u.Id into egroup
                            from u in egroup.DefaultIfEmpty()
                            select new
                            {
                                id = d.Id,
                                description = d.Description,
                                associatedNote = d.AssociatedNote,
                                docFileName = d.DocFileName,
                                uploadedBy = u != null ? u.FirstName + " " + u.LastName : "Applicant",
                                uploadedOn = TimeZoneInfo.ConvertTime(d.ReceivedDate, centralZone).ToShortDateString() + "  " + TimeZoneInfo.ConvertTime(d.ReceivedDate, centralZone).ToShortTimeString(),
                                encryptedId = _encrypt.Encrypt256(Convert.ToString(d.Id))
                            };

            return Json(new { data = documents.ToList(), count = documents.Count() });
        }

        [Route("/UploadDoc/{id?}")]
        [HttpPost]
        public async Task<IActionResult> DocsUpload(string EstId, Document model)
        {
            if (ModelState.IsValid)
            {
                if (model.DocFile != null)
                {
                    var EId = _encrypt.Decrypt256(EstId);
                    model.EstablishmentId = Convert.ToInt32(EId);
                    //model.EstablishmentId = id;
                    bool bReturn = await _document.AddAsync(model);

                    //var Est = await _establishment.GetFirstOrDefaultAsync(x=>x.Id== model.EstablishmentId);

                    var uploadPath = _environment.WebRootPath + Container + model.EstablishmentId;
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    var fileName = model.EstablishmentId + "-" + model.Id + Path.GetExtension(model.DocFile.FileName);
                    var filePath = Path.Combine(uploadPath, fileName);
                    var fs = new FileStream(filePath, FileMode.Create);
                    model.DocFile.CopyTo(fs);
                    fs.Close();
                    //fs.Dispose();
                    model.DocFileName = fileName;
                    model.UploadedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    await _document.UpdateAsync(model);

                    return Json(new { success = true });
                }
                return Json(new { success = false });
            }
            return Json(new { success = false });
        }

        [Route("/DeleteDoc/{id?}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteDocs(string id)
        {
            var docId = _encrypt.Decrypt256(id);
            var doc = await _document.GetById(Convert.ToInt32(docId));
            if (doc == null)
            {
                return Json(new { success = false });
            }
            var FileName = doc.DocFileName;
            var path = _environment.WebRootPath + Container + doc.EstablishmentId + "\\";
            var ExistingFile = Path.Combine(path, FileName!);

            if (System.IO.File.Exists(ExistingFile))
            {
                int retries = 3;
                int delayMilliseconds = 500;

                for (int i = 0; i < retries; i++)
                {
                    try
                    {
                        System.IO.File.Delete(ExistingFile);
                        break;
                    }
                    catch (IOException)
                    {
                        Thread.Sleep(delayMilliseconds);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        break;
                    }
                }
                await _document.RemoveAsync(doc);
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [Route("/ViewDoc/{id?}/{file?}")]
        [HttpGet]
        public async Task<IActionResult> ViewDoc(string id, string file)
        {
            if (id == null || file == null)
            {
                return NotFound();
            }
            var EstId = _encrypt.Decrypt256(id);
            var File = _encrypt.Decrypt256(file);
            var doc = await _document.GetById(Convert.ToInt32(File));
            var ext = Path.GetExtension(doc.DocFileName);
            ViewBag.Id = EstId;
            ViewBag.FileName = doc.DocFileName;
            ViewBag.Ext = ext;
            return View();
        }
    }
}
