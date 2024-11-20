using BrazosApp.DataAccess.Repository.IRepository;
using BrazosApp.Models;
using BrazosApp.Models.APIDTOs;
using BrazosApp.Models.DTOs;
using BrazosApp.Models.DTOs.ViewModels;
using BrazosApp.Models.ViewModels;
using BrazosApp.Utility.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;
using Wkhtmltopdf.NetCore;

namespace BrazosApp.Areas.Inspector.Controllers
{
    [Area("Inspector")]
      [Authorize(AuthenticationSchemes = "InspectorLoginScheme", Policy = "InspectorPolicy")]
      public class InspectionController : Controller
      {
            private readonly IConfiguration _configuration;
            private readonly IGeneratePdf _generatePdf;
            private readonly IEmailSenderService _emailSender;
            private readonly IRepository<InspectionPurposes> _purpose;
            private readonly IWebHostEnvironment _hostEnvironment;
            private readonly string containerName = @"\InspectionImage\Food\";
            public InspectionController(IConfiguration configuration, IGeneratePdf generatePdf, IEmailSenderService emailSender, IWebHostEnvironment hostEnvironment, IRepository<InspectionPurposes> purpose)
            {
                  _configuration = configuration;
                  _generatePdf = generatePdf;
                  _emailSender = emailSender;
                  _purpose = purpose;
                  _hostEnvironment = hostEnvironment;

            }

            [HttpGet("/Inspection/{code?}")]
            public async Task<IActionResult> Index(string code)
            {
                  ViewBag.Code = code;
                  var token = User.FindFirstValue("Token");
                  using (var httpClient = new HttpClient())
                  {
                        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                        httpClient.DefaultRequestHeaders.Add("Role", User.FindFirstValue(ClaimTypes.Role));
                        using (var response = await httpClient.GetAsync(_configuration.GetSection("APIUrl").Value + "/LoadInspectionIndexSearchData"))
                        {
                            var responseResult = await response.Content.ReadAsStringAsync();
                            var apiResponse = JsonConvert.DeserializeObject<APIResponse>(responseResult);
                            if (apiResponse!.IsSuccess == true)
                            {
                                var apiResultObj = JsonConvert.DeserializeObject<InspectionSearchParamsVM>(Convert.ToString(apiResponse.Result)!);
                                InspectionSearchParamsVM model = new InspectionSearchParamsVM();
                                model.PurposeList = apiResultObj!.PurposeList;
                                model.AssignInspectorList = apiResultObj!.AssignInspectorList;
                                return View(model);
                            }
                        }
                  }
                  return View();
            }

            [HttpGet("/NewInspection/{id?}")]
            public async Task<IActionResult> Create(string? id)
            {
                  var token = User.FindFirstValue("Token");
                  var code = "";
                  if (id == null)
                  {
                        //InspectionCreateVM model = new InspectionCreateVM();
                        //return View(model);
                        code = Request.Query["code"].ToString();
                        id = "Null";

                        //
                        //using (var httpClient = new HttpClient())
                        //{
                        //      httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                        //      using (var response = await httpClient.GetAsync(_configuration.GetSection("APIUrl").Value + "/LoadNewInspectionData"))
                        //      {
                        //            var responseResult = await response.Content.ReadAsStringAsync();
                        //            var apiResponse = JsonConvert.DeserializeObject<APIResponse>(responseResult);
                        //            if (apiResponse!.IsSuccess == true)
                        //            {
                        //                  var apiResultObj = JsonConvert.DeserializeObject<InspectionResponseDTO>(Convert.ToString(apiResponse.Result)!);
                        //                  InspectionCreateVM model = new InspectionCreateVM();
                        //                  model.Response = apiResultObj;
                        //                  model.Response.Code = code;
                        //                  return View(model);
                        //            }
                        //      }
                        //}
                  }
                  else
                  {
                        //using (var httpClient = new HttpClient())
                        //{
                        //      httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                        //      using (var response = await httpClient.GetAsync(_configuration.GetSection("APIUrl").Value + "/LoadInspectionData?id=" + id))
                        //      {
                        //            var responseResult = await response.Content.ReadAsStringAsync();
                        //            var apiResponse = JsonConvert.DeserializeObject<APIResponse>(responseResult);
                        //            if (apiResponse!.IsSuccess == true)
                        //            {
                        //                  var apiResultObj = JsonConvert.DeserializeObject<InspectionResponseDTO>(Convert.ToString(apiResponse.Result)!);
                        //                  InspectionCreateVM model = new InspectionCreateVM();
                        //                  model.Response = apiResultObj;
                        //                  return View(model);
                        //            }
                        //      }
                        //}
                  }

                  using (var httpClient = new HttpClient())
                  {
                        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                        using (var response = await httpClient.GetAsync(_configuration.GetSection("APIUrl").Value + "/LoadInspectionData?id=" + id))
                        {
                              var responseResult = await response.Content.ReadAsStringAsync();
                              var apiResponse = JsonConvert.DeserializeObject<APIResponse>(responseResult);
                              if (apiResponse!.IsSuccess == true)
                              {
                                    var apiResultObj = JsonConvert.DeserializeObject<InspectionResponseDTO>(Convert.ToString(apiResponse.Result)!);
                                    InspectionCreateVM model = new InspectionCreateVM();
                                    model.Response = apiResultObj;

                                    if (id == "Null")
                                    {
                                          model.Response.Code = code;
                                    }

                                    return View(model);
                              }
                        }
                  }
                  return Ok();
            }

