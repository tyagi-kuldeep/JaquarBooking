using Application.Common.Protector;
using Application.Services.Booking;
using Application.Services.Menu;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using WebApp.Areas.Admin.Models.Menu;
using WebApp.Models;
using X.PagedList;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize]
    public class MenuController : Controller
    {
        private readonly IMenuServices _menuServices;
        private readonly IMapper _mapper;
        private readonly IDataProtector _protector;
        public MenuController(IMenuServices MenuServices, IMapper mapper, IDataProtectionProvider dataProtectionProvider, ProtectData protectData)
        {
            _menuServices = MenuServices;
            _mapper = mapper;
            this._protector = dataProtectionProvider.CreateProtector(protectData.IdRouteValue);
        }
        public async Task<IActionResult> ListMenu(string SortBy, int? page, string Name, string Code, string Excel)
        {
            ViewBag.SortNameParam = SortBy == "Name" ? "Name desc" : "Name";
            ViewBag.SortCodeParam = SortBy == "Code" ? "Code desc" : "Code";
            ViewBag.SortCreatedOnParam = SortBy == "CreatedOn" ? "CreatedOn desc" : "CreatedOn";
            ViewBag.SrName = Name;
            ViewBag.SrCode = Code;
            ViewBag.SortDefaultValue = SortBy ?? "";

            var model = _menuServices.GetAll(w => w.Id>0).Select(s => new ListMenuVm
            {
                Id = s.Id,
                //EncryptedId = _protector.Protect(s.Id.ToString()),
                ParentMenu = s.ParentMenu,
                SubMenu = s.SubMenu,
            }).AsEnumerable();
            return View(await model.ToPagedListAsync(page ?? 1, ModelsUtility.PgSize));
        }
        public IActionResult AddMenu()
        {
            return View();
        }
    }
}
