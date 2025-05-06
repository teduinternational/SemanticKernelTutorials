using Microsoft.SemanticKernel;
using Microsoft.Extensions.Configuration;
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

Console.WriteLine("Enter your question:");
string? input = Console.ReadLine();
if (string.IsNullOrEmpty(input))
{
    throw new InvalidOperationException("Input cannot be null or empty.");
}

var result = await kernel.InvokePromptAsync(input);

Console.WriteLine($"\n Response: {result}");
