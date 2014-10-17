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

        public void AddMany(IList<BasicPropertyViewModel> items, int travelId)
        {
            //todo validate against is user owner of the travel

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var properties = new List<Property>();

                foreach (var item in items)
                {
                    item.TravelId = travelId;
                    properties.Add(item.ToModel());
                }

                db.Properties.AddRange(properties);
                db.SaveChanges();

            }
        }
    }
}
