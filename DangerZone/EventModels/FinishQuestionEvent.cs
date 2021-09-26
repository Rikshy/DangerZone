using DangerZone.Models;

namespace DangerZone.EventModels
{
    public class FinishQuestionEvent
    {
        public FinishQuestionEvent(Question question)
            => Question = question;

        public Question Question { get; }
    }
}
