using Xunit;
using mass.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace mass.Tests.Data
{
    public class MassDbContextTests
    {
        private MassDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<MassDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new MassDbContext(options);
        }

        [Fact]
        public async Task DbContext_CanCreateStory()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var user = new MassIdentityUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "author@example.com",
                Email = "author@example.com"
            };

            var story = new Story
            {
                Title = "Test Story",
                Description = "A test story",
                CreatedBy = user,
                IsPublic = false
            };

            // Act
            context.Stories.Add(story);
            await context.SaveChangesAsync();

            // Assert
            var savedStory = await context.Stories
                .Include(s => s.CreatedBy)
                .FirstOrDefaultAsync(s => s.Title == "Test Story");

            Assert.NotNull(savedStory);
            Assert.Equal("Test Story", savedStory.Title);
            Assert.Equal("A test story", savedStory.Description);
            Assert.False(savedStory.IsPublic);
        }

        [Fact]
        public async Task DbContext_CanCreateChapterWithStory()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var user = new MassIdentityUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "author@example.com",
                Email = "author@example.com"
            };

            var story = new Story
            {
                Title = "Test Story",
                Description = "Description",
                CreatedBy = user
            };

            var chapter = new Chapter
            {
                Title = "Chapter 1",
                Order = 1,
                Story = story,
                CreatedBy = user
            };

            // Act
            context.Stories.Add(story);
            context.Chapters.Add(chapter);
            await context.SaveChangesAsync();

            // Assert
            var savedChapter = await context.Chapters
                .Include(c => c.Story)
                .FirstOrDefaultAsync(c => c.Title == "Chapter 1");

            Assert.NotNull(savedChapter);
            Assert.Equal(1, savedChapter.Order);
            Assert.NotNull(savedChapter.Story);
            Assert.Equal("Test Story", savedChapter.Story.Title);
        }

        [Fact]
        public async Task DbContext_CanCreateEntryWithChapter()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var user = new MassIdentityUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "author@example.com",
                Email = "author@example.com"
            };

            var story = new Story
            {
                Title = "Test Story",
                Description = "Description",
                CreatedBy = user
            };

            var chapter = new Chapter
            {
                Title = "Chapter 1",
                Order = 1,
                Story = story,
                CreatedBy = user
            };

            var entry = new Entry
            {
                Content = "Entry content",
                Order = 1,
                Chapter = chapter,
                CreatedBy = user
            };

            // Act
            context.Stories.Add(story);
            context.Chapters.Add(chapter);
            context.Entries.Add(entry);
            await context.SaveChangesAsync();

            // Assert
            var savedEntry = await context.Entries
                .Include(e => e.Chapter)
                .FirstOrDefaultAsync(e => e.Content == "Entry content");

            Assert.NotNull(savedEntry);
            Assert.Equal("Entry content", savedEntry.Content);
            Assert.NotNull(savedEntry.Chapter);
            Assert.Equal("Chapter 1", savedEntry.Chapter.Title);
        }

        [Fact]
        public async Task DbContext_CanCreateWritingPrompt()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var prompt = new WritingPrompt
            {
                PromptText = "Write about your biggest dream"
            };

            // Act
            context.WritingPrompts.Add(prompt);
            await context.SaveChangesAsync();

            // Assert
            var savedPrompt = await context.WritingPrompts
                .FirstOrDefaultAsync(p => p.PromptText == "Write about your biggest dream");

            Assert.NotNull(savedPrompt);
            Assert.Equal("Write about your biggest dream", savedPrompt.PromptText);
        }

        [Fact]
        public async Task DbContext_CanUpdateStory()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var user = new MassIdentityUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "author@example.com",
                Email = "author@example.com"
            };

            var story = new Story
            {
                Title = "Original Title",
                Description = "Original Description",
                CreatedBy = user,
                IsPublic = false
            };

            context.Stories.Add(story);
            await context.SaveChangesAsync();
            var storyId = story.Id;

            // Act
            story.Title = "Updated Title";
            story.IsPublic = true;
            await context.SaveChangesAsync();

            // Assert
            var updatedStory = await context.Stories.FindAsync(storyId);
            Assert.NotNull(updatedStory);
            Assert.Equal("Updated Title", updatedStory.Title);
            Assert.True(updatedStory.IsPublic);
        }

        [Fact]
        public async Task DbContext_CanDeleteStory()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var user = new MassIdentityUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "author@example.com",
                Email = "author@example.com"
            };

            var story = new Story
            {
                Title = "Story to Delete",
                Description = "Delete me",
                CreatedBy = user
            };

            context.Stories.Add(story);
            await context.SaveChangesAsync();
            var storyId = story.Id;

            // Act
            context.Stories.Remove(story);
            await context.SaveChangesAsync();

            // Assert
            var deletedStory = await context.Stories.FindAsync(storyId);
            Assert.Null(deletedStory);
        }
    }
}
