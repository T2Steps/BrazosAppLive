using BrazosApp.DataAccess.Repository.IRepository;
using BrazosApp.Models;
using BrazosApp.Models.ViewModels;
using BrazosApp.Utility;
using BrazosApp.Utility.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Security.Claims;

namespace BrazosApp.Areas.LoggedIn.Controllers
{
    [Area("LoggedIn")]
    [Authorize("UserManagePolicy")]
    public class TerritoryController : Controller
    {
        private readonly IRepository<Users> _users;
        private readonly IRepository<Territory> _territory;
        private readonly IRepository<TerritoryWiseInspectors> _inspectors;
        private readonly IEncrypt _encrypt;

        public TerritoryController(IRepository<Users> users, IRepository<Territory> territory, IRepository<TerritoryWiseInspectors> inspectors, IEncrypt encrypt)
        {
            _territory = territory;
            _users = users;
            _inspectors = inspectors;
            _encrypt = encrypt;
        }

        [Route("/Territories")]
        public async Task<IActionResult> Index()
        {
            TerritoryUpsertVM model = new TerritoryUpsertVM();
            var users = await _users.GetAllAsync(filter: x => x.Role.Name == SD.Inspector, includeProperties: "Role");
            model.InspectorList = users.Select(x => new SelectListItem
            {
                Text = x.FirstName + " " + x.LastName,
                Value = x.Id.ToString(),
            }).ToList();
            model.TerritoryWiseInspectors = new TerritoryWiseInspectors();
            return View(model);
        }

        //[Route("/GetAllTerritories")]
        //[HttpGet]
        //public async Task<IActionResult> GetAllTerritories()
        //{
        //    //var Territory = await _territory.GetAllAsync(
        //    //                filter: x => x.IsActive == true && x.IsDeleted == false,
        //    //                includeProperties: "Inspectors.AssignedUser");

        //    var Territory = await _territory.GetAllAsync(
        //                         filter: x => x.IsActive == true && x.IsDeleted == false, includeProperties: "Inspectors");

        //    var Inspectors = from ter in Territory
        //                     select new
        //                     {
        //                         Inspectors = _inspectors.GetAllAsync(filter: x => x.TerritoryId == ter.Id).GetAwaiter()
        //                     };

        //    var TerritoryList = from ter in Territory
        //                        select new TerritoryListDTO()
        //                        {
        //                            Id = ter.Id,
        //                            Name = ter.Name,
        //                            ColorCode = ter.ColorCode
        //                        };



        //    //var TerritoryList = (from ter in Territory
        //    //                         join assignTer in await _inspectors.GetAllAsync() on ter.Id equals assignTer.TerritoryId
        //    //                         //join user in await _users.GetAllAsync() on assignTer.AssignedUserId equals user.Id
        //    //                         select new
        //    //                         {
        //    //                             Id = ter.Id,
        //    //                             Name = ter.Name,
        //    //                             ColorCode = ter.ColorCode,
        //    //                             AssignId = assignTer.AssignedUserId
        //    //                             //AssignedUsernames = user.FirstName + " " + user.LastName,
        //    //                         });

        //    //var TerritoryList = Territory.Select(t => new {
        //    //    Id = t.Id,
        //    //    Name = t.Name,
        //    //    ColorCode = t.ColorCode,
        //    //    AssignedUsernames = t.Inspectors!.Select(i => i.AssignedUser!.FirstName + " " + i.AssignedUser.LastName).ToList()
        //    //});

        //                             //var TerritoryList = Territory.Select(t => new
        //                             //{
        //                             //    Id = t.Id,
        //                             //    Name = t.Name,
        //                             //    ColorCode = t.ColorCode,
        //                             //    AssignedUsernames = t.Inspectors != null ?
        //                             //    t.Inspectors.Select(i => i.AssignedUser != null ? $"{i.AssignedUser.FirstName} {i.AssignedUser.LastName}" : "")
        //                             //                .ToList() :
        //                             //    new List<string>()
        //                             //});


        //    //var TerritoryList = Territory.Select(t => new
        //    //{
        //    //    Id = t.Id,
        //    //    Name = t.Name,
        //    //    ColorCode = t.ColorCode,
        //    //    AssignedUsernames = t.Inspectors != null ?
        //    //    t.Inspectors.Select(i => i.AssignedUser != null ? i.AssignedUser.FirstName + " " + i.AssignedUser.LastName : "")
        //    //                .ToList() :
        //    //    new List<string>()
        //    //});

