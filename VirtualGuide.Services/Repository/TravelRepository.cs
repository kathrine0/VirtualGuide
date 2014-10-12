using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualGuide.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace VirtualGuide.Services.Repository
{
    public class TravelRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        UserManager<User> userManager = new UserManager<User>(new UserStore<User>(new ApplicationDbContext()));

        
        public IList<BasicTravelViewModel> GetApprovedTravelList()
        {
            var items = db.Travels.Where(x => x.ApprovalStatus == true);
            var result = new List<BasicTravelViewModel>();

            foreach (var item in items)
            {
                result.Add(new BasicTravelViewModel(item));
            }

            return result;
        }

        public IList<CustomerTravelViewModel> GetOwnedTravelList(string userEmail)
        {

            var user = userManager.FindByEmail(userEmail);

            if (user == null) return null;

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
            var user = userManager.FindByEmail(userEmail);

            if (user == null) return null;

            var items = db.Travels.Where(x => x.CreatorId == user.Id);
            var result = new List<BasicTravelViewModel>();

            foreach (var item in items)
            {
                result.Add(new BasicTravelViewModel(item));
            }

            return result;
        }

        public CreatorTravelViewModel GetTravelDetailsForCreator(int id, string userEmail)
        {
            var user = userManager.FindByEmail(userEmail);
            var travel = db.Travels.Where(x => x.Id == id).FirstOrDefault();

            if (travel == null)
            {
                return null;
            }

            if (travel.CreatorId != user.Id && travel.ApproverId != user.Id)
            {
                return null;
            }

            return new CreatorTravelViewModel(travel);
        }

        public CustomerTravelViewModel GetTravelDetailsForCustomer(int id, string userEmail)
        {
            var user = userManager.FindByEmail(userEmail);
            var travel = db.Travels.Where(x => x.Id == id).FirstOrDefault();

            if (travel == null)
            {
                return null;
            }

            var isPurchased = user.PurchasedTravels.Where(x => x.TravelId == travel.Id).Count();

            if (isPurchased == 0)
            {
                return null;
            }

            return new CustomerTravelViewModel(travel);
        }
    }
}
