using BrazosAPI.Extensions;
using BrazosAPI.Models;
using BrazosApp.DataAccess.Repository.IRepository;
using BrazosApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BrazosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PermitDataPoolController : Controller
    {
        private readonly IRepository<Establishment> _establishment;
        private readonly IRepository<EstablishmentOwner> _owner;
        private readonly IRepository<RFOperationDetails> _rfoperationaldetails;
        private readonly IRepository<PublicSewage> _publicSewage;
        private readonly IRepository<WaterSource> _waterSource;
        private readonly IRepository<BusinessType> _businessTypes;
        private readonly IRepository<OperationType> _operationTypes;
        private readonly IRepository<MFOperationDetails> _mfoperationaldetails;
        private readonly IRepository<InspectionPurposes> _inspectionPurposes;
        private readonly IRepository<AgencyStaffReqFields> _agencyStaffReqFields;
        private readonly IRepository<Section> _section;
        private readonly IRepository<SubSection> _subSection;
        private readonly IRepository<Item> _items;
        private readonly IRepository<Users> _users;
        private readonly APIResponse _response;
        //private readonly IRepository<Establishment> _establishment;

        public PermitDataPoolController(IRepository<Establishment> establishment,
            IRepository<EstablishmentOwner> owner,
            IRepository<RFOperationDetails> rfoperationaldetails,
            IRepository<PublicSewage> publicSewage,
            IRepository<WaterSource> waterSource,
            IRepository<BusinessType> businessTypes,
            IRepository<OperationType> operationTypes,
            IRepository<MFOperationDetails> mfoperationaldetails,
            IRepository<Section> section,
            IRepository<SubSection> subSection,
            IRepository<Item> items,
            IRepository<Users> users,
            IRepository<InspectionPurposes> inspectionPurposes,
            IRepository<AgencyStaffReqFields> agencyStaffReqFields)
        {
            _establishment = establishment;
            _owner = owner;
            _rfoperationaldetails = rfoperationaldetails;
            _publicSewage = publicSewage;
            _response = new APIResponse();
            _waterSource = waterSource;
            _businessTypes = businessTypes;
            _operationTypes = operationTypes;   
            _mfoperationaldetails = mfoperationaldetails;
            _inspectionPurposes = inspectionPurposes;
            _agencyStaffReqFields = agencyStaffReqFields;
            _section = section;
            _subSection = subSection;
            _items = items;
            _users = users;
        }

        [HttpGet("/GetAllPermits", Name = "GetAllPermits")]
        public async Task<ActionResult<APIResponse>> GetAllPermits(string code, DateTime? SyncDate)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var str = TokenValidator.Validation(token);
            if (str == "Authorized")
            {
                //var Establishments = from est in await _establishment.GetAllAsync(filter: x => x.IsDeleted != true && x.ApplicationFor!.Code == code, includeProperties: "PermitStatus,ApplicationFor")
                //                     join own in await _owner.GetAllAsync()
                //                     on est.Id equals own.EstablishmentId into egroup
                //                     join det in await _rfoperationaldetails.GetAllAsync()
                //                     on est.Id equals det.EstablishmentId into mgroup
                //                     from own in egroup.DefaultIfEmpty().ToList()
                //                     from det in mgroup.DefaultIfEmpty().ToList()
                //                     select new
                //                     {
                //                         id = est.Id,
                //                         name = est.Name,
                //                         address = est.Address,
                //                         state = est.State,
                //                         city = est.City,
                //                         zip = est.Zip,
                //                         contact = est.ContactNo,
                //                         permitStatus = est.PermitStatusId,
                //                         ownerId = own.Id,
                //                         ownName = own.Name,
                //                         ownemail = own.EmailId,
                //                         ownContact = own.ContactNo,
                //                         ownMailAddr = own.MailingAddress,
                //                         ownMailState = own.State,
                //                         ownMailCity = own.City,
                //                         ownMailZip = own.Zip,
                //                         typeOfBusiness = det.BusinessTypeId,
                //                         typeofOperation = det.OperationTypeId,
                //                         numberofEmployee = det.NumberOfEmployees,
                //                         waterSource = det.WaterSourceId,
                //                         publicSewage = det.PublicSewageId,
                //                         privateSewage = det.PrivateSeptic,
                //                         certifiedFoodManager = det.CertifiedFoodManager,
                //                         certifiedManagerCertificate = det.CertificateExpirationDt
                //                     };

                var Ests = await _establishment.GetAllAsync(filter: x => x.IsDeleted != true && x.ApplicationFor!.Code == code && ((x.PermitStatusId >= 6 && x.PermitStatusId <= 11)|| x.PermitStatusId==13) && (SyncDate==null || x.SyncDate>SyncDate), includeProperties: "PermitStatus,ApplicationFor");

                var Establishments = from est in Ests /*await _establishment.GetAllAsync(filter: x => x.IsDeleted != true && x.ApplicationFor!.Code == code && (x.PermitStatusId>=8 && x.PermitStatusId<=9), includeProperties: "PermitStatus,ApplicationFor")*/
                                     join own in await _owner.GetAllAsync()
                                     on est.Id equals own.EstablishmentId into egroup
                                     from own in egroup.DefaultIfEmpty()
                                     join det in await _rfoperationaldetails.GetAllAsync()
                                     on est.Id equals det.EstablishmentId into rgroup
                                     from det in rgroup.DefaultIfEmpty()
                                     join mdet in await _mfoperationaldetails.GetAllAsync()
                                     on est.Id equals mdet.EstablishmentId into mgroup
                                     from mdet in mgroup.DefaultIfEmpty()
                                     orderby est.SyncDate descending
                                     select new
                                     {
                                         id = est.Id,
                                         name = est.Name,
                                         permitNumber = est?.PermitNumber,
                                         address = est.Address,
                                         state = est.State,
                                         city = est.City,
                                         zip = est.Zip,
                                         contact = est.ContactNo,
                                         permitStatus = est.PermitStatusId,
                                         risk = est.RiskCategory,
                                         riskId = _agencyStaffReqFields.GetFirstOrDefault(filter:x=>x.EstablishmentId== est.Id).RiskCategoryId,
                                         ownerId = own?.Id,
                                         ownName = own?.Name,
                                         ownemail = own?.EmailId,
                                         ownContact = own?.ContactNo,
                                         ownMailAddr = own?.MailingAddress,
                                         ownMailState = own?.State,
                                         ownMailCity = own?.City,
                                         ownMailZip = own?.Zip,
                                         typeOfBusiness = det?.BusinessTypeId,
                                         typeofOperation = det?.OperationTypeId,
                                         numberofEmployee = det?.NumberOfEmployees,
                                         waterSource = det?.WaterSourceId,
                                         publicSewage = det?.PublicSewageId,
                                         privateSewage = det?.PrivateSeptic,
                                         certifiedFoodManager = det?.CertifiedFoodManager,
                                         certifiedManagerCertificate = det?.CertificateExpirationDt,
                                         mobiletypeOfBusiness = mdet?.BusinessTypeId,
                                         mobiletypeofOperation = mdet?.OperationTypeId,
                                         mobilewaterSource = mdet?.WaterSourceId,
                                         centralprocessingfacility = mdet?.CentralProcessingFacility,
                                         centralprocessingfacilityAddress = mdet?.Street,
                                         centralprocessingfacilityState = mdet?.State,
                                         centralprocessingfacilityCity = mdet?.City,
                                         centralprocessingfacilityZip = mdet?.Zip,
                                         mobilewatertanksize = mdet?.Portablewatertanksize,
                                         mobilewastetanksize = mdet?.Wastewatertanksize,
                                         mobilecertifiedFoodManager = mdet?.CertifiedFoodManager,
                                         mobilecertifiedManagerCertificate = mdet?.CertificateExpirationDt,
                                         createdDate = est.CreatedOn,
                                         updatedDate = est.UpdatedOn, 
                                         lastUpdatedDate = est.SyncDate
                                     };

                //foreach(var Est in Ests)
                //{
                //    Est.IsSync = true;
                //    Est.SyncDate = DateTime.Now;
                //    await _establishment.UpdateAsync(Est);
                //}

                DateTime? LatestDt = new DateTime();

                if (Establishments.Any())
                {
                   LatestDt = Establishments.FirstOrDefault()!.lastUpdatedDate!.Value;
                }
                else
                {
                    LatestDt = null;
                }

                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Message = "Success";
                _response.Result = new { Establishments, LatestDate = LatestDt };
                return Ok(_response);
            }
            //var role = TokenValidator.GetUserRole(token);

            _response.StatusCode = HttpStatusCode.Unauthorized;
            _response.IsSuccess = false;
            _response.Message = "Unauthorized";
            _response.Result = "";
            return Ok(_response);

        }

        [HttpGet("/GetAllPublicSewage", Name = "GetAllPublicSewage")]
        public async Task<ActionResult<APIResponse>> GetAllPublicSewage()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var str = TokenValidator.Validation(token);
            if (str == "Authorized")
            {
                var sewage = await _publicSewage.GetAllAsync(filter:x=>x.IsActive==true);
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Message = "Success";
                _response.Result = sewage;
                return Ok(_response);
            }
            _response.StatusCode = HttpStatusCode.Unauthorized;
            _response.IsSuccess = false;
            _response.Message = "Unauthorized";
            _response.Result = "";
            return Ok(_response);
        }

        [HttpGet("/GetAllPublicWaterSource", Name = "GetAllPublicWaterSource")]
        public async Task<ActionResult<APIResponse>> GetAllPublicWaterSource()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var str = TokenValidator.Validation(token);
            if (str == "Authorized")
            {
                var waterSource = await _waterSource.GetAllAsync(filter: x => x.IsActive == true);
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Message = "Success";
                _response.Result = waterSource;
                return Ok(_response);
            }
            _response.StatusCode = HttpStatusCode.Unauthorized;
            _response.IsSuccess = false;
            _response.Message = "Unauthorized";
            _response.Result = "";
            return Ok(_response);
        }

        [HttpGet("/GetAllTypeOfBusiness", Name = "GetAllTypeOfBusiness")]
        public async Task<ActionResult<APIResponse>> GetAllTypeOfBusiness()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var str = TokenValidator.Validation(token);
            if (str == "Authorized")
            {
                var businessTypes = await _businessTypes.GetAllAsync(filter: x => x.IsActive == true);
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Message = "Success";
                _response.Result = businessTypes;
                return Ok(_response);
            }
            _response.StatusCode = HttpStatusCode.Unauthorized;
            _response.IsSuccess = false;
            _response.Message = "Unauthorized";
            _response.Result = "";
            return Ok(_response);
        }

        [HttpGet("/GetAllTypeOfOperations", Name = "GetAllTypeOfOperations")]
        public async Task<ActionResult<APIResponse>> GetAllTypeOfOperations()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var str = TokenValidator.Validation(token);
            if (str == "Authorized")
            {
                var operationTypes = await _operationTypes.GetAllAsync(filter: x => x.IsActive == true);
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Message = "Success";
                _response.Result = operationTypes;
                return Ok(_response);
            }
            _response.StatusCode = HttpStatusCode.Unauthorized;
            _response.IsSuccess = false;
            _response.Message = "Unauthorized";
            _response.Result = "";
            return Ok(_response);
        }

        [HttpGet("/GetAllInspectionPurposes", Name = "GetAllInspectionPurposes")]
        public async Task<ActionResult<APIResponse>> GetAllInspectionPurposes()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var str = TokenValidator.Validation(token);
            if (str == "Authorized")
            {
                var purposes = await _inspectionPurposes.GetAllAsync(filter: x => x.IsActive == true);
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Message = "Success";
                _response.Result = purposes;
                return Ok(_response);
            }
            _response.StatusCode = HttpStatusCode.Unauthorized;
            _response.IsSuccess = false;
            _response.Message = "Unauthorized";
            _response.Result = "";
            return Ok(_response);
        }


        [HttpGet("/GetInspectionSection", Name = "GetInspectionSection")]
        public async Task<ActionResult<APIResponse>> GetInspectionSection()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var str = TokenValidator.Validation(token);
            if (str == "Authorized")
            {
                var sections = await _section.GetAllAsync();
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Message = "Success";
                _response.Result = sections;
                return Ok(_response);
            }
            _response.StatusCode = HttpStatusCode.Unauthorized;
            _response.IsSuccess = false;
            _response.Message = "Unauthorized";
            _response.Result = "";
            return Ok(_response);
        }

        [HttpGet("/GetInspectionSubSection", Name = "GetInspectionSubSection")]
        public async Task<ActionResult<APIResponse>> GetInspectionSubSection()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var str = TokenValidator.Validation(token);
            if (str == "Authorized")
            {
                var subSections = await _subSection.GetAllAsync();
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Message = "Success";
                _response.Result = subSections;
                return Ok(_response);
            }
            _response.StatusCode = HttpStatusCode.Unauthorized;
            _response.IsSuccess = false;
            _response.Message = "Unauthorized";
            _response.Result = "";
            return Ok(_response);
        }


        [HttpGet("/GetInspectionItems", Name = "GetInspectionItems")]
        public async Task<ActionResult<APIResponse>> GetInspectionItems(string code)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var str = TokenValidator.Validation(token);
            if (str == "Authorized")
            {
                if (code == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    _response.Message = "Not Found";
                    _response.Result = "";
                    return Ok(_response);
                }
                var items = await _items.GetAllAsync(filter: x => (x.Code!.Name!).ToLower() == code.ToLower(), includeProperties: "Code");
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Message = "Success";
                _response.Result = items;
                return Ok(_response);
            }
            _response.StatusCode = HttpStatusCode.Unauthorized;
            _response.IsSuccess = false;
            _response.Message = "Unauthorized";
            _response.Result = "";
            return Ok(_response);
        }

        [HttpGet("/GetAllUsers", Name = "GetAllUsers")]
        public async Task<ActionResult<APIResponse>> GetAllUsers(DateTime? SyncDate)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var str = TokenValidator.Validation(token);
            if (str == "Authorized")
            {
                var users = await _users.GetAllAsync(filter: x => x.IsDelete==false && (SyncDate == null || x.SyncDate > SyncDate));
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Message = "Success";
                _response.Result = users;
                return Ok(_response);
            }
            _response.StatusCode = HttpStatusCode.Unauthorized;
            _response.IsSuccess = false;
            _response.Message = "Unauthorized";
            _response.Result = "";
            return Ok(_response);
        }
    }
}
