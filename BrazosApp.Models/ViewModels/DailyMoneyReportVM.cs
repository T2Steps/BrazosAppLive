using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models.ViewModels
{
    public class DailyMoneyReportVM
    {
        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }
        public string[] programs { get; set; }
        public string[] jurisdictions { get; set; }
        public string[] accountDescriptions { get; set; }
        public string[] users { get; set; }
        public string paymentModes { get; set; } = "Cash, Check, Card, Money Order";
        public bool reportFlag { get; set; } = false;

        public List<SelectListItem>? ProgramList { get; set; }
        public List<SelectListItem>? JurisdictionList { get; set; }
        public List<SelectListItem>? AccountDescriptionList { get; set; }
        public List<SelectListItem>? UserList { get; set; }
        public List<SelectListItem>? PaymentModeList { get; set; }

        public DailyMoneyReport dailyMoneyReport { get; set; }
    }

    public class DailyMoneyReport
    {
        public string? strFromDate { get; set; }
        public string? strToDate { get; set; }
        public string strPrograms { get; set; }
        public string strJurisdictions { get; set; }
        public string strAccountDescriptions { get; set; }
        public string strUserIds { get; set; }
        public string strUserNames { get; set; }
        public string strPaymentModes { get; set; } = "Cash,Check,Card,Money Order";

        public List<DailyMoneySummary>? dailyMoneySummaries { get; set; }
        public List<DailyMoneyUserWiseSummary>? dailyMoneyUserWiseSummaries { get; set; }
        public List<DailyMoneyUserWise>? dailyMoneyUserWises { get; set; }
    }

    public class DailyMoneySummary
    {
        public decimal TotalCashAmount { get; set; }
        public decimal TotalChequeAmount { get; set; }
        public decimal TotalCardAmount { get; set; }
        public decimal TotalMoneyOrderAmount { get; set; }
        public decimal TotalVoidAmount { get; set; }
        public decimal TotalRefundAmount { get; set; }
        public decimal TotalFinalAmount { get; set; }
    }

    public class DailyMoneyUserWiseSummary
    {
        public int? Id { get; set; }
        public string? UserAlias { get; set; }
        public decimal CashAmount { get; set; }
        public decimal ChequeAmount { get; set; }
        public decimal CardAmount { get; set; }
        public decimal MoneyOrderAmount { get; set; }
        public decimal VoidAmount { get; set; }
        public decimal RefundAmount { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class DailyMoneyUserWise
    {
        public int? Id { get; set; }
        public string? UserAlias { get; set; }
        public string? ReceiptNo { get; set; }
        public string? InvoiceNo { get; set; }
        public DateTime? PaymentOn { get; set; }
        public decimal TotalAmount { get; set; }
        public string? PaymentMethod { get; set; }
        public decimal AmountReceived { get; set; }
        public string? Program { get; set; }
        public string? Jurisdiction { get; set; }
    }
}
