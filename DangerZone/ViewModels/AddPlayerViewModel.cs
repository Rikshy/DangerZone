using Caliburn.Micro;
using DangerZone.EventModels;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DangerZone.ViewModels
{
    public class AddPlayerViewModel : Screen
    {
        private readonly IEventAggregator events;

        public AddPlayerViewModel(SimpleContainer container)
        {
            events = container.GetInstance<IEventAggregator>();

            Directory.CreateDirectory("images");
            foreach (string f in Directory.EnumerateFiles("images").Where(file => file.ToLower().EndsWith("png") || file.ToLower().EndsWith("jpeg")))
            {
                var image = new BitmapImage();

                using var stream = new FileStream(f, FileMode.Open);
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = stream;
                image.EndInit();

                PlayerIcons.Add(image);
            }
        }

        public ObservableCollection<ImageSource> PlayerIcons { get; } = new();

        public string PlayerName { get; set; }

        public ImageSource SelectedIcon { get; set; }

        public void AddImage()
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg";
            ofd.InitialDirectory = Path.Combine(Environment.CurrentDirectory, "images");
            if (ofd.ShowDialog() == true)
            {
                foreach (string f in ofd.FileNames)
                {
                    string destFileName = Path.Combine( "images", Path.GetFileName( f ) );
                    if (File.Exists(destFileName))
                        MessageBox.Show("A file with this name already exists!", "RIP", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    else
                    {
                        File.Copy(f, destFileName);
                        PlayerIcons.Add(new BitmapImage(new Uri(f)));
                    }
                }
            }
        }

        public void Done()
        {
            if (SelectedIcon == null || string.IsNullOrEmpty(PlayerName))
            {
                MessageBox.Show("Please select an image and type in your desired name", "RIP", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            events.PublishOnUIThreadAsync(new AddPlayerEvent(PlayerName, (BitmapImage)SelectedIcon));
        }

        public void Cancel()
            => events.PublishOnUIThreadAsync(new CloseScreenEvent());
    }
}
