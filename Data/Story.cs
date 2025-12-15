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

        public required virtual MassIdentityUser CreatedBy { get; set; }
    }
}