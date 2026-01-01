using Quizzz.Api.Models;

namespace Quizzz.Api.Mappers;

public static class QuizRunMapper
{
    public static QuizRunViewModel ToViewModel(this QuizRun quizRun)
    {
        return new QuizRunViewModel
        {
            QuizRunId = quizRun.QuizRunId,
            QuizId = quizRun.QuizId,
            CurrentQuestionId = quizRun.CurrentQuestionId,
            Participants = quizRun.Participants
                .Select(p => p.ToListItem())
                .ToList()
        };
    }

    public static ParticipantListItem ToListItem(this Participant participant)
    {
        return new ParticipantListItem
        {
            Name = participant.Name
        };
    }   
}
