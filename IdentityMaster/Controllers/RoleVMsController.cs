using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IdentityMaster.Data;
using IdentityMaster.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityMaster.Controllers
{
    public class RoleVMsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleVMsController(ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _roleManager =  roleManager;
        }

        // GET: RoleVMs
        public async Task<IActionResult> Index()
        {
            var roles =  _roleManager.Roles.ToList();

            List<RoleVM> vm = new List<RoleVM>();
            foreach (var item in roles)
            {
                vm.Add(
                    new RoleVM
                    {
                        Id = item.Id,
                        Name = item.Name
                    }
                    );
            }

            return View(vm);
        }

        // GET: RoleVMs/Details/5
        public async Task<IActionResult> Details(string id)
        {
            var role = await _roleManager.Roles.Where(x => x.Id == id).FirstOrDefaultAsync();
            RoleVM vm = new RoleVM();
            vm.Id = role.Id;
            vm.Name = role.Name;

            return View(vm);
        }

        // GET: RoleVMs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RoleVMs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( RoleVM roleVM)
        {
            await _roleManager.CreateAsync(new IdentityRole(roleVM.Name));

            return RedirectToAction("Index");
        }

        // GET: RoleVMs/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            var roles = _roleManager.Roles.Where(x => x.Id == id).FirstOrDefault();

           RoleVM vm = new RoleVM();
            vm.Id = roles.Id;
            vm.Name = roles.Name;

            return View(vm);
        }

        // POST: RoleVMs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, RoleVM roleVM)
        {
            if (id != roleVM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var role = await _roleManager.Roles.Where(x => x.Id == id).FirstOrDefaultAsync();
                    role.Name = roleVM.Name;
                    await _roleManager.UpdateAsync(role);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoleVMExists(roleVM.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(roleVM);
        }

        // GET: RoleVMs/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _roleManager.Roles.Where(x => x.Id == id).FirstOrDefaultAsync();
            RoleVM vm = new RoleVM();
            vm.Id = role.Id;
            vm.Name = role.Name;

            if (vm == null)
            {
                return NotFound();
            }

            return View(vm);
        }

        // POST: RoleVMs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(RoleVM roleVM)
        {
            var role = await _roleManager.Roles.Where(x => x.Id == roleVM.Id).FirstOrDefaultAsync();
            role.Name = roleVM.Name;
            await _roleManager.DeleteAsync(role);

            return RedirectToAction(nameof(Index));
        }

        private bool RoleVMExists(string id)
        {
            return _context.RoleVM.Any(e => e.Id == id);
        }





    }
}
