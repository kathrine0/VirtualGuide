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

        public IList<ComplexReadTravelViewModel> GetOwnedTravelList(string userEmail)
        {

            var user = userManager.FindByEmail(userEmail);

            if (user == null) return null;

            var items = user.PurchasedTravels;
            var result = new List<ComplexReadTravelViewModel>();

            foreach(var item in items)
            {
                result.Add(new ComplexReadTravelViewModel(item.Travel));
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

        public BasicTravelViewModel GetOwnedTravelDetails(int id, string userEmail)
        {
            throw new NotImplementedException();
        }
    }
}
