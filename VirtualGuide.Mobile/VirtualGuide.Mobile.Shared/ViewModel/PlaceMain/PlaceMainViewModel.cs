﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Text;
using VirtualGuide.Mobile.BindingModel;
using VirtualGuide.Mobile.Helper;
using VirtualGuide.Mobile.Model;
using VirtualGuide.Mobile.Repository;
using Windows.UI.Xaml.Media;

namespace VirtualGuide.Mobile.ViewModel.PlaceMain
{
    public class PlaceMainViewModel : BaseViewModel
    {   
        #region readonly properties

        public enum OPTION
        {
            MAP, GALLERY, SUBPLACES, NAVIGATION
        }

        #endregion

        #region events

        //public event Action DataLoaded;

        //public event Action<GuideMainPropertyBindingModel> ScrollRequested;

        #endregion

        #region constructors

        public PlaceMainViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Place = new PlaceMainBindingModel();

            Initialize();
        }

        #endregion

        #region commands

        //public RelayCommand NavigateToMapCommand { get; set; }

        //public RelayCommand<GuideMainPropertyBindingModel> PropertyClickCommand { get; set; }

        #endregion

        #region public properties

        public PlaceMainBindingModel Place { get; set; }

        public ObservableCollection<PlaceMainOptions> Options { get; set; }

        #endregion

        #region private properties

        private int _placeId;
        private PlaceRepository _placeRepository = new PlaceRepository();

        #endregion

        #region private methods

        private void Initialize()
        {
            //NavigateToMapCommand = new RelayCommand(NavigateToMapExecute);
            //PropertyClickCommand = new RelayCommand<GuideMainPropertyBindingModel>(PropertyClickExecute);

            Options = new ObservableCollection<PlaceMainOptions>() 
            { 
                new PlaceMainOptions("Map", OPTION.MAP, "&#x1f30d;", ColorHelper.BLUE),
                new PlaceMainOptions("Navigate", OPTION.NAVIGATION, "&#x27A4;", ColorHelper.RED),
                new PlaceMainOptions("Gallery", OPTION.GALLERY, "&#x1f4db;", ColorHelper.GREEN),
                new PlaceMainOptions("More", OPTION.SUBPLACES, "&#xe109;", ColorHelper.YELLOW)
            };
        }

        #endregion

        #region public methods

        public void LoadData(int placeId)
        {
            _placeId = placeId;
            LoadData();
        }

        public async void LoadData()
        {
            IsWorkInProgress = true;
            
            if (_placeId != 0 || (Place != null && Place.Id != 0))
            {
                Place = await _placeRepository.GetPlaceById<PlaceMainBindingModel>(_placeId);
            }
            
            IsWorkInProgress = false;
        }

        //public void NavigateToMapExecute()
        //{
        //    App.RootFrame.Navigate(_mapsPage, Travel.Id);
        //}

        #endregion

        #region helper class

        [ImplementPropertyChanged]
        public class PlaceMainOptions
        {
            public PlaceMainOptions(string name, OPTION type, string icon, Brush color)
            {
                Icon = icon;
                Name = name;
                Background = color;
                Type = type;
            }

            private string _icon;
            public string Icon
            {
                get
                {
                    return WebUtility.HtmlDecode(_icon);
                }
                set
                {
                    _icon = value;
                }
            }
            public Brush Background { get; set; }
            public string Name { get; set; }

            public OPTION Type { get; set; }
        }

        #endregion
    }
}
