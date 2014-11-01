using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualGuide.Models;

namespace VirtualGuide.Services
{
    public static class AutoMapperConfig
    {
        public static void Configure()
        {
            ///Property
            Mapper.CreateMap<Property, BasicPropertyViewModel>();
            Mapper.CreateMap<BasicPropertyViewModel, Property>();

            ///Icon
            Mapper.CreateMap<Icon, IconViewModel>();
        }
    }
}
