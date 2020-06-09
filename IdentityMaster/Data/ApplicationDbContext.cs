using IdentityMaster.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityMaster.ViewModels;

namespace IdentityMaster.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {

        }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<IdentityMaster.ViewModels.UserVM> UserVM { get; set; }
        public DbSet<IdentityMaster.Models.RoleVM> RoleVM { get; set; }

    }
}
