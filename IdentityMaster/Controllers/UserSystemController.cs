using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityMaster.Data;
using IdentityMaster.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IdentityMaster.Controllers
{
    public class UserSystemController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
        private readonly RoleManager<IdentityRole> _roleManager;
        public IEnumerable<ApplicationUser> ApplicationUserList;
        public ApplicationUser ApplicationUserSingle;

        public UserSystemController(ApplicationDbContext db, UserManager<ApplicationUser> userManager, IPasswordHasher<ApplicationUser> passwordHasher, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _passwordHasher = passwordHasher;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            ApplicationUserList = _db.ApplicationUser.ToList();
            return View(ApplicationUserList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ApplicationUser model, string password)
        {
            IdentityResult identityResult = await _userManager.CreateAsync(model, password);

            return RedirectToAction("Index");
        }

        public IActionResult Edit(string id)
        {
           ApplicationUserSingle = _db.ApplicationUser.Find(id);

            return View(ApplicationUserSingle);
        }

        [HttpPost]
        public IActionResult Edit(ApplicationUser model, string password)
        {
            ApplicationUserSingle = _db.ApplicationUser.Find(model.Id);
            ApplicationUserSingle.FullName = model.FullName;
            ApplicationUserSingle.UserName = model.UserName;
            ApplicationUserSingle.Email = model.Email;
            if (!string.IsNullOrEmpty(password))
            {
                ApplicationUserSingle.PasswordHash = _passwordHasher.HashPassword(model, password);
            }

            _db.Update(ApplicationUserSingle);
            _db.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Details(string id)
        {
            ApplicationUserSingle = _db.ApplicationUser.Find(id);

            return View(ApplicationUserSingle);
        }

        public IActionResult Delete(string id)
        {
            ApplicationUserSingle = _db.ApplicationUser.Find(id);

            return View(ApplicationUserSingle);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ApplicationUser model)
        {
            ApplicationUserSingle = _db.ApplicationUser.Find(model.Id);
            await _userManager.DeleteAsync(ApplicationUserSingle);

            return RedirectToAction("index");
        }

        public IActionResult AssignRole(string id)
        {

            var users = _db.ApplicationUser.Where(x => x.Id == id).SingleOrDefault();
            var userRoles = _db.UserRoles.Where(x => x.UserId == id).Select(x => x.RoleId).ToList();
            ManageUserRolesVM userRolesVM = new ManageUserRolesVM();
            userRolesVM.AppUser = users;
            userRolesVM.Roles = _roleManager.Roles.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id,
                Selected = userRoles.Contains(x.Id)
            }).ToList();

            return View(userRolesVM);
        }


        [HttpPost]
        public IActionResult AssignRole(ManageUserRolesVM model)
        {

            var selectedRoleId = model.Roles.Where(x => x.Selected).Select(x => x.Value);
            var alreadyExitsRoleId = _db.UserRoles.Where(x => x.UserId == model.AppUser.Id).Select(x => x.RoleId).ToList();
            var toAdd = selectedRoleId.Except(alreadyExitsRoleId);
            var toRemove = alreadyExitsRoleId.Except(selectedRoleId);

            foreach (var item in toRemove)
            {
                _db.UserRoles.Remove(new IdentityUserRole<string>
                {
                    RoleId = item,
                    UserId = model.AppUser.Id
                });
            }

            foreach (var item in toAdd)
            {
                _db.UserRoles.Add(new IdentityUserRole<string>
                {
                    RoleId = item,
                    UserId = model.AppUser.Id
                });
            }
            _db.SaveChanges();

            return RedirectToAction("AssignRole");
        }

    }
}