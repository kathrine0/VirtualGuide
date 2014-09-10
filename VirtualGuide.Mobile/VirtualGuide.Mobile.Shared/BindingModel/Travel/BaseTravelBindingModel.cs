using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Text;
using VirtualGuide.Mobile.Model;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace VirtualGuide.Mobile.BindingModel
{
    abstract public class BaseTravelBindingModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public BaseTravelBindingModel(Travel travel)
        {
            Id = travel.Id;
            Name = travel.Name;
        }

        public BaseTravelBindingModel()
        {

        }
    }
}
