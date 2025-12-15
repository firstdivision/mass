using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mass.Data
{
    public class Chapter
    {
        public int Id { get; set; }
        public int Order { get; set; }
        public required string Title { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTimeOffset LastModifiedAt { get; set; } = DateTime.UtcNow;

        public required virtual Story Story { get; set; }
        public required virtual MassIdentityUser CreatedBy { get; set; }
    }
}