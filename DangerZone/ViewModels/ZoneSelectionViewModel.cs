using Caliburn.Micro;
using DangerZone.EventModels;
using DangerZone.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace DangerZone.ViewModels
{
    public class ZoneSelectionViewModel : Screen
    {
        private readonly IEventAggregator events;

        public ZoneSelectionViewModel(IEventAggregator events)
            => this.events = events;

        public ObservableCollection<Zone> Zones { get; } = new();

        protected override Task OnInitializeAsync(CancellationToken cancellationToken)
        {
            Zones.Clear();
            LoadZones().ForEach(z => Zones.Add(z));
            return base.OnInitializeAsync(cancellationToken);
        }

        private List<Zone> LoadZones()
        {
            var zones = new List<Zone>();
            var errors = new List<string>();
            var di = Directory.CreateDirectory("zones");
            foreach (var fi in di.GetFiles("*.jz"))
            {
                try
                {
                    var z = JsonConvert.DeserializeObject<Zone>(File.ReadAllText(fi.FullName));
                    if (z.Validate(out var validErrors))
                    {
                        zones.Add(z);
                    }
                    else
                    {
                        if (errors.Any())
                        {
                            errors.Add(Environment.NewLine);
                        }

                        errors.AddRange(validErrors);
                    }
                }
                catch (Exception ex)
                {
                    if (errors.Any())
                    {
                        errors.Add(Environment.NewLine);
                    }

                    errors.Add($"-- | {fi.Name} | -- ");
                    errors.Add(Environment.NewLine);
                    errors.Add(">Parsing Error:");
                    errors.Add(ex.Message);
                }
            }

            if (errors.Any())
                OnUIThread(() => MessageBox.Show(string.Join(Environment.NewLine, errors), "Loading Errors"));

            return zones;
        }

        public void ZoneSelect(Zone zone)
            => events.PublishOnUIThreadAsync(new StartGameEvent(zone));
    }
}
