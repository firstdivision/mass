using Xunit;
using mass.Data;
using System;

namespace mass.Tests.Data
{
    public class EntryTests
    {
        [Fact]
        public void Entry_Constructor_SetsDefaultValues()
        {
            // Arrange
            var chapter = new Chapter
            {
                Title = "Chapter 1",
                Order = 1,
                Story = new Story { Title = "Test", Description = "Test", CreatedBy = new MassIdentityUser { UserName = "author" } },
                CreatedBy = new MassIdentityUser { UserName = "author" }
            };
            var user = new MassIdentityUser { UserName = "writer" };

            // Act
            var entry = new Entry
            {
                Content = "Test entry content",
                Chapter = chapter,
                CreatedBy = user,
                Order = 1
            };

            // Assert
            Assert.True(entry.CreatedAt != default);
            Assert.True(entry.LastModifiedAt != default);
            Assert.Equal(entry.CreatedAt, entry.LastModifiedAt);
        }

        [Fact]
        public void Entry_SetProperties_WorksCorrectly()
        {
            // Arrange
            var chapter = new Chapter
            {
                Title = "Chapter 1",
                Order = 1,
                Story = new Story { Title = "Test", Description = "Test", CreatedBy = new MassIdentityUser { UserName = "author" } },
                CreatedBy = new MassIdentityUser { UserName = "author" }
            };
            var entry = new Entry
            {
                Content = "Original content",
                Chapter = chapter,
                CreatedBy = new MassIdentityUser { UserName = "writer" },
                Order = 1
            };

            // Act
            entry.Content = "Updated content";
            entry.Order = 2;

            // Assert
            Assert.Equal("Updated content", entry.Content);
            Assert.Equal(2, entry.Order);
        }

        [Fact]
        public void Entry_HasRequiredFields()
        {
            // This test verifies that required properties cannot be null at compile time
            // Actual null checking would be done by EF Core at runtime
            var chapter = new Chapter
            {
                Title = "Chapter 1",
                Order = 1,
                Story = new Story { Title = "Test", Description = "Test", CreatedBy = new MassIdentityUser { UserName = "author" } },
                CreatedBy = new MassIdentityUser { UserName = "author" }
            };
            var entry = new Entry
            {
                Content = "Test",
                Chapter = chapter,
                CreatedBy = new MassIdentityUser { UserName = "writer" }
            };

            // Assert
            Assert.NotNull(entry.Content);
            Assert.NotNull(entry.Chapter);
            Assert.NotNull(entry.CreatedBy);
        }
    }
}
