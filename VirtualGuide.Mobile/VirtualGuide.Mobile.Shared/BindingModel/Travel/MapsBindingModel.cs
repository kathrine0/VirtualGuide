using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Text;
using VirtualGuide.Mobile.Model;
using Windows.UI.Xaml.Media;

namespace VirtualGuide.Mobile.BindingModel
{
    [ImplementPropertyChanged]
    public class MapsBindingModel : BaseTravelBindingModel
    {
            #region constructors

            public MapsBindingModel() : base()
            {

            }

            public MapsBindingModel(Travel travel) : base(travel)
            {
                Latitude = travel.Latitude;
                Longitude = travel.Longitude;
                ZoomLevel = travel.ZoomLevel;
            }

            #endregion

            #region properties
            public double Latitude { get; set; }

            public double Longitude { get; set; }

            public double ZoomLevel { get; set; }


            #endregion
        
    }
}
