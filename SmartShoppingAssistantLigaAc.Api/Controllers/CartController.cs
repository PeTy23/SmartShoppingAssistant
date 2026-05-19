using Microsoft.AspNetCore.Mvc;
using SmartShoppingAssistantLigaAc.BusinessLogic.Agents;
using SmartShoppingAssistantLigaAc.BusinessLogic.DTOs;
using SmartShoppingAssistantLigaAc.BusinessLogic.Services.Interfaces;
using System.Text.Json;
namespace SmartShoppingAssistantLigaAc.Api.Controllers;

[Route("api/cart")]
[ApiController]
public class CartController(ICartService cartService,
    ICategoryService categoryService,
    IPromotionCheckerAgent promotionCheckerAgent,
    ISuggestionComposerAgent suggestionComposerAgent) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<CartGetDTO>> GetCart()
    {
        var cart = await cartService.GetCartAsync();
        return Ok(cart);
    }

    [HttpPost("items")]
    public async Task<ActionResult<CartItemGetDTO>> AddItem([FromBody] AddCartItemDTO dto)
    {
        try
        {
            var item = await cartService.AddItemAsync(dto);
            return Ok(item);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPut("items/{itemId}")]
    public async Task<ActionResult<CartItemGetDTO>> UpdateItem(int itemId, [FromBody] UpdateCartItemDTO dto)
    {
        try
        {
            var item = await cartService.UpdateItemAsync(itemId, dto);
            return Ok(item);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("items/{itemId}")]
    public async Task<IActionResult> RemoveItem(int itemId)
    {
        try
        {
            await cartService.RemoveItemAsync(itemId);
            return NoContent();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete]
    public async Task<IActionResult> ClearCart()
    {
        await cartService.ClearCartAsync();
        return NoContent();
    }

    [HttpPost("analyze")]

    public async Task<IActionResult> AnalyzeCart()
    {
        var analysis = await cartService.AnalyzeCartAsync();
        return Ok(analysis);
    } 

    //[HttpGet("analyze")]
    //public async Task<IActionResult> AnalyzeCartWithAI()
    //{
    //    try
    //    {
    //        // 1. Pregătim datele (Coșul și Categoriile)
    //        var cart = await cartService.GetCartAsync();
    //        var categories = await categoryService.GetAllAsync(); // Presupunem ca ai o metoda GetAllAsync in ICategoryService

    //        // Convertim obiectele C# in string-uri JSON pentru AI
    //        var cartJson = JsonSerializer.Serialize(cart);
    //        var categoriesJson = JsonSerializer.Serialize(categories);

    //        // 2. Apelăm Agentul 1 (Promotion Checker)
    //        var checkerAgent = promotionCheckerAgent.Build(cartJson);

    //        // Trimitem un mesaj simplu ca să declanșăm analiza
    //        var checkerResponse = await checkerAgent.RunAsync("Analyze my cart for promotions.");
    //        var promotionAnalysisJson = checkerResponse.Text; // Aici se află rezultatul (JSON-ul cu Near-Miss deals)

    //        // 3. Apelăm Agentul 2 (Suggestion Composer) și îi dăm tot contextul!
    //        var composerAgent = suggestionComposerAgent.Build(cartJson, categoriesJson, promotionAnalysisJson);

    //        var composerResponse = await composerAgent.RunAsync("Generate recommendations based on my cart and promotions.");

    //        // 4. Returnăm JSON-ul curat generat de al doilea agent către Frontend/Client
    //        // Modelul returnează un string (care e formatat JSON), deci folosim ContentResult
    //        return Content(composerResponse.Text, "application/json");
    //    }
    //    catch (Exception ex)
    //    {
    //        return StatusCode(500, new { Error = "Eroare la analiza AI", Details = ex.Message });
    //    }
    //}
}
