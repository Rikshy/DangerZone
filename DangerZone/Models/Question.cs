using Caliburn.Micro;
using DangerZone.EventModels;
using DangerZone.Helper;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace DangerZone.Models
{
    public class Question : INotifyPropertyChanged
    {
        private bool isCompleted;

        public event PropertyChangedEventHandler PropertyChanged;

        public Question()
            => Activated = new LightCommand(() => IoC.Get<IEventAggregator>().PublishOnUIThreadAsync(new QuestionClickedEvent(this)));


        [JsonProperty("content"), JsonRequired]
        public string Content { get; set; }

        [JsonProperty("value"), JsonRequired]
        public int Value { get; set; }

        [JsonProperty("type"), JsonConverter(typeof(StringEnumConverter)), JsonRequired]
        public QType Type { get; set; }

        [JsonIgnore]
        public bool IsCompleted
        {
            get => isCompleted;
            set
            {
                isCompleted = value;
                OnPropertyChanged();
            }
        }

        [JsonIgnore]
        public ICommand Activated { get; }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
