using Application.Services.Participant;
using Application.Services.State;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Areas.Admin.Models.User;

namespace WebApp.Areas.Admin.Controllers
{
    public class AjaxController : Controller
    {
        private readonly IStateServices _stateServices;
        private readonly IParticipantServices _participantServices;

        public AjaxController(IStateServices stateServices, IParticipantServices participantServices)
        {
            _stateServices = stateServices;
            _participantServices = participantServices;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult LoadDd(string Type, int RefId)
        {
            List<DropDownModel> data = new List<DropDownModel>();
            if (Type == "GetStateByCountryID")
            {
                data = _stateServices.GetAll(w => w.IsActive == true && w.IsDeleted == false && w.CountryId == RefId).
                   Select(s => new DropDownModel
                   {
                       value = s.Id.ToString(),
                       text = s.Name,
                       selected = false
                   }
                   ).ToList();
            }
            return Json(data);
        }

        public class DropDownModel
        {
            public string value { get; set; }
            public string text { get; set; }
            public bool selected { get; set; }
        }

        [AllowAnonymous]
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
                       VisitorTypeId = s.VisitorTypeId,
                       Phone = s.Phone,
                       Email = s.Email,
                       JaquarProductPromoting = s.JaquarProductPromoting,
                       Document = s.Document,
                       FlightReturnDate = s.FlightReturnDate,
                       FlightStartDate = s.FlightStartDate,
                       Company = s.Company,
                       Name = s.Name,
                       IsHospitalityProjectId = s.IsHospitalityProjectId
                   }
                   ).ToList();
            }
            return Json(data);
        }

        public IActionResult GetParticipantById(int RefId)
        {
            var data = _participantServices.GetAll(w => w.Id == RefId).
                Select(s => new ListParticipantVm
                {
                    Id = s.Id,
                    VisitorTypeId = s.VisitorTypeId,
                    Phone = s.Phone,
                    Email = s.Email,
                    JaquarProductPromoting = s.JaquarProductPromoting,
                    Document = s.Document,
                    FlightReturnDate = s.FlightReturnDate,
                    FlightStartDate = s.FlightStartDate,
                    Company = s.Company,
                    Name = s.Name,
                    IsHospitalityProjectId = s.IsHospitalityProjectId,
                }
                ).FirstOrDefault();
            return Json(data);
        }
        public class ListParticipantVm
        {
            public int Id { get; set; }
            public int BookingId { get; set; }
            public int VisitorTypeId { get; set; }
            public int IsHospitalityProjectId { get; set; }

            public string Name { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
            public string Company { get; set; }
            public string JaquarProductPromoting { get; set; }
            public string Document { get; set; } = string.Empty;
            public DateTime CreatedDate { get; set; }

            public DateTime FlightStartDate { get; set; }

            public DateTime FlightReturnDate { get; set; }
            public string EncryptedId { get; set; }
        }
    }
}
