using BrazosApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BrazosApp.Areas.LoggedIn.Controllers
{
      public partial class PermitController : Controller
      {
            [Route("/GetAllNotes/{id?}")]
            [HttpGet]
            public async Task<IActionResult> GetAllNotes(string id)
            {
                  var EiD = _encrypt.Decrypt256(id);
                  TimeZoneInfo centralZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
                  var NoteList = from d in await _notes.GetAllAsync(filter: x => x.EstablishmentId == Convert.ToInt32(EiD) && x.Status!=2)
                                 join u in await _users.GetAllAsync()
                                 on d.CreatedBy equals u.Id into egroup
                                 from u in egroup.DefaultIfEmpty()
                                 select new
                                 {
                                       Id = d.NoteId,
                                       Description = d.Description,
                                       Title = d.Title,
                                       isEdited = d.Status==1?"Yes":"No",
                                       UploadedBy = u.FirstName + " " + u.LastName,
                                       UploadedOn = TimeZoneInfo.ConvertTime(d.CreatedOn, centralZone).ToShortDateString() + "   " + TimeZoneInfo.ConvertTime(d.CreatedOn, centralZone).ToShortTimeString(),
                                       updatedOn = d.UpdatedOn,
                                       encryptedId = _encrypt.Encrypt256(Convert.ToString(d.NoteId))
                                 };
                  return Json(new { data = NoteList });
            }

            [Route("/GetNote/{id?}")]
            [HttpGet]
            public async Task<IActionResult> GetNote(string id)
            {
                  var nId = _encrypt.Decrypt256(id);
                  var note = await _notes.GetById(Convert.ToInt32(nId));
                  return Json(new { note });
            }

            [Route("/NotesUpsert/{id?}")]
            [HttpPost]
            public async Task<IActionResult> NotesUpload(Notes model)
            {
                  if (ModelState.IsValid)
                  {
                        if (model.NoteId == 0)
                        {
                              model.CreatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                              await _notes.AddAsync(model);
                              return Json(new { success = true, msg = "Successfully Added" });
                        }
                        else
                        {
                              var note = await _notes.GetFirstOrDefaultAsync(filter: x => x.NoteId == model.NoteId, isTracking:false);
                              if (note == null)
                              {
                                    return Json(new { success = false, msg = "Error" });
                              }
                              model.Status = 1;
                              model.UpdatedOn = DateTime.Now;
                              model.CreatedOn = note.CreatedOn;
                              model.CreatedBy = note.CreatedBy;
                              await _notes.UpdateAsync(model);
                              return Json(new { success = true, msg = "Successfully Updated" });
                        }
                  }
                  return Json(new { success = false, msg = "Failed" });
            }

            [Route("/DeleteNote/{id?}")]
            [HttpDelete]
            public async Task<IActionResult> DeleteNotes(string id)
            {
                  var nId = _encrypt.Decrypt256(id);
                  var note = await _notes.GetById(Convert.ToInt32(nId));
                  if (note == null)
                  {
                        return Json(new { success = false, message = "Error while deleting" });
                  }
                  note.Status = 2;
                  await _notes.UpdateAsync(note);
                  //await _notes.RemoveAsync(note);
                  return Json(new { success = true, message = "Successfully Deleted" });
            }
      }
}
