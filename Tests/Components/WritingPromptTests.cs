using Xunit;
using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Security.Claims;
using mass.Data;
using WritingPromptComponent = mass.Components.Pages.WritingPrompt;

namespace mass.Tests.Components
{
    public class WritingPromptTests : BunitContext
    {
        private MassDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<MassDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new MassDbContext(options);
        }

        [Fact]
        public void WritingPrompt_RequiresAuthentication()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            Services.Add(new ServiceDescriptor(typeof(MassDbContext), _ => dbContext, ServiceLifetime.Scoped));

            // Add a mock authentication state provider that returns unauthenticated user
            var authenticationStateMock = new Mock<AuthenticationStateProvider>();
            var unauthenticatedUser = new ClaimsPrincipal();
            var authState = new AuthenticationState(unauthenticatedUser);
            authenticationStateMock
                .Setup(x => x.GetAuthenticationStateAsync())
                .ReturnsAsync(authState);

            Services.Add(new ServiceDescriptor(typeof(AuthenticationStateProvider), _ => authenticationStateMock.Object, ServiceLifetime.Scoped));

            // Act - attempt to render the component
            var cut = Render<WritingPromptComponent>();

            // Assert - component should require authorization (route should redirect)
            // The [Authorize] attribute will prevent unauthorized access
            // This test verifies the component has the [Authorize] attribute applied
            var component = typeof(WritingPromptComponent);
            var authorizeAttributes = component.GetCustomAttributes(typeof(AuthorizeAttribute), false);
            Assert.NotEmpty(authorizeAttributes);
        }

        [Fact]
        public void WritingPrompt_AllowsAuthenticatedUsers()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            Services.Add(new ServiceDescriptor(typeof(MassDbContext), _ => dbContext, ServiceLifetime.Scoped));

            // Add a mock authentication state provider that returns authenticated user
            var authenticationStateMock = new Mock<AuthenticationStateProvider>();
            var authenticatedUser = new ClaimsPrincipal(
                new ClaimsIdentity(
                    new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, "test-user-id"),
                        new Claim(ClaimTypes.Name, "testuser"),
                    },
                    "TestScheme"
                )
            );
            var authState = new AuthenticationState(authenticatedUser);
            authenticationStateMock
                .Setup(x => x.GetAuthenticationStateAsync())
                .ReturnsAsync(authState);

            Services.Add(new ServiceDescriptor(typeof(AuthenticationStateProvider), _ => authenticationStateMock.Object, ServiceLifetime.Scoped));
            Services.AddAuthorizationCore();

            // Act - render the component as authenticated user
            var cut = Render<WritingPromptComponent>();

            // Assert - component should render successfully for authenticated users
            var component = typeof(WritingPromptComponent);
            var authorizeAttributes = component.GetCustomAttributes(typeof(AuthorizeAttribute), false);
            Assert.NotEmpty(authorizeAttributes);
            Assert.NotNull(cut);
        }
    }
}
