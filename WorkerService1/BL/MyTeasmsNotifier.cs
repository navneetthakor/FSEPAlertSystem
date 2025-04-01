//using Microsoft.Graph;
//using Microsoft.Graph.Models;
//using Microsoft.Graph.Users.Item.Chats.Item.Messages;
//using Azure.Identity;
//using System;
//using System.Threading.Tasks;
//using WorkerService1.Modal;

//namespace WorkerService1.BL
//{
//    internal class MyTeamsNotifier
//    {
//        public static async Task NotifyAsync(TeamsMessageContent msgContent)
//        {
//            try
//            {
//                GraphServiceClient graphServiceClient = CreateGraphServiceClient();

//                // Create a new chat message
//                var chatMessage = new ChatMessage
//                {
//                    Body = new ItemBody
//                    {
//                        Content = msgContent.MessageContent,
//                        ContentType = BodyType.Text
//                    }
//                };

//                // Find or create a chat with the user
//                var chat = await FindOrCreateChatWithUser(graphServiceClient, msgContent.MicrosoftId);

//                // Send message to the chat
//                await graphServiceClient.Chats[chat.Id].Messages
//                    .PostAsync(chatMessage);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Error sending Teams message: {ex.Message}");
//                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
//            }
//        }

//        private static GraphServiceClient CreateGraphServiceClient()
//        {
//            var clientId = Environment.GetEnvironmentVariable("AZURE_CLIENT_ID");
//            var clientSecret = Environment.GetEnvironmentVariable("AZURE_CLIENT_SECRET");
//            var tenantId = Environment.GetEnvironmentVariable("AZURE_TENANT_ID");

//            var options = new ClientSecretCredentialOptions
//            {
//                AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
//            };

//            var clientSecretCredential = new ClientSecretCredential(
//                tenantId,
//                clientId,
//                clientSecret,
//                options
//            );

//            return new GraphServiceClient(clientSecretCredential);
//        }

//        private static async Task<Chat> FindOrCreateChatWithUser(GraphServiceClient graphClient, string microsoftId)
//        {
//            try
//            {
//                // Try to find an existing chat with the user
//                var chatsRequest = graphClient.Chats
//                    .GetAsync(config =>
//                    {
//                        config.QueryParameters.Filter = $"members/any(m:m/userId eq '{microsoftId}')";
//                        config.QueryParameters.Expand = new[] { "members" };
//                    });

//                var chatsResponse = await chatsRequest;

//                if (chatsResponse?.Value != null && chatsResponse.Value.Count > 0)
//                {
//                    return chatsResponse.Value.First();
//                }

//                // If no existing chat, create a new one
//                var newChat = new Chat
//                {
//                    ChatType = ChatType.OneOnOne,
//                    Members = new List<ConversationMember>
//            {
//                new AadUserConversationMember
//                {
//                    OdataType = "#microsoft.graph.aadUserConversationMember",
//                    UserId = microsoftId,
//                    Roles = new List<string> { "owner" }
//                }
//            }
//                };

//                return await graphClient.Chats.PostAsync(newChat);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Error finding/creating chat: {ex.Message}");
//                throw;
//            }
//        }
//    }
//}