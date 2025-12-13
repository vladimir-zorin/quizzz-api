using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Quizzz.Api.Hubs;
using Quizzz.Api.Models;
using Quizzz.Api.Repositories;

namespace Quizzz.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuizRunController : Controller
    {
        private readonly IQuizRepository _quizRepository;
        private readonly IQuizRunRepository _quizRunRepository;
        private readonly IHubContext<QuizRunHub> _hubContext;

        public QuizRunController(
            IHubContext<QuizRunHub> hubContext,
            IQuizRepository quizRepository,
            IQuizRunRepository quizRunRepository)
        {
            _hubContext = hubContext;
            _quizRepository = quizRepository;
            _quizRunRepository = quizRunRepository;
        }

        /// <summary>
        /// Create a new quizrun
        /// </summary>
        [HttpPost("quiz/{quizId}/start")]
        public async Task<ActionResult<long>> StartQuizRun(
            [FromRoute] long quizId)
        {
            var quizRun = new QuizRun
            {
                QuizId = quizId,
            };

            var quizRunId = _quizRunRepository.SaveQuizRun(quizRun);
            var groupName = QuizRunHub.GetQuizRunGroupName(quizRunId);
            await _hubContext.Clients
                .Group(groupName)
                .SendAsync("QuizStarted", new { quizRunId });

            return Ok(quizRunId);
        }

        /// <summary>
        /// Get quizrun by id
        /// </summary>
        [HttpGet("{quizRunId}")]
        public ActionResult<QuizRun> GetQuizRun(
            [FromRoute] long quizRunId)
        {
            var quizRun = _quizRunRepository.GetQuizRun(quizRunId);
            if (quizRun == null)
            {
                return NotFound("Cannot find quizrun");
            }

            return Ok(quizRun);
        }

        /// <summary>
        /// Add participant to quizrun with given id
        /// </summary>
        [HttpPost("participant/{quizRunId}")]
        public async Task<ActionResult> AddParticipant(
            [FromRoute] long quizRunId,
            [FromBody] Participant participant)
        {
            var participants = _quizRunRepository.AddParticipant(participant, quizRunId);
            var groupName = QuizRunHub.GetQuizRunGroupName(quizRunId);
            await _hubContext.Clients
                .Group(groupName)
                .SendAsync("ParticipantJoined", participants);

            return Ok(participant);
        }

        /// <summary>
        /// Give answer to a question
        /// </summary>
        [HttpPost("answer")]
        public ActionResult AnswerQuestion(
            [FromBody] GivenAnswerRequest request)
        {
            var quizRun = _quizRunRepository.GetQuizRun(request.QuizRunId);
            if (quizRun == null)
            {
                return BadRequest($"cannot find quizrun with id {request.QuizRunId}");
            }

            var quiz = _quizRepository.GetQuizById(quizRun.QuizId);
            var question = quiz?.Questions.FirstOrDefault(q => q.QuestionId == request.QuestionId);
            if (question == null)
            {
                return BadRequest($"Cannot find question with id {request.QuestionId} on quiz");
            }

            var answer = question.Answers.FirstOrDefault(a => a.AnswerId == request.AnswerId);
            if (answer == null)
            {
                return BadRequest($"Cannot find answer with id {request.AnswerId} on question");
            }

            var participant = quizRun.Participants.FirstOrDefault(p => p.Name == request.ParticipantName);
            if (participant == null)
            {
                return BadRequest($"Cannot find participant {request.ParticipantName}");
            }

            participant.GivenAnswers.Add(
                new GivenAnswer
                {
                    IsCorrect = answer.IsCorrect,
                    QuestionId = request.QuestionId,
                });

            return Ok("answer submitted");
        }
    }
}
