using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DangerZone.Models
{
    public class Category : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Category()
            => Questions = new ObservableCollection<Question>();

        private string name;
        [JsonProperty("name"), JsonRequired]
        public string Name
        {
            get => name;
            set
            {
                name = value;
                NotifyOfPropertyChange();
            }
        }

        [JsonProperty("qs"), JsonRequired]
        public ObservableCollection<Question> Questions { get; set; }

        public override string ToString() => Name;

        protected virtual void NotifyOfPropertyChange([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
