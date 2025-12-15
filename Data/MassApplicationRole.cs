using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace mass.Data
{
    public class MassApplicationRole : IdentityRole
    {
        public virtual ICollection<MassMassIdentityUserRole> UserRoles { get; set; } = null!;
    }
}