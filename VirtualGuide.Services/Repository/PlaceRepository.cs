using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualGuide.Models;

namespace VirtualGuide.Services.Repository
{
    public class PlaceRepository : BaseRepository
    {

        public IList<PlaceCategoryViewModel> GetPlaceCategories(string language)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                IQueryable<PlaceCategory> categories = db.PlaceCategory.Where(x => x.Language == language);

                var result = new List<PlaceCategoryViewModel>();

                foreach (var category in categories)
                {
                    result.Add(new PlaceCategoryViewModel(category));
                }

                return result;
            }
    
        }

        public void AddMany(IList<BasicPlaceViewModel> items, int travelId)
        {
            //todo validate against is user owner of the travel

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var places = new List<Place>();

                foreach (var item in items)
                {
                    item.TravelId = travelId;
                    places.Add(item.ToModel());
                }

                db.Places.AddRange(places);
                db.SaveChanges();
            }
        }

        public void Update(int id, BasicPlaceViewModel item)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Place property = item.ToModel();

                var entry = db.Entry(property);
                entry.State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    //todo check if exists
                    throw;
                }
            }
        }
    }
}
