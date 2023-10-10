using Application.Common.Protector;
using Application.Services.Booking;
using Application.Services.Participant;
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
    public class ParticipantController : Controller
    {
        private readonly IParticipantServices _participantServices;
        private readonly IMapper _mapper;
        private readonly IDataProtector _protector;
        public ParticipantController(IParticipantServices participantServices, IMapper mapper, IDataProtectionProvider dataProtectionProvider, ProtectData protectData)
        {
            _participantServices = participantServices;
            _mapper = mapper;
            this._protector = dataProtectionProvider.CreateProtector(protectData.IdRouteValue);
        }
        public async Task<IActionResult> ListParticipantAsync(string SortBy, int? page, string Name, string Code, string Excel)
        {
            ViewBag.SortNameParam = SortBy == "Name" ? "Name desc" : "Name";
            ViewBag.SortCodeParam = SortBy == "Code" ? "Code desc" : "Code";
            ViewBag.SortCreatedOnParam = SortBy == "CreatedOn" ? "CreatedOn desc" : "CreatedOn";
            ViewBag.SrName = Name;
            ViewBag.SrCode = Code;
            ViewBag.SortDefaultValue = SortBy ?? "";

            var model = _participantServices.GetAll(w => w.IsDeleted == false).Select(s => new ListParticipantVm
            {
                Id = s.Id,
                EncryptedId = _protector.Protect(s.Id.ToString()),
                Name = s.Name,
                Company = s.Company,
                Phone = s.Phone,
                CreatedDate = s.CreatedDate
            }).AsEnumerable();

            return View(await model.ToPagedListAsync(page ?? 1, ModelsUtility.PgSize));
        }


        [HttpGet]
        public async Task<IActionResult> AddParticipantAsync(string eid)
        {
            AddBookingVm model = new AddBookingVm();
            if (!string.IsNullOrEmpty(eid))
            {
                var decryptedid = ModelsUtility.ConvertToInt(_protector.Unprotect(eid));
                if (decryptedid > 0)
                {
                    var entity = await _participantServices.GetByIdAsync(decryptedid);
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
        public async Task<IActionResult> AddParticipant(WebApp.Areas.Admin.Models.Booking.AddParticipantVm model)
        {
            //var DuplicateChk = await _participantServices.IsDuplicateAsync(model.Id, model.City);
            //if (!DuplicateChk)
            //{
                //model.CreatedDate = System.DateTime.Now;
                //model.CreatedBy = ModelsUtility.GetUserID();
                var newentity = _mapper.Map<JB_Participant>(model);
                newentity.IsActive = true;
                var entity = await _participantServices.UpsertAsync(newentity);
                if (model.Id > 0)
                    ViewBag.Msg = "Record has been updated successfully";
                else
                    ViewBag.Msg = "Record has been added successfully";
                return View(model);
            //}
            //else
            //{
            //    TempData["Msg"] = "Duplicate Data.";
            //    return RedirectToAction("AddCountry", "Master");
            //}
        }
        public IActionResult DeleteCountry()
        {
            AddBookingVm model = new AddBookingVm();
            return View(model);
        }
        [HttpGet]
        public IActionResult LoadParticipants(string Type, int RefId)
        {
            List<ListParticipantVm> data = new List<ListParticipantVm>();
            if (Type == "GetStateByCountryID")
            {
                data = _participantServices.GetAll(w => w.IsActive == true && w.IsDeleted == false && w.BookingId == RefId).
                   Select(s => new ListParticipantVm
                   {
                       Id = s.Id,
                       Company = s.Company,
                       Name = s.Name,
                   }
                   ).ToList();
            }
            return Json(data);
        }

    }
}