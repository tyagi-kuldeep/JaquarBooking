using Application.Common.Protector;
using Application.Services.Role;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebApp.Areas.Admin.Models.Role;
using WebApp.Models;
using X.PagedList;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize]
    public class RoleController : Controller
    {
        private readonly IRoleServices _rolesServices;
        private readonly IMapper _mapper;
        private readonly IDataProtector _protector;
        private object _roleServices;

        public RoleController(IRoleServices rolesServices, IDataProtectionProvider dataProtectionProvider, ProtectData protectData, IMapper mapper)
        {
            _rolesServices = rolesServices;
            this._protector = dataProtectionProvider.CreateProtector(protectData.IdRouteValue);
            _mapper = mapper;
        }
        public async Task<IActionResult> ListRoleAsync(string SortBy, int? page)
        {
            ViewBag.SortRoleparameter = (SortBy == null || SortBy == "Role") ? "Role desc" : "Role";
            var model = _rolesServices.GetAllRoles(w => w.Id == w.Id).Select(s => new ListRoleVm
            {
                Id = s.Id,
                EncryptedId =_protector.Protect(s.Id.ToString()),
                RoleName = s.Name
            }).AsEnumerable();
            model = SortBy switch
            {
                "Role desc" => model.OrderByDescending(x => x.RoleName),
                "Role" => model.OrderBy(x => x.RoleName),
                _ => model.OrderBy(x => x.Id),
            };
            return View(await model.ToPagedListAsync(page ?? 1, ModelsUtility.PgSize));
        }
        public async Task<IActionResult> AddRoleAsync(string eid)
        {
            AddRoleVm model = new AddRoleVm();
            if (!string.IsNullOrEmpty(eid))
            {
                var decryptedid = ModelsUtility.ConvertToInt(_protector.Unprotect(eid));
                    if (decryptedid > 0)
                    {
                        var entity = await _rolesServices.GetByIdAsync(decryptedid);
                        if (entity != null)
                            _mapper.Map(entity, model);
                    }
                    var msg = TempData["Msg"] != null ? TempData["Msg"].ToString() : "";
                    ViewBag.Msg = "";
                    if (!string.IsNullOrEmpty(msg))
                        ViewBag.Msg = msg;
                    return View(model);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddRoleAsync(AddRoleVm model)
        {
            if (ModelState.IsValid)
            {
                var DuplicateChk = await _rolesServices.IsDuplicateAsync(model.Id, model.Name);
                if (!DuplicateChk)
                {
                    var newentity = _mapper.Map<AuthRole>(model);
                    var entity = await _rolesServices.UpsertAsync(newentity);
                    if (model.Id > 0)
                        ViewBag.Msg = "Record has been updated successfully";
                    else
                        ViewBag.Msg = "Record has been added successfully";
                    return View(model);
                }
                else
                {
                    TempData["Msg"] = "Duplicate Data.";
                    return RedirectToAction("AddRole", "Role");
                }
            }
            return View(model);
        }

    }
}