        [HttpGet("/EditInspectionInspectorPanel/{id?}")]
        public async Task<IActionResult> Edit(string? id)
        {
            var token = User.FindFirstValue("Token");
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                using (var response = await httpClient.GetAsync(_configuration.GetSection("APIUrl").Value + "/LoadInspectionEditData?id=" + id))
                {
                    var responseResult = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<APIResponse>(responseResult);
                    if (apiResponse!.IsSuccess == true)
                    {
                        var apiResultObj = JsonConvert.DeserializeObject<InspectionEditDTO>(Convert.ToString(apiResponse.Result)!);
                        InspectionEditSaveDTO model = new InspectionEditSaveDTO();
                        model.Request = apiResultObj;
                        return View(model);
                    }
                }
            }
            return Ok();
        }

        //[HttpPost("/SaveInspectionItems")]
        //public async Task<IActionResult> SaveInspectionItems(InspectionItemDetails vmodel)
        //{
        //    var token = User.FindFirstValue("Token");
        //    InspectionItemDetailsRequestDTO model = new InspectionItemDetailsRequestDTO();
        //    model.InspectionId = vmodel.InspectionId;
        //    model.Status = vmodel.Status;
        //    model.Cos = vmodel.Cos;
        //    model.R = vmodel.R;
        //    model.ItemId = vmodel.ItemId;
        //    //model.ImageFile = "";

        //    using (var memoryStream = new MemoryStream())
        //    {
        //        await vmodel.ImageFile!.CopyToAsync(memoryStream);
        //        var fileBytes = memoryStream.ToArray();
        //        model.ImageFile = Convert.ToBase64String(fileBytes);
        //    }
        //    using (var httpClient = new HttpClient())
        //    {
        //        StringContent stringContent = new StringContent("");
        //        try
        //        {
        //            stringContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
        //        }
        //        catch (Exception ex) { }
        //        //StringContent stringContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
        //        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        //        using (var response = await httpClient.PostAsync(_configuration.GetSection("APIUrl").Value + "/SaveInspectionItemsAPI", stringContent))
        //        {
        //            var responseResult = await response.Content.ReadAsStringAsync();
        //            var apiResponse = JsonConvert.DeserializeObject<APIResponse>(responseResult);
        //            if (apiResponse!.IsSuccess == true)
        //            {
        //                if (model.ImageFile != null)
        //                {
        //                    var uploadPath = _hostEnvironment.WebRootPath + containerName + model.InspectionId;
        //                    if (!Directory.Exists(uploadPath))
        //                    {
        //                        Directory.CreateDirectory(uploadPath);
        //                    }
        //                    var fileName = model.ItemId.ToString() + Path.GetExtension(vmodel.ImageFile!.FileName);
        //                    var filePath = Path.Combine(uploadPath, fileName);
        //                    await vmodel.ImageFile.CopyToAsync(new FileStream(filePath, FileMode.Create));
        //                    vmodel.Image = fileName;
        //                }

        //                //var apiResultObj = JsonConvert.DeserializeObject<InspectionResponseDTO>(Convert.ToString(apiResponse.Result)!);
        //                //InspectionCreateVM model = new InspectionCreateVM();
        //                //model.Response = apiResultObj;
        //                return Json(new { sucess = true });
        //            }
        //        }
        //    }

        //    return Json(new { success = false });
        //}

