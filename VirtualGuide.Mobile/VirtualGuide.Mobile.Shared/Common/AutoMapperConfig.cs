using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using VirtualGuide.Mobile.BindingModel;
using VirtualGuide.Mobile.Model;
using Windows.Devices.Geolocation;

namespace VirtualGuide.Mobile.Common
{
    public static class AutoMapperConfig
    {
        public static void Configure()
        {
            ///Place
            Mapper.CreateMap<Place, ListPlaceBindingModel>()
                .ConstructUsing(source => new ListPlaceBindingModel(source.Latitude, source.Longitude));

            Mapper.CreateMap<Place, MapPlaceBindingModel>()
                .ForMember(dest => dest.Point, m => m.MapFrom(source => new Geopoint(new BasicGeoposition() { Latitude = source.Latitude, Longitude = source.Longitude })))
                .ForMember(dest => dest.CategoryName, m => m.MapFrom(source => source.Category))
                .ForMember(dest => dest.Category, opt => opt.Ignore());

            Mapper.CreateMap<Place, PlaceMainBindingModel>();
                

            ///Property
            Mapper.CreateMap<Property, PropertyBindingModel>()
                .ForMember(dest => dest.Icon, m => m.MapFrom(source => source.IconSymbol))
                .ForMember(dest => dest.Name, m => m.MapFrom(source => source.Title))
                .ForMember(dest => dest.Type, opt => opt.Ignore());
            
            ///Travel
            Mapper.CreateMap<Travel, BaseTravelBindingModel>();

            Mapper.CreateMap<Travel, GuideListBindingModel>();

            Mapper.CreateMap<Travel, GuideMainBindingModel>();

            Mapper.CreateMap<Travel, MapTravelBindingModel>();
        }
    }
}
