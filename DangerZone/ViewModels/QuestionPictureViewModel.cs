using Caliburn.Micro;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DangerZone.ViewModels
{
    public class QuestionPictureViewModel : Screen
    {
        private Visibility isRevealed = Visibility.Hidden;
        private ImageSource revealImage;

        public Visibility IsRevealed
        {
            get => isRevealed;
            set
            {
                isRevealed = value;
                NotifyOfPropertyChange();
            }
        }

        public ImageSource RevealImage
        {
            get => revealImage;
            set
            {
                revealImage = value;
                NotifyOfPropertyChange();
            }
        }

        public void LoadImage(string fileName)
        {
            var image = new BitmapImage();

            using var stream = new FileStream(fileName, FileMode.Open);
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.StreamSource = stream;
            image.EndInit();
            RevealImage = image;
        }

        public void Reveal()
            => IsRevealed = Visibility.Visible;
    }
}
