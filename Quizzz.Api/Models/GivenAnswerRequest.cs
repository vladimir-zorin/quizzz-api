namespace Quizzz.Api.Models;

public class GivenAnswerRequest
{
    public long QuizRunId { get; set; }
    public string ParticipantName { get; set; }
    public long QuestionId { get; set; }
    public long AnswerId { get; set; }
}
