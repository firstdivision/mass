using Xunit;
using mass.Data;
using System;

namespace mass.Tests.Data
{
    public class StoryTests
    {
        [Fact]
        public void Story_Constructor_SetsDefaultValues()
        {
            // Arrange & Act
            var story = new Story
            {
                Title = "Test Story",
                Description = "A test story",
                CreatedBy = new MassIdentityUser { UserName = "testuser" }
            };

            // Assert
            Assert.False(story.IsPublic);
            Assert.False(story.IsArchived);
            Assert.False(story.IsLocked);
            Assert.True(story.CreatedAt != default);
            Assert.True(story.LastModifiedAt != default);
            Assert.Equal(story.CreatedAt, story.LastModifiedAt);
        }

        [Fact]
        public void Story_SetProperties_WorksCorrectly()
        {
            // Arrange
            var story = new Story
            {
                Title = "Original",
                Description = "Original description",
                CreatedBy = new MassIdentityUser { UserName = "author" }
            };

            // Act
            story.IsPublic = true;
            story.IsArchived = true;

            // Assert
            Assert.True(story.IsPublic);
            Assert.True(story.IsArchived);
        }

        [Fact]
        public void Story_Contributors_InitializesAsEmptyList()
        {
            // Arrange & Act
            var story = new Story
            {
                Title = "Test",
                Description = "Test",
                CreatedBy = new MassIdentityUser { UserName = "author" }
            };

            // Assert
            Assert.NotNull(story.Contributors);
            Assert.Empty(story.Contributors);
        }

        [Fact]
        public void Story_Chapters_InitializesAsEmptyList()
        {
            // Arrange & Act
            var story = new Story
            {
                Title = "Test",
                Description = "Test",
                CreatedBy = new MassIdentityUser { UserName = "author" }
            };

            // Assert
            Assert.NotNull(story.Chapters);
            Assert.Empty(story.Chapters);
        }
    }
}
