using System;
using System.Collections.Generic;

namespace mass.Data
{
    public class StoryWithNewEntries
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public bool IsArchived { get; set; }
        public bool IsLocked { get; set; }
        public DateTimeOffset LastModifiedAt { get; set; }
        
        public required string CreatedBy { get; set; }
        public string? LockedBy { get; set; }
        public List<string> Contributors { get; set; } = new();
        
        public bool HasNewEntries { get; set; }
    }
}
