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
    public class GuideListBindingModel : BaseTravelBindingModel
    {
        #region constructors

        public GuideListBindingModel()
        {

        }

        public GuideListBindingModel(Travel travel) : base(travel)
        {
            Price = travel.Price;
            _imageSrc = travel.ImageSrc;
            IsOwned = travel.IsOwned;
        }

        #endregion

        #region properties

        public double Price { get; set; }

        public bool IsOwned { get; set; }

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
