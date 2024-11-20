using BrazosApp.DataAccess.Repository.IRepository;
using BrazosApp.Models;
using BrazosApp.Models.ViewModels;
using BrazosApp.Utility.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BrazosApp.Areas.LoggedIn.Controllers
{
      [Area("LoggedIn")]
      [Authorize("CommonPolicy")]
      public class PaymentController : Controller
      {
            private readonly IRepository<Establishment> _establishment;
            private readonly IRepository<EstablishmentOwner> _owner;
            private readonly IRepository<Fees> _fees;
            private readonly IRepository<FeesDetailsTable> _feesDetailsTable;
            private readonly IRepository<Payment> _paymentRepo;
            private readonly IRepository<PaymentDetailsTable> _paymentDetailsTable;
            private readonly IRepository<PaymentSplit> _paymentSplit;
            private readonly IEncrypt _encrypt;

            public PaymentController(IRepository<Establishment> establishment,
            IRepository<EstablishmentOwner> owner,
            IRepository<Fees> fees,
            IRepository<FeesDetailsTable> feesDetailsTable,
            IRepository<Payment> paymentRepo,
            IRepository<PaymentDetailsTable> paymentDetailsTable,
            IRepository<PaymentSplit> paymentSplit,
            IEncrypt encrypt)
            {
                  _establishment = establishment;
                  _owner = owner;
                  _fees = fees;
                  _feesDetailsTable = feesDetailsTable;
                  _paymentRepo = paymentRepo;
                  _paymentDetailsTable = paymentDetailsTable;
                  _paymentSplit = paymentSplit;
                  _encrypt = encrypt;
            }
            [HttpGet("/Financials")]
            public IActionResult Payments()
            {
                  PaymentIndexVM model = new PaymentIndexVM();
                  model.OfflinePaymentVM = new OfflinePaymentVM();
                  model.OfflinePaymentVM.PaymentSplit = new List<PaymentSplit>();
                  return View(model);
            }

            [HttpPost("/Payments/GetAllPayments")]
            public async Task<IActionResult> GetAllPayments(PaymentIndexVM model)
            {
                  //var PaymentList = from d in await _paymentRepo.GetAllAsync(filter: x => (x.PaymentStatus == 2 || x.PaymentStatus == 5 || x.PaymentStatus == 6 || x.PaymentStatus == 3), includeProperties: "Fees")
                  //                      join a in await _agencyRequiredField.GetAllAsync(includeProperties: "Establishment")
                  //                      on d.Fees!.EstablishmentId equals a.EstablishmentId into mgroup
                  //                      from a in mgroup.DefaultIfEmpty()
                  //                      orderby (d.RefundVoidDate != null ? d.RefundVoidDate : d.PaymentOn) descending
                  //                      select new
                  //                      {
                  //                            //Id = d.Id,
                  //                            Permit = a.Establishment!.PermitNumber,
                  //                            Name = a.Establishment!.Name,
                  //                            Address = a.Establishment!.Address,
                  //                            InvoiceNumber = d.InvoiceNo,
                  //                            ReceiptNumber = d.ReceiptNo,
                  //                            Amount = d.PaymentStatus == 3 ? 0 : d.Amount,
                  //                            //Amount = d.PaymentStatus==3? "0" : d.Amount.ToString("N0"),
                  //                            //PaymentOn = d.PaymentOn!.Value.Date,
                  //                            //PaymentDate = d.PaymentOn!.Value.ToShortDateString(),
                  //                            //PaymentOn = d.PaymentOn != null ? d.PaymentOn.Value.Date : (DateTime?)null,
                  //                            PaymentOn = d.PaymentOn != null ? (d.RefundVoidDate != null ? d.RefundVoidDate : d.PaymentOn.Value.Date) : null,
                  //                            //PaymentDate = d.PaymentOn != null ? d.PaymentOn.Value.ToShortDateString() : "--",
                  //                            PaymentDate = d.PaymentOn != null ? (d.RefundVoidDate != null ? d.RefundVoidDate.Value.ToShortDateString() : d.PaymentOn.Value.ToShortDateString()) : "--",
                  //                            EstTypeId = a.EstablishmentTypeId,
                  //                            //EncryptedId = _encrypt.Encrypt256(d.Id.ToString())
                  //                      };

                  var PaymentList = from d in await _paymentRepo.GetAllAsync(filter: x=>x.PaymentStatus!=3 && x.PaymentStatus != 4 && x.PaymentStatus != 7, includeProperties: "Fees", orderBy: x => x.OrderByDescending(x => x.InvoiceDate))
                                    join a in await _establishment.GetAllAsync()
                                    on d.EstablishmentId equals a.Id into mgroup
                                    from a in mgroup.DefaultIfEmpty()
                                    select new
                                    {
                                        Id = d.Id,
                                        Name = a.Name,
                                        Permit = a.PermitNumber,
                                        Address = a.Address,
                                        Invoice_Number = d.InvoiceNo,
                                        Receipt_Number = d.ReceiptNo,
                                        InvoiceDate = d.InvoiceDate?.ToShortDateString(),
                                        Owner = _owner.GetFirstOrDefault(filter:x=>x.EstablishmentId==a.Id).Name,
                                        PaidFor = "Permit Fees",
                                        Amount = d.Amount,
                                        PaymentOn = d.PaymentOn != null ? (d.RefundVoidDate != null ? d.RefundVoidDate : d.PaymentOn.Value.Date) : null,
                                        dueDays = (DateTime.Now - d.InvoiceDate!.Value).Days,
                                        Status = d.PaymentStatus == 1 ? "Unpaid" : (d.PaymentStatus == 2 ? "Paid" : (d.PaymentStatus == 3 ? "Cancelled" : (d.PaymentStatus == 5 ? "Refunded" : (d.PaymentStatus == 6 ? "Voided" : "")))),
                                        feesStat = d.Fees!.Status,
                                        //ispermitFees = payment.IsPermitFee,
                                        isvoidEnabled = d.IsVoidEnabled,
                                        modeofPay = d.PaymentType == null ? "--" : (d.PaymentType == 1 ? "Online" : "Offline"),
                                        //method = payment.PaymentMethod == null ? "--" : (payment.PaymentMethod == 0 ? "Credit Card" : (payment.PaymentMethod == 1 ? "E-Check" : (payment.PaymentMethod == 2 ? "Cash" : (payment.PaymentMethod == 3 ? "Check" : (payment.PaymentMethod == 4 ? "Card" : "Money Order"))))),
                                        method = d.PayMethodType == null ? "--" : d.PayMethodType,
                                        encryptedId = _encrypt.Encrypt256(Convert.ToString(d.Id)),
                                        paymentDescription = _paymentDetailsTable.GetAll(x => x.PaymentId == d.Id).Select(x => x.Title).ToList(),
                                    };



                  //if (model.FromDate != null)
                  //{

                  //      PaymentList = PaymentList.Where(x => x.PaymentOn != null && (x.PaymentOn!.Value.Date >= model.FromDate.Value.Date));
                  //}

                  //if (model.ToDate != null)
                  //{
                  //      TransactionList = TransactionList.Where(x => x.PaymentOn != null && (x.PaymentOn!.Value.Date <= model.ToDate.Value.Date));
                  //}

                  //var PaymentFinalList = from d in PaymentList
                  //                           join e in await _establishmentTypes.GetAllAsync(includeProperties: "Jurisdiction")
                  //                           on d.EstTypeId equals e.Id into egroup
                  //                           from e in egroup.DefaultIfEmpty()
                  //                           select new
                  //                           {
                  //                                 Permit = d.Permit,
                  //                                 Name = d.Name,
                  //                                 Address = d.Address,
                  //                                 Invoice_Number = d.InvoiceNumber,
                  //                                 Receipt_Number = d.ReceiptNumber,
                  //                                 Amount = d.Amount,
                  //                                 //PaymentOn = d.PaymentOn,
                  //                                 Payment_Date = d.PaymentDate,
                  //                                 Account_Description = e?.Jurisdiction!.AccountDescription,
                  //                                 Account_Code = e?.Jurisdiction!.AccountCode,
                  //                           };

                  //var TransactionFinalList = FinalList.Where(x => x.Permit.Contains(code));

                  if (model.Name != null)
                  {
                        PaymentList = PaymentList.Where(x => x.Name.ToLower().Contains(model.Name.ToLower()));
                  }
                  if (model.Owner != null)
                  {
                        PaymentList = PaymentList.Where(x => x.Owner.ToLower().Contains(model.Owner.ToLower()));
                  }
                  if (model.Address != null)
                  {
                        PaymentList = PaymentList.Where(x => x.Address.ToLower().Contains(model.Address.ToLower()));
                  }
                  if (model.Permit != null)
                  {
                        PaymentList = PaymentList.Where(x => x.Permit != null && x.Permit.ToLower().Contains(model.Permit.ToLower()));
                  }
                  if (model.Amount != null)
                  {
                        PaymentList = PaymentList.Where(x => x.Amount == Convert.ToDecimal(model.Amount));
                  }
                  if (model.InvoiceNo != null)
                  {
                        //PaymentList = PaymentList.Where(x => x.Invoice_Number == model.InvoiceNo);
                        PaymentList = PaymentList.Where(x => x.Invoice_Number.Contains(model.InvoiceNo));
                  }

                  return Json(new { data = PaymentList.ToList() });
            }
      }
}
