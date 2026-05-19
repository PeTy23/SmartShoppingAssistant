using SmartShoppingAssistantLigaAc.DataAccess.Entities;

namespace SmartShoppingAssistantLigaAc.DataAccess.Repositories;

public interface ICartItemRepository : IRepository<CartItem>
{
    Task<List<CartItem>> GetAllWithProductAsync();
    Task<CartItem?> GetByProductIdAsync(int productId);
    Task<CartItem> GetByIdWithProductAsync(int id);
    Task ClearAsync();
    Task<List<CartItem>> GetAllWithProductWithCategoriesAsync();
}
