using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Net.NetworkInformation;
using System.Collections.Generic;
using Newtonsoft.Json;
using VirtualGuide.Mobile.Model;
using System.Net.Http;
using System.Threading.Tasks;
using VirtualGuide.Mobile.Common;
using PropertyChanged;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using System.Collections;
using Windows.UI.Xaml.Data;
using VirtualGuide.Mobile.Helper;

namespace VirtualGuide.Mobile.ViewModel
{

    [ImplementPropertyChanged]
    public class TravelViewModel
    {
        public TravelViewModel()
        {

        }

        public TravelViewModel(Travel travel)
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
        public ImageSource ImagePath
        {
            get
            {
                ImageSource bitmap = null;
                Uri uri = null;
                try
                {
                    if (IsOwned)
                    {
                        uri = new Uri("ms-appdata:///local/images/" + _imageSrc);
                    }
                    else
                    {
                        uri = new Uri(App.WebService + _imageSrc);
                    }

                    bitmap = new BitmapImage(uri);
                }
                catch
                {
                }
                return bitmap;
            }
            set { ;}
        }

        public List<TravelViewModel> Data
        {
            get;
            set;
        }

        private CollectionViewSource _collection;
        public CollectionViewSource Collection
        {
            get
            {
                _collection = new CollectionViewSource();
                if (Data != null)
                {
                    var grouped = Data.ToGroups(x => x.Name, x => x.IsOwned, true);
                    _collection.Source = grouped;
                    _collection.IsSourceGrouped = true;
                }
                return _collection;
            }
        }
    }
}
