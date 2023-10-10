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
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Areas.Admin.Models.User;
using WebApp.Areas.Admin.Models.Profile;
using WebApp.Models;
using X.PagedList;
using Application.Services.Profile;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.Admin.Models.ChangePassword;
using Microsoft.AspNetCore.Identity;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IUserServices _userServices;
        private readonly IProfileServices _ProfileServices;
        private readonly ICountryServices _countryServices;
        private readonly IStateServices _stateServices;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        private readonly IDataProtector _protector;
        private readonly UserManager<Users> _userManager;
        public ProfileController(IProfileServices ProfileServices, ICountryServices countryServices, IStateServices stateServices, IMapper mapper, IDataProtectionProvider dataProtectionProvider, ProtectData protectData, ApplicationDbContext context, IUserServices userServices, UserManager<Users> userManager)
        {
            _ProfileServices = ProfileServices;
            _countryServices = countryServices;
            _stateServices = stateServices;
            _mapper = mapper;
            _context = context;
            this._protector = dataProtectionProvider.CreateProtector(protectData.IdRouteValue);
            _userServices = userServices;
            _userManager = userManager;
        }

        public async Task<IActionResult> AddProfile()
        {
            AddProfileVm model = new AddProfileVm();
            var entity = await _ProfileServices.GetUserByIdAsync(ModelsUtility.GetUserID());
            _mapper.Map(entity, model);
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
        public async Task<IActionResult> AddProfileAsync(AddProfileVm model)
        {
            var userEntity = await _ProfileServices.GetByIdAsync(model.Id);
            model.Image = userEntity.Image;
            _mapper.Map(model, userEntity);
            var uploadedfilename = "";
            if (model.Photo != null && model.Photo.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    uploadedfilename = DateTime.Now.Ticks + "_" + Path.GetFileName(model.Photo.FileName);
                    await model.Photo.CopyToAsync(memoryStream);
                    var path = WebApp.Models.ModelsUtility.GetWebRootPath() + "\\SiteFiles\\UserImages";
                    var targetpath = Path.Combine(path, uploadedfilename);
                    uploadedfilename = FileManager.UploadPhoto(model.Photo, path, "");
                    userEntity.Image = uploadedfilename;
                }
            }
            var entity = await _ProfileServices.UpsertAsync(userEntity);


            if (model.Id > 0)
                ViewBag.Msg = "Record has been updated successfully";
            else
                ViewBag.Msg = "Record has been added successfully";
            model.AvailableCountries = _countryServices.GetAll(w => w.IsActive == true && w.IsDeleted == false).
           Select(s => new SelectListItem
           {
               Value = s.Id.ToString(),
               Text = s.Name,
               Selected = (s.Name == "Russia" ? true : false)
           }
           ).AsNoTracking().ToList();
            return View(model);

        }


        public async Task<IActionResult> AjaxDeleteUser(int id)
        {
            return Json(await _ProfileServices.DeleteAsync(id, ModelsUtility.GetUserID()));
        }
        public async Task<IActionResult> ChangeAjaxUserState(int id)
        {
            return Json(await _ProfileServices.UpdateStatus(id, ModelsUtility.GetUserID()));
        }

        public IActionResult ChangePassword()
        {
            ChangePasswordVm model = new ChangePasswordVm();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePasswordAsync(ChangePasswordVm model)
        {
            var user = await _context.Users.FindAsync(ModelsUtility.GetUserID());
            if (user == null)
                ModelState.AddModelError("","Invalid user.");
            else
            {
                var isoldpasswordvalid = _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, model.OldPassword);
                if (isoldpasswordvalid != PasswordVerificationResult.Success)
                    ModelState.AddModelError("", "Invalid old password.");
            }
            

            if (ModelState.IsValid)
            {
                
                var hasher = new PasswordHasher<Users>();
                user.PasswordHash = hasher.HashPassword(user, model.NewPassword);
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }          
            
            return View(model);
        }

    }
}
