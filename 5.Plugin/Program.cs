using Microsoft.SemanticKernel;
using Microsoft.Extensions.Configuration;
using _5.Plugin.Skills;
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

var promptTemplate = @"Summarize the following text in a concise and clear manner: {0}";

var summarizeSkill = kernel.CreateFunctionFromPrompt(promptTemplate);

// 1. Summarize a text using the prompt template
string input = "Microsoft Corporation is an American multinational corporation and technology conglomerate headquartered in Redmond, Washington.[2] Founded in 1975, the company became influential in the rise of personal computers through software like Windows, and the company has since expanded to Internet services, cloud computing, video gaming and other fields. Microsoft is the largest software maker, one of the most valuable public U.S. companies,[a] and one of the most valuable brands globally.";

var summarizedResult = await kernel.InvokePromptAsync(string.Format(promptTemplate, input));

Console.WriteLine($"Summarized: {summarizedResult}");

// 2. Load the MathSkill plugin
kernel.ImportPluginFromObject(new MathSkill(), "math");

var matchResult = await kernel.InvokeAsync("math", "Add", new() {
    { "a", "5" },
    { "b", "7" }
});

Console.WriteLine($"Sum result: {matchResult}");

// 3: Count the number of words in the summarized text
var wordCountResult = await kernel.InvokeAsync("math", "WordCount", new() {
    { "text", summarizedResult }
});
Console.WriteLine($"Word count of summarized text: {wordCountResult}");
