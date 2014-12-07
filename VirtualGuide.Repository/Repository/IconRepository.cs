using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using VirtualGuide.Models;
using VirtualGuide.ViewModels;

namespace VirtualGuide.Repository
{
    public class IconRepository
    {
        public IList<IconViewModel> GetIcons()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                IList<Icon> icons = db.Icon.ToList();

                return Mapper.Map<IList<IconViewModel>>(icons);
            }

        }
    }
}