        //    //          var TerritoryList = Territory.Select(t => new {
        //    //                Id = t.Id,
        //    //                Name = t.Name,
        //    //                ColorCode = t.ColorCode,
        //    //                AssignedUsernames = t.Inspectors != null ?
        //    //t.Inspectors.Select(i => i.AssignedUser != null ? $"{i.AssignedUser.FirstName} {i.AssignedUser.LastName}" : "")
        //    //            .ToList() :
        //    //new List<string>()
        //    //          });

        //    //var TerritoryList = Territory.Select(t => new {
        //    //      Id = t.Id,
        //    //      Name = t.Name,
        //    //      ColorCode = t.ColorCode,
        //    //      AssignedUsernames = t.Inspectors != null ?
        //    //        t.Inspectors.Select(i => i.AssignedUser != null ? $"{i.AssignedUser.FirstName} {i.AssignedUser.LastName}" : "")
        //    //                    .Where(name => !string.IsNullOrEmpty(name))
        //    //                    .Distinct()
        //    //                    .ToList() :
        //    //        new List<string>()
        //    //                  })
        //    //.GroupBy(t => new { t.Id, t.Name, t.ColorCode })
        //    //.Select(group => new {
        //    //      Id = group.Key.Id,
        //    //      Name = group.Key.Name,
        //    //      ColorCode = group.Key.ColorCode,
        //    //      AssignedUsernames = group.SelectMany(t => t.AssignedUsernames).ToList()
        //    //});

        //    //var TerritoryList = Territory.SelectMany(t => t.Inspectors)
        //    //           .Where(i => i.AssignedUser != null)
        //    //           .GroupBy(i => i.Territory)
        //    //           .Select(group => new {
        //    //                 TerritoryId = group.Key.Id,
        //    //                 TerritoryName = group.Key.Name,
        //    //                 ColorCode = group.Key.ColorCode,
        //    //                 AssignedUsernames = group.Select(i => $"{i.AssignedUser.FirstName} {i.AssignedUser.LastName}").ToList()
        //    //           }).ToList();

        //    //var usersByTerritory = Territory.SelectMany(t => t.Inspectors)
        //    //              .Where(i => i.AssignedUser != null)
        //    //              .GroupBy(i => i.TerritoryId)
        //    //              .ToDictionary(g => g.Key, g => g.Select(i => $"{i.AssignedUser.FirstName} {i.AssignedUser.LastName}").ToList());

        //    //var TerritoryList = Territory.Select(t => new {
        //    //      Id = t.Id,
        //    //      Name = t.Name,
        //    //      ColorCode = t.ColorCode,
        //    //      AssignedUsernames = usersByTerritory.ContainsKey(t.Id) ? usersByTerritory[t.Id] : new List<string>()
        //    //}).ToList();

        //    //// Gather all assigned users
        //    //var allAssignedUsers = Territory.SelectMany(t => t.Inspectors!)
        //    //                                .Where(i => i.AssignedUser != null)
        //    //                                .Select(i => $"{i.AssignedUser!.FirstName} {i.AssignedUser.LastName}")
        //    //                                .Distinct()
        //    //                                .ToList();

        //    //// Associate assigned users with each territory
        //    //var TerritoryList = Territory.Select(t => new {
        //    //      Id = t.Id,
        //    //      Name = t.Name,
        //    //      ColorCode = t.ColorCode,
        //    //      AssignedUsernames = allAssignedUsers
        //    //}).ToList();

        //    //// Gather assigned users specific to each territory
        //    //var assignedUsersByTerritory = Territory.Select(t => new {
        //    //      TerritoryId = t.Id,
        //    //      AssignedUsers = t.Inspectors
        //    //                        .Where(i => i.AssignedUser != null)
        //    //                        .Select(i => $"{i.AssignedUser.FirstName} {i.AssignedUser.LastName}")
        //    //                        .Distinct()
        //    //}).ToList();

        //    //// Associate assigned users with each territory
        //    //var TerritoryList = assignedUsersByTerritory.Select(t => new {
        //    //      Id = t.TerritoryId,
        //    //      Name = Territory.FirstOrDefault(tt => tt.Id == t.TerritoryId)?.Name,
        //    //      ColorCode = Territory.FirstOrDefault(tt => tt.Id == t.TerritoryId)?.ColorCode,
        //    //      AssignedUsernames = t.AssignedUsers.ToList()
        //    //}).ToList();

        //    return Json(new { data = TerritoryList.ToList() });
        //}

        [Route("/GetAllTerritories")]
        [HttpGet]
        public async Task<IActionResult> GetAllTerritories()
        {
            var Territory = await _territory.GetAllAsync(
                            filter: x => x.IsDeleted == false,
                            includeProperties: "Inspectors.AssignedUser");

            //var TerritoryList = Territory.Select(t => new
            //{
            //      Id = t.Id,
            //      Name = t.Name,
            //      ColorCode = t.ColorCode,
            //      AssignedUsernames = t.Inspectors != null ?
            //        t.Inspectors.Select(i => i.AssignedUser != null ? $"{i.AssignedUser.FirstName} {i.AssignedUser.LastName}" : "")
            //                    .ToList() :
            //        new List<string>()
            //            });

            var InspectorList = await _inspectors.GetAllAsync();

            var TerritoryList = Territory
              .Select(t => new
              {
                  Id = t.Id,
                  Name = t.Name,
                  ColorCode = t.ColorCode,
                  IsActive = t.IsActive,
                  encryptedId = _encrypt.Encrypt256(Convert.ToString(t.Id)),
                  AssignedUsernames = InspectorList
                              .Where(i => i.TerritoryId == t.Id)
                              .Select(async i =>
                              {
                                  var Name = await _users.GetById(i.AssignedUserId);
                                  return Name.FirstName + " " + Name.LastName;
                              })
                              .ToList()
              })
              .ToList();

            //var TerritoryList = Territory.Select(t => new {
            //    Id = t.Id,
            //    Name = t.Name,
            //    ColorCode = t.ColorCode,
            //    AssignedUsernames = t.Inspectors!.Select(i => i.AssignedUser!.FirstName + " " + i.AssignedUser.LastName).ToList()
            //});



            //          var TerritoryList = Territory.Select(t => new {
            //                Id = t.Id,
            //                Name = t.Name,
            //                ColorCode = t.ColorCode,
            //                AssignedUsernames = t.Inspectors != null ?
            //t.Inspectors.Select(i => i.AssignedUser != null ? $"{i.AssignedUser.FirstName} {i.AssignedUser.LastName}" : "")
            //            .ToList() :
            //new List<string>()
            //          });

            //var TerritoryList = Territory.Select(t => new {
            //      Id = t.Id,
            //      Name = t.Name,
            //      ColorCode = t.ColorCode,
            //      AssignedUsernames = t.Inspectors != null ?
            //        t.Inspectors.Select(i => i.AssignedUser != null ? $"{i.AssignedUser.FirstName} {i.AssignedUser.LastName}" : "")
            //                    .Where(name => !string.IsNullOrEmpty(name))
            //                    .Distinct()
            //                    .ToList() :
            //        new List<string>()
            //                  })
            //.GroupBy(t => new { t.Id, t.Name, t.ColorCode })
            //.Select(group => new {
            //      Id = group.Key.Id,
            //      Name = group.Key.Name,
            //      ColorCode = group.Key.ColorCode,
            //      AssignedUsernames = group.SelectMany(t => t.AssignedUsernames).ToList()
            //});

            //var TerritoryList = Territory.SelectMany(t => t.Inspectors)
            //           .Where(i => i.AssignedUser != null)
            //           .GroupBy(i => i.Territory)
            //           .Select(group => new {
            //                 TerritoryId = group.Key.Id,
            //                 TerritoryName = group.Key.Name,
            //                 ColorCode = group.Key.ColorCode,
            //                 AssignedUsernames = group.Select(i => $"{i.AssignedUser.FirstName} {i.AssignedUser.LastName}").ToList()
            //           }).ToList();

            //var usersByTerritory = Territory.SelectMany(t => t.Inspectors)
            //              .Where(i => i.AssignedUser != null)
            //              .GroupBy(i => i.TerritoryId)
            //              .ToDictionary(g => g.Key, g => g.Select(i => $"{i.AssignedUser.FirstName} {i.AssignedUser.LastName}").ToList());

            //var TerritoryList = Territory.Select(t => new {
            //      Id = t.Id,
            //      Name = t.Name,
            //      ColorCode = t.ColorCode,
            //      AssignedUsernames = usersByTerritory.ContainsKey(t.Id) ? usersByTerritory[t.Id] : new List<string>()
            //}).ToList();

            //// Gather all assigned users
            //var allAssignedUsers = Territory.SelectMany(t => t.Inspectors!)
            //                                .Where(i => i.AssignedUser != null)
            //                                .Select(i => $"{i.AssignedUser!.FirstName} {i.AssignedUser.LastName}")
            //                                .Distinct()
            //                                .ToList();

            //// Associate assigned users with each territory
            //var TerritoryList = Territory.Select(t => new {
            //      Id = t.Id,
            //      Name = t.Name,
            //      ColorCode = t.ColorCode,
            //      AssignedUsernames = allAssignedUsers
            //}).ToList();

            // Gather assigned users specific to each territory
            //var assignedUsersByTerritory = Territory.Select(t => new {
            //      TerritoryId = t.Id,
            //      AssignedUsers = t.Inspectors
            //                        .Where(i => i.AssignedUser != null)
            //                        .Select(i => $"{i.AssignedUser.FirstName} {i.AssignedUser.LastName}")
            //                        .Distinct()
            //}).ToList();

            //// Associate assigned users with each territory
            //var TerritoryList = assignedUsersByTerritory.Select(t => new {
            //      Id = t.TerritoryId,
            //      Name = Territory.FirstOrDefault(tt => tt.Id == t.TerritoryId)?.Name,
            //      ColorCode = Territory.FirstOrDefault(tt => tt.Id == t.TerritoryId)?.ColorCode,
            //      AssignedUsernames = t.AssignedUsers.ToList()
            //}).ToList();

            return Json(new { data = TerritoryList.ToList() });
        }


