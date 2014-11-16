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
                IQueryable<Icon> icons = db.Icon;

                var result = new List<IconViewModel>();

                foreach (var icon in icons)
                {
                    result.Add(Mapper.Map<IconViewModel>(icon));
                }

                return result;
            }

        }
    }
}
