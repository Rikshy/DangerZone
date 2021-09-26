using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DangerZone.Models
{
    public class Zone
    {
        public Zone()
            => Categories = new ObservableCollection<Category>();

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("icon")]
        public string IconPath { get; set; }

        [JsonProperty("cats"), JsonRequired]
        public ObservableCollection<Category> Categories { get; set; }

        [JsonIgnore]
        public bool IsValid { get; private set; }

        [JsonIgnore]
        public bool IsCompleted => Categories.All(c => c.Questions.All(q => q.IsCompleted));

        [JsonIgnore]
        public ImageSource Icon
        {
            get
            {
                var image = new BitmapImage();

                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                if (string.IsNullOrEmpty(IconPath))
                {
                    image.UriSource = new Uri("pack://application:,,,/DangerZone;component/Resources/defaultzone.png");
                }
                else
                {
                    using var stream = new FileStream(IconPath, FileMode.Open);
                    image.StreamSource = stream;
                }
                image.EndInit();

                return image;
            }
        }

        public bool Validate(out List<string> errors)
        {
            errors = new();

            if (!Categories.Any())
            {
                errors.Add(">No Catergories Found");
            }
            else
            {
                var missinFiles = new List<string>();
                foreach (var cat in Categories)
                {
                    if (cat.Questions.Any())
                    {
                        foreach (var q in cat.Questions)
                        {
                            if (q.Type != QType.Text)
                            {
                                if (!File.Exists(q.Content))
                                    missinFiles.Add(q.Content);
                            }
                        }
                    }
                    else
                    {
                        errors.Add($"> Category {cat.Name} has no questions");
                    }
                }

                if (missinFiles.Any())
                {
                    errors.Add($">Missing Files:");
                    errors.AddRange(missinFiles);
                }
            }

            if (errors.Any())
            {
                errors.Insert(0, $"-- | {Name} | -- ");
                errors.Insert(1, Environment.NewLine);
            }

            IsValid = !errors.Any();
            return IsValid;
        }
    }
}
