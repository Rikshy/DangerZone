using Caliburn.Micro;
using DangerZone.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace DangerZone.ViewModels
{
    public class DangerZoneViewModel : Screen
    {
        private ObservableCollection<Category> cats;
        public ObservableCollection<Category> Cats
        {
            get => cats;
            set
            {
                cats = value;
                NotifyOfPropertyChange();
            }
        }

        public WindowProperties WinProps { get; set; }
    }
}
