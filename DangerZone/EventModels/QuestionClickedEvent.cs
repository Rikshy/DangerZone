using DangerZone.Models;

namespace DangerZone.EventModels
{
    public class QuestionClickedEvent
    {
        public QuestionClickedEvent(Question question)
            => Question = question;

        public Question Question { get; }
    }
}
