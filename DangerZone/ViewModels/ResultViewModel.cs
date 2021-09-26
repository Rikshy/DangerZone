using Caliburn.Micro;
using DangerZone.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DangerZone.ViewModels
{
    public class ResultViewModel : Screen
    {
        public string WinnerText { get; set; }

        public ImageSource WinnerIcon { get; set; }

        public WindowProperties WinProps { get; set; }

        public ObservableCollection<Player> Players { get; set; }

        protected override Task OnInitializeAsync(CancellationToken cancellationToken)
        {
            var winner = Players.OrderByDescending(p => p.Points).First();

            WinnerText = $"!{winner.Name} won with {winner.Points} Points!";
            WinnerIcon = winner.Icon;

            return base.OnInitializeAsync(cancellationToken);
        }
    }
}
