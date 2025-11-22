using Microsoft.AspNetCore.Mvc;
using Quizzz.Api.Models;
using Quizzz.Api.Repositories;

namespace Quizzz.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class QuizController : Controller
{
    private readonly IQuizRepository _quizRepository;
    private readonly ILogger<QuizController> _logger;

    public QuizController(
        IQuizRepository quizRepository,
        ILogger<QuizController> logger)
    {
        _logger = logger;
        _quizRepository = quizRepository;
    }

    [HttpGet]
    public IReadOnlyList<Quiz> Get()
    {
        return _quizRepository.GetQuizzes();
    }

    [HttpGet("{id}")]
    public ActionResult<Quiz> GetQuizById(
        [FromRoute] long id)
    {
        var quiz = _quizRepository.GetQuizById(id);
        if (quiz == null)
        {
            return NotFound("Cannot find quiz");
        }

        return Ok(quiz);
    }
}
