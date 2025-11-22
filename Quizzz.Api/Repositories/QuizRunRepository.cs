using Microsoft.Extensions.Caching.Memory;
using Quizzz.Api.Models;

namespace Quizzz.Api.Repositories;

public class QuizRunRepository : IQuizRunRepository
{
    private readonly IMemoryCache _memoryCache;

    public QuizRunRepository(
        IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public QuizRun? GetQuizRun(long id)
    {
        var key = GetQuizRunKey(id);
        return _memoryCache.Get<QuizRun>(key);
    }

    public long SaveQuizRun(QuizRun quizRun)
    {
        var currId = GetCurrentQuizRunId();
        currId++;
        quizRun.QuizRunId = currId;
        var key = GetQuizRunKey(currId);
        _memoryCache.Set(key, quizRun);
        SetCurrentQuizRunId(currId);
        return currId;
    }

    public void AddParticipant(Participant participant, long quizRunId)
    {
        var quizRun = GetQuizRun(quizRunId);
        if (quizRun == null)
        {
            throw new Exception($"Cannot find quizrun with id {quizRunId}");
        }

        if (quizRun.Participants.Any(p => p.Name == participant.Name))
        {
            throw new Exception($"Participant with name '{participant.Name}' already exists");
        }

        quizRun.Participants.Add(participant);
    }

    private long GetCurrentQuizRunId()
    {
        var key = GetQuizRunCurrIdKey();
        if (!_memoryCache.TryGetValue<long>(key, out var currId))
        {
            currId = 0;
            _memoryCache.Set(key, currId);
        }

        return currId;
    }

    private void SetCurrentQuizRunId(long id)
    {
        var key = GetQuizRunCurrIdKey();
        _memoryCache.Set(key, id);
    }

    private string GetQuizRunCurrIdKey()
        => $"{nameof(QuizRun)}_currentId";

    private string GetQuizRunKey(long id)
        => $"{nameof(QuizRun)}_${id}";
}

public interface IQuizRunRepository
{
    QuizRun? GetQuizRun(long id);
    long SaveQuizRun(QuizRun quizRun);
    void AddParticipant(Participant participant, long quizRunId);
}