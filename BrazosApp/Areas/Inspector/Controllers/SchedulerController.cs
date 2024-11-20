using BrazosApp.Models;
using BrazosApp.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace BrazosApp.Areas.Inspector.Controllers
{
    [Area("Inspector")]
    [Authorize(AuthenticationSchemes = "InspectorLoginScheme", Policy = "InspectorPolicy")]
    public class SchedulerController : Controller
    {
        private readonly IConfiguration _configuration;
        public SchedulerController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        //private readonly IRepository<Establishment> _establishment;
        //private readonly IRepository<Users> _users;
        //private readonly IRepository<Schedule> _schedule;
        //private readonly IRepository<Territory> _territory;
        //private readonly IRepository<TerritoryWiseInspectors> _territorywiseIns;

        //public SchedulerController(IRepository<Establishment> establishment, IRepository<Users> users, IRepository<Schedule> schedule,
        //    IRepository<Territory> territory, IRepository<TerritoryWiseInspectors> territorywiseIns)
        //{
        //    _establishment = establishment;
        //    _users = users;
        //    _schedule = schedule;
        //    _territory = territory;
        //    _territorywiseIns = territorywiseIns;
        //}

        [HttpGet("/Schedule/{code?}")]
        public async Task<IActionResult> Index(string code)
        {
            ViewBag.Code = code;
            //ScheduleModalDataRequestDTO model = new ScheduleModalDataRequestDTO();
            var token = User.FindFirstValue("Token");
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                httpClient.DefaultRequestHeaders.Add("Role", User.FindFirstValue(ClaimTypes.Role));
                using (var response = await httpClient.GetAsync(_configuration.GetSection("APIUrl").Value + "/LoadInspectorScheduleData"))
                {
                    var responseResult = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<APIResponse>(responseResult);
                    if (apiResponse!.IsSuccess == true)
                    {
                        var apiResultObj = JsonConvert.DeserializeObject<ScheduleModalDataResponseDTO>(Convert.ToString(apiResponse.Result)!);
                        ScheduleModalDataRequestDTO model = new ScheduleModalDataRequestDTO();
                        model.Purposes = apiResultObj!.Purposes;
                        model.AssignInspectorList = apiResultObj!.AssignInspectorList;
                        model.SearchParamsVM = new Models.ViewModels.InspectionSearchParamsVM();
                        model.SearchParamsVM!.PurposeList = apiResultObj.SearchPurposeList;
                        model.SearchParamsVM.AssignInspectorList = apiResultObj.SearchAssignInspectorList;
                        return View(model);
                    }
                }
            }
            //return View(model);
            return Ok();
        }


        
    }
}
