using PropertyChanged;
using System;
using VirtualGuide.Mobile.Model;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace VirtualGuide.Mobile.BindingModel
{
    [ImplementPropertyChanged]
    public class GuideListBindingModel : BaseTravelBindingModel
    {
        #region constructors

        public GuideListBindingModel() : base()
        {

        }


        #endregion

        #region properties

        public string Description { get; set; }

        public double Price { get; set; }

        public bool IsOwned { get; set; }

        public string ImageSrc { get; private set; }

        private ImageSource _imagePath;
        public ImageSource ImagePath
        {
            get
            {
                ImageSource bitmap = null;
                Uri uri = null;
                try
                {
                    uri = new Uri("ms-appdata:///local/images/" + ImageSrc);
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
