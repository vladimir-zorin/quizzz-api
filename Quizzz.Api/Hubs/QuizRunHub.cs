using Microsoft.AspNetCore.SignalR;
using Quizzz.Api.Repositories;

namespace Quizzz.Api.Hubs;

public class QuizRunHub : Hub
{
    private readonly IQuizRunRepository _quizRunRepository;

    public QuizRunHub(
        IQuizRunRepository quizRunRepository)
    {
        _quizRunRepository = quizRunRepository;
    }

    public async Task AddListener(long quizRunId)
    {
        var groupName = GetQuizRunGroupName(quizRunId);
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        // Clean up connections
        ////var entriesToRemove = QuizRunConnections
        ////    .Where(kvp => kvp.Value.Count == 0)
        ////    .Select(kvp => kvp.Key)
        ////    .ToList();

        ////foreach (var key in entriesToRemove)
        ////{
        ////    QuizRunConnections.Remove(key);
        ////}

        await base.OnDisconnectedAsync(exception);
    }

    public static string GetQuizRunGroupName(long quizRunId) => $"quizrun_{quizRunId}";
}