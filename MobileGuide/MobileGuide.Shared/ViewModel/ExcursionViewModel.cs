using MobileGuide.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MobileGuide.ViewModel
{
    class ExcursionViewModel
    {
        public ObservableCollection<Excursion> Excursions { get; set; }

        // public void GetAccomplishments()
        //{
        //    if (IsolatedStorageSettings.ApplicationSettings.Count > 0)
        //    {
        //        GetSavedAccomplishments();
        //    }
        //    else
        //    {
        //        GetDefaultAccomplishments();
        //    }
        //}


        //public void GetDefaultAccomplishments()
        //{
        //    ObservableCollection<Excursion> a = new ObservableCollection<Excursion>();

        //    // Items to collect
        //    a.Add(new Excursion() { Name = "Paryż" });
        //    a.Add(new Excursion() { Name = "Londyn" });
        //    a.Add(new Excursion() { Name = "Nowy Jork" });

        //    Excursions = a;
        //    //MessageBox.Show("Got accomplishments from default");
        //}


        //public void GetSavedAccomplishments()
        //{
        //    ObservableCollection<Excursion> a = new ObservableCollection<Excursion>();

        //    foreach (Object o in IsolatedStorageSettings.ApplicationSettings.Values)
        //    {
        //        a.Add((Accomplishment)o);
        //    }

        //    Excursions = a;
        //    //MessageBox.Show("Got accomplishments from storage");
        //}
    
    }
}
