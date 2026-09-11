using Domain.Entities;

namespace Data.Services.Interfaces;

public interface IResourceService
{
    Task<List<Resource>> GetAllResources();
    Task<List<Resource>> GetAllActiveResources();
    Task<Resource?> GetResourceById(int id);

    Task<Resource> AddOrUpdateResource(Resource resource);
}