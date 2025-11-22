using Quizzz.Api.Models;

namespace Quizzz.Api.Repositories;

public class QuizRepository : IQuizRepository
{
    private static Quiz[] Quizzes = [
            new Quiz
            {
                QuizId = 1,
                Name = "My First Quiz",
                Questions = [
                    new Question
                    {
                        Text = "Hva er hovedstaden i Norge?",
                        Answers = [
                            new Answer { Text = "Oslo", IsCorrect = true },
                            new Answer { Text = "Bergen", IsCorrect = false },
                            new Answer { Text = "Stavanger", IsCorrect = false },
                            new Answer { Text = "Trondheim", IsCorrect = false },
                        ]
                    },
                    new Question
                    {
                        Text = "Hva er best: hunder eller katter?",
                        Answers = [
                            new Answer { Text = "Katter", IsCorrect = true },
                            new Answer { Text = "Hunder", IsCorrect = false },
                        ]
                    }
                ]
            }
        ];

    public IReadOnlyList<Quiz> GetQuizzes()
    {
       return Quizzes;
    }

    public Quiz? GetQuizById(long id)
    {
        return Quizzes.FirstOrDefault(q => q.QuizId == id);
    }
}

public interface IQuizRepository
{
    IReadOnlyList<Quiz> GetQuizzes();
    Quiz? GetQuizById(long id);
}