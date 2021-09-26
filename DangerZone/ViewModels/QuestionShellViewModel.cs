using Caliburn.Micro;
using DangerZone.EventModels;
using DangerZone.Models;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace DangerZone.ViewModels
{
    public class QuestionShellViewModel : Conductor<object>, IScreen
    {
        private readonly IEventAggregator events;
        private readonly SimpleContainer container;
        public QuestionShellViewModel(IEventAggregator events, SimpleContainer container)
        {
            this.container = container;
            this.events = events;
        }
        public ObservableCollection<Player> Players { get; set; } = new ObservableCollection<Player>();

        public WindowProperties WinProps { get; set; }

        public string QuestionName { get; set; }

        public Question Question { get; set; }

        protected override async Task OnInitializeAsync(CancellationToken cancellationToken)
        {
            switch (Question.Type)
            {
                case QType.Text:
                    {
                        QuestionName = "!Text Zone!";
                        var vm = container.GetInstance<QuestionTextViewModel>();
                        vm.Text = Question.Content;
                        vm.WinProps = WinProps;
                        await ActivateItemAsync(vm, cancellationToken);
                    }
                    break;
                case QType.Picture:
                    {
                        QuestionName = "!Picture Zone!";
                        var vm = container.GetInstance<QuestionPictureViewModel>();
                        vm.LoadImage(Question.Content);
                        await ActivateItemAsync(vm, cancellationToken);
                    }
                    break;
                case QType.Audio:
                    {
                        QuestionName = "!Sound Zone!";
                        var vm = container.GetInstance<QuestionAudioViewModel>();
                        vm.LoadAudio(Question.Content);
                        await ActivateItemAsync(vm, cancellationToken);
                    }
                    break;
                case QType.Video:
                    {
                        QuestionName = "!Video Zone!";
                        var vm = container.GetInstance<QuestionVideoViewModel>();
                        vm.VideoSource = new Uri(Question.Content, UriKind.RelativeOrAbsolute);
                        await ActivateItemAsync(vm, cancellationToken);
                    }
                    break;
                case QType.List:
                    {
                        QuestionName = "!Category Zone!";
                        var vm = container.GetInstance<QuestionListViewModel>();
                        vm.Init(Question.Content, WinProps.UISize);
                        await ActivateItemAsync(vm, cancellationToken);
                    }
                    break;
                default:
                    break;
            }

            await base.OnInitializeAsync(cancellationToken);
        }

        public void Accept() => events.PublishOnUIThreadAsync(new FinishQuestionEvent(Question));

        public void Cancel() => events.PublishOnUIThreadAsync(new CancelQuestionEvent());
    }
}
