using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using SmartShoppingAssistantLigaAc.BusinessLogic.Models;
using SmartShoppingAssistantLigaAc.BusinessLogic.Services;
using SmartShoppingAssistantLigaAc.BusinessLogic.Services.Interfaces;
using SmartShoppingAssistantLigaAc.BusinessLogic.Tools;
using System.ComponentModel;

namespace SmartShoppingAssistantLigaAc.BusinessLogic.Agents;

public class SuggestionComposerAgent(IChatClient chatClient, IProductService productService) : ISuggestionComposerAgent
{
    public ChatClientAgent Build(string cartJson, string categoriesJson)
    {
        return new ChatClientAgent(
            chatClient,
            new ChatClientAgentOptions
            {
                Name = "SuggestionComposer",
                Description = "Compune recomandări de produse pentru a activa promoții sau a completa coșul.",
                ChatOptions = new ChatOptions
                {
                    // Aici este "Sufletul" agentului - Promptul de sistem
                    Instructions = $"""
                        You are a strict shopping recommendation assistant.
                        Here is the current Cart: {cartJson}
                        Here are the available Categories: {categoriesJson}

                        Your task:
                        1. Look at the 'near-miss' deals. Suggest products from the available Categories that would help the user activate those deals.
                        2. If there are no near-miss deals, suggest complementary products based on what is already in the Cart and the available Categories.
                        3. You MUST provide exactly up to 5 recommendations. Maximum 5.
                        4. Provide a logical 'reasoning' for each suggestion (e.g., "Add 1 more Spaghetti to get 1 free!").
                        """,
                    // Forțăm output-ul să fie structurat după clasa noastră nouă!
                    ResponseFormat = ChatResponseFormat.ForJsonSchema<Models.AnalysisResponse>(),
                    Tools =
                            [
                                AIFunctionFactory.Create(
                                ([Description("The category ID to get products for")] int categoryId) =>
                                    ShoppingTools.GetProductsByCategory(categoryId, productService),
                                "GetProductsByCategory",
                                "Get a list of available products for a specific category ID."
                            )
                            ]

                                            }
            },

            null!,
            null!
        );
    }
}