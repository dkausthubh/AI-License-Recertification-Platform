/* using Azure;
using Azure.Identity;
using Azure.AI.Projects;
using Azure.AI.Agents.Persistent;

 public async Task<string> GetRecommendationAsync(string userId, string appId, int requestId)
{
    // Get usage data from Databricks
    var usageData = await _databricksRepo.GetUserAccessStatsAsync(userId, appId, requestId);

    var payload = new
    {
        input = new
        {
            userId,
            appId,
            requestId,
            accessStats = usageData
        }
    };
}
async Task RunAgentConversation()
    {
        var endpoint = new Uri("https://bh-in-openai-aireimagin-resource.services.ai.azure.com/api/projects/bh-in-openai-aireimagin-project");
        AIProjectClient projectClient = new(endpoint, new DefaultAzureCredential());

        PersistentAgentsClient agentsClient = projectClient.GetPersistentAgentsClient();

        PersistentAgent agent = agentsClient.Administration.GetAgent("asst_TOYNhU4klwFGcozqPTWZMyrB");

        PersistentAgentThread thread = agentsClient.Threads.GetThread("thread_vjDx4W7CbSPIym3gmJXMZQKW");

        PersistentThreadMessage messageResponse = agentsClient.Messages.CreateMessage(
            thread.Id,
            MessageRole.User,
            "Get the confidence score for this user_id=  U_10004533 and application_id =App_53a4afd1
    ");
    

        ThreadRun run = agentsClient.Runs.CreateRun(
            thread.Id,
            agent.Id);

        // Poll until the run reaches a terminal status
        do
        {
            await Task.Delay(TimeSpan.FromMilliseconds(500));
            run = agentsClient.Runs.GetRun(thread.Id, run.Id);
        }
        while (run.Status == RunStatus.Queued
            || run.Status == RunStatus.InProgress);
        if (run.Status != RunStatus.Completed)
        {
            throw new InvalidOperationException($"Run failed or was canceled: {run.LastError?.Message}");
        }

        Pageable<PersistentThreadMessage> messages = agentsClient.Messages.GetMessages(
            thread.Id, order: ListSortOrder.Ascending);

        // Display messages
        foreach (PersistentThreadMessage threadMessage in messages)
        {
            Console.Write($"{threadMessage.CreatedAt:yyyy-MM-dd HH:mm:ss} - {threadMessage.Role,10}: ");
            foreach (MessageContent contentItem in threadMessage.ContentItems)
            {
                if (contentItem is MessageTextContent textItem)
                {
                    Console.Write(textItem.Text);
                }
                else if (contentItem is MessageImageFileContent imageFileItem)
                {
                    Console.Write($"<image from ID: {imageFileItem.FileId}");
                }
                Console.WriteLine();
            }
        }
    }

// Main execution
await RunAgentConversation(); */