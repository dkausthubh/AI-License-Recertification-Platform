/* using Azure;
using Azure.AI.Agents.Persistent;
using Azure.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
 
public class AgentService
{
    public async Task<string> GetAgentResponseAsync(string userMessage)
    {
        var projectEndpoint = "https://bh-in-openai-aireimagin-resource.services.ai.azure.com/api/projects/bh-in-openai-aireimagin-project";
        var agentId = "asst_TOYNhU4klwFGcozqPTWZMyrB"; // ID of the existing agent

        // Create the Agent Client
        PersistentAgentsClient agentClient = new(projectEndpoint, new DefaultAzureCredential());

        // Create a thread for the conversation
        PersistentAgentThread thread = await agentClient.Threads.CreateThreadAsync();

        // Send a message to the existing agent
        PersistentThreadMessage message = await agentClient.Messages.CreateMessageAsync(
            thread.Id,
            MessageRole.User,
            userMessage // Use the userMessage parameter
        );

        // Run the existing agent with the created thread
        ThreadRun run = await agentClient.Runs.CreateRunAsync(thread.Id, agentId);

        // Wait for the agent to complete and check the output
        do
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(500));
            run = await agentClient.Runs.GetRunAsync(thread.Id, run.Id);
        }
        while (run.Status == RunStatus.Queued || run.Status == RunStatus.InProgress);

        // Retrieve the output messages from the thread
        var messages = await agentClient.Messages.GetMessagesAsync(thread.Id);

        // Find the last message from the agent
        var agentMessage = messages.LastOrDefault(m => m.Role == MessageRole.Agent);

        // Return the content of the agent's message
        return agentMessage?.Content ?? "No response from the agent.";
    }
} */