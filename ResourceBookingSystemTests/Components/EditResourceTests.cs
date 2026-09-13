using Bunit;
using Data.Repositories.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using ResourceBookingSystem.Components.Pages;
using ResourceBookingSystemTests.Components.FakeRepositories;

namespace ResourceBookingSystemTests.Components;

public class EditResourceTests : IDisposable
{
    private readonly BunitContext _context = new();

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public void EditResource_LoadsExistingResource()
    {
        var resource = new Resource { Id = 9, Name = "Board Room", IsActive = true };
        var repo = new FakeResourceRepository([resource]);
        _context.Services.AddSingleton<IResourceRepository>(repo);

        var cut = _context.Render<EditResource>(parameters => parameters
            .Add(p => p.ResourceId, 9));

        cut.WaitForAssertion(() =>
        {
            Assert.Contains("Edit Resource", cut.Markup);
            Assert.Contains("Board Room", cut.Markup);
            Assert.Contains("Save edits", cut.Markup);
        });
    }

    [Fact]
    public void EditResource_Submit_UpdatesResourceAndNavigatesToAdminPage()
    {
        var resource = new Resource { Id = 10, Name = "Old Room", IsActive = true };
        var repo = new FakeResourceRepository([resource]);
        _context.Services.AddSingleton<IResourceRepository>(repo);

        var cut = _context.Render<EditResource>(parameters => parameters
            .Add(p => p.ResourceId, 10));

        cut.Find("input[name='_resourceForm.Name']").Change("Updated Room");
        cut.Find("input[type='checkbox']").Change(false);
        cut.Find("form").Submit();

        cut.WaitForAssertion(() =>
        {
            Assert.Equal("Updated Room", repo.GetResourceById(resource.Id).Result?.Name);
            Assert.False(repo.GetResourceById(resource.Id).Result?.IsActive);
            Assert.Contains("/AdminPage", _context.Services.GetRequiredService<NavigationManager>().Uri);
        });
    }

    [Fact]
    public void EditResource_WhenResourceDoesNotExist_ShowsNotFoundMessage()
    {
        var repo = new FakeResourceRepository([]);
        _context.Services.AddSingleton<IResourceRepository>(repo);

        var cut = _context.Render<EditResource>(parameters => parameters
            .Add(p => p.ResourceId, 999));

        cut.WaitForAssertion(() =>
        {
            Assert.Contains("Resource not found.", cut.Markup);
        });
    }
}
