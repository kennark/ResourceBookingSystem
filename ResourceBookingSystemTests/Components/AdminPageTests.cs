using Bunit;
using Data.Repositories.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using ResourceBookingSystem.Components.Pages;
using ResourceBookingSystemTests.Components.FakeRepositories;

namespace ResourceBookingSystemTests.Components;

public class AdminPageTests : IDisposable
{
    private readonly BunitContext _context = new();

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public void AdminPage_RendersExistingResources()
    {
        var repo = new FakeResourceRepository(
        [
            new Resource { Id = 1, Name = "Room A", IsActive = true },
            new Resource { Id = 2, Name = "Room B", IsActive = false }
        ]);

        _context.Services.AddSingleton<IResourceRepository>(repo);

        var cut = _context.Render<AdminPage>();

        Assert.Contains("Admin Page", cut.Markup);
        Assert.Contains("Room A", cut.Markup);
        Assert.Contains("Room B", cut.Markup);
        Assert.Contains("Yes", cut.Markup);
        Assert.Contains("No", cut.Markup);
    }

    [Fact]
    public void AdminPage_CreatesNewResourceAndRefreshesList()
    {
        var repo = new FakeResourceRepository(
        [
            new Resource { Id = 1, Name = "Room A", IsActive = true }
        ]);

        _context.Services.AddSingleton<IResourceRepository>(repo);

        var cut = _context.Render<AdminPage>();

        cut.Find("input").Change("Room C");
        cut.Find("input[type='checkbox']").Change(true);
        cut.Find("form").Submit();

        cut.WaitForAssertion(() =>
        {
            Assert.Equal(2, repo.Resources.Count);
            Assert.Contains("Room C", cut.Markup);
            Assert.Contains("Yes", cut.Markup);
        });
    }
    
}