            [HttpPost("/SaveInspectionItems")]
            public async Task<IActionResult> SaveInspectionItems([FromForm] InspectionItemDetails vmodel)
            {
                  var token = User.FindFirstValue("Token");
                  InspectionItemDetailsRequestDTO model = new InspectionItemDetailsRequestDTO
                  {
                        InspectionId = vmodel.InspectionId,
                        Status = vmodel.Status,
                        Cos = vmodel.Cos,
                        R = vmodel.R,
                        ItemId = vmodel.ItemId,
                        Comment = vmodel.Comment,
                  };

                  if (vmodel.ImageFile != null)
                  {
                        using (var memoryStream = new MemoryStream())
                        {
                              await vmodel.ImageFile.CopyToAsync(memoryStream);
                              var fileBytes = memoryStream.ToArray();
                              model.ImageFile = Convert.ToBase64String(fileBytes);
                        }
                  }

                  using (var httpClient = new HttpClient())
                  {
                        try
                        {
                              var jsonContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
                              httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                              var apiUrl = _configuration.GetSection("APIUrl").Value + "/SaveInspectionItemsAPI";
                              using (var response = await httpClient.PostAsync(apiUrl, jsonContent))
                              {
                                    var responseResult = await response.Content.ReadAsStringAsync();
                                    var apiResponse = JsonConvert.DeserializeObject<APIResponse>(responseResult);

                                    if (apiResponse != null && apiResponse.IsSuccess)
                                    {
                                          //if (model.ImageFile != null)
                                          //{
                                          //      var uploadPath = Path.Combine(_hostEnvironment.WebRootPath, "containerName", model.InspectionId.ToString());
                                          //      if (!Directory.Exists(uploadPath))
                                          //      {
                                          //            Directory.CreateDirectory(uploadPath);
                                          //      }
                                          //      var fileName = $"{model.ItemId}{Path.GetExtension(vmodel.ImageFile!.FileName)}";
                                          //      var filePath = Path.Combine(uploadPath, fileName);
                                          //      using (var fileStream = new FileStream(filePath, FileMode.Create))
                                          //      {
                                          //            await vmodel.ImageFile.CopyToAsync(fileStream);
                                          //      }
                                          //      vmodel.Image = fileName;
                                          //}
                                          return Json(new { success = true });
                                    }
                              }
                        }
                        catch (Exception ex)
                        {
                              
                        }
                  }

                  return Json(new { success = false });
            }

            [HttpGet("/InspectionPdf/{id?}")]
            public async Task<IActionResult> GeneratePermitCertificatepdf(string? id)
            {
                var token = User.FindFirstValue("Token");
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    using (var response = await httpClient.GetAsync(_configuration.GetSection("APIUrl").Value + "/GetInspectionDataPdf?id=" + id))
                    {
                        var responseResult = await response.Content.ReadAsStringAsync();
                        var apiResponse = JsonConvert.DeserializeObject<APIResponse>(responseResult);
                        if (apiResponse!.IsSuccess == true)
                        {
                            var apiResultObj = JsonConvert.DeserializeObject<FoodInspectionPdfDTO>(Convert.ToString(apiResponse.Result)!);
                            //OpeningInspectionCreateVM model = new OpeningInspectionCreateVM();
                            //model.Response = apiResultObj;
                            return await _generatePdf.GetPdf("Views/PdfTemplates/InspectionReportPdf.cshtml", apiResultObj);
                            //return View(model);
                        }
                    }
                }
                return Ok();

                //return await _generatePdf.GetPdf("Views/PdfTemplates/InspectionReportPdf.cshtml");
            }


            [HttpGet("/DownloadInspectionCommentCertificateAPIPdf/{id?}")]
            public async Task<IActionResult> DownloadPermitCertificatePdf(string id)
            {
                var token = User.FindFirstValue("Token");
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    using (var response = await httpClient.GetAsync(_configuration.GetSection("APIUrl").Value + "/GetInspectionCommentSectionAPIDataPdf?id=" + id))
                    {
                        var responseResult = await response.Content.ReadAsStringAsync();
                        var apiResponse = JsonConvert.DeserializeObject<APIResponse>(responseResult);
                        if (apiResponse!.IsSuccess == true)
                        {
                            var apiResultObj = JsonConvert.DeserializeObject<InspectionCommentPdfDTO>(Convert.ToString(apiResponse.Result)!);
                            var generatedPdf = await _generatePdf.GetByteArray("Views/PdfTemplates/InspectionCommentPagePdf.cshtml", apiResultObj);
                            Response.ContentType = "application/pdf";
                            Response.Headers.Add("Content-Disposition", "attachment; filename=InspectionCommentSection.pdf");
                            return new FileContentResult(generatedPdf, "application/pdf");
                        }
                    }
                }
                return Ok();
            }

            [HttpPost("/SendInspectionMailPdf")]
            public async Task<IActionResult> SendMail(string InsId, string toEmail, string ccEmail, string Subject, string Body)
            {
                  var token = User.FindFirstValue("Token");
                  using (var httpClient = new HttpClient())
                  {
                        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                        using (var response = await httpClient.GetAsync(_configuration.GetSection("APIUrl").Value + "/GetInspectionDataPdf?id=" + InsId))
                        {
                              var responseResult = await response.Content.ReadAsStringAsync();
                              var apiResponse = JsonConvert.DeserializeObject<APIResponse>(responseResult);
                              if (apiResponse!.IsSuccess == true)
                              {
                                    var apiResultObj = JsonConvert.DeserializeObject<FoodInspectionPdfDTO>(Convert.ToString(apiResponse.Result)!);
                                    //OpeningInspectionCreateVM model = new OpeningInspectionCreateVM();
                                    //model.Response = apiResultObj;
                                    //return await _generatePdf.GetPdf("Views/PdfTemplates/OpeningInspectionReportPdf.cshtml", apiResultObj);
                                    //return View(model);
                                    var generatedPdf = await _generatePdf.GetByteArray("Views/PdfTemplates/InspectionReportPdf.cshtml", apiResultObj);
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
