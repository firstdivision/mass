using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace mass.Data
{
    public class MassMassIdentityUserRole : IdentityUserRole<string>
    {
        public virtual MassIdentityUser User { get; set; } = null!;
        public virtual MassApplicationRole Role { get; set; } = null!;
    }
}