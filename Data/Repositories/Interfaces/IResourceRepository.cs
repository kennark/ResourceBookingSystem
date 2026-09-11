using Domain.Entities;

namespace Data.Repositories.Interfaces;

public interface IResourceRepository
{
    Task<List<Resource>> GetAllResources();
    Task<List<Resource>> GetAllActiveResources();
    Task<Resource?> GetResourceById(int id);

    Task<Resource> AddOrUpdateResource(Resource resource);
}