using DangerZone.Models;
using System.Windows.Media.Imaging;

namespace DangerZone.EventModels
{
    public class AddPlayerEvent
    {
        public AddPlayerEvent(string name, BitmapImage icon)
            => Player = new Player { Name = name, Icon = icon };

        public Player Player { get; }
    }
}
