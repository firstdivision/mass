using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mass.Data
{
    public class Story
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public bool IsPublic { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTimeOffset LastModifiedAt { get; set; } = DateTime.UtcNow;

        public required virtual MassIdentityUser CreatedBy { get; set; }
        public virtual ICollection<MassIdentityUser> Contributors { get; set; } = new List<MassIdentityUser>();
        public virtual ICollection<Chapter> Chapters { get; set; } = new List<Chapter>();
    }
}