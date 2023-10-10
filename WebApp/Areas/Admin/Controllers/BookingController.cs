using Application.Common.Protector;
using Application.Services.Booking;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using WebApp.Areas.Admin.Models.Booking;
using WebApp.Areas.Admin.Models.Participant;
using WebApp.Models;
using X.PagedList;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize]
    public class BookingController : Controller
    {
        private readonly IBookingServices _bookingServices;
        private readonly IBookingServices _participantServices;
        private readonly IMapper _mapper;
        private readonly IDataProtector _protector;
        public BookingController(IBookingServices bookingServices, IMapper mapper, IDataProtectionProvider dataProtectionProvider, ProtectData protectData)
        {
            _bookingServices = bookingServices;
            _mapper = mapper;
            this._protector = dataProtectionProvider.CreateProtector(protectData.IdRouteValue);
        }
        public async Task<IActionResult> ListBookingAsync(string SortBy, int? page, string Name, string Code, string Excel)
        {
            ViewBag.SortNameParam = SortBy == "Name" ? "Name desc" : "Name";
            ViewBag.SortCodeParam = SortBy == "Code" ? "Code desc" : "Code";
            ViewBag.SortCreatedOnParam = SortBy == "CreatedOn" ? "CreatedOn desc" : "CreatedOn";
            ViewBag.SrName = Name;
            ViewBag.SrCode = Code;
            ViewBag.SortDefaultValue = SortBy ?? "";

            var model = _bookingServices.GetAll(w => w.IsDeleted == false).Select(s => new ListBookingVm
            {
                Id = s.Id,
                EncryptedId = _protector.Protect(s.Id.ToString()),
                Objective = s.Objective,
                Location = s.Location,
                Description = s.Description,
                Phone = s.Phone,
                City = s.City,
                CreatedDate = s.CreatedDate
            }).AsEnumerable();
             
            return View(await model.ToPagedListAsync(page ?? 1, ModelsUtility.PgSize));
        }


        [HttpGet]
        public async Task<IActionResult> AddBookingAsync(string eid)
        {
            AddBookingVm model = new AddBookingVm();
            if (!string.IsNullOrEmpty(eid))
            {
                var decryptedid = ModelsUtility.ConvertToInt(_protector.Unprotect(eid));
                if (decryptedid > 0)
                {                  
                    var entity = await _bookingServices.GetByIdAsync(decryptedid);
                    _mapper.Map(entity, model);
                }
            }
           

            var msg = TempData["Msg"] != null ? TempData["Msg"].ToString() : "";
            ViewBag.Msg = "";
            if (!string.IsNullOrEmpty(msg))
                ViewBag.Msg = msg;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddBooking(AddBookingVm model)
        {
            //var DuplicateChk = await _bookingServices.IsDuplicateAsync(model.Id, model.City);
            //if (!DuplicateChk)
            //{
                model.CreatedDate = System.DateTime.Now;
                model.CreatedBy = ModelsUtility.GetUserID();
                var newentity = _mapper.Map<JB_Booking>(model);
                newentity.IsActive = true;
                var uploadedfilename = "";
                if (model.Doc != null && model.Doc.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        uploadedfilename = DateTime.Now.Ticks + "_" + Path.GetFileName(model.Doc.FileName);
                        await model.Doc.CopyToAsync(memoryStream);
                        var path = WebApp.Models.ModelsUtility.GetWebRootPath() + "\\SiteFiles\\UserImages";
                        var targetpath = Path.Combine(path, uploadedfilename);
                        uploadedfilename = FileManager.UploadPhoto(model.Doc, path, "");
                        model.Document = uploadedfilename;
                        newentity.Document = model.Document;
                    }
                }
                var entity = await _bookingServices.UpsertAsync(newentity);
                if(model.Id>0)
                    ViewBag.Msg = "Record has been updated successfully";
                else
                    ViewBag.Msg = "Record has been added successfully";
                return View(model);
            //}
            //else
            //{
            //    TempData["Msg"] = "Duplicate Data.";
            //    return RedirectToAction("AddBooking", "Booking");
            //}
        }
        public IActionResult DeleteCountry()
        {
            AddBookingVm model = new AddBookingVm();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            bool bvar = await _bookingServices.DeleteAsync(id, ModelsUtility.GetUserID());
            return RedirectToAction("ListCountry", "Master");
        }
        public async Task<IActionResult> ChangeStatusCountry(int id)
        {    
            var entity = await _bookingServices.UpdateStatusAsync(id, ModelsUtility.GetUserID());
            return RedirectToAction("ListCountry", "Master");
        }
        public async Task<IActionResult> ChangeAjaxStatusCountry(int id)
        {
            return Json(await _bookingServices.UpdateStatusAsync(id, ModelsUtility.GetUserID()));
        }
        public async Task<IActionResult> AjaxDeleteCountry(int id)
        {
            return Json(await _bookingServices.DeleteAsync(id, ModelsUtility.GetUserID()));            
        }
    }
}