using Application.Services.Country;
using Application.Services.State;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Areas.Admin.Models.Booking;
using WebApp.Areas.Admin.Models.Master;
using WebApp.Models;
using X.PagedList;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize]
    public class MasterController : Controller
    {
        private readonly ICountryServices _countryServices;
        private readonly IStateServices _stateServices;
        private readonly IMapper _mapper;
        private readonly IMapper _mapperState;
        public MasterController(ICountryServices countryServices, IMapper mapper, IMapper mapperState, IStateServices stateServices)
        {
            _countryServices = countryServices;
            _stateServices = stateServices;
            _mapper = mapper;
            _mapperState = mapperState;
        }


        public async Task<IActionResult> ListCountryAsync(string SortBy, int? page, string Name, string Code, string Excel)
        {
            ViewBag.SortNameParam = SortBy == "Name" ? "Name desc" : "Name";
            ViewBag.SortCodeParam = SortBy == "Code" ? "Code desc" : "Code";
            ViewBag.SortCreatedOnParam = SortBy == "CreatedOn" ? "CreatedOn desc" : "CreatedOn";
            ViewBag.SrName = Name;
            ViewBag.SrCode = Code;
            ViewBag.SortDefaultValue = SortBy ?? "";

            var model = _countryServices.GetAll(w => w.IsDeleted == false
                ).Select(s => new ListCountryVm
                {
                    Id = s.Id,
                    Code = s.Code,
                    Name=s.Name,
                    CreatedDate = s.CreatedDate
                }).AsEnumerable();
          
            return View(await model.ToPagedListAsync(page ?? 1, ModelsUtility.PgSize));
        }


        [HttpGet]
        public async Task<IActionResult> AddCountryAsync(int id)
        {
            AddCountryVm model = new AddCountryVm();
            if(id>0)
            {
                var entity=await _countryServices.GetByIdAsync(id);
                _mapper.Map(entity, model);
            }
            var msg = TempData["Msg"] != null ? TempData["Msg"].ToString() : "";
            ViewBag.Msg = "";
            if (!string.IsNullOrEmpty(msg))
                ViewBag.Msg = msg;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddCountry(AddCountryVm model)
        {
            var DuplicateChk = await _countryServices.IsDuplicateAsync(model.Id, model.Name);
            if (!DuplicateChk)
            {
                model.CreatedDate = System.DateTime.Now;
                model.CreatedBy = ModelsUtility.GetUserID();
                var newentity = _mapper.Map<JB_MasterCountry>(model);
                newentity.IsActive = true;
                var entity = await _countryServices.UpsertAsync(newentity);
                if(model.Id>0)
                    ViewBag.Msg = "Record has been updated successfully";
                else
                    ViewBag.Msg = "Record has been added successfully";
                return View(model);
            }
            else
            {
                TempData["Msg"] = "Duplicate Data.";
                return RedirectToAction("AddCountry", "Master");
            }
        }
        public IActionResult DeleteCountry()
        {
            AddCountryVm model = new AddCountryVm();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            bool bvar = await _countryServices.DeleteAsync(id, ModelsUtility.GetUserID());
            return RedirectToAction("ListCountry", "Master");
        }
        public async Task<IActionResult> ChangeStatusCountry(int id)
        {    
            var entity = await _countryServices.UpdateStatusAsync(id, ModelsUtility.GetUserID());
            return RedirectToAction("ListCountry", "Master");
        }
        public async Task<IActionResult> ChangeAjaxStatusCountry(int id)
        {
            return Json(await _countryServices.UpdateStatusAsync(id, ModelsUtility.GetUserID()));
        }
        public async Task<IActionResult> AjaxDeleteCountry(int id)
        {
            return Json(await _countryServices.DeleteAsync(id, ModelsUtility.GetUserID()));            
        }


        //State Work Start
        public async Task<IActionResult> ListStateAsync(string SortBy, int? page, string Name, string Code, string Excel)
        {   
            ViewBag.SortNameParam = SortBy == "Name" ? "Name desc" : "Name";
            ViewBag.SortCodeParam = SortBy == "Code" ? "Code desc" : "Code";
            ViewBag.SortCreatedOnParam = SortBy == "CreatedOn" ? "CreatedOn desc" : "CreatedOn";
            ViewBag.SrName = Name;
            ViewBag.SrCode = Code;
            ViewBag.SortDefaultValue = SortBy ?? "";

            var model1 = _stateServices.GetAll(
                    w => w.IsDeleted == false &&
                    w.Name.Contains((!string.IsNullOrEmpty(Name) ? Name : w.Name)) &&
                    w.Code.Contains((!string.IsNullOrEmpty(Code) ? Code : w.Code))
                ).Select(s => new ListStateVm
                {
                    Id = s.Id,
                    EncId = "",
                    Name = s.Name,
                    Code = s.Code,
                    IsActive = s.IsActive,
                    CreatedDate = s.CreatedDate
                }).AsEnumerable();
            model1 = SortBy switch
            {
                "Name desc" => model1.OrderByDescending(x => x.Name),
                "Name" => model1.OrderBy(x => x.Name),
                "Code desc" => model1.OrderByDescending(x => x.Code),
                "Code" => model1.OrderBy(x => x.Code),
                "CreatedOn desc" => model1.OrderByDescending(x => x.CreatedDate),
                "CreatedOn" => model1.OrderBy(x => x.CreatedDate),
                _ => model1.OrderByDescending(x => x.CreatedDate),
            };
            //if (Excel == "Excel")
            //{
            //    return _exportManager.ExportToExcel(model.Select(s => new
            //    {
            //        s.Id,
            //        s.Name,
            //        s.TwoLetterIsoCode,
            //        s.ThreeLetterIsoCode,
            //        s.DisplayOrder,
            //        s.IsActive,
            //        s.CreatedDate
            //    }));
            //}

            return View(await model1.ToPagedListAsync(page ?? 1, ModelsUtility.PgSize));
        }

        public async Task<IActionResult> AddStateAsync(int id)
        {
            AddStateVm modelState = new AddStateVm();
            modelState.AvailableCountries = _countryServices.GetAll(w => w.IsActive == true && w.IsDeleted == false).
              Select(s => new SelectListItem
              {
                  Value = s.Id.ToString(),
                  Text = s.Name,
                  Selected = (s.Name == "Russia" ? true : false)
              }
              ).ToList();

            if (id > 0)
            {
                var entity = await _stateServices.GetByIdAsync(id);
                _mapperState.Map(entity, modelState);
            }
            var msg = TempData["Msg"] != null ? TempData["Msg"].ToString() : "";
            ViewBag.Msg = "";
            if (!string.IsNullOrEmpty(msg))
                ViewBag.Msg = msg;
            return View(modelState);
        }

        [HttpPost]
        public async Task<IActionResult> AddState(AddStateVm modelState)
        {
            var DuplicateChk = await _stateServices.IsDuplicateAsync(modelState.Id, modelState.Name);
            if (!DuplicateChk)
            {           
                modelState.CreatedDate = System.DateTime.Now;
                modelState.CreatedBy = ModelsUtility.GetUserID();
                var newentity = _mapperState.Map<JB_MasterState>(modelState);
                newentity.IsActive = true;
                var entity = await _stateServices.UpsertAsync(newentity);
                if (modelState.Id > 0)
                    ViewBag.Msg = "Record has been updated successfully";
                else
                    ViewBag.Msg = "Record has been added successfully";
                return View(modelState);
            }
            else
            {
                TempData["Msg"] = "Duplicate Data.";
                return RedirectToAction("AddState", "Master");
            }
        }

        public async Task<IActionResult> AjaxDeleteState(int id)
        {
            return Json(await _stateServices.DeleteAsync(id, ModelsUtility.GetUserID()));
        }
        public async Task<IActionResult> ChangeAjaxStatusState(int id)
        {
            return Json(await _stateServices.UpdateStatusAsync(id, ModelsUtility.GetUserID()));
        }

    }
}