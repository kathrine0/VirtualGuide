using System;
using System.Collections.Generic;
using PropertyChanged;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Data;
using VirtualGuide.Mobile.Helper;
using VirtualGuide.Mobile.DBModel;
using VirtualGuide.Mobile.Model;

namespace VirtualGuide.Mobile.ViewModel.GuideMain
{

    [ImplementPropertyChanged]
    public class GuideMainViewModel 
    {
        public GuideMainViewModel()
        {

        }

        public GuideMainViewModel(Travel travel)
        {
        }

        public List<TravelModel> Data
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
