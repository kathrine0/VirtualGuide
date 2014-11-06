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
    public class PropertyRepository : BaseRepository
    {
        public IList<BasicPropertyViewModel> GetPropertiesList(int travelId)
        {
            
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                IQueryable<Property> items = db.Properties.Where(x => x.Travel.Id == travelId);
           
                var result = new List<BasicPropertyViewModel>();

                foreach (var item in items)
                {
                    result.Add(Mapper.Map<BasicPropertyViewModel>(item));
                }

                return result;
            }
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
                    properties.Add(Mapper.Map<Property>(item));
                }

                db.Properties.AddRange(properties);
                db.SaveChanges();

            }
        }
        
        public void Create(BasicPropertyViewModel item)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Property property = Mapper.Map<Property>(item);

                db.Properties.Add(property);
                db.SaveChanges();
            }
        }

        public void Update(int id, BasicPropertyViewModel item)
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
