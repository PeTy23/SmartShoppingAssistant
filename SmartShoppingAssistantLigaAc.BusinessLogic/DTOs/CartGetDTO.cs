namespace SmartShoppingAssistantLigaAc.BusinessLogic.DTOs;

public class CartGetDTO
{
    public List<CartItemGetDTO> Items { get; set; } = new();
}
