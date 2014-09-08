using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Text;
using VirtualGuide.Mobile.DBModel;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace VirtualGuide.Mobile.Model
{
    [ImplementPropertyChanged]
    public class TravelModel
    {
        #region constructors

        public TravelModel()
        {

        }

        public TravelModel(Travel travel)
        {
            Id = travel.Id;
            Name = travel.Name;
            Description = travel.Description;
            Price = travel.Price;
            Latitude = travel.Latitude;
            Longitude = travel.Longitude;
            ZoomLevel = travel.ZoomLevel;
            _imageSrc = travel.ImageSrc;
            IsOwned = travel.IsOwned;
        }

        #endregion

        #region properties

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public List<Property> Properties { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public bool IsOwned { get; set; }

        public double ZoomLevel { get; set; }

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

        #endregion
    }
}
