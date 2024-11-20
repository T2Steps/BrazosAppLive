using BrazosApp.Models;
using BrazosApp.Models.DTOs;
using BrazosApp.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BrazosApp.Areas.LoggedIn.Controllers
{
    public partial class PermitController : Controller
    {
        [Route("/GetAllPayment/{id?}")]
        public async Task<IActionResult> GetAllPayment(string id)
        {
            if (id == null)
            {
                return Json(new { success = false, msg = "Failed" });
            }
            var establishmentId = _encrypt.Decrypt256(id);
            //var paymentList = /*from d in*/ await _paymentRepo.GetAllAsync(filter: x => x.Fees.EstablishmentId == Convert.ToInt32(establishmentId), includeProperties: "Fees");
            //                  //join estType in await _estTypes.GetAllAsync() on d.Fees.EstablishmentTypeId equals estType.Id into estTypeGroup
            //                  //from et in estTypeGroup.DefaultIfEmpty()
            //                  //select new
            //                  //{
            //                  //    Id = d.Id,
            //                  //    InvoiceNo = d.InvoiceNo,
            //                  //    PaidFor = et.Name,
            //                  //    Amount = d.Amount,
            //                  //    Status = d.PaymentStatus == 1 ? "Pending" : (d.PaymentStatus == 2 ? "Success" : "Failure"),
            //                  //    encryptedId = _encrypt.Encrypt256(Convert.ToString(d.Id)),
            //                  //    message = d.RedirectApiMessage
            //                  //};


            //var FeesList = from fe in await _fees.GetAllAsync(filter: x => x.EstablishmentId == Convert.ToInt32(establishmentId))
            //           join p in await _paymentRepo.GetAllAsync() on fe.Id equals p.FeesId into eGroup
            //           from p in eGroup.DefaultIfEmpty()
            //           select new
            //           {
            //               Id = fe.Id,
            //               InvoiceNo = fe.InvoiceNo,
            //               PaidFor = "Permit Fees",
            //               Amount = fe.Amount,
            //               Status = fe.Status == 1 ? "Pending" : (fe.Status == 2 ? "Paid" : "Cancelled"),
            //               modeofPay = p.PaymentType == 1 ? "": (p.PaymentType == 2? "Offline" : "--"),
            //               method = p.PaymentMethod == 0? "Credit Card": p.PaymentMethod == 1 ? "E-Cheque": p.PaymentMethod == 2? "Cash" : p.PaymentMethod == 2 ? "Cheque": "--",
            //               encryptedId = _encrypt.Encrypt256(Convert.ToString(fe.Id)),
            //           };

            //var FeesList = from fe in await _fees.GetAllAsync(filter: x => x.EstablishmentId == Convert.ToInt32(establishmentId))
            //               join p in await _paymentRepo.GetAllAsync() on fe.Id equals p.FeesId into eGroup
            //               from p in eGroup.DefaultIfEmpty()
            //               select new
            //               {
            //                   Id = fe.Id,
            //                   InvoiceNo = fe.InvoiceNo,
            //                   InvoiceDate = fe.CreatedOn.ToShortDateString(),
            //                   PaidFor = "Permit Fees",
            //                   Amount = p == null ? fe.Amount : p.Amount,
            //                   dueDays = (DateTime.Now - fe.CreatedOn).Days,
            //                   Status = fe.Status == 1 ? "Pending" : (fe.Status == 2 ? "Paid" : (fe.Status == 3 ? "Cancelled" : (fe.Status == 4 ? "Renewal Pending" : (fe.Status == 5 ? "Late Renewal Pending" : (fe.Status == 6 ? "30 days" : (fe.Status == 7 ? "60 days" : (fe.Status == 8 ? "Delinquent" :(fe.Status == 9 ? "Voided" : (fe.Status == 10 ? "Refund Requested": (fe.Status == 11 ? (Convert.ToString(p.PaymentStatus)=="2"?"Paid":"Refunded") : Convert.ToString(fe.Status) )))))))))),
            //                   feesStat = fe.Status,
            //                   modeofPay = p == null ? "--" : p.PaymentType==null?"--": (p.PaymentType == 1 ? "Online" : "Offline"),
            //                   method = p == null ? "--" : p.PaymentMethod==null? "--" : (p.PaymentMethod==0? "Credit Card": (p.PaymentMethod==1? "E-Check":(p.PaymentMethod==2? "Cash": (p.PaymentMethod==3? "Check": (p.PaymentMethod == 4 ? "Card" : "Money Order"))))),
            //                   encryptedId = _encrypt.Encrypt256(Convert.ToString(fe.Id)),
            //               };

            var FeesList = from payment in await _paymentRepo.GetAllAsync(filter: x => x.EstablishmentId == Convert.ToInt32(establishmentId), includeProperties:"Fees", orderBy:x=>x.OrderByDescending(x=>x.InvoiceDate))
                           select new
                           {
                               Id = payment.Id,
                               permitStatus = _establishment.GetFirstOrDefault(filter:x=>x.Id== Convert.ToInt32(establishmentId), includeProperties: "PermitStatus").PermitStatus!.Name,
                               InvoiceNo = payment.InvoiceNo,
                               InvoiceDate = payment.InvoiceDate?.ToShortDateString(),
                               PaidFor = "Permit Fees",
                               Amount = payment.Amount,
                               dueDays = (DateTime.Now - payment.InvoiceDate!.Value).Days,
                               Status = payment.PaymentStatus == 1 ? "Unpaid":(payment.PaymentStatus==2? "Paid": (payment.PaymentStatus == 3 ? "Cancelled" : (payment.PaymentStatus==5? "Refunded": (payment.PaymentStatus==6? "Voided":"")))),
                               feesStat = payment.Fees!.Status,
                               //ispermitFees = payment.IsPermitFee,
                               isvoidEnabled = payment.IsVoidEnabled,
                               modeofPay = payment.PaymentType == null ? "--" : (payment.PaymentType == 1 ? "Online" : "Offline"),
                               //method = payment.PaymentMethod == null ? "--" : (payment.PaymentMethod == 0 ? "Credit Card" : (payment.PaymentMethod == 1 ? "E-Check" : (payment.PaymentMethod == 2 ? "Cash" : (payment.PaymentMethod == 3 ? "Check" : (payment.PaymentMethod == 4 ? "Card" : "Money Order"))))),
                               method = payment.PayMethodType == null ? "--" : payment.PayMethodType,
                               encryptedId = _encrypt.Encrypt256(Convert.ToString(payment.Id)),
                               paymentDescription = _paymentDetailsTable.GetAll(x=>x.PaymentId == payment.Id).Select(x=>x.Title).ToList(),
                           };


            //var FeesList = from fe in Fees
            //               select new
            //               {
            //                   Id = fe.Id,
            //                   InvoiceNo = fe.InvoiceNo,
            //                   PaidFor = "Permit Fees",
            //                   Amount = fe.Amount,
            //                   Status = fe.Status == 1 ? "Pending" : (fe.Status == 2 ? "Paid" : "Cancelled"),
            //                   encryptedId = _encrypt.Encrypt256(Convert.ToString(fe.Id)),
            //               };

            return Json(new { data = FeesList });
        }


        [HttpGet("/GetAllIns/{id?}")]
        public async Task<IActionResult> GetAllIns(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                var InspectionList = from d in await _inspection.GetAllAsync(filter:x=>x.EstablishmentId==Convert.ToInt32(_encrypt.Decrypt256(id)), orderBy:x=>x.OrderByDescending(x=>x.InspectionDate))
                                     join u in await _users.GetAllAsync(/*filter: x => x.PermitNumber!.StartsWith(code)*/)
                                     //join u in est
                                     on d.InspectedBy equals u.Id into egroup
                                     join p in await _insPurposes.GetAllAsync()
                                     on d.PurposeId equals p.Id into mgroup
                                     join insData in await _rfmfInspectionData.GetAllAsync()
                                     on d.Id equals insData.InspectionId into insDatagroup
                                     from u in egroup.DefaultIfEmpty()
                                     from p in mgroup.DefaultIfEmpty()
                                     from insData in insDatagroup.DefaultIfEmpty()
                                     orderby d.InspectionDate descending
                                     select new
                                     {
                                         Id = d.Id,
                                         InspectionDate = d.InspectionDate,
                                         InspectedBy = u.FirstName + " " + u.LastName,
                                         Purpose = p.Name,
                                         FollowUp = d.FollowUp,
                                         FollowUpDate = d.FollowUpDate,
                                         Score = insData?.Score,
                                         isPermitSuspended = d.IsPermitSuspended,
                                         EncryptedId = _encrypt.Encrypt256(d.Id.ToString())
                                     };
                //var InspectionFinalList = InspectionList.Where(x => x.Permit.Contains(code)).ToList();
                return Json(new { data = InspectionList.ToList() });
            }
        }

        [HttpGet("/GetInspectionPdf/{id?}")]
        public async Task<IActionResult> GeneratePermitCertificatepdf(string? id)
        {
            FoodInspectionPdfDTO model = new FoodInspectionPdfDTO();
            var inspectionId = Convert.ToInt32(_encrypt.Decrypt256(id));
            var inspection = await _inspection.GetFirstOrDefaultAsync(filter: x => x.Id == inspectionId, includeProperties: "Establishment");
            var InspectionDatas = await _rfmfInspectionData.GetFirstOrDefaultAsync(filter: x => x.InspectionId == inspectionId);
            var inspectionItems = await _inspectionItems.GetAllAsync(filter: x => x.InspectionId == inspectionId, includeProperties: "Items");
            var TemperatureObs = await _temperatureObs.GetAllAsync(filter: x => x.InspectionId == inspectionId);
            var user = await _users.GetFirstOrDefaultAsync(filter: x => x.Id == inspection.InspectedBy);

            model.InspectionDetails = new InspectionDetails();
            TimeZoneInfo centralZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
            model.InspectionDetails.Id = inspectionId;
            model.InspectionDetails.EstId = inspection.EstablishmentId;
            model.InspectionDetails.EstName = inspection.Establishment!.Name;
            model.InspectionDetails.Permit = inspection.Establishment!.PermitNumber;
            model.InspectionDetails.Risk = inspection.Establishment!.RiskCategory;
            model.InspectionDetails.PurposeId = inspection.PurposeId;
            model.InspectionDetails.Address = inspection.Establishment!.Address + ", " + inspection.Establishment!.State + ", " + inspection.Establishment!.City + ", " + inspection.Establishment!.Zip;
            model.InspectionDetails.Date = inspection.InspectionDate.ToShortDateString();
            if (inspection.UpdatedOn == null)
            {
                model.InspectionDetails.TimeIn = TimeZoneInfo.ConvertTime(inspection.TimeIn, centralZone).ToShortTimeString();
                model.InspectionDetails.TimeOut = TimeZoneInfo.ConvertTime(inspection.TimeOut, centralZone).ToShortTimeString();
            }
            else
            {
                model.InspectionDetails.TimeIn = inspection.TimeIn.ToShortTimeString();
                model.InspectionDetails.TimeOut = inspection.TimeOut.ToShortTimeString();
            }
            model.InspectionDetails.CFM = InspectionDatas.CFM;
            model.InspectionDetails.CFMExpDate = InspectionDatas.CFMExpiryDate;
            model.InspectionDetails.FHC = InspectionDatas.FHC;
            model.InspectionDetails.NumberOfEmployees = InspectionDatas.NumberOfEmployees;
            model.InspectionDetails.Score = InspectionDatas.Score;
            model.InspectionDetails.PermitSuspend = inspection.IsPermitSuspended;
            model.InspectionDetails.SampleCollected = InspectionDatas.SampleCollected;
            model.InspectionDetails.InspectedBy = user.FirstName + " " + user.LastName;
            model.InspectionDetails.InspectedByEmail = user.EmailId;
            model.InspectionDetails.ReceivedBy = InspectionDatas.PersonInCharge;
            model.InspectionDetails.ReceivedBySign = InspectionDatas.PersonInChargeSign;
            model.InspectionDetails.InspectedBySign = inspection.InspectedBySign;
            model.InspectionDetails.InspectedBySignFile = inspection.InspectorSignFile;
            model.InspectionDetails.SecondaryInspector = InspectionDatas.SecondaryInspector;
            model.InspectionDetails.SecondaryInspectorSign = InspectionDatas.SecondaryInspectorSign;
            model.InspectionDetails.SecondaryInspectorSignFile = InspectionDatas.SecondaryInspectorSignFile;
            model.InspectionDetails.FollowUp = inspection.FollowUp;
            model.InspectionDetails.Comment = inspection.Comment;
            //model.InspectionDetails.PermitApproval = openingInspectionDatas.PermitApproval == true ? "Yes" : "No";

            if (inspection.Establishment.PermitNumber!.StartsWith("RF"))
            {
                model.Code = "RF";
            }
            else if (inspection.Establishment.PermitNumber!.StartsWith("MF"))
            {
                model.Code = "MF";
            }

            model.InspectionItemList = new List<InspectionItems>();
            foreach (var items in inspectionItems)
            {
                model.InspectionItemList!.Add(new InspectionItems
                {
                    Id = items.Id,
                    ItemId = items.ItemId,
                    ItemNumber = items.Items!.ItemNumber,
                    SubCategoryId = items.Items!.SubSectionId,
                    IsNA = items.Items!.IsNA,
                    IsNO = items.Items!.IsNO,
                    Name = items.Items!.Name,
                    Point = items.Items!.Points,
                    Status = items.Status,
                    CDI = items.Cos,
                    R = items.R,
                    Image = items.Image,
                    Comment = items.Comment,
                });
            }

            model.TemperatureObservations = new List<TemperatureObservation>();
            foreach (var temperatureObservation in TemperatureObs)
            {
                model.TemperatureObservations.Add(new TemperatureObservation
                {
                    Id = temperatureObservation.Id,
                    InspectionId = temperatureObservation.InspectionId,
                    ItemName = temperatureObservation.ItemName,
                    Temperature = temperatureObservation.Temperature,
                });
            }
            //return Ok();

            return await _generatePdf.GetPdf("Views/PdfTemplates/InspectionReportPdf.cshtml", model);
        }

        [HttpPost("/SendInsReportMailPdf")]
        public async Task<IActionResult> SendMail(string InsId, string toEmail, string ccEmail, string Subject, string Body)
        {
            FoodInspectionPdfDTO model = new FoodInspectionPdfDTO();
            var inspectionId = Convert.ToInt32(_encrypt.Decrypt256(InsId));
            var inspection = await _inspection.GetFirstOrDefaultAsync(filter: x => x.Id == inspectionId, includeProperties: "Establishment");
            var InspectionDatas = await _rfmfInspectionData.GetFirstOrDefaultAsync(filter: x => x.InspectionId == inspectionId);
            var inspectionItems = await _inspectionItems.GetAllAsync(filter: x => x.InspectionId == inspectionId, includeProperties: "Items");
            var TemperatureObs = await _temperatureObs.GetAllAsync(filter: x => x.InspectionId == inspectionId);
            var user = await _users.GetFirstOrDefaultAsync(filter: x => x.Id == inspection.InspectedBy);

            model.InspectionDetails = new InspectionDetails();
            TimeZoneInfo centralZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
            model.InspectionDetails.Id = inspectionId;
            model.InspectionDetails.EstId = inspection.EstablishmentId;
            model.InspectionDetails.EstName = inspection.Establishment!.Name;
            model.InspectionDetails.Permit = inspection.Establishment!.PermitNumber;
            model.InspectionDetails.Risk = inspection.Establishment!.RiskCategory;
            model.InspectionDetails.PurposeId = inspection.PurposeId;
            model.InspectionDetails.Address = inspection.Establishment!.Address + ", " + inspection.Establishment!.State + ", " + inspection.Establishment!.City + ", " + inspection.Establishment!.Zip;
            model.InspectionDetails.Date = inspection.InspectionDate.ToShortDateString();
            //model.InspectionDetails.TimeIn = TimeZoneInfo.ConvertTime(inspection.TimeIn, centralZone).ToShortTimeString();
            //model.InspectionDetails.TimeOut = TimeZoneInfo.ConvertTime(inspection.TimeOut, centralZone).ToShortTimeString();
            if (inspection.UpdatedOn == null)
            {
                model.InspectionDetails.TimeIn = TimeZoneInfo.ConvertTime(inspection.TimeIn, centralZone).ToShortTimeString();
                model.InspectionDetails.TimeOut = TimeZoneInfo.ConvertTime(inspection.TimeOut, centralZone).ToShortTimeString();
            }
            else
            {
                model.InspectionDetails.TimeIn = inspection.TimeIn.ToShortTimeString();
                model.InspectionDetails.TimeOut = inspection.TimeOut.ToShortTimeString();
            }
            model.InspectionDetails.Score = InspectionDatas.Score;
            model.InspectionDetails.CFM = InspectionDatas.CFM;
            model.InspectionDetails.CFMExpDate = InspectionDatas.CFMExpiryDate;
            model.InspectionDetails.FHC = InspectionDatas.FHC;
            model.InspectionDetails.NumberOfEmployees = InspectionDatas.NumberOfEmployees;
            model.InspectionDetails.SampleCollected = InspectionDatas.SampleCollected;
            model.InspectionDetails.InspectedBy = user.FirstName + " " + user.LastName;
            model.InspectionDetails.InspectedByEmail = user.EmailId;
            model.InspectionDetails.ReceivedBy = InspectionDatas.PersonInCharge;
            model.InspectionDetails.ReceivedBySign = InspectionDatas.PersonInChargeSign;
            model.InspectionDetails.SecondaryInspector = InspectionDatas.SecondaryInspector;
            model.InspectionDetails.SecondaryInspectorSign = InspectionDatas.SecondaryInspectorSign;
            model.InspectionDetails.SecondaryInspectorSignFile = InspectionDatas.SecondaryInspectorSignFile;
            model.InspectionDetails.InspectedBySign = inspection.InspectedBySign;
            model.InspectionDetails.InspectedBySignFile = inspection.InspectorSignFile;
            model.InspectionDetails.FollowUp = inspection.FollowUp;
            model.InspectionDetails.Comment = inspection.Comment;
            //model.InspectionDetails.PermitApproval = openingInspectionDatas.PermitApproval == true ? "Yes" : "No";

            if (inspection.Establishment.PermitNumber!.StartsWith("RF"))
            {
                model.Code = "RF";
            }
            else if (inspection.Establishment.PermitNumber!.StartsWith("MF"))
            {
                model.Code = "MF";
            }

            model.InspectionItemList = new List<InspectionItems>();
            foreach (var items in inspectionItems)
            {
                model.InspectionItemList!.Add(new InspectionItems
                {
                    Id = items.Id,
                    ItemId = items.ItemId,
                    ItemNumber = items.Items!.ItemNumber,
                    SubCategoryId = items.Items!.SubSectionId,
                    IsNA = items.Items!.IsNA,
                    IsNO = items.Items!.IsNO,
                    Name = items.Items!.Name,
                    Point = items.Items!.Points,
                    Status = items.Status,
                    CDI = items.Cos,
                    R = items.R,
                    Image = items.Image,
                    Comment = items.Comment,
                });
            }

            model.TemperatureObservations = new List<TemperatureObservation>();
            foreach (var temperatureObservation in TemperatureObs)
            {
                model.TemperatureObservations.Add(new TemperatureObservation
                {
                    Id = temperatureObservation.Id,
                    InspectionId = temperatureObservation.InspectionId,
                    ItemName = temperatureObservation.ItemName,
                    Temperature = temperatureObservation.Temperature,
                });
            }
            var generatedPdf = await _generatePdf.GetByteArray("Views/PdfTemplates/InspectionReportPdf.cshtml", model);
            
            Byte[][] bytearray = new Byte[][] { generatedPdf };
            string[] pdfName = new string[] { "InspectionReport.Pdf" };
            await _emailSenderService.SendEmail(toEmail, ccEmail, Subject, Body, bytearray, pdfName);
            return Json(new { success = true, msg = "Mail sent successfully" });
        }

        [HttpGet("/GetOpeningInspectionPdf/{id?}")]
        public async Task<IActionResult> GenerateOpeningPermitCertificatepdf(string? id)
        {
            OpeningCheckListPdfDTO model = new OpeningCheckListPdfDTO();
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
            //model.InspectionDetails.Date = inspection.InspectionDate.ToShortDateString() + "/" + TimeZoneInfo.ConvertTime(inspection.TimeIn, centralZone).ToShortTimeString() + "-" + TimeZoneInfo.ConvertTime(inspection.TimeOut, centralZone).ToShortTimeString(); 
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
            return await _generatePdf.GetPdf("Views/PdfTemplates/OpeningInspectionReportPdf.cshtml", model);
        }

        [HttpPost("/SendOpeningInsReportMailPdf")]
        public async Task<IActionResult> SendOpeningMail(string InsId, string toEmail, string ccEmail, string Subject, string Body)
        {
            OpeningCheckListPdfDTO model = new OpeningCheckListPdfDTO();
            var inspectionId = Convert.ToInt32(_encrypt.Decrypt256(InsId));
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

            var generatedPdf = await _generatePdf.GetByteArray("Views/PdfTemplates/OpeningInspectionReportPdf.cshtml", model);
            Byte[][] bytearray = new Byte[][] { generatedPdf };
            string[] pdfName = new string[] { "InspectionReport.Pdf" };
            await _emailSenderService.SendEmail(toEmail, ccEmail, Subject, Body, bytearray, pdfName);
            return Json(new { success = true, msg = "Mail sent successfully" });
        }


        [Route("/GetAllSchedule/{id?}")]
        public async Task<IActionResult> GetAllSchedule(string id)
        {
            if (id == null)
            {
                return Json(new { success = false, msg = "Failed" });
            }
            var EiD = _encrypt.Decrypt256(id);
            var InspectionList = from d in await _schedule.GetAllAsync(filter: x => x.EstablishmentId == Convert.ToInt32(EiD) && x.IsAdhoc==false && (x.StatusId==1|| x.StatusId==2), includeProperties: "Purpose", orderBy: x => x.OrderByDescending(x => x.ScheduledDate))
                                 join u in await _users.GetAllAsync()
                                 on d.AssignedTo equals u.Id into egroup
                                 join m in await _users.GetAllAsync()
                                 on d.CreatedBy equals m.Id into fgroup
                                 from u in egroup.DefaultIfEmpty()
                                 from m in fgroup.DefaultIfEmpty()
                                     //orderby d.ScheduledDate descending
                                 select new
                                 {
                                     Id = d.Id,
                                     ScheduledDate = d.ScheduledDate,
                                     Purpose = d.Purpose!.Name,
                                     AssignedTo = u == null ? "NA" : (u.FirstName + " " + u.LastName),
                                     StatusId = d.StatusId,
                                     IsInspected = d.StatusId == 4 ? "Yes" : "No",
                                     CreatedBy = m.FirstName + " " + m.LastName,
                                     encryptedId = _encrypt.Encrypt256(Convert.ToString(d.Id)),
                                 };
            return Json(new { data = InspectionList });
        }

        [Route("/CreateOpeningSchedule/{id?}")]
        [HttpPost]
        public async Task<IActionResult> ScheduleOpeningIns(string id)
        {
            if (id == null)
            {
                return Json(new { success = false, msg = "Failed" });
            }
            var EiD = _encrypt.Decrypt256(id);
            //var Est = await _establishment.GetById(Convert.ToInt32(EiD));
            var AgencyStaffReqFields = await _agencystaffreqfields.GetFirstOrDefaultAsync(filter: x => x.EstablishmentId == Convert.ToInt32(EiD), includeProperties: "Territory");
            var TerritoryWiseIns = await _areawiseIns.GetFirstOrDefaultAsync(filter: x => x.AreaId == AgencyStaffReqFields.AreaId);
            var Schedule = new Schedule();
            Schedule.PurposeId = 1;
            Schedule.AssignedTo = TerritoryWiseIns.AssignedUserId;
            Schedule.EstablishmentId = Convert.ToInt32(EiD);
            Schedule.ScheduledDate = DateTime.Now;
            Schedule.CreatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            Schedule.CreatedOn = DateTime.Now;
            Schedule.StatusId = 2;
            Schedule.SyncDate = DateTime.Now;
            await _schedule.AddAsync(Schedule);
            return Json(new { success = true });
        }

        [Route("/CheckOpeningSchStat/{id?}")]
        [HttpGet]
        public async Task<IActionResult> CheckOpeningSchStat(string id)
        {
            if (id == null)
            {
                return Json(new { success = false, msg = "Failed" });
            }
            var EiD = _encrypt.Decrypt256(id);
            //var Est = await _establishment.GetById(Convert.ToInt32(EiD));

            //var schedule = await _schedule.GetAllAsync(filter: x => x.EstablishmentId == Convert.ToInt32(EiD) && x.PurposeId == 1);
            
            var inspections = await _inspection.GetAllAsync(filter:x=>x.PurposeId==1 && x.EstablishmentId== Convert.ToInt32(EiD));
            if(inspections.Any())
            {
                  var flg = 0;
                  foreach (var inspection in inspections)
                  {
                        var openingChkData = await _openinginsData.GetFirstOrDefaultAsync(filter: x => x.InspectionId == inspection.Id);
                        if (openingChkData.PermitApproval == true)
                        {
                              flg = 1;
                              break;
                        }
                  }
                  if (flg == 1)
                  {
                        return Json(new { success = true, msg = "Pass" });
                  }
                  else
                  {
                        return Json(new { success = true, msg = "Not Pass" });
                  }
            }
            return Json(new { success = true, msg = "Not Pass" });
            //if (!schedule.Any())
            //{
            //    return Json(new { success = true, msg = "Not Pass" });
            //}
            //else
            //{
            //    foreach (var item in schedule)
            //    {
            //        if(item.StatusId == 1 || item.StatusId == 2)
            //        {
            //            return Json(new { success = true, msg = "Pass" });
            //        }
            //    }
            //    return Json(new { success = true, msg = "Not Pass" });
            //}

        }

        [Route("/ScheduleInspection")]
        [HttpPost]
        public async Task<IActionResult> ScheduleInspection(ScheduleVM model)
        {
            if (model == null)
            {
                return Json(new { success = false, msg = "Unexpected Error Occurred" });
            }
            else
            {
                if (model.Schedule!.Id == 0)
                {
                    model.Schedule.ScheduledDate = model.ScheduleDate;
                    model.Schedule.StatusId = 2;
                    model.Schedule.CreatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    model.Schedule.SyncDate = DateTime.Now;
                    try
                    {
                        //var ScheduleFromDb = await _schedule.GetAllAsync(filter: x => x.EstablishmentId == model.Schedule.EstablishmentId && x.CreatedBy == model.Schedule.CreatedBy && ((model.Schedule.CreatedOn - x.CreatedOn).Minutes) < 1);
                        var scheduleFromDb = await _schedule.GetAllAsync(filter: x => x.EstablishmentId == model.Schedule.EstablishmentId);
                        if(!scheduleFromDb.Any())
                        {
                            await _schedule.AddAsync(model.Schedule);
                            return Json(new { success = true, msg = "Successfully Scheduled" });
                        } 
                        else
                        {
                            var scheduleLastFromDb = scheduleFromDb.Where(x => x.CreatedBy == model.Schedule.CreatedBy).ToList().OrderByDescending(x => x.CreatedOn).FirstOrDefault();

                            if((model.Schedule.CreatedOn - scheduleLastFromDb!.CreatedOn).Seconds > 5)
                            {
                                await _schedule.AddAsync(model.Schedule);
                                return Json(new { success = true, msg = "Successfully Scheduled" });
                            }
                            else
                            {
                                return Json(new { success = true, info = "Please try again after a few seconds" });
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        return Json(new { success = true });
                    }
                    
                    //if (ExistingSchedule == null)
                    //{
                    //    await _schedule.AddAsync(model.Schedule);
                    //    return Json(new { success = true, msg = "Successfully Scheduled" });
                    //}
                    //else
                    //{
                    //    return Json(new { success = true });
                    //}
                    
                }
                else
                {
                    var schedule = await _schedule.GetById(model.Schedule.Id);
                    schedule.ScheduledDate = model.ScheduleDate;
                    schedule.PurposeId = model.Schedule.PurposeId;
                    schedule.AssignedTo = model.Schedule.AssignedTo;
                    schedule.UpdatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    schedule.UpdatedOn = DateTime.Now;
                    schedule.SyncDate = DateTime.Now;
                    await _schedule.UpdateAsync(schedule);
                    return Json(new { success = true, msg = "Record Successfully Updated" });
                }

            }

        }

        [Route("/GetPermitSchedule/{id?}")]
        [HttpGet]
        public async Task<IActionResult> GetSchedule(string id)
        {
            if (id == null) { return Json(new { success = false, msg = "Unexpected Error Occurred" }); }
            var Schedule = await _schedule.GetFirstOrDefaultAsync(filter: x => x.Id == Convert.ToInt32(_encrypt.Decrypt256(id)));
            if (Schedule == null) { return Json(new { success = false, msg = "Not Found" }); }
            else
            {
                return Json(new { success = true, schedule = Schedule });
            }
        }

        [HttpGet("/GetDefaultAreaInspector/{EstId?}")]
        public async Task<IActionResult> GetDefaultAreaInspector(string EstId)
        {
            var Est = await _establishment.GetFirstOrDefaultAsync(filter:x=>x.Id==Convert.ToInt32(_encrypt.Decrypt256(EstId)));
            var areaWiseInspector = new AreaWiseInspectors(); 
            try
            {
                areaWiseInspector = await _areawiseIns.GetFirstOrDefaultAsync(filter: x => x.Area.AreaNumber == Est.Area, includeProperties: "Area");
                var defaultUserId = 0;
                if (areaWiseInspector != null)
                {
                    defaultUserId = areaWiseInspector.AssignedUserId;
                }
                return Json(new { success = true, defaultUserId = defaultUserId });
            }
            catch(Exception ex)
            {
                return Json(new { success=false ,error = ex.Message });
            }
        }
    }
}
