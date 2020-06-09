using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMaster.Models
{
    public class ManageUserRolesVM
    {
        public ApplicationUser AppUser { get; set; }
        public List<SelectListItem> Roles { get; set; }
    }
}
