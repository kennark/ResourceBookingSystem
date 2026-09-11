using Data.Services.Interfaces;
using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Data.Services;

public class ResourceService(DatabaseContext dbContext) : IResourceService
{
    public async Task<List<Resource>> GetAllResources()
    {
        return await dbContext.Resources.ToListAsync();
    }

    public async Task<List<Resource>> GetAllActiveResources()
    {
        return await dbContext.Resources.Where(resource => resource.IsActive).ToListAsync();
    }

    public async Task<Resource?> GetResourceById(int id)
    {
        return await dbContext.Resources.FindAsync(id);
    }

    public async Task<Resource> AddOrUpdateResource(Resource resource)
    {
        EntityEntry<Resource> result;
        if (dbContext.Resources.Any(x => x.Id == resource.Id))
        {
            result = dbContext.Resources.Update(resource);
        }
        else
        {
            result = await dbContext.Resources.AddAsync(resource);
        }

        await dbContext.SaveChangesAsync();
        return result.Entity;
    }
}