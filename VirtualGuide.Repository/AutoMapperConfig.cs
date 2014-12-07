using AutoMapper;
using VirtualGuide.Models;
using VirtualGuide.BindingModels;

namespace VirtualGuide.Repository
{
    public static class AutoMapperConfig
    {
        public static void Configure()
        {
            ///Travel
            Mapper.CreateMap<Travel, BasicTravelBindingModel>();
            Mapper.CreateMap<Travel, CreatorTravelBindingModel>();
            Mapper.CreateMap<CreatorTravelBindingModel, Travel>();
            Mapper.CreateMap<Travel, SimpleCreatorTravelBindingModel>();
            Mapper.CreateMap<SimpleCreatorTravelBindingModel, Travel>();
            Mapper.CreateMap<Travel, CustomerTravelBindingModel>();                

            ///Place
            Mapper.CreateMap<Place, BasicPlaceBindingModel>();
            Mapper.CreateMap<BasicPlaceBindingModel, Place>();
            Mapper.CreateMap<Place, MobilePlaceBindingModel>()
                .ForMember(dest => dest.Category, m => m.MapFrom(source => source.Category == null ? string.Empty : source.Category.Name))
                .ForMember(dest => dest.IconName, m => m.MapFrom(source => source.Category == null ? string.Empty : source.Category.IconName));

            //Place Categories
            Mapper.CreateMap<PlaceCategory, PlaceCategoryBindingModel>();

            ///Property
            Mapper.CreateMap<Property, BasicPropertyBindingModel>();
            Mapper.CreateMap<Property, MobilePropertyBindingModel>();
            Mapper.CreateMap<BasicPropertyBindingModel, Property>();

            ///Icon
            Mapper.CreateMap<Icon, IconBindingModel>();
            Mapper.CreateMap<IconBindingModel, Icon>();
        }
    }
}
