using BrazosApp.DataAccess.Repository.IRepository;
using BrazosApp.Models;
using BrazosApp.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using System.Data;
using Wkhtmltopdf.NetCore;

namespace BrazosApp.Areas.LoggedIn.Controllers
{
      [Area("LoggedIn")]
    [Authorize("CommonPolicy")]
    public class DailyMoneyReportController : Controller
    {
        private readonly IRepository<FeePrograms> _programRepo;
        private readonly IRepository<JurisdictionAccounts> _jurisdictionAccounts;
        private readonly IRepository<Users> _userRepo;
        private IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;
        private readonly IGeneratePdf _generatePdf;

        public DailyMoneyReportController(IRepository<FeePrograms> programRepo
            , IRepository<JurisdictionAccounts> jurisdictionAccounts
            , IRepository<Users> userRepo
            , IWebHostEnvironment environment
            , IConfiguration configuration
            , IGeneratePdf generatePdf)
        {
            _programRepo = programRepo;
            _jurisdictionAccounts = jurisdictionAccounts;
            _userRepo = userRepo;
            _environment = environment;
            _configuration = configuration;
            _generatePdf = generatePdf;
        }

        [HttpGet("/DailyMoneyReport")]
        public async Task<IActionResult> Index()
        {
            DailyMoneyReportVM dailyMoneyReportVM = new DailyMoneyReportVM();

            var programs = await _programRepo.GetAllAsync();
            dailyMoneyReportVM.ProgramList = programs.Select(x => new SelectListItem
            {
                Text = x.Name!.ToString(),
                Value = x.Name.ToString()
            }).ToList();

            var jurisdictionAccounts = await _jurisdictionAccounts.GetAllAsync();
            
            var jurisdictions = jurisdictionAccounts.GroupBy(x => x.Name).ToList();
            dailyMoneyReportVM.JurisdictionList = jurisdictions.Select(x => new SelectListItem
            {
                Text = x.Key,
                Value = x.Key
            }).ToList();

            var accountDescriptions = jurisdictionAccounts.GroupBy(x => x.AccountDescription).ToList();
            dailyMoneyReportVM.AccountDescriptionList = accountDescriptions.Select(x => new SelectListItem
            {
                Text = x.Key,
                Value = x.Key
            }).ToList();

            var users = await _userRepo.GetAllAsync();
            dailyMoneyReportVM.UserList = users.Select(x => new SelectListItem
            {
                Text = x.FirstName + " " + x.LastName,
                Value = x.Id.ToString()
            }).ToList();

            return View(dailyMoneyReportVM);
        }

        [Route("/DailyMoneyReport")]
        [HttpPost]
        public async Task<IActionResult> Index(DailyMoneyReportVM dailyMoneyReportVM)
        {
            dailyMoneyReportVM.dailyMoneyReport = new DailyMoneyReport();

            List<DailyMoneySummary> dailyMoneySummaryList = new List<DailyMoneySummary>();
            dailyMoneySummaryList = DailyMoneySummary(
                    dailyMoneyReportVM.fromDate == null ? null : dailyMoneyReportVM.fromDate.Value.ToShortDateString(),
                    dailyMoneyReportVM.toDate == null ? null : dailyMoneyReportVM.toDate.Value.ToShortDateString(),
                    ArrayToString(dailyMoneyReportVM.programs),
                    ArrayToString(dailyMoneyReportVM.jurisdictions),
                    ArrayToString(dailyMoneyReportVM.accountDescriptions), 
                    ArrayToString(dailyMoneyReportVM.users)
                ).AsEnumerable().Select(s => new DailyMoneySummary()
            {
                TotalCashAmount = s.Field<decimal>("TotalCashAmount"),
                TotalChequeAmount = s.Field<decimal>("TotalChequeAmount"),
                TotalCardAmount = s.Field<decimal>("TotalCardAmount"),
                TotalMoneyOrderAmount = s.Field<decimal>("TotalMoneyOrderAmount"),
                TotalVoidAmount = s.Field<decimal>("TotalVoidAmount"),
                TotalRefundAmount = s.Field<decimal>("TotalRefundAmount"),
                TotalFinalAmount = s.Field<decimal>("TotalFinalAmount")
            }).ToList();
            dailyMoneyReportVM.dailyMoneyReport.dailyMoneySummaries = dailyMoneySummaryList;

            List<DailyMoneyUserWiseSummary> dailyMoneyUserWiseSummaryList = new List<DailyMoneyUserWiseSummary>();
            dailyMoneyUserWiseSummaryList = DailyMoneyUserWiseSummary(
                    dailyMoneyReportVM.fromDate == null ? null : dailyMoneyReportVM.fromDate.Value.ToShortDateString(),
                    dailyMoneyReportVM.toDate == null ? null : dailyMoneyReportVM.toDate.Value.ToShortDateString(),
                    ArrayToString(dailyMoneyReportVM.programs),
                    ArrayToString(dailyMoneyReportVM.jurisdictions),
                    ArrayToString(dailyMoneyReportVM.accountDescriptions),
                    ArrayToString(dailyMoneyReportVM.users)
                ).AsEnumerable().Select(s => new DailyMoneyUserWiseSummary()
            {
                Id = s.Field<int>("Id"),
                UserAlias = s.Field<string>("UserAlias"),
                CashAmount = s.Field<decimal>("CashAmount"),
                ChequeAmount = s.Field<decimal>("ChequeAmount"),
                CardAmount = s.Field<decimal>("CardAmount"),
                MoneyOrderAmount = s.Field<decimal>("MoneyOrderAmount"),
                VoidAmount = s.Field<decimal>("VoidAmount"),
                RefundAmount = s.Field<decimal>("RefundAmount"),
                TotalAmount = s.Field<decimal>("TotalAmount")
            }).ToList();
            dailyMoneyReportVM.dailyMoneyReport.dailyMoneyUserWiseSummaries = dailyMoneyUserWiseSummaryList;

            List<DailyMoneyUserWise> dailyMoneyUserWiseList = new List<DailyMoneyUserWise>();
            dailyMoneyUserWiseList = DailyMoneyUserWise(
                    dailyMoneyReportVM.fromDate == null ? null : dailyMoneyReportVM.fromDate.Value.ToShortDateString(),
                    dailyMoneyReportVM.toDate == null ? null : dailyMoneyReportVM.toDate.Value.ToShortDateString(),
                    ArrayToString(dailyMoneyReportVM.programs),
                    ArrayToString(dailyMoneyReportVM.jurisdictions),
                    ArrayToString(dailyMoneyReportVM.accountDescriptions),
                    ArrayToString(dailyMoneyReportVM.users)
                ).AsEnumerable().Select(s => new DailyMoneyUserWise()
            {
                Id = s.Field<int>("Id"),
                UserAlias = s.Field<string>("UserAlias"),
                ReceiptNo = s.Field<string>("ReceiptNo"),
                InvoiceNo = s.Field<string>("InvoiceNo"),
                PaymentOn = s.Field<DateTime>("PaymentOn"),
                TotalAmount = s.Field<decimal>("TotalAmount"),
                PaymentMethod = s.Field<string>("PaymentMethod"),
                AmountReceived = s.Field<decimal>("AmountReceived"),
                Program = s.Field<string>("Program"),
                Jurisdiction = s.Field<string>("Jurisdiction")
            }).ToList();
            dailyMoneyReportVM.dailyMoneyReport.dailyMoneyUserWises = dailyMoneyUserWiseList;

            var programs = await _programRepo.GetAllAsync();
            dailyMoneyReportVM.ProgramList = programs.Select(x => new SelectListItem
            {
                Text = x.Name.ToString(),
                Value = x.Name.ToString()
            }).ToList();

            var jurisdictionAccounts = await _jurisdictionAccounts.GetAllAsync();

            var jurisdictions = jurisdictionAccounts.GroupBy(x => x.Name).ToList();
            dailyMoneyReportVM.JurisdictionList = jurisdictions.Select(x => new SelectListItem
            {
                Text = x.Key,
                Value = x.Key
            }).ToList();

            var accountDescriptions = jurisdictionAccounts.GroupBy(x => x.AccountDescription).ToList();
            dailyMoneyReportVM.AccountDescriptionList = accountDescriptions.Select(x => new SelectListItem
            {
                Text = x.Key,
                Value = x.Key
            }).ToList();

            var users = await _userRepo.GetAllAsync();
            dailyMoneyReportVM.UserList = users.Select(x => new SelectListItem
            {
                Text = x.FirstName + " " + x.LastName,
                Value = x.Id.ToString()
            }).ToList();

            dailyMoneyReportVM.reportFlag = true;
            dailyMoneyReportVM.dailyMoneyReport.strFromDate = dailyMoneyReportVM.fromDate == null ? null : dailyMoneyReportVM.fromDate.Value.ToShortDateString();
            dailyMoneyReportVM.dailyMoneyReport.strToDate = dailyMoneyReportVM.toDate == null ? null : dailyMoneyReportVM.toDate.Value.ToShortDateString();
            dailyMoneyReportVM.dailyMoneyReport.strPrograms = ArrayToString(dailyMoneyReportVM.programs);
            dailyMoneyReportVM.dailyMoneyReport.strJurisdictions = ArrayToString(dailyMoneyReportVM.jurisdictions);
            dailyMoneyReportVM.dailyMoneyReport.strAccountDescriptions = ArrayToString(dailyMoneyReportVM.accountDescriptions);
            dailyMoneyReportVM.dailyMoneyReport.strUserIds = ArrayToString(dailyMoneyReportVM.users);
            dailyMoneyReportVM.dailyMoneyReport.strUserNames = GetUserNameArray(dailyMoneyReportVM.users);
            dailyMoneyReportVM.dailyMoneyReport.strPaymentModes = dailyMoneyReportVM.paymentModes;            

            return View(dailyMoneyReportVM);
        }

        [Route("/GetDailyMoneyReportPdf")]
        [HttpPost]
        public async Task<IActionResult> GetDailyMoneyReportPdf(DailyMoneyReportVM dailyMoneyReportVM)
        {
            //dailyMoneyReportVM.dailyMoneyReport = new DailyMoneyReport();

            List<DailyMoneySummary> dailyMoneySummaryList = new List<DailyMoneySummary>();
            dailyMoneySummaryList = DailyMoneySummary(
                    dailyMoneyReportVM.dailyMoneyReport.strFromDate,
                    dailyMoneyReportVM.dailyMoneyReport.strToDate,
                    dailyMoneyReportVM.dailyMoneyReport.strPrograms,
                    dailyMoneyReportVM.dailyMoneyReport.strJurisdictions,
                    dailyMoneyReportVM.dailyMoneyReport.strAccountDescriptions,
                    dailyMoneyReportVM.dailyMoneyReport.strUserIds
                ).AsEnumerable().Select(s => new DailyMoneySummary()
            {
                TotalCashAmount = s.Field<decimal>("TotalCashAmount"),
                TotalChequeAmount = s.Field<decimal>("TotalChequeAmount"),
                TotalCardAmount = s.Field<decimal>("TotalCardAmount"),
                TotalMoneyOrderAmount = s.Field<decimal>("TotalMoneyOrderAmount"),
                TotalVoidAmount = s.Field<decimal>("TotalVoidAmount"),
                TotalRefundAmount = s.Field<decimal>("TotalRefundAmount"),
                TotalFinalAmount = s.Field<decimal>("TotalFinalAmount")
            }).ToList();
            dailyMoneyReportVM.dailyMoneyReport.dailyMoneySummaries = dailyMoneySummaryList;

            List<DailyMoneyUserWiseSummary> dailyMoneyUserWiseSummaryList = new List<DailyMoneyUserWiseSummary>();
            dailyMoneyUserWiseSummaryList = DailyMoneyUserWiseSummary(
                    dailyMoneyReportVM.dailyMoneyReport.strFromDate,
                    dailyMoneyReportVM.dailyMoneyReport.strToDate,
                    dailyMoneyReportVM.dailyMoneyReport.strPrograms,
                    dailyMoneyReportVM.dailyMoneyReport.strJurisdictions,
                    dailyMoneyReportVM.dailyMoneyReport.strAccountDescriptions,
                    dailyMoneyReportVM.dailyMoneyReport.strUserIds
                ).AsEnumerable().Select(s => new DailyMoneyUserWiseSummary()
            {
                Id = s.Field<int>("Id"),
                UserAlias = s.Field<string>("UserAlias"),
                CashAmount = s.Field<decimal>("CashAmount"),
                ChequeAmount = s.Field<decimal>("ChequeAmount"),
                CardAmount = s.Field<decimal>("CardAmount"),
                MoneyOrderAmount = s.Field<decimal>("MoneyOrderAmount"),
                VoidAmount = s.Field<decimal>("VoidAmount"),
                RefundAmount = s.Field<decimal>("RefundAmount"),
                TotalAmount = s.Field<decimal>("TotalAmount")
            }).ToList();
            dailyMoneyReportVM.dailyMoneyReport.dailyMoneyUserWiseSummaries = dailyMoneyUserWiseSummaryList;

            List<DailyMoneyUserWise> dailyMoneyUserWiseList = new List<DailyMoneyUserWise>();
            dailyMoneyUserWiseList = DailyMoneyUserWise(
                    dailyMoneyReportVM.dailyMoneyReport.strFromDate,
                    dailyMoneyReportVM.dailyMoneyReport.strToDate,
                    dailyMoneyReportVM.dailyMoneyReport.strPrograms,
                    dailyMoneyReportVM.dailyMoneyReport.strJurisdictions,
                    dailyMoneyReportVM.dailyMoneyReport.strAccountDescriptions,
                    dailyMoneyReportVM.dailyMoneyReport.strUserIds
                ).AsEnumerable().Select(s => new DailyMoneyUserWise()
            {
                Id = s.Field<int>("Id"),
                UserAlias = s.Field<string>("UserAlias"),
                ReceiptNo = s.Field<string>("ReceiptNo"),
                InvoiceNo = s.Field<string>("InvoiceNo"),
                PaymentOn = s.Field<DateTime>("PaymentOn"),
                TotalAmount = s.Field<decimal>("TotalAmount"),
                PaymentMethod = s.Field<string>("PaymentMethod"),
                AmountReceived = s.Field<decimal>("AmountReceived"),
                Program = s.Field<string>("Program"),
                Jurisdiction = s.Field<string>("Jurisdiction")
            }).ToList();
            dailyMoneyReportVM.dailyMoneyReport.dailyMoneyUserWises = dailyMoneyUserWiseList;

            return await _generatePdf.GetPdf("Views/PdfTemplates/DailyMoneyReport.cshtml", dailyMoneyReportVM);
        }

        //[Route("/DailyMoneyReport")]
        //[HttpPost]
        //public IActionResult Index(DailyMoneyReportVM dailyMoneyReportVM)
        //{
        //    string mimeType;
        //    string encoding;
        //    string extension;
        //    string[] streams;
        //    Warning[] warnings;

        //    string reportFilePath = string.Concat(_environment.WebRootPath, "\\Reports\\DailyMoneyReport.rdl");
        //    using (Stream reportDefinition = new FileStream(reportFilePath, FileMode.Open, FileAccess.Read))
        //    {
        //        //DailyMoney dailyMoney = new DailyMoney();
        //        //IEnumerable dsDailyMoneySummary = dailyMoney.DailyMoneySummary(dailyMoneyReportVM).AsEnumerable();
        //        IEnumerable dsDailyMoneySummary = DailyMoneySummary(dailyMoneyReportVM).AsEnumerable();
        //        IEnumerable dsDailyMoney = DailyMoney(dailyMoneyReportVM).AsEnumerable();

        //        using (LocalReport report = new LocalReport())
        //        {
        //            report.LoadReportDefinition(reportDefinition);
        //            report.DataSources.Add(new ReportDataSource("DailyMoneySummaryReport", dsDailyMoneySummary));
        //            report.DataSources.Add(new ReportDataSource("DailyMoneyReport", dsDailyMoney));

        //            ReportParameter[] rptParams = {
        //                new ReportParameter("FromDate", (dailyMoneyReportVM.fromDate == null ? "" : dailyMoneyReportVM.fromDate)),
        //                new ReportParameter("ToDate", (dailyMoneyReportVM.toDate == null ? "" : dailyMoneyReportVM.toDate)),
        //                new ReportParameter("Program", (dailyMoneyReportVM.programs == null ? "" : dailyMoneyReportVM.programs)),
        //                new ReportParameter("Jurisdiction", (dailyMoneyReportVM.jurisdictions == null ? "" : dailyMoneyReportVM.jurisdictions)),
        //                new ReportParameter("AccountDescription", (dailyMoneyReportVM.accountDescriptions == null ? "" : dailyMoneyReportVM.accountDescriptions)),
        //                new ReportParameter("User", (dailyMoneyReportVM.users == null ? "" : dailyMoneyReportVM.users)),
        //                new ReportParameter("PaymentMode", (dailyMoneyReportVM.paymentModes == null ? "" : dailyMoneyReportVM.paymentModes))
        //            };

        //            //List<ReportParameter> rptParams = new List<ReportParameter>();
        //            //rptParams.Add(new ReportParameter("FromDate", dailyMoneyReportVM.fromDate));
        //            //rptParams.Add(new ReportParameter("ToDate", dailyMoneyReportVM.toDate));
        //            //rptParams.Add(new ReportParameter("Program", dailyMoneyReportVM.programs));
        //            //rptParams.Add(new ReportParameter("Jurisdiction", dailyMoneyReportVM.jurisdictions));
        //            //rptParams.Add(new ReportParameter("AccountDescription", dailyMoneyReportVM.accountDescriptions));
        //            //rptParams.Add(new ReportParameter("User", dailyMoneyReportVM.users));
        //            //rptParams.Add(new ReportParameter("PaymentMode", dailyMoneyReportVM.paymentModes));
        //            //report.SetParameters(rptParams);

        //            //List<ReportParameter> rptParams = new List<ReportParameter>();
        //            //rptParams.Add(new ReportParameter("FromDate", ""));
        //            //rptParams.Add(new ReportParameter("ToDate", ""));
        //            //rptParams.Add(new ReportParameter("Program", ""));
        //            //rptParams.Add(new ReportParameter("Jurisdiction", ""));
        //            //rptParams.Add(new ReportParameter("AccountDescription", ""));
        //            //rptParams.Add(new ReportParameter("User", ""));
        //            //rptParams.Add(new ReportParameter("PaymentMode", ""));
        //            report.SetParameters(rptParams);

        //            byte[] reportData = report.Render("pdf", string.Empty, out mimeType, out encoding, out extension, out streams, out warnings);
        //            return File(reportData, "application/pdf");
        //        }
        //    }
        //}

        private DataTable DailyMoneySummary(string? strFromDate, string? strToDate, string strPrograms, string strJurisdictions, string strAccountDescriptions, string strUserIds)
        {
            DataTable dt = new DataTable();

            SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("dbConString"));
            SqlCommand command = new SqlCommand("DailyMoneySummaryReport", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@FromDate", strFromDate);
            command.Parameters.AddWithValue("@ToDate", strToDate);
            command.Parameters.AddWithValue("@Program", strPrograms);
            command.Parameters.AddWithValue("@Jurisdiction", strJurisdictions);
            command.Parameters.AddWithValue("@AccountDescription", strAccountDescriptions);
            command.Parameters.AddWithValue("@User", strUserIds);

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);
            sqlDataAdapter.Fill(dt);

            return dt;
        }

        private DataTable DailyMoneyUserWiseSummary(string? strFromDate, string? strToDate, string strPrograms, string strJurisdictions, string strAccountDescriptions, string strUserIds)
        {
            DataTable dt = new DataTable();

            SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("dbConString"));
            SqlCommand command = new SqlCommand("DailyMoneyUserWiseSummaryReport", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@FromDate", strFromDate);
            command.Parameters.AddWithValue("@ToDate", strToDate);
            command.Parameters.AddWithValue("@Program", strPrograms);
            command.Parameters.AddWithValue("@Jurisdiction", strJurisdictions);
            command.Parameters.AddWithValue("@AccountDescription", strAccountDescriptions);
            command.Parameters.AddWithValue("@User", strUserIds);

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);
            sqlDataAdapter.Fill(dt);

            return dt;
        }

        private DataTable DailyMoneyUserWise(string? strFromDate, string? strToDate, string strPrograms, string strJurisdictions, string strAccountDescriptions, string strUserIds)
        {
            DataTable dt = new DataTable();

            SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("dbConString"));
            SqlCommand command = new SqlCommand("DailyMoneyUserWiseReport", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@FromDate", strFromDate);
            command.Parameters.AddWithValue("@ToDate", strToDate);
            command.Parameters.AddWithValue("@Program", strPrograms);
            command.Parameters.AddWithValue("@Jurisdiction", strJurisdictions);
            command.Parameters.AddWithValue("@AccountDescription", strAccountDescriptions);
            command.Parameters.AddWithValue("@User", strUserIds);

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);
            sqlDataAdapter.Fill(dt);

            return dt;
        }

        //private string ArrayToString(string[] strArray)
        //{
        //    string strParam = string.Empty;

        //    if(strArray.Length > 0)
        //    {
        //        foreach(string str in strArray)
        //        {
        //            strParam = strParam == string.Empty ? str : strParam + ", " + str;
        //        }
        //    }

        //    return strParam;
        //}

        private string GetUserNameArray(string[] strArray)
        {
            string strParam = string.Empty;

            if (strArray.Length > 0)
            {
                foreach (string str in strArray)
                {
                    var user = _userRepo.GetFirstOrDefault(filter: x => x.Id == Convert.ToInt32(str));
                    strParam = strParam == string.Empty ? user.FirstName + " " + user.LastName : strParam + ", " + user.FirstName + " " + user.LastName;
                }
            }

            return strParam;
        }

        private string ArrayToString(string[] strArray)
        {
            string strParam = string.Empty;

            if (strArray.Length > 0)
            {
                  foreach (string str in strArray)
                  {
                        strParam = strParam == string.Empty ? str : strParam + "," + str;
                  }
            }

            return strParam;
        }
    }
}
