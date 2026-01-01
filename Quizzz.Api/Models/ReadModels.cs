using System.ComponentModel.DataAnnotations;

namespace Quizzz.Api.Models;

public class ParticipantListItem
{
    [Required]
    public string Name { get; set; }
}

public class QuizRunViewModel
{
   public long QuizRunId { get; set; }
    public long QuizId { get; set; }
    public long? CurrentQuestionId { get; set; }
    public List<ParticipantListItem> Participants { get; set; } = new List<ParticipantListItem>();
}
