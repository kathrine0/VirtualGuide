﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualGuide.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.Core;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using AutoMapper;
using VirtualGuide.ViewModels;

namespace VirtualGuide.Repository
{
    public class TravelRepository : BaseRepository
    {
        public IList<BasicTravelViewModel> GetApprovedTravelList()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                IList<Travel> items = db.Travels.Where(x => x.ApprovalStatus == true).ToList();

                return Mapper.Map<IList<BasicTravelViewModel>>(items);
            }

        }

        public IList<CustomerTravelViewModel> GetOwnedTravelList(string userEmail)
        {
            User user = findUserByEmail(userEmail);

            IList<Travel> items = new List<Travel>();

            foreach (var item in user.PurchasedTravels)
            {
                items.Add(item.Travel);
            }

            return Mapper.Map<IList<CustomerTravelViewModel>>(items);

        }

        public IList<BasicTravelViewModel> GetCreatedTravelList(string userEmail)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                User user = findUserByEmail(userEmail);
                IList<Travel> items = db.Travels.Where(x => x.CreatorId == user.Id).ToList();

                return Mapper.Map<IList<BasicTravelViewModel>>(items);
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

                return Mapper.Map<CreatorTravelViewModel>(travel);
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

                return Mapper.Map<CustomerTravelViewModel>(travel);
            }
            
        }

        public CreatorTravelViewModel Add(CreatorTravelViewModel item)
        {
            //todo validate user role
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Travel travel = Mapper.Map<Travel>(item);

                db.Travels.Add(travel);
                db.SaveChanges();

                return Mapper.Map<CreatorTravelViewModel>(travel);
            }
        }

        public CreatorTravelViewModel Update(int id, SimpleCreatorTravelViewModel item)
        {
            //todo validate user role
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Travel oldTravel = db.Travels.Where(x => x.Id == id).FirstOrDefault();

                Travel travel = Mapper.Map<SimpleCreatorTravelViewModel, Travel>(item, oldTravel);
                //Travel travel = Mapper.Map<Travel>(item);
                var entry = db.Entry(travel);
                entry.State = EntityState.Modified;

                entry.Property(e => e.ApproverId).IsModified = false;
                entry.Property(e => e.CreatorId).IsModified = false;
                entry.Property(e => e.ApprovalStatus).IsModified = false;

                //entry.Property(e => e.Properties).IsModified = false;
                //entry.Property(e => e.Places).IsModified = false;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    //todo check if exists
                    throw;
                }

                return Mapper.Map<CreatorTravelViewModel>(travel);
            }
        }
    }
}