using Microsoft.Agents.AI.Workflows;
using Microsoft.Extensions.AI;
using SmartShoppingAssistantLigaAc.BusinessLogic.Agents;
using SmartShoppingAssistantLigaAc.BusinessLogic.DTOs;
using SmartShoppingAssistantLigaAc.BusinessLogic.Services.Interfaces;
using SmartShoppingAssistantLigaAc.DataAccess.Entities;
using SmartShoppingAssistantLigaAc.DataAccess.Repositories;
using System.Text.Json;

namespace SmartShoppingAssistantLigaAc.BusinessLogic.Services;

public class CartService(ICartItemRepository cartItemRepository, IProductRepository productRepository, ICategoryRepository categoryRepository,
    IPromotionCheckerAgent PromotionCheckerAgent, ISuggestionComposerAgent SuggestionComposerAgent) : ICartService
{
    public async Task<CartGetDTO> GetCartAsync()
    {
        var items = await cartItemRepository.GetAllWithProductAsync();
        return new CartGetDTO { Items = items.Select(MapToDTO).ToList() };
    }

    public async Task<CartItemGetDTO> AddItemAsync(AddCartItemDTO dto)
    {
        await productRepository.GetByIdAsync(dto.ProductId); // throws if not found

        var existing = await cartItemRepository.GetByProductIdAsync(dto.ProductId);
        if (existing != null)
        {
            existing.Quantity += dto.Quantity;
            await cartItemRepository.UpdateAsync(existing);
            return MapToDTO(existing);
        }

        var item = new CartItem { ProductId = dto.ProductId, Quantity = dto.Quantity };
        await cartItemRepository.AddAsync(item);
        var added = await cartItemRepository.GetByIdWithProductAsync(item.Id);
        return MapToDTO(added);
    }

    public async Task<CartItemGetDTO> UpdateItemAsync(int itemId, UpdateCartItemDTO dto)
    {
        var item = await cartItemRepository.GetByIdWithProductAsync(itemId);
        item.Quantity = dto.Quantity;
        await cartItemRepository.UpdateAsync(item);
        return MapToDTO(item);
    }

    public Task RemoveItemAsync(int itemId) => cartItemRepository.DeleteAsync(itemId);

    public Task ClearCartAsync() => cartItemRepository.ClearAsync();

    private static CartItemGetDTO MapToDTO(CartItem ci) => new()
    {
        Id = ci.Id,
        ProductId = ci.ProductId,
        ProductName = ci.Product.Name,
        UnitPrice = ci.Product.Price,
        Quantity = ci.Quantity
    };

    public async Task<AnalysisResponse> AnalyzeCartAsync()
    {
        var cart = await cartItemRepository.GetAllWithProductWithCategoriesAsync();
        var categories = await categoryRepository.GetAllAsync();
        var cartJson = JsonSerializer.Serialize(cart.Select(c => new
        {
            c.ProductId,
            c.Product.Name,
            c.Product.Price,
            c.Quantity,
            LineTotal = c.Product.Price * c.Quantity,
            categories = c.Product.Categories.Select(cat => new { CategoryId = cat.Id, CategoryName = cat.Name }).ToList()
        }));


        var categoryJson = JsonSerializer.Serialize(categories.Select(c => new
        {
            CategoryId = c.Id,
            CategoryName = c.Name,
        }));

        var promotionCheckerAgent = PromotionCheckerAgent.Build(cartJson);
        var sugestionAgent = SuggestionComposerAgent.Build(cartJson, categoryJson);

        var workflow = new WorkflowBuilder(promotionCheckerAgent).AddEdge(promotionCheckerAgent, sugestionAgent).
            WithOutputFrom(sugestionAgent).Build();

        var chatMessage = new List<ChatMessage> {
            new(ChatRole.User, "Analyze the cart and provide a summary of any promotions that apply, along with suggestions to optimize the cart based on the products and their categories.")
        };

        await using var result = await InProcessExecution.RunStreamingAsync(workflow, chatMessage);

        await result.TrySendMessageAsync(new TurnToken(emitEvents: true));

        var jsonBuilder = new System.Text.StringBuilder();

        await foreach (var message in result.WatchStreamAsync())
        {
            if (message is AgentResponseUpdateEvent update && update.ExecutorId.StartsWith("SuggestionComposer"))
            {
                jsonBuilder.Append(update.Update.Text);
            }
            else if (message is WorkflowErrorEvent errorEvent)
            {
                throw new InvalidOperationException(errorEvent.Exception.Message);
            }
        }

        var json = jsonBuilder.ToString();
        return JsonSerializer.Deserialize<AnalysisResponse>(json) ?? throw new InvalidOperationException("Failed to deserialize analysis response.");
    }
}