using System;
using System.Collections.Generic;
using System.Text;
using VirtualGuide.Mobile.Model;
using Windows.UI.Xaml.Controls;

namespace VirtualGuide.Mobile.View
{
    public sealed partial class GuideList : Page
    {
        //TODO: observable collection
        private List<Travel> _availableTravels = new List<Travel>();
        private List<Travel> _ownedTravels = new List<Travel>();
    }
}
