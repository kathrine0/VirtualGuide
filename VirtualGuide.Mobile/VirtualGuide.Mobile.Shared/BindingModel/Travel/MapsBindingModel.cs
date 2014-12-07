using PropertyChanged;
using VirtualGuide.Mobile.Model;

namespace VirtualGuide.Mobile.BindingModel
{
    [ImplementPropertyChanged]
    public class MapTravelBindingModel : BaseTravelBindingModel
    {
            #region constructors

            public MapTravelBindingModel() : base()
            {

            }

            public MapTravelBindingModel(Travel travel) : base(travel)
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
