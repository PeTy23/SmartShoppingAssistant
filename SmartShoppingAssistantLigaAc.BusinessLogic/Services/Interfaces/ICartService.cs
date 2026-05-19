using SmartShoppingAssistantLigaAc.BusinessLogic.DTOs;

namespace SmartShoppingAssistantLigaAc.BusinessLogic.Services.Interfaces;

public interface ICartService
{
    Task<CartGetDTO> GetCartAsync();
    Task<CartItemGetDTO> AddItemAsync(AddCartItemDTO dto);
    Task<CartItemGetDTO> UpdateItemAsync(int itemId, UpdateCartItemDTO dto);
    Task RemoveItemAsync(int itemId);
    Task ClearCartAsync();

    Task<AnalysisResponse> AnalyzeCartAsync();
}
