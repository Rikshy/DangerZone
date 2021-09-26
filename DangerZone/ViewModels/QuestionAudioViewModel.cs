using Caliburn.Micro;
using System;
using System.Windows.Media;
using System.Windows.Threading;

namespace DangerZone.ViewModels
{
    public class QuestionAudioViewModel : Screen
    {
        private readonly MediaPlayer mediaPlayer = new();
        private string statusText;

        public QuestionAudioViewModel()
        {
            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += delegate
            {
                StatusText = mediaPlayer.Source != null
                    ? $"{mediaPlayer.Position:mm\\:ss} / {mediaPlayer.NaturalDuration.TimeSpan:mm\\:ss}"
                    : "No file selected...";
            };

            timer.Start();
        }

        public string StatusText
        {
            get => statusText;
            set
            {
                statusText = value;
                NotifyOfPropertyChange();
            }
        }

        public void LoadAudio(string f)
            => mediaPlayer.Open(new Uri(f, UriKind.RelativeOrAbsolute));

        public void Play()
            => mediaPlayer.Play();
        public void Pause()
            => mediaPlayer.Pause();
        public void Stop()
            => mediaPlayer.Stop();
    }
}
