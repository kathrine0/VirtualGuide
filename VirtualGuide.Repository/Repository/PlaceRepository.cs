using AutoMapper;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using VirtualGuide.Models;
using VirtualGuide.BindingModels;

namespace VirtualGuide.Repository
{
    public class PlaceRepository : BaseRepository
    {

        public IList<PlaceCategoryBindingModel> GetPlaceCategories(string language)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                IList<PlaceCategory> categories = db.PlaceCategory.Where(x => x.Language == language).ToList();

                return Mapper.Map<IList<PlaceCategoryBindingModel>>(categories);
            }
    
        }

        public IList<BasicPlaceBindingModel> GetAllForTravel(int travelId)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                IList<Place> items = db.Places.Where(x => x.TravelId == travelId).ToList();

                IList<BasicPlaceBindingModel> result = Mapper.Map<IList<BasicPlaceBindingModel>>(items);

                return result;
            }
        }

        public void AddMany(IList<BasicPlaceBindingModel> items)
        {
            //todo validate against is user owner of the travel

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                IList<Place> places = Mapper.Map<IList<Place>>(items);

                db.Places.AddRange(places);
                db.SaveChanges();
            }
        }

        public BasicPlaceBindingModel Add(BasicPlaceBindingModel item)
        {
            //todo validate against is user owner of the travel

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Place place = Mapper.Map<Place>(item);

                db.Places.Add(place);
                db.SaveChanges();

                return Mapper.Map<BasicPlaceBindingModel>(place);
            }
        }

        public void UpdateMany(IList<BasicPlaceBindingModel> items)
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

        public void Update(int id, BasicPlaceBindingModel item)
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
