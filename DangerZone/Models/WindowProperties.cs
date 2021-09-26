using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DangerZone.Models
{
    public class WindowProperties : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private UISize uiSize = UISize.Medium;
        public UISize UISize
        {
            get => uiSize;
            set
            {
                uiSize = value;
                NotifyOfPropertyChange();
            }
        }
        protected virtual void NotifyOfPropertyChange([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
