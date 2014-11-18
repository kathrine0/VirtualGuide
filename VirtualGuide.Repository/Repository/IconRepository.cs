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
