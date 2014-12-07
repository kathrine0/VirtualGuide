using System.ComponentModel;

namespace VirtualGuide.Mobile.BindingModel
{
    public class CategoryVisibilityModel : INotifyPropertyChanged
    {
        public bool Visibile
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
