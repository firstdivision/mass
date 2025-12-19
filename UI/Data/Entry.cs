using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mass.Data
{
    public class Entry
    {
        public Entry()
        {
            // When a new Entry is created, set CreatedAt and LastModifiedAt to now / the same value
            var now = DateTime.UtcNow;
            CreatedAt = now;
            LastModifiedAt = now;
        }
        public int Id { get; set; }
        public int Order { get; set; }
        public required string Content { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset LastModifiedAt { get; set; }
        public required virtual Chapter Chapter { get; set; }
        public required virtual MassIdentityUser CreatedBy { get; set; }
    }
}