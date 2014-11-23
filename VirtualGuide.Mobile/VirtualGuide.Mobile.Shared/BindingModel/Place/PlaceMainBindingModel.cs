using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Text;
using VirtualGuide.Mobile.Model;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace VirtualGuide.Mobile.BindingModel
{
    [ImplementPropertyChanged]
    public class PlaceMainBindingModel
    {
        public PlaceMainBindingModel() { }
        public PlaceMainBindingModel(Place place)
        {
            Id = place.Id;
            Name = place.Name;
            Category = place.Category;
            Description = place.Description;
            _imageSrc = place.ImageSrc;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        private string _imageSrc;

        private ImageSource _imagePath;
        public ImageSource ImagePath
        {
            get
            {
                ImageSource bitmap = null;
                Uri uri = null;
                try
                {
                    uri = new Uri("ms-appdata:///local/images/" + _imageSrc);
                    bitmap = new BitmapImage(uri);
                    _imagePath = bitmap;
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.ToString());
                }
                return _imagePath;
            }
        }

        public string Category
        { 
            get; set; 
        }

    }
}
