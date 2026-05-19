using SmartShoppingAssistantLigaAc.DataAccess.Entities;

namespace SmartShoppingAssistantLigaAc.DataAccess.Repositories;

public interface ICategoryRepository : IRepository<Category>
{
    Task<List<Category>> GetByIdsAsync(IEnumerable<int> ids);
}
