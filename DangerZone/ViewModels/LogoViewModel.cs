using Caliburn.Micro;
using DangerZone.EventModels;

namespace DangerZone.ViewModels
{
    public class LogoViewModel : Screen
    {
        private readonly IEventAggregator events;

        public LogoViewModel(IEventAggregator events)
            => this.events = events;

        public void StartGame()
            => events.PublishOnUIThreadAsync(new ZoneSelectionEvent());
    }
}
