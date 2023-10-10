using Application.Common.Protector;
using Application.Services.Country;
using Application.Services.State;
using Application.Services.Users;
using AutoMapper;
using Domain.Entities;
using Infrastrucure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using WebApp.Areas.Admin.Models.User;
using WebApp.Models;
using X.PagedList;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserServices _userServices;
        private readonly ICountryServices _countryServices;
        private readonly IStateServices _stateServices;
        private readonly IMapper _mapperUser;
        private readonly ApplicationDbContext _context;
        private readonly IDataProtector _protector;
        public UserController(IUserServices userServices, ICountryServices countryServices, IStateServices stateServices, IMapper mapperUser, IDataProtectionProvider dataProtectionProvider, ProtectData protectData, ApplicationDbContext context)
        {
            _userServices = userServices;
            _countryServices = countryServices;
            _stateServices = stateServices;
            _mapperUser = mapperUser;
            _context = context;
            this._protector = dataProtectionProvider.CreateProtector(protectData.IdRouteValue);
        }

        public async Task<IActionResult> ListUserAsync(string SortBy, int? page, string Name, string Email, string Excel)
        {
            ViewBag.SortNameParam = SortBy == "Name" ? "Name desc" : "Name";
            ViewBag.SortEmailParam = SortBy == "Email" ? "Email desc" : "Email";
            ViewBag.SortCountryParam = SortBy == "Country" ? "Country desc" : "Country";
            ViewBag.SortStateParam = SortBy == "State" ? "State desc" : "State";
            ViewBag.SortCityParam = SortBy == "City" ? "City desc" : "City";
            ViewBag.SortCreatedOnParam = SortBy == "CreatedOn" ? "CreatedOn desc" : "CreatedOn";
            //ViewBag.SrName = Name;
            //ViewBag.SrCode = Email;
            ViewBag.SortDefaultValue = SortBy ?? "";
            
            var model = _userServices.GetAllUsers(
                    w => w.IsDeleted == false &&
                    w.FirstName.Contains((!string.IsNullOrEmpty(Name) ? Name : w.FirstName)) &&
                    w.Email.Contains((!string.IsNullOrEmpty(Email) ? Email : w.Email))
                ).Select(s => new ListUserVm
                {
                    Id = s.Id,
                    EncryptedId = _protector.Protect(s.Id.ToString()),
                    FirstName = s.FirstName,
                    Email = s.Email,
                    Country = _countryServices.GetCountryByName(s.CountryId),
                    //Country= _context.JB_MasterCountry.Where(w=>w.Id==s.CountryId).Select(s=>s.Name).FirstOrDefault(),
                    State = _stateServices.GetStateByName(s.StateId),
                    City = s.City,
                    IsActive = s.IsActive,
                    CreatedDate = s.CreatedOnUtc
                }).AsEnumerable();
            model = SortBy switch
            {
                "Name desc" => model.OrderByDescending(x => x.FirstName),
                "Name" => model.OrderBy(x => x.FirstName),
                "Email desc" => model.OrderByDescending(x => x.Email),
                "Email" => model.OrderBy(x => x.Email),
                "Country desc" => model.OrderByDescending(x => x.Country),
                "Country" => model.OrderBy(x => x.Country),
                "State desc" => model.OrderByDescending(x => x.State),
                "State" => model.OrderBy(x => x.State),
                "City desc" => model.OrderByDescending(x => x.City),
                "City" => model.OrderBy(x => x.City),
                _ => model.OrderByDescending(x => x.FirstName),
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

            return View(await model.ToPagedListAsync(page ?? 1, ModelsUtility.PgSize));
        }

        public async Task<IActionResult> AddUser(string eid)
        {
            AddUserVm model = new AddUserVm();
            if (!string.IsNullOrEmpty(eid))
            {
                var decryptedid = ModelsUtility.ConvertToInt(_protector.Unprotect(eid));
                if (decryptedid > 0)
                {
                    var entity = await _userServices.GetByIdAsync(decryptedid);
                    _mapperUser.Map(entity, model);
                }
            }
                model.AvailableCountries = _countryServices.GetAll(w => w.IsActive == true && w.IsDeleted == false).
                    Select(s => new SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = s.Name,
                        Selected = (s.Id == model.CountryId)
                    }
                    ).ToList();

                model.AvailableStates = _stateServices.GetAll(w => w.IsActive == true && w.IsDeleted == false).
                    Select(s => new SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = s.Name,
                        Selected = (s.Id == model.StateId)
                    }
                    ).ToList();
                var msg = TempData["Msg"] != null ? TempData["Msg"].ToString() : "";
                ViewBag.Msg = "";
                if (!string.IsNullOrEmpty(msg))
                    ViewBag.Msg = msg;          

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddUserAsync(AddUserVm modelUser)
        {
            var DuplicateChk = await _userServices.IsDuplicateAsync(modelUser.Id, modelUser.Email);
            if (!DuplicateChk)
            {
                modelUser.AvailableCountries = _countryServices.GetAll(w => w.IsActive == true && w.IsDeleted == false).
                Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Name,
                    Selected = (s.Name == "Russia" ? true : false)
                }
                ).ToList();

                //modelUser.AvailableStates = _stateServices.GetAll(w => w.IsActive == true && w.IsDeleted == false).
                //    Select(s => new SelectListItem
                //    {
                //        Value = s.Id.ToString(),
                //        Text = s.Name
                //    }
                //    ).ToList();
                //modelUser.CreatedDate = System.DateTime.Now;
                //modelUser.CreatedBy = Utility.GetUserID();
                var newentity = _mapperUser.Map<Users>(modelUser);
                newentity.IsActive = true;
                var uploadedfilename = "";
                if (modelUser.Photo != null && modelUser.Photo.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        uploadedfilename = DateTime.Now.Ticks + "_" + Path.GetFileName(modelUser.Photo.FileName);
                        await modelUser.Photo.CopyToAsync(memoryStream);
                        var path = WebApp.Models.ModelsUtility.GetWebRootPath() + "\\SiteFiles\\UserImages";
                        var targetpath = Path.Combine(path, uploadedfilename);
                        uploadedfilename = FileManager.UploadPhoto(modelUser.Photo, path, "");
                        modelUser.Image = uploadedfilename;
                        newentity.Image = modelUser.Image;
                    }
                }
                var entity = await _userServices.UpsertAsync(newentity);
                
                if (modelUser.Id > 0)
                    ViewBag.Msg = "Record has been updated successfully";
                else
                    ViewBag.Msg = "Record has been added successfully";

                return View(modelUser);
            }
            else
            {
                ViewBag.Msg = "Duplicate User";
                return View(modelUser);
            }

        }

        public async Task<IActionResult> AjaxDeleteUser(int id)
        {
            return Json(await _userServices.DeleteAsync(id, ModelsUtility.GetUserID()));
        }
        public async Task<IActionResult> ChangeAjaxUserState(int id)
        {
            return Json(await _userServices.UpdateStatus(id, ModelsUtility.GetUserID()));
        }

    }

}
