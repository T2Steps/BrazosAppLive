using BrazosApp.Models;
using BrazosApp.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using Wkhtmltopdf.NetCore;
using BrazosApp.Utility.Services;
using BrazosApp.Models.ViewModels;

namespace BrazosApp.Areas.Inspector.Controllers
{
    [Area("Inspector")]
    [Authorize(AuthenticationSchemes = "InspectorLoginScheme", Policy = "InspectorPolicy")]
    public class OpeningInspectionController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IGeneratePdf _generatePdf;
        private readonly IEmailSenderService _emailSender;
        public OpeningInspectionController(IConfiguration configuration, IGeneratePdf generatePdf, IEmailSenderService emailSender)
        {
            _configuration = configuration;
            _generatePdf = generatePdf;
            _emailSender = emailSender;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("/OpeningInspection/{id?}")]
        public async Task<IActionResult> Create(string? id)
        {
            var token = User.FindFirstValue("Token");
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                using (var response = await httpClient.GetAsync(_configuration.GetSection("APIUrl").Value + "/LoadOpeningInspectionData?id=" + id))
                {
                    var responseResult = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<APIResponse>(responseResult);
                    if (apiResponse!.IsSuccess == true)
                    {
                        var apiResultObj = JsonConvert.DeserializeObject<OpeningInspectionResponseDTO>(Convert.ToString(apiResponse.Result)!);
                        OpeningInspectionCreateVM model = new OpeningInspectionCreateVM();
                        model.Response = apiResultObj;
                        return View(model);
                    }
                }
            }

            return Ok();
        }

        [HttpGet("/OpeningInspectionEdit/{id?}")]
        public async Task<IActionResult> Edit(string? id)
        {
            var token = User.FindFirstValue("Token");
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                using (var response = await httpClient.GetAsync(_configuration.GetSection("APIUrl").Value + "/LoadOpeningInspectionEditData?id=" + id))
                {
                    var responseResult = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<APIResponse>(responseResult);
                    if (apiResponse!.IsSuccess == true)
                    {
                        var apiResultObj = JsonConvert.DeserializeObject<EditOpeningInspectionVM>(Convert.ToString(apiResponse.Result)!);
                        //OpeningInspectionCreateVM model = new OpeningInspectionCreateVM();
                        //model.Response = apiResultObj;
                        return View(apiResultObj);
                    }
                }
            }

            return Ok();
        }

        [HttpGet("/OpeningInspectionPdf/{id?}")]
        public async Task<IActionResult> GeneratePermitCertificatepdf(string? id)
        {
            var token = User.FindFirstValue("Token");
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                using (var response = await httpClient.GetAsync(_configuration.GetSection("APIUrl").Value + "/GetOpeningCheckDataPdf?id=" + id))
                {
                    var responseResult = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<APIResponse>(responseResult);
                    if (apiResponse!.IsSuccess == true)
                    {
                        var apiResultObj = JsonConvert.DeserializeObject<OpeningCheckListPdfDTO>(Convert.ToString(apiResponse.Result)!);
                        //OpeningInspectionCreateVM model = new OpeningInspectionCreateVM();
                        //model.Response = apiResultObj;
                        return await _generatePdf.GetPdf("Views/PdfTemplates/OpeningInspectionReportPdf.cshtml", apiResultObj);
                        //return View(model);
                    }
                }
            }
            return Ok();
        }

        [HttpPost("/SendOpeningInspectionMailPdf")]
        public async Task<IActionResult> SendMail(string InsId, string toEmail, string ccEmail, string Subject, string Body)
        {
            var token = User.FindFirstValue("Token");
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                using (var response = await httpClient.GetAsync(_configuration.GetSection("APIUrl").Value + "/GetOpeningCheckDataPdf?id=" + InsId))
                {
                    var responseResult = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<APIResponse>(responseResult);
                    if (apiResponse!.IsSuccess == true)
                    {
                        var apiResultObj = JsonConvert.DeserializeObject<OpeningCheckListPdfDTO>(Convert.ToString(apiResponse.Result)!);
                        //OpeningInspectionCreateVM model = new OpeningInspectionCreateVM();
                        //model.Response = apiResultObj;
                        //return await _generatePdf.GetPdf("Views/PdfTemplates/OpeningInspectionReportPdf.cshtml", apiResultObj);
                        //return View(model);
                        var generatedPdf = await _generatePdf.GetByteArray("Views/PdfTemplates/OpeningInspectionReportPdf.cshtml", apiResultObj);
                        Byte[][] bytearray = new Byte[][] { generatedPdf };
                        string[] pdfName = new string[] { "InspectionReport.Pdf" };
                        await _emailSender.SendEmail(toEmail, ccEmail, Subject, Body, bytearray, pdfName);
                        return Json(new { success = true, msg = "Mail sent successfully" });
                    }
                }
            }
            
            return Ok();
        }

    }
}
