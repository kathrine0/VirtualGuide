using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualGuide.Models;
using VirtualGuide.ViewModels;

namespace VirtualGuide.Repository
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

        public IList<BasicPlaceViewModel> GetAllForTravel(int travelId)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                IList<Place> items = db.Places.Where(x => x.TravelId == travelId).ToList();

                IList<BasicPlaceViewModel> result = Mapper.Map<IList<BasicPlaceViewModel>>(items);

                return result;
            }
        }

        public void AddMany(IList<BasicPlaceViewModel> items)
        {
            //todo validate against is user owner of the travel

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                IList<Place> places = Mapper.Map<IList<Place>>(items);

                db.Places.AddRange(places);
                db.SaveChanges();
            }
        }

        public BasicPlaceViewModel Add(BasicPlaceViewModel item)
        {
            //todo validate against is user owner of the travel

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Place place = Mapper.Map<Place>(item);

                db.Places.Add(place);
                db.SaveChanges();

                return Mapper.Map<BasicPlaceViewModel>(place);
            }
        }

        public void UpdateMany(IList<BasicPlaceViewModel> items)
        {
            //todo validate against is user owner of the travel

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var places = new List<Place>();

                foreach (var item in items)
                {
                    var entry = db.Entry(Mapper.Map<Place>(item));
                    entry.State = EntityState.Modified;
                }

                db.SaveChanges();
            }
        }

        public void Update(int id, BasicPlaceViewModel item)
        {
            //todo validate against is user owner of the travel

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
