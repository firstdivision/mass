using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace mass.Data
{
    public class MassIdentityUser : IdentityUser
    {
        public virtual ICollection<MassMassIdentityUserRole> UserRoles { get; set; } = null!;
        public virtual ICollection<Story> CreatedStories { get; set; } = new List<Story>();
        public virtual ICollection<Story> ContributedStories { get; set; } = new List<Story>();
        public virtual ICollection<Entry> CreatedEntries { get; set; } = new List<Entry>();
    }
}