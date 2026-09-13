using Bunit;
using Data.Repositories.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using ResourceBookingSystem.Components.Pages;
using ResourceBookingSystemTests.Components.FakeRepositories;

namespace ResourceBookingSystemTests.Components;

public class ResourcesComponentTests : IDisposable
{
    private readonly BunitContext _ctx = new();

    public void Dispose()
    {
        _ctx.Dispose();
    }

    [Fact]
    public void ResourcesComponent_RendersAvailableResources()
    {
        var resource = new Resource { Id = 1, Name = "RoomX", IsActive = true };
        _ctx.Services.AddSingleton<IResourceRepository>(new FakeResourceRepository([
            resource
        ]));

        var comp = _ctx.Render<Resources>();

        Assert.Contains("Available Resources", comp.Markup);
        Assert.Contains("RoomX", comp.Markup);
    }
}