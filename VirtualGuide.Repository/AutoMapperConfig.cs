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
                .ForMember(mpvm => mpvm.Category, m => m.MapFrom(s => s.Category == null ? string.Empty : s.Category.Name))
                .ForMember(mpvm => mpvm.IconName, m => m.MapFrom(s => s.Category == null ? string.Empty : s.Category.IconName));

            //Place Categories
            Mapper.CreateMap<PlaceCategory, PlaceCategoryViewModel>();

            ///Property
            Mapper.CreateMap<Property, BasicPropertyViewModel>();
            Mapper.CreateMap<BasicPropertyViewModel, Property>();

            ///Icon
            Mapper.CreateMap<Icon, IconViewModel>();
            Mapper.CreateMap<IconViewModel, Icon>();
        }
    }
}
