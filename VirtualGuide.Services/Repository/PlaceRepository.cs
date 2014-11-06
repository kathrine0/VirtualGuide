using AutoMapper;
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
                IList<PlaceCategory> categories = db.PlaceCategory.Where(x => x.Language == language).ToList();

                return Mapper.Map<IList<PlaceCategoryViewModel>>(categories);
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
                    places.Add(Mapper.Map<Place>(item));
                }

                db.Places.AddRange(places);
                db.SaveChanges();
            }
        }

        public void Update(int id, BasicPlaceViewModel item)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Place property = Mapper.Map<Place>(item);

                var entry = db.Entry(property);
                entry.State = EntityState.Modified;

                entry.Property(e => e.TravelId).IsModified = false;
                entry.Property(e => e.ParentId).IsModified = false;

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
