using Microsoft.EntityFrameworkCore;
using SmartShoppingAssistantLigaAc.DataAccess.Entities;

namespace SmartShoppingAssistantLigaAc.DataAccess.Repositories;

public class CartItemRepository(SmartShoppingAssistantDbContext context)
    : BaseRepository<CartItem>(context), ICartItemRepository
{
    private IQueryable<CartItem> WithProduct() =>
        GetAllAsQueryable().Include(ci => ci.Product);
    
    private IQueryable<CartItem> WithProductWithCategories() =>
        GetAllAsQueryable().Include(ci => ci.Product).ThenInclude(p => p.Categories);

    public async Task<List<CartItem>> GetAllWithProductAsync()
    {
        return await WithProduct().ToListAsync();
    }

    public async Task<List<CartItem>> GetAllWithProductWithCategoriesAsync()
    {
        return await WithProductWithCategories().ToListAsync();
    }
    public async Task<CartItem?> GetByProductIdAsync(int productId)
    {
        return await WithProduct().FirstOrDefaultAsync(ci => ci.ProductId == productId);
    }

    public async Task<CartItem> GetByIdWithProductAsync(int id)
    {
        return await WithProduct().FirstOrDefaultAsync(ci => ci.Id == id)
            ?? throw new Exception($"Cart item with id {id} not found");
    }

    public async Task ClearAsync()
    {
        context.CartItems.RemoveRange(context.CartItems);
        await context.SaveChangesAsync();
    }
}
