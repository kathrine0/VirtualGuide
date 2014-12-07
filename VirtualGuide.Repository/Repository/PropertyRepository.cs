using AutoMapper;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using VirtualGuide.Models;
using VirtualGuide.BindingModels;

namespace VirtualGuide.Repository
{
    public class PropertyRepository : BaseRepository
    {
        public IList<BasicPropertyBindingModel> GetPropertiesList(int travelId)
        {
            //todo check if user is permitted
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                IList<Property> items = db.Properties.Where(x => x.Travel.Id == travelId).ToList();
           
                return Mapper.Map <IList<BasicPropertyBindingModel>>(items);
            }
        }

        public void AddMany(IList<BasicPropertyBindingModel> items)
        {
            //todo validate against is user owner of the travel

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                IList<Property> properties = Mapper.Map<IList<Property>>(items);

                db.Properties.AddRange(properties);
                db.SaveChanges();

            }
        }

        public BasicPropertyBindingModel Add(BasicPropertyBindingModel item)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Property property = Mapper.Map<Property>(item);

                db.Properties.Add(property);
                db.SaveChanges();

                return Mapper.Map<BasicPropertyBindingModel>(property);
            }
        }

        public void Update(int id, BasicPropertyBindingModel item)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Property property = Mapper.Map<Property>(item);

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