        [Route("/GetTerritory/{id?}")]
        [HttpGet]
        public async Task<IActionResult> GetTerritory(string id)
        {
            var TiD = _encrypt.Decrypt256(id);
            var Territory = await _territory.GetFirstOrDefaultAsync(filter: x => x.Id == Convert.ToInt32(TiD));
            var DefaultIns = await _inspectors.GetFirstOrDefaultAsync(filter: x => x.TerritoryId == Convert.ToInt32(TiD) && x.TypeId == 2, includeProperties: "AssignedUser");
            var data = new TerritoryUpsertVM();
            data.ColorCode = Territory.ColorCode;
            data.Name = Territory.Name;
            data.TerritoryId = Territory.Id;
            data.DefaultInspectorId = DefaultIns.AssignedUserId;
            var users = await _users.GetAllAsync(filter: x => x.Role.Name == SD.Inspector, includeProperties: "Role");
            data.InspectorList = users.Select(x => new SelectListItem
            {
                Text = x.FirstName + " " + x.LastName,
                Value = x.Id.ToString(),
            }).ToList();

            return Json(new { success = true, territory = data });
        }

        [Route("/GetAllTerritoryAssignedUsers/{id?}")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers(string id)
        {
            var TiD = _encrypt.Decrypt256(id);
            var Inspectors = await _inspectors.GetAllAsync(filter: x => x.TerritoryId == Convert.ToInt32(TiD), includeProperties: "Territory,AssignedUser");
            foreach (var Inspector in Inspectors)
            {
                Inspector.EncryptedId = _encrypt.Encrypt256(Convert.ToString(Inspector.Id));
            }
            return Json(new { success = true, inspectors = Inspectors });
        }

        //[Route("/GetAllSystemDefinedUsers")]
        //[HttpGet]
        //public async Task<IActionResult> GetAllSystemDefinedUsers()
        //{
        //      var users = await _users.GetAllAsync(filter: x => x.Role.Name == SD.SuperAdmin || x.Role.Name == SD.AdminInspector, includeProperties: "Role");
        //      return Json(new { success = true, users = users, count = users.Count() });
        //}

        [Route("/TerritoryUpsert")]
        [HttpPost]
        public async Task<IActionResult> TerritoryUpsert(TerritoryUpsertVM model)
        {
            if (ModelState.IsValid)
            {
                if (model.TerritoryId != 0)
                {
                    var Territory = await _territory.GetFirstOrDefaultAsync(filter: x => x.Id == model.TerritoryId);
                    Territory.Name = model.Name;
                    Territory.ColorCode = model.ColorCode;
                    Territory.UpdatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    Territory.UpdatedOn = DateTime.Now;
                    await _territory.UpdateAsync(Territory);
                    var ExistingIns = await _inspectors.GetFirstOrDefaultAsync(x => x.AssignedUserId == model.DefaultInspectorId && x.TerritoryId == Territory.Id && x.TypeId==3, isTracking: false);
                    if (ExistingIns != null)
                    {
                        await _inspectors.RemoveAsync(ExistingIns);
                    }
                    //var InsData = new TerritoryWiseInspectors();
                    var InsData = await _inspectors.GetFirstOrDefaultAsync(filter: x => x.TerritoryId == Territory.Id && x.TypeId == 2);
                    if (InsData == null)
                    {
                        InsData = new TerritoryWiseInspectors();
                        InsData.TerritoryId = Territory.Id;
                        InsData.AssignedUserId = model.DefaultInspectorId;
                        InsData.TypeId = 2;
                        InsData.CreatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                        await _inspectors.AddAsync(InsData);

                    }
                    else
                    {
                        InsData.AssignedUserId = model.DefaultInspectorId;
                        InsData.UpdatedOn = DateTime.Now;
                        InsData.UpdatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                        await _inspectors.UpdateAsync(InsData);
                    }
                    return Json(new { success = true, id = Territory.Id, msg = "Successfully Updated" });
                }
                else
                {
                    var Territory = new Territory();
                    Territory.Name = model.Name;
                    Territory.ColorCode = model.ColorCode;
                    Territory.CreatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    Territory.IsActive = true;
                    await _territory.AddAsync(Territory);
                    //var users = await _users.GetAllAsync(filter: x => x.Role.Name == SD.SuperAdmin || x.Role.Name == SD.AdminInspector || x.Id==model.DefaultInspectorId, includeProperties: "Role");
                    //var UserList = new List<KeyValuePair<string,int>>();

                    //foreach (var user in users)
                    //{
                    //    UserList.Add(new KeyValuePair<string, int>("Id", user.Id));
                    //    UserList.Add(new KeyValuePair<string, int>("RoleId", user.RoleId));

                    //}
                    //UserList.Add(model.DefaultInspectorId);
                    //foreach(var user in UserList) { }

                    var UserList = from u in await _users.GetAllAsync(filter: x => x.Role.Name == SD.SuperAdmin || x.Role.Name == SD.AdminInspector || x.Id == model.DefaultInspectorId, includeProperties: "Role")
                                   select new
                                   {
                                       Id = u.Id,
                                       Role = u.Role.Name,
                                   };

                    foreach (var user in UserList)
                    {
                        var InsData = new TerritoryWiseInspectors();
                        InsData.TerritoryId = Territory.Id;
                        InsData.AssignedUserId = user.Id;
                        if (user.Role == SD.Inspector)
                        {
                            InsData.TypeId = 2;
                        }
                        else
                        {
                            InsData.TypeId = 1;
                        }
                        InsData.CreatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                        await _inspectors.AddAsync(InsData);
                    }

                    return Json(new { success = true, id = Territory.Id, msg = "Successfully Created" });
                }
            }
            return Json(new { success = true, msg = "Unexpected Error Occurred While Creating/Updating Territory" });
        }

        [Route("/AssignInspectorsToTerritory")]
        [HttpPost]
        public async Task<IActionResult> AssignInspector(TerritoryUpsertVM model)
        {
            if (ModelState.IsValid)
            {
                var ExistingInspector = await _inspectors.GetFirstOrDefaultAsync(filter: x => x.AssignedUserId == model.TerritoryWiseInspectors!.AssignedUserId && x.TerritoryId == model.TerritoryWiseInspectors.TerritoryId);
                if (ExistingInspector != null)
                {
                    return Json(new { success = false, msg = "Inspector Already Assigned To This Territory" });
                }
                model.TerritoryWiseInspectors!.CreatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                await _inspectors.AddAsync(model.TerritoryWiseInspectors);
                var user = await _users.GetFirstOrDefaultAsync(filter: x => x.Id == model.TerritoryWiseInspectors.AssignedUserId);
                return Json(new { success = true, msg = "Successfully Assigned", id = model.TerritoryWiseInspectors.Id, name = user.FirstName + " " + user.LastName, encryptedId = _encrypt.Encrypt256(Convert.ToString(model.TerritoryWiseInspectors.Id)) });
            }
            return Json(new { success = false, msg = "Unexpected Error Occurred" });
        }

        [Route("/RemoveInspectorFromTerritory/{id?}")]
        [HttpDelete]
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


        [Route("/ChangeTerritoryState/{id?}")]
        [HttpPost]
        public async Task<IActionResult> ActiveInactive(string id)
        {
            var TiD = _encrypt.Decrypt256(id);
            string msg = "";
            var territory = await _territory.GetById(Convert.ToInt32(TiD));
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
            await _territory.UpdateAsync(territory);
            return Json(new { success = true, msg = msg });
        }
    }
}
