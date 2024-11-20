using BrazosApp.DataAccess.Repository.IRepository;
using BrazosApp.Models;
using BrazosApp.Utility.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BrazosApp.Areas.LoggedIn.Controllers
{
    [Area("LoggedIn")]
    [Authorize("UserManagePolicy")]
    public class EventController : Controller
    {
        private readonly IRepository<Events> _events;
        private readonly IEncrypt _encrypt;
        public EventController(IRepository<Events> events, IEncrypt encrypt)
        {
            _events = events;
            _encrypt = encrypt;
        }

        [Route("/Events")]
        public IActionResult Index()
        {
            Events model = new Events();
            return View(model);
        }

        [HttpGet("/GetAllEvents")]
        public async Task<IActionResult> GetAllEvents()
        {
            var Events = from e in await _events.GetAllAsync()
                         select new
                         {
                             Id = e.Id,
                             Name = e.Name,
                             Location = e.Location,
                             StartDate = e.StartDate.ToShortDateString(),
                             EndDate = e.EndDate.ToShortDateString(),
                             IsActive = e.IsActive,
                             EncryptedId = _encrypt.Encrypt256(e.Id.ToString())
                         };
            return Json(new { data = Events.ToList() });

        }

        [HttpGet("/GetEvent/{id?}")]
        public async Task<IActionResult> GetEvent(string id)
        {
            var Event = await _events.GetFirstOrDefaultAsync(filter: x=>x.Id == Convert.ToInt32(_encrypt.Decrypt256(id)));
            return Json(new { success= true, Event = Event });

        }

        [HttpPost("/EventsUpsert")]
        public async Task<IActionResult> EventsUpsert(Events model)
        {
            if(ModelState.IsValid)
            {
                if(model.Id == 0)
                {
                    model.CreatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    await _events.AddAsync(model);                    
                }
                else
                {
                    var Event = await _events.GetFirstOrDefaultAsync(filter:x=>x.Id==model.Id);
                    Event.Name = model.Name;
                    Event.Location = model.Location;
                    Event.StartDate = model.StartDate;
                    Event.EndDate = model.EndDate;
                    Event.UpdatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    Event.UpdatedOn = DateTime.Now;
                    await _events.UpdateAsync(Event);
                }
                return Json(new { success = true, msg = "Information Saved Successfully" });
            }
            return Json(new {success = false, msg="Unexpected Error Occurred"});
        }

        
        [HttpPost("/ChangeEventState/{id?}")]
        public async Task<IActionResult> ActiveInactive(string id)
        {
            var TiD = _encrypt.Decrypt256(id);
            string msg = "";
            var territory = await _events.GetById(Convert.ToInt32(TiD));
            if (territory.IsActive == true)
            {
                territory.IsActive = false;
                msg = "Inactivated";
            }
            else
            {
                territory.IsActive = true;
                msg = "Activated";
            }
            await _events.UpdateAsync(territory);
            return Json(new { success = true, msg = msg });
        }
    }
}
