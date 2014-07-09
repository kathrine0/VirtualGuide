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

        public IList<BasicTravelViewModel> GetOwnedTravelList(string userId)
        {
            var user = userManager.FindById(userId);
            var items = user.PurchasedTravels;
            var result = new List<BasicTravelViewModel>();

            foreach(var item in items)
            {
                result.Add(new BasicTravelViewModel(item.Travel));
            }

            return result;
        }


    }
}
