using Xunit;
using mass.Data;
using System;

namespace mass.Tests.Data
{
    public class ChapterTests
    {
        [Fact]
        public void Chapter_Constructor_SetsDefaultValues()
        {
            // Arrange & Act
            var chapter = new Chapter
            {
                Title = "Chapter 1",
                Order = 1,
                Story = new Story { Title = "Test", Description = "Test", CreatedBy = new MassIdentityUser { UserName = "author" } },
                CreatedBy = new MassIdentityUser { UserName = "author" }
            };

            // Assert
            Assert.True(chapter.CreatedAt != default);
            Assert.True(chapter.LastModifiedAt != default);
        }

        [Fact]
        public void Chapter_Entries_InitializesAsEmptyList()
        {
            // Arrange & Act
            var chapter = new Chapter
            {
                Title = "Chapter 1",
                Order = 1,
                Story = new Story { Title = "Test", Description = "Test", CreatedBy = new MassIdentityUser { UserName = "author" } },
                CreatedBy = new MassIdentityUser { UserName = "author" }
            };

            // Assert
            Assert.NotNull(chapter.Entries);
            Assert.Empty(chapter.Entries);
        }

        [Fact]
        public void Chapter_SetProperties_WorksCorrectly()
        {
            // Arrange
            var story = new Story { Title = "Test", Description = "Test", CreatedBy = new MassIdentityUser { UserName = "author" } };
            var chapter = new Chapter
            {
                Title = "Chapter 1",
                Order = 1,
                Story = story,
                CreatedBy = new MassIdentityUser { UserName = "author" }
            };

            // Act
            chapter.Title = "Updated Chapter";
            chapter.Order = 2;

            // Assert
            Assert.Equal("Updated Chapter", chapter.Title);
            Assert.Equal(2, chapter.Order);
        }
    }
}
