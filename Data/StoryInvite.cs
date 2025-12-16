using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mass.Data
{
    public class StoryInvite
    {
        public int Id { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTimeOffset? AcceptedAt { get; set; }
        public bool IsAccepted { get; set; }

        public required virtual Story Story { get; set; }
        public required virtual MassIdentityUser InvitedUser { get; set; }
        public required virtual MassIdentityUser InvitedBy { get; set; }
    }
}