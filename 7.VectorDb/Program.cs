using Microsoft.SemanticKernel;
using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel.Data;
using Microsoft.SemanticKernel.Connectors.Qdrant;
using _7.VectorDb;
using System.Reflection;


// Load configuration from config.json
var configuration = new ConfigurationBuilder()
   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
   .AddUserSecrets(Assembly.GetExecutingAssembly())
   .Build();

// Retrieve the API key and model from environment variables
string? apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
string? model = Environment.GetEnvironmentVariable("OPENAI_MODEL");
if (string.IsNullOrEmpty(apiKey))
{
    throw new InvalidOperationException("API key is missing or null in the environment variables.");
}
if (string.IsNullOrEmpty(model))
{
    throw new InvalidOperationException("Model is missing or null in the environment variables.");
}

// Retrieve Qdrant and embedding configurations
string? embeddingModel = configuration["OpenAI:Embeddings:ModelId"];
string? embeddingApiKey = configuration["OpenAI:Embeddings:ApiKey"];
string? embeddingOrgId = configuration["OpenAI:Embeddings:OrgId"];

string? qdrantCollectionName = configuration["Qdrant:CollectionName"];
string? qdrantHost = configuration["Qdrant:Host"];
int qdrantPort = int.Parse(configuration["Qdrant:Port"]);
bool qdrantHttps = bool.Parse(configuration["Qdrant:Https"]);
string? qdrantApiKey = configuration["Qdrant:ApiKey"];


#pragma warning disable SKEXP0010 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
var kernel = Kernel.CreateBuilder()
                   .AddOpenAIChatCompletion(model, apiKey)
                   .AddOpenAITextEmbeddingGeneration(embeddingModel, embeddingApiKey, embeddingOrgId)
                   .AddQdrantVectorStoreRecordCollection<string, TextSnippet<string>>(
                       qdrantCollectionName,
                       qdrantHost,
                       qdrantPort,
                       qdrantHttps,
                       qdrantApiKey)
                   .Build();
#pragma warning restore SKEXP0010 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
