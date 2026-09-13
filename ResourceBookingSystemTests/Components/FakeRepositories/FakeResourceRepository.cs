using Data.Repositories.Interfaces;
using Domain.Entities;

namespace ResourceBookingSystemTests.Components.FakeRepositories;

public class FakeResourceRepository(IEnumerable<Resource> existingResources) : IResourceRepository

{
    public List<Resource> Resources { get; } = [.. existingResources];

    public Task<List<Resource>> GetAllResources() => Task.FromResult(Resources.ToList());

    public Task<List<Resource>> GetAllActiveResources() => Task.FromResult(Resources.Where(r => r.IsActive).ToList());

    public Task<Resource?> GetResourceById(int id) => Task.FromResult(Resources.FirstOrDefault(r => r.Id == id));

    public Task<Resource> AddOrUpdateResource(Resource resource)
    {
        var existing = Resources.FirstOrDefault(r => r.Id == resource.Id);

        if (existing is null)
        {
            resource.Id = Resources.Count == 0 ? 1 : Resources.Max(r => r.Id) + 1;
            Resources.Add(resource);
            return Task.FromResult(resource);
        }

        existing.Name = resource.Name;
        existing.IsActive = resource.IsActive;
        return Task.FromResult(existing);
    }
}