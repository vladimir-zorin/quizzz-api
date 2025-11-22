namespace Quizzz.Api.Models;

public class Quiz
{
    public long QuizId { get; set; }
    public string Name { get; set; }
    public List<Question> Questions { get; set; } = new List<Question>();
}

public class Question
{
    public long QuestionId { get; set; }
    public string Text { get; set; }
    public List<Answer> Answers { get; set; } = new List<Answer>();
}

public class Answer
{
    public long AnswerId { get; set; }
    public string Text { get; set; }
    public bool IsCorrect { get; set; }
}