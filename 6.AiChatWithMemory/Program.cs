using Microsoft.SemanticKernel;
using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel.ChatCompletion;
using System.Reflection;

// Load configuration from appsettings.json  
var configuration = new ConfigurationBuilder()
   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
   .AddUserSecrets(Assembly.GetExecutingAssembly())
   .Build();

// Retrieve the API key and model from configuration  
string? apiKey = configuration["OpenAI:ApiKey"];
string? model = configuration["OpenAI:Model"];
if (string.IsNullOrEmpty(apiKey))
{
    throw new InvalidOperationException("API key is missing or null in the configuration.");
}
if (string.IsNullOrEmpty(model))
{
    throw new InvalidOperationException("Model is missing or null in the configuration.");
}

var kernel = Kernel.CreateBuilder()
   .AddOpenAIChatCompletion(model, apiKey) // Ensures 'model' is not null
   .Build();

var chatService = kernel.GetRequiredService<IChatCompletionService>();
var chatHistory = new ChatHistory();

Console.WriteLine("AI Chatbot (has memory). Type 'exit' to exit.\n");

while (true)
{
    Console.Write("You: ");
    var userInput = Console.ReadLine();

    if (string.Equals(userInput, "exit", StringComparison.OrdinalIgnoreCase))
        break;

    // Lưu message người dùng
    chatHistory.AddUserMessage(userInput);

    // Gọi AI model, truyền theo history
    var result = await chatService.GetChatMessageContentAsync(chatHistory);

    // Lưu message AI
    chatHistory.AddAssistantMessage(result.Content);

    Console.WriteLine($"AI: {result.Content}\n");
}

