using Bunit;
using Data.Repositories.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using ResourceBookingSystem.Components.Pages;

namespace ResourceBookingSystemTests;

public class ResourcesComponentTests : IDisposable
{
    private readonly BunitContext ctx = new();

    public void Dispose()
    {
        ctx.Dispose();
    }

    [Fact]
    public void ResourcesComponent_RendersAvailableResources()
    {
        var resource = new Resource { Id = 1, Name = "RoomX", IsActive = true };
        ctx.Services.AddSingleton<IResourceRepository>(new FakeResourceRepository(resource));

        var comp = ctx.Render<Resources>();

        Assert.Contains("Available Resources", comp.Markup);
        Assert.Contains("RoomX", comp.Markup);
    }

    private class FakeResourceRepository : IResourceRepository
    {
        private readonly Resource _resource;
        public FakeResourceRepository(Resource resource) => _resource = resource;
        public Task<List<Resource>> GetAllResources() => Task.FromResult(new List<Resource> { _resource });
        public Task<List<Resource>> GetAllActiveResources() => Task.FromResult(new List<Resource> { _resource });

        public Task<Resource?> GetResourceById(int id) =>
            Task.FromResult(id == _resource.Id ? _resource : null as Resource);

        public Task<Resource> AddOrUpdateResource(Resource resource) => Task.FromResult(resource);
    }
}