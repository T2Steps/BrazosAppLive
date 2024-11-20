using BrazosApp.DataAccess.Repository.IRepository;
using BrazosApp.Models;
using BrazosApp.Models.ViewModels;
using BrazosApp.Utility;
using BrazosApp.Utility.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace BrazosApp.Areas.LoggedIn.Controllers
{
    [Area("LoggedIn")]
    [Authorize("UserManagePolicy")]
    public class AreaController : Controller
    {
        private readonly IRepository<Users> _users;
        private readonly IRepository<Area> _area;
        private readonly IRepository<AreaWiseInspectors> _inspectors;
        private readonly IEncrypt _encrypt;

        public AreaController(IRepository<Users> users, IRepository<Area> area, IRepository<AreaWiseInspectors> inspectors, IEncrypt encrypt)
        {
            _area = area;
            _users = users;
            _inspectors = inspectors;
            _encrypt = encrypt;
        }
        [Route("/Areas")]
        public async Task<IActionResult> Index()
        {
            AreaUpsertVM model = new AreaUpsertVM();
            var users = await _users.GetAllAsync(filter: x => x.Role!.Name == SD.Inspector||x.Role.Name == SD.AdminInspector || x.Role.Name==SD.SuperAdmin, includeProperties: "Role");
            model.InspectorList = users.Select(x => new SelectListItem
            {
                Text = x.FirstName + " " + x.LastName,
                Value = x.Id.ToString(),
            }).ToList();
            model.AreaWiseInspectors = new AreaWiseInspectors();
            model.Area = new Area();
            return View(model);
        }

        [Route("/GetAllAreas")]
        [HttpGet]
        public async Task<IActionResult> GetAllAreas()
        {
            var Areas = await _area.GetAllAsync(/*filter:x=>x.AreaNumber>=1 && x.AreaNumber<=7,*/ includeProperties: "Inspectors.AssignedUser");
            var InspectorList = await _inspectors.GetAllAsync();

            var AreaList = Areas
              .Select(t => new
              {
                  Id = t.Id,
                  Area = t.AreaNumber,
                  Description = t.Description,
                  IsActive = t.IsActive,
                  encryptedId = _encrypt.Encrypt256(Convert.ToString(t.Id)),
                  AssignedUsernames = InspectorList
                              .Where(i => i.AreaId == t.Id)
                              .Select(async i =>
                              {
                                  var Name = await _users.GetById(i.AssignedUserId);
                                  return Name.FirstName + " " + Name.LastName;
                              })
                              .ToList()
              })
              .ToList();
            return Json(new { data = AreaList.ToList() });
        }

        [HttpPost("/AreaUpsert")]
        public async Task<IActionResult> AreaUpsert(Area model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            else
            {
                var msg = "";
                if (model.Id == 0)
                {
                    model.IsActive = true;
                    await _area.AddAsync(model);
                    msg = "Successfully Created";
                }
                else
                {
                    model.IsActive = true;
                    await _area.UpdateAsync(model);
                    msg = "Successfully Updated";
                }
                return Json(new { success = true, msg = msg });
            }
        }

        [HttpGet("/GetArea/{id?}")]
        public async Task<IActionResult> GetArea(string id)
        {
            if (id == null)
            {
                return Json(new { success= false });
            }
            var area = await _area.GetById(Convert.ToInt32(_encrypt.Decrypt256(id)));
            return Json(new {success = true, area = area});
        }


        [Route("/GetAllAreaAssignedUsers/{id?}")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers(string id)
        {
            var TiD = _encrypt.Decrypt256(id);
            var Area = await _area.GetFirstOrDefaultAsync(filter:x=>x.Id== Convert.ToInt32(TiD));
            var Inspectors = await _inspectors.GetAllAsync(filter: x => x.AreaId == Convert.ToInt32(TiD), includeProperties: "AssignedUser");
            foreach (var Inspector in Inspectors)
            {
                Inspector.EncryptedId = _encrypt.Encrypt256(Convert.ToString(Inspector.Id));
            }
            return Json(new { success = true, inspectors = Inspectors, area = Area });
        }

        [Route("/AssignInspectorsToAreas")]
        [HttpPost]
        public async Task<IActionResult> AssignInspector(AreaUpsertVM model)
        {
            if (ModelState.IsValid)
            {
                var ExistingInspector = await _inspectors.GetFirstOrDefaultAsync(filter: x => x.AssignedUserId == model.AreaWiseInspectors!.AssignedUserId && x.AreaId == model.AreaWiseInspectors.AreaId);
                if (ExistingInspector != null)
                {
                    return Json(new { success = false, msg = "Inspector Already Assigned To This Territory" });
                }
                model.AreaWiseInspectors!.CreatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                await _inspectors.AddAsync(model.AreaWiseInspectors);
                var user = await _users.GetFirstOrDefaultAsync(filter: x => x.Id == model.AreaWiseInspectors.AssignedUserId);
                return Json(new { success = true, msg = "Successfully Assigned", id = model.AreaWiseInspectors.Id, name = user.FirstName + " " + user.LastName, encryptedId = _encrypt.Encrypt256(Convert.ToString(model.AreaWiseInspectors.Id)) });
            }
            return Json(new { success = false, msg = "Unexpected Error Occurred" });
        }

        [Route("/RemoveInspectorFromArea/{id?}")]
        [HttpPost]
        public async Task<IActionResult> RemoveInspector(string id)
        {
            var insId = _encrypt.Decrypt256(id);
            if (id != null)
            {
                var data = await _inspectors.GetFirstOrDefaultAsync(filter: x => x.Id == Convert.ToInt32(insId), includeProperties: "AssignedUser");
                var userId = data.AssignedUser!.Id;
                var name = data.AssignedUser!.FirstName + " " + data.AssignedUser!.LastName;
                await _inspectors.RemoveAsync(data);
                return Json(new { success = true, msg = "Success", id = userId, name = name, encryptedId = id });
            }
            return Json(new { success = false, msg = "Not Found" });
        }


        [Route("/ChangeAreaState/{id?}")]
        [HttpPost]
        public async Task<IActionResult> ActiveInactive(string id)
        {
            var TiD = _encrypt.Decrypt256(id);
            string msg = "";
            var territory = await _area.GetById(Convert.ToInt32(TiD));
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
            await _area.UpdateAsync(territory);
            return Json(new { success = true, msg = msg });
        }

    }
}
