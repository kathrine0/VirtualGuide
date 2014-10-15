using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualGuide.Models;

namespace VirtualGuide.Services.Repository
{
    public class PropertyRepository : BaseRepository
    {
        public IList<BasicPropertyViewModel> GetPropertiesList(int travelId)
        {
            IQueryable<Property> items;
            
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                items = db.Properties.Where(x => x.Travel.Id == travelId);
            }
            
            var result = new List<BasicPropertyViewModel>();

            foreach (var item in items)
            {
                result.Add(new BasicPropertyViewModel(item));
            }

            return result;
        }
    }
}
