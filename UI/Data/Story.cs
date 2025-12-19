using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mass.Data
{
    public class Story
    {
        public Story()
        {
            // When a new Story is created, set CreatedAt and LastModifiedAt to now / the same value
            var now = DateTime.UtcNow;
            CreatedAt = now;
            LastModifiedAt = now;
        }
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public bool IsPublic { get; set; }
        public bool IsArchived { get; set; }
        public bool IsLocked { get; set; }
        public DateTimeOffset CreatedAt { get; set; } 
        public DateTimeOffset LastModifiedAt { get; set; }

        public required virtual MassIdentityUser CreatedBy { get; set; }
        public virtual MassIdentityUser? LockedBy { get; set; }
        public virtual ICollection<MassIdentityUser> Contributors { get; set; } = new List<MassIdentityUser>();
        public virtual ICollection<Chapter> Chapters { get; set; } = new List<Chapter>();
    }
}