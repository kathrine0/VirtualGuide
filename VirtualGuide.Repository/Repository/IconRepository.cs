using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using VirtualGuide.Models;
using VirtualGuide.BindingModels;

namespace VirtualGuide.Repository
{
    public class IconRepository
    {
        public IList<IconBindingModel> GetIcons()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                IList<Icon> icons = db.Icon.ToList();

                return Mapper.Map<IList<IconBindingModel>>(icons);
            }

        }
    }
}
