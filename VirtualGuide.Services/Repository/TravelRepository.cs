using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualGuide.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.Core;

namespace VirtualGuide.Services.Repository
{
    public class TravelRepository : BaseRepository
    {


        public IList<BasicTravelViewModel> GetApprovedTravelList()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                IQueryable<Travel> items = db.Travels.Where(x => x.ApprovalStatus == true);
            
                var result = new List<BasicTravelViewModel>();

                foreach (var item in items)
                {
                    result.Add(new BasicTravelViewModel(item));
                }

                return result;
            }

        }

        public IList<CustomerTravelViewModel> GetOwnedTravelList(string userEmail)
        {
            User user = findUserByEmail(userEmail);
            var items = user.PurchasedTravels;
            var result = new List<CustomerTravelViewModel>();

            foreach(var item in items)
            {
                result.Add(new CustomerTravelViewModel(item.Travel));
            }

            return result;
        }

        public IList<BasicTravelViewModel> GetCreatedTravelList(string userEmail)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                User user = findUserByEmail(userEmail);
                IQueryable<Travel> items = db.Travels.Where(x => x.CreatorId == user.Id);
                
                var result = new List<BasicTravelViewModel>();
                foreach (var item in items)
                {
                    result.Add(new BasicTravelViewModel(item));
                }

                return result;
            }

        }

        public CreatorTravelViewModel GetTravelDetailsForCreator(int id, string userEmail)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                User user = findUserByEmail(userEmail);
                Travel travel = db.Travels.Where(x => x.Id == id).FirstOrDefault();
            
                if (travel == null || (travel.CreatorId != user.Id && travel.ApproverId != user.Id))
                {
                    throw new ObjectNotFoundException("Travel not found");
                }

                return new CreatorTravelViewModel(travel);
            }

        }

        public CustomerTravelViewModel GetTravelDetailsForCustomer(int id, string userEmail)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                User user = userManager.FindByEmail(userEmail);
                Travel travel = db.Travels.Where(x => x.Id == id).FirstOrDefault();
            
                if (travel == null)
                {
                    throw new ObjectNotFoundException("Travel not found");
                }

                int isPurchased = user.PurchasedTravels.Where(x => x.TravelId == travel.Id).Count();

                if (isPurchased == 0)
                {
                    throw new UnauthorizedAccessException("This user is not authorised to see the details");
                }

                return new CustomerTravelViewModel(travel);
            }
            
        }

        public CreatorTravelViewModel Add(CreatorTravelViewModel item)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Travel travel = item.ToModel();

                db.Travels.Add(travel);
                db.SaveChanges();

                return new CreatorTravelViewModel(travel);
            }
        }
    }
}
