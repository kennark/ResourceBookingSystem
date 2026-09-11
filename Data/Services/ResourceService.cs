using Data.Services.Interfaces;
using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Services;

public class ResourceService(DatabaseContext dbContext) : IResourceService
{
    public async Task<List<Resource>> GetAllResources()
    {
        return await dbContext.Resources.ToListAsync();
    }

    public async Task<Resource> AddResource(Resource resource)
    {
        var result = dbContext.Resources.Add(resource);
        await dbContext.SaveChangesAsync();
        return result.Entity;
    }
}