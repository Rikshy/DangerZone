using Caliburn.Micro;
using DangerZone.EventModels;
using DangerZone.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace DangerZone.ViewModels
{
    public class ShellViewModel : Conductor<object>, IScreen, IHandle<ZoneSelectionEvent>, IHandle<StartGameEvent>, IHandle<AddPlayerEvent>, IHandle<CloseScreenEvent>, IHandle<QuestionClickedEvent>, IHandle<CancelQuestionEvent>, IHandle<FinishQuestionEvent>
    {
        private readonly SimpleContainer container;

        private bool canAddPlayer;

        private Zone activeZone;

        public ShellViewModel(IEventAggregator events, SimpleContainer container)
        {
            this.container = container;
            events.SubscribeOnPublishedThread(this);
            CanAddPlayer = true;
            ActivateItemAsync(container.GetInstance<LogoViewModel>());
        }

        public ObservableCollection<Player> Players { get; set; } = new ObservableCollection<Player>();

        public WindowProperties WinProps { get; } = new();

        public bool CanAddPlayer
        {
            get => canAddPlayer;
            set
            {
                canAddPlayer = value;
                NotifyOfPropertyChange();
            }
        }

        private bool questionActive;

        public bool QuestionActive
        {
            get => questionActive;
            set
            {
                questionActive = value;
                NotifyOfPropertyChange();
            }
        }


        public void Reset()
        {
            if (MessageBox.Show("Do you really want to reset the board?", "Reset?", MessageBoxButton.YesNo) == MessageBoxResult.No)
                return;

            QuestionActive = false;
            CanAddPlayer = true;
            ActivateItemAsync(container.GetInstance<LogoViewModel>());
            activeZone = null;

            if (Players.Any() && MessageBox.Show("Also clear players?", "Clear Players?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                Players.Clear();
            else
            {
                foreach (var player in Players)
                {
                    player.Points = 0;
                    player.Participation = Participation.NoParticipation;
                }
            }
        }

        public void AddPlayer()
            => ActivateItemAsync(container.GetInstance<AddPlayerViewModel>());

        private async Task OpenDangerZone()
        {
            QuestionActive = false;
            var vm = container.GetInstance<DangerZoneViewModel>();
            vm.WinProps = WinProps;
            vm.Cats = activeZone.Categories;
            await ActivateItemAsync(vm);
        }

        public override Task<bool> CanCloseAsync(CancellationToken cancellationToken = default)
        {
            var t = Task.Run(() =>
            {
                bool ret = true;
                OnUIThread(() =>
                {
                    if (activeZone is not null && !activeZone.IsCompleted)
                    {
                        ret = MessageBox.Show("Do you really want to close your active game?", "Do you really want to close me?", MessageBoxButton.YesNo) == MessageBoxResult.Yes;

                    }
                });

                return ret;
            });
            return t;
        }

        public async Task HandleAsync(AddPlayerEvent message, CancellationToken cancellationToken)
        {
            Players.Add(message.Player);

            await ActivateItemAsync(container.GetInstance<LogoViewModel>(), cancellationToken);
        }

        public async Task HandleAsync(StartGameEvent message, CancellationToken cancellationToken)
        {
            if (Players.Any())
            {
                CanAddPlayer = false;
                activeZone = message.Zone;
                await OpenDangerZone();
            }
            else
            {
                MessageBox.Show("Please add atleast 1 player!", "No one played your game");

                await ActivateItemAsync(container.GetInstance<LogoViewModel>(), cancellationToken);
            }
        }

        public async Task HandleAsync(CloseScreenEvent message, CancellationToken cancellationToken)
            => await ActivateItemAsync(container.GetInstance<LogoViewModel>(), cancellationToken);

        public async Task HandleAsync(ZoneSelectionEvent message, CancellationToken cancellationToken)
            => await ActivateItemAsync(container.GetInstance<ZoneSelectionViewModel>(), cancellationToken);

        public async Task HandleAsync(QuestionClickedEvent message, CancellationToken cancellationToken)
        {
            QuestionActive = true;
            var vm = container.GetInstance<QuestionShellViewModel>();
            vm.Players = Players;
            vm.WinProps = WinProps;
            vm.Question = message.Question;
            await ActivateItemAsync(vm, cancellationToken);
        }

        public async Task HandleAsync(FinishQuestionEvent message, CancellationToken cancellationToken)
        {
            message.Question.IsCompleted = true;
            foreach (var player in Players)
            {
                switch (player.Participation)
                {
                    case Participation.Right:
                        player.Points += message.Question.Value;
                        break;
                    case Participation.Wrong:
                        player.Points -= message.Question.Value;
                        break;
                    case Participation.NoParticipation:
                    default:
                        break;
                }

                player.Participation = Participation.NoParticipation;
            }

            if (activeZone.IsCompleted)
            {
                var vm = container.GetInstance<ResultViewModel>();
                vm.Players = Players;
                vm.WinProps = WinProps;
                await ActivateItemAsync(vm, cancellationToken);
            }
            else
            {
                await OpenDangerZone();
            }
        }

        public async Task HandleAsync(CancelQuestionEvent message, CancellationToken cancellationToken)
            => await OpenDangerZone();
    }
}
