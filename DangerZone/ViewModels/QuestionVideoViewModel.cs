using Caliburn.Micro;
using DangerZone.Helper;
using System;
using System.Windows;
using System.Windows.Controls;

namespace DangerZone.ViewModels
{
    public class QuestionVideoViewModel : Screen
    {
        private bool isPaused;
        private Visibility isRevealed;

        public MediaElement MediaPlayer { get; set; }
        public Visibility IsRevealed
        {
            get => isRevealed;
            set
            {
                isRevealed = value;
                NotifyOfPropertyChange();
            }
        }

        private Uri videoSource;

        public Uri VideoSource
        {
            get => videoSource;
            set
            {
                videoSource = value;
                NotifyOfPropertyChange();
            }
        }

        public IResult Play()
        {
            var play = new PlayMediaResult(isPaused);
            isPaused = false;

            return play;
        }

        public IResult Pause()
        {
            isPaused = true;
            return new PauseMediaResult();
        }

        public IResult Stop()
        {
            isPaused = false;
            return new StopMediaResult();
        }

        public class PlayMediaResult : IResult
        {
            private readonly bool isPaused;

            public PlayMediaResult(bool isPaused)
            {
                this.isPaused = isPaused;
            }

            public void Execute(CoroutineExecutionContext context)
            {
                var view = context.View as FrameworkElement;
                var mediaElement = WpfHelper.FindVisualChild<MediaElement>(view);

                if (mediaElement != null)
                {
                    if (!isPaused)
                        mediaElement.Position = TimeSpan.FromMilliseconds(1);
                    mediaElement.Play();
                }

                Completed?.Invoke(this, new ResultCompletionEventArgs());
            }

            public event EventHandler<ResultCompletionEventArgs> Completed;
        }

        public class StopMediaResult : IResult
        {
            public void Execute(CoroutineExecutionContext context)
            {
                var view = context.View as FrameworkElement;
                var mediaElement = WpfHelper.FindVisualChild<MediaElement>(view);

                if (mediaElement != null)
                {
                    mediaElement.Stop();
                }

                Completed?.Invoke(this, new ResultCompletionEventArgs());
            }

            public event EventHandler<ResultCompletionEventArgs> Completed;
        }

        public class PauseMediaResult : IResult
        {
            public void Execute(CoroutineExecutionContext context)
            {
                var view = context.View as FrameworkElement;
                var mediaElement = WpfHelper.FindVisualChild<MediaElement>(view);

                if (mediaElement != null)
                {
                    mediaElement.Pause();
                }

                Completed?.Invoke(this, new ResultCompletionEventArgs());
            }

            public event EventHandler<ResultCompletionEventArgs> Completed;
        }
    }
}
