using System.ComponentModel.DataAnnotations;

namespace Quizzz.Api.Models;

public class QuizRun
{
    public long QuizRunId { get; set; }
    public long QuizId { get; set; }
    public long? CurrentQuestionId { get; set; }
    public List<Participant> Participants { get; set; } = new List<Participant>();
}

public class Participant
{
    [Required]
    public string Name { get; set; }

    public List<GivenAnswer> GivenAnswers { get; set; } = new List<GivenAnswer>();
}

public class GivenAnswer
{
    /// <summary>
    /// Id of <see cref="Question"/> which was answered
    /// </summary>
    public long QuestionId { get; set; }

    /// <summary>
    /// Whether the question was answered correctly
    /// </summary>
    public bool IsCorrect { get; set; }
}