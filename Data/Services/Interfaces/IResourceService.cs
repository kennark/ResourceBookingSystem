using Domain.Entities;

namespace Data.Services.Interfaces;

public interface IResourceService
{
    Task<List<Resource>> GetAllResources();

    Task<Resource> AddResource(Resource resource);
}