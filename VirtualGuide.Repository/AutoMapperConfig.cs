using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualGuide.Models;
using VirtualGuide.ViewModels;

namespace VirtualGuide.Repository
{
    public static class AutoMapperConfig
    {
        public static void Configure()
        {
            ///Travel
            Mapper.CreateMap<Travel, BasicTravelViewModel>();
            Mapper.CreateMap<Travel, CreatorTravelViewModel>();
            Mapper.CreateMap<CreatorTravelViewModel, Travel>();
            Mapper.CreateMap<Travel, SimpleCreatorTravelViewModel>();
            Mapper.CreateMap<SimpleCreatorTravelViewModel, Travel>();
            Mapper.CreateMap<Travel, CustomerTravelViewModel>();                

            ///Place
            Mapper.CreateMap<Place, BasicPlaceViewModel>();
            Mapper.CreateMap<BasicPlaceViewModel, Place>();
            Mapper.CreateMap<Place, MobilePlaceViewModel>()
                .ForMember(dest => dest.Category, m => m.MapFrom(source => source.Category == null ? string.Empty : source.Category.Name))
                .ForMember(dest => dest.IconName, m => m.MapFrom(source => source.Category == null ? string.Empty : source.Category.IconName));

            //Place Categories
            Mapper.CreateMap<PlaceCategory, PlaceCategoryViewModel>();

            ///Property
            Mapper.CreateMap<Property, BasicPropertyViewModel>();
            Mapper.CreateMap<Property, MobilePropertyViewModel>();
            Mapper.CreateMap<BasicPropertyViewModel, Property>();

            ///Icon
            Mapper.CreateMap<Icon, IconViewModel>();
            Mapper.CreateMap<IconViewModel, Icon>();
        }
    }
}
