using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using VirtualGuide.Models;

namespace VirtualGuide.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service.svc or Service.svc.cs at the Solution Explorer and start debugging.
    public class Service : IService
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public string GetData(int value)
        {
            return string.Format("You testetest: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }


        public List<Travel> GetTravelsList()
        {
            //return db.Travels.ToList();
            return new List<Travel>() {new Travel() { Id = 1, Name = "test" }};
        }

        public Travel GetTravelById(string id)
        {
            try
            {
                int travelId = Convert.ToInt32(id);
                return db.Travels.SingleOrDefault(travel => travel.Id == travelId);
            }
            catch
            {
                throw new FaultException("Something went wrong");
            }
        }

        public void AddTravel(string name)
        {
            Travel travel = new Travel { Name = name };
            db.Travels.Add(travel);
            db.SaveChanges();
        }

        public void UpdateTravel(string id, string name)
        {
            try
            {
                int travelId = Convert.ToInt32(id);

                Travel travel = db.Travels.SingleOrDefault(b => b.Id == travelId);
                travel.Name = name;
                db.SaveChanges();
            }
            catch
            {
                throw new FaultException("Something went wrong");
            }
        }

        public void DeleteTravel(string id)
        {
            try
            {
                int travelId = Convert.ToInt32(id);

                Travel travel = db.Travels.SingleOrDefault(b => b.Id == travelId);
                db.Travels.Remove(travel);
                db.SaveChanges();
            }
            catch
            {
                throw new FaultException("Something went wrong");
            }
        }
    }
}
