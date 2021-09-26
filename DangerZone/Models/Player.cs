using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace DangerZone.Models
{
    public class Player : INotifyPropertyChanged
    {

        private int points;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Name { get; set; }

        public ImageSource Icon { get; set; }

        public int Points
        {
            get => points;
            set
            {
                points = value;
                NotifyOfPropertyChange();
            }
        }

        public Participation Participation { get; set; }

        protected virtual void NotifyOfPropertyChange([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public enum Participation { NoParticipation, Right, Wrong }
}
