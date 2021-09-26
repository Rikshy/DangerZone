using Caliburn.Micro;
using DangerZone.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;

namespace DangerZone.ViewModels
{
    public class QuestionListViewModel : Screen
    {
        public class ListItem : INotifyPropertyChanged
        {
            public int Width { get; set; }
            public int Height { get; set; }
            public int FontSize { get; set; }
            public string Answer { get; set; }
            public int Id { get; set; }

            public string Text => Visible ? Answer : Id.ToString();
            public string BackColor => Visible ? "White" : "Black";
            public string ForeColor => Visible ? "Black" : "White";

            private bool visible;
            public bool Visible
            {
                get => visible;
                set
                {
                    visible = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Text"));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BackColor"));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ForeColor"));
                }
            }
            public event PropertyChangedEventHandler PropertyChanged;
        }

        public ObservableCollection<ListItem> ListItems { get; } = new();

        public void Init(string content, UISize uiSize)
        {
            string longest = string.Empty;
            string[] lines = File.ReadAllLines( content, Encoding.Default );
            foreach (string line in lines)
                longest = line.Length > longest.Length ? line : longest;

            int itemWidth = 0, itemHeight = 0;
            int fontSize = uiSize == UISize.Medium ? 12 : uiSize == UISize.Big ? 22 : 37;
            using (var graphics = Graphics.FromImage(new Bitmap(1, 1)))
            {
                var size = graphics.MeasureString( longest, new Font( "Segoe UI", fontSize, FontStyle.Regular, GraphicsUnit.Point ) );
                itemWidth = (int)size.Width - 25;
                itemHeight = (int)size.Height - (uiSize == UISize.Medium ? 4 : uiSize == UISize.Big ? 8 : 16);
            }

            for (int i = 0; i < lines.Length; i++)
                ListItems.Add(new ListItem { Id = i, Answer = lines[i], Width = itemWidth, Height = itemHeight, FontSize = fontSize });
        }

        public void ListItemClicked(ListItem item) => item.Visible = true;
    }
}
