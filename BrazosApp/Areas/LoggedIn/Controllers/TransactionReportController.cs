using BrazosApp.DataAccess.Repository.IRepository;
using BrazosApp.Models;
using BrazosApp.Models.ViewModels;
using BrazosApp.Utility;
using BrazosApp.Utility.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BrazosApp.Areas.LoggedIn.Controllers
{
    [Area("LoggedIn")]
    [Authorize("CommonPolicy")]
    public class TransactionReportController : Controller
    {
        private readonly IRepository<Payment> _payment;
        private readonly IRepository<Fees> _fees;
        private readonly IRepository<FeesDetailsTable> _feesDetailsTable;
        private readonly IRepository<Establishment> _establishment;
        private readonly IRepository<EstablishmentTypes> _establishmentTypes;
        private readonly IRepository<AgencyStaffReqFields> _agencyRequiredField;
        private readonly IEncrypt _encrypt;

        public TransactionReportController(IRepository<Payment> payment,
            IRepository<Fees> fees,
            IRepository<FeesDetailsTable> feesDetailsTable,
            IRepository<Establishment> establishment,
            IRepository<EstablishmentTypes> establishmentTypes,
            IRepository<AgencyStaffReqFields> agencyRequiredField,
            IEncrypt encrypt)
        {
            _payment = payment;
            _fees = fees;
            _feesDetailsTable = feesDetailsTable;
            _establishment = establishment;
            _establishmentTypes = establishmentTypes;
            _agencyRequiredField = agencyRequiredField;
            _encrypt = encrypt;
        }


        [HttpGet("/Transactions/{code?}")]
        public IActionResult Transactions(string code)
        {
            TransactionSearchVM model = new TransactionSearchVM();
            ViewBag.Code = code;
            return View(model);
        }

        [HttpPost("/GetAllTransactions/{code?}")]
        public async Task<IActionResult> GetAllTransactions(TransactionSearchVM model, string code)
        {
            var TransactionList = from d in await _payment.GetAllAsync(filter:x=>(x.PaymentStatus==2 || x.PaymentStatus == 5 || x.PaymentStatus == 6 || x.PaymentStatus == 3) , includeProperties: "Fees")
                                 join a in await _agencyRequiredField.GetAllAsync(includeProperties: "Establishment")
                                 on d.Fees!.EstablishmentId equals a.EstablishmentId into mgroup
                                 from a in mgroup.DefaultIfEmpty()
                                 orderby (d.RefundVoidDate!=null? d.RefundVoidDate : d.PaymentOn) descending
                                 select new
                                 {
                                     //Id = d.Id,
                                     Permit = a.Establishment!.PermitNumber,
                                     Name = a.Establishment!.Name,
                                     Address = a.Establishment!.Address,
                                     InvoiceNumber = d.InvoiceNo,
                                     ReceiptNumber = d.ReceiptNo,
                                     Amount = d.PaymentStatus==3? 0 : d.Amount,
                                     //Amount = d.PaymentStatus==3? "0" : d.Amount.ToString("N0"),
                                     //PaymentOn = d.PaymentOn!.Value.Date,
                                     //PaymentDate = d.PaymentOn!.Value.ToShortDateString(),
                                     //PaymentOn = d.PaymentOn != null ? d.PaymentOn.Value.Date : (DateTime?)null,
                                     PaymentOn = d.PaymentOn != null ? (d.RefundVoidDate != null ? d.RefundVoidDate : d.PaymentOn.Value.Date) : null,
                                     //PaymentDate = d.PaymentOn != null ? d.PaymentOn.Value.ToShortDateString() : "--",
                                     PaymentDate = d.PaymentOn != null ? (d.RefundVoidDate != null ? d.RefundVoidDate.Value.ToShortDateString(): d.PaymentOn.Value.ToShortDateString()) : "--",
                                     EstTypeId = a.EstablishmentTypeId,
                                     //EncryptedId = _encrypt.Encrypt256(d.Id.ToString())
                                 };

            TransactionList = TransactionList.Where(x => x.Permit.Contains(code));

            if (model.FromDate != null)
            {

                TransactionList = TransactionList.Where(x => x.PaymentOn!=null && (x.PaymentOn!.Value.Date >= model.FromDate.Value.Date));
            }

            if (model.ToDate != null)
            {
                TransactionList = TransactionList.Where(x => x.PaymentOn!=null && (x.PaymentOn!.Value.Date <= model.ToDate.Value.Date));
            }

            var TransactionFinalList = from d in TransactionList
                            join e in await _establishmentTypes.GetAllAsync(includeProperties: "Jurisdiction")
                            on d.EstTypeId equals e.Id into egroup
                            from e in egroup.DefaultIfEmpty()
                            select new
                            {
                                Permit = d.Permit,
                                Name = d.Name,
                                Address = d.Address,
                                Invoice_Number = d.InvoiceNumber,
                                Receipt_Number = d.ReceiptNumber,
                                Amount = d.Amount,
                                //PaymentOn = d.PaymentOn,
                                Payment_Date = d.PaymentDate,
                                Account_Description = e?.Jurisdiction!.AccountDescription,
                                Account_Code = e?.Jurisdiction!.AccountCode, 
                            };

            //var TransactionFinalList = FinalList.Where(x => x.Permit.Contains(code));

            if (model.Name != null)
            {
                TransactionFinalList = TransactionFinalList.Where(x => x.Name.ToLower().Contains(model.Name.ToLower()));
            }
            if (model.Address != null)
            {
                TransactionFinalList = TransactionFinalList.Where(x => x.Address.ToLower().Contains(model.Address.ToLower()));
            }
            if (model.Permit != null)
            {
                TransactionFinalList = TransactionFinalList.Where(x => x.Permit != null && x.Permit.ToLower().Contains(model.Permit.ToLower()));
            }
            if (model.Amount != null)
            {
                TransactionFinalList = TransactionFinalList.Where(x => x.Amount==Convert.ToDecimal(model.Amount));
            }
            

            return Json(new { data = TransactionFinalList.ToList() });
        }
    }
}
