using Azure;
using Azure.AI.OpenAI;
using AzureOpenAi.Speech;
using OpenAI;
using OpenAI.Chat;
using static System.Net.Mime.MediaTypeNames;

namespace AzureOpenAi
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var azureSpeech = new AzureSpeech();
            string text = string.Empty;

            do
            {
                text = Console.ReadLine();
                var response = await GetConversationResponse(text);

                if (response.Content.Count > 0)
                {
                    var  completionText = response.Content[0].Text;
                    Console.WriteLine(completionText);
                    await azureSpeech.SpeakAsync(completionText);
                }

                Console.WriteLine();

            }
            while (!string.IsNullOrEmpty(text));

        }


        public static async Task<ChatCompletion> GetConversationResponse(string userMessage)
        {
            AppSettings settings = new AppSettings();
            AzureOpenAIClient azureClient = new(
                new Uri(settings.EndpointUrl),
                new AzureKeyCredential(settings.OpenAiKey));

            ChatClient chatClient = azureClient.GetChatClient(settings.DeploymentName);

            ChatCompletion completion = await chatClient.CompleteChatAsync(
    [
        new SystemChatMessage(settings.SystemBehaviourMessage2),
        new UserChatMessage("Hi, can you help me?"),
        new AssistantChatMessage(userMessage),
        new UserChatMessage(userMessage),
    ]);


            return completion;
        }
    }
}
