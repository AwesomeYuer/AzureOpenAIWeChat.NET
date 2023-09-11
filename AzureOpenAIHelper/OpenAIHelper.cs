﻿namespace AzureOpenAI.Utilities
{
    using System;
    using System.Threading.Tasks;
    using Azure;
    using Azure.AI.OpenAI;

    public class OpenAI
    {
        public static async Task<string> GetOpenAIResultAsync(string inputText, Settings settings)
        {
            try
            {
                OpenAIClient openAIClient = new OpenAIClient(new Uri(settings.AzureEndpoint!), new AzureKeyCredential(settings.AzureKeyCredential!));

                var cts = new CancellationTokenSource(TimeSpan.FromSeconds(60));

                Response<ChatCompletions> responseWithoutStream =
                        await openAIClient
                                    .GetChatCompletionsAsync
                                            (
                                                settings.GptModel
                                                , new ChatCompletionsOptions
                                                {
                                                    Messages =
                                                        {
                                                            new ChatMessage(ChatRole.System, settings.System),
                                                            new ChatMessage(ChatRole.User, inputText)
                                                        },
                                                    Temperature = 0.7f,
                                                    MaxTokens = 800,
                                                    NucleusSamplingFactor = 0.95f,
                                                    FrequencyPenalty = 0f,
                                                    PresencePenalty = 0f
                                                }
                                                , cts.Token
                                            );
                _ = responseWithoutStream.Value;
                return responseWithoutStream.Value.Choices[0].Message.Content;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return e.ToString();
            }
        }
    }

}
