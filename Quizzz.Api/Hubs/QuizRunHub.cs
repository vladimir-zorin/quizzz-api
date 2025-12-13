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

    ////public async Task JoinQuizRun(int quizRunId, string participantName)
    ////{
    ////    var groupName = GetQuizRunGroupName(quizRunId);
    ////    var participant = new Models.Participant
    ////    {
    ////        Name = participantName
    ////    };

    ////    var allParticipants = _quizRunRepository.AddParticipant(participant, quizRunId);

    ////    await Clients.Group(groupName).SendAsync("ParticipantJoined", allParticipants);
    ////}

    ////public async Task StartQuiz(int quizRunId)
    ////{
    ////    var groupName = GetQuizRunGroupName(quizRunId);
    ////    await Clients.Group(groupName).SendAsync("QuizStarted", new
    ////    {
    ////        startedAt = DateTime.UtcNow
    ////    });
    ////}

    ////public async Task AdvanceQuestion(int quizRunId, int nextQuestionId)
    ////{
    ////    var groupName = GetQuizRunGroupName(quizRunId);
    ////    await Clients.Group(groupName).SendAsync("QuestionAdvanced", new
    ////    {
    ////        questionId = nextQuestionId,
    ////        advancedAt = DateTime.UtcNow
    ////    });
    ////}

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