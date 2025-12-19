using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mass.Data
{
    public class Chapter
    {
        public Chapter()
        {
            // When a new Chapter is created, set CreatedAt and LastModifiedAt to now / the same value
            var now = DateTime.UtcNow;
            CreatedAt = now;
            LastModifiedAt = now;
        }
        public int Id { get; set; }
        public int Order { get; set; }
        public required string Title { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset LastModifiedAt { get; set; }

        public required virtual Story Story { get; set; }
        public required virtual MassIdentityUser CreatedBy { get; set; }
        public virtual ICollection<Entry> Entries { get; set; } = new List<Entry>();
    }
}