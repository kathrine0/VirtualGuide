﻿using AutoMapper;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Linq;
using VirtualGuide.Models;
using VirtualGuide.BindingModels;

namespace VirtualGuide.Repository
{
    public class TravelRepository : BaseRepository
    {
        public IList<BasicTravelBindingModel> GetApprovedTravelList()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                IList<Travel> items = db.Travels.Where(x => x.ApprovalStatus == true).ToList();

                return Mapper.Map<IList<BasicTravelBindingModel>>(items);
            }

        }

        public IList<CustomerTravelBindingModel> GetOwnedTravelList(string userEmail)
        {
            User user = findUserByEmail(userEmail);

            IList<Travel> items = new List<Travel>();

            foreach (var item in user.PurchasedTravels)
            {
                items.Add(item.Travel);
            }

            return Mapper.Map<IList<CustomerTravelBindingModel>>(items);

        }

        public IList<BasicTravelBindingModel> GetCreatedTravelList(string userEmail)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                User user = findUserByEmail(userEmail);
                IList<Travel> items = db.Travels.Where(x => x.CreatorId == user.Id).ToList();

                return Mapper.Map<IList<BasicTravelBindingModel>>(items);
            }

        }

        public CreatorTravelBindingModel GetTravelDetailsForCreator(int id, string userEmail)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                User user = findUserByEmail(userEmail);
                Travel travel = db.Travels.Where(x => x.Id == id).FirstOrDefault();
            
                if (travel == null || (travel.CreatorId != user.Id && travel.ApproverId != user.Id))
                {
                    throw new ObjectNotFoundException("Travel not found");
                }

                return Mapper.Map<CreatorTravelBindingModel>(travel);
            }

        }

        public CustomerTravelBindingModel GetTravelDetailsForCustomer(int id, string userEmail)
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

                return Mapper.Map<CustomerTravelBindingModel>(travel);
            }
            
        }

        public CustomerTravelBindingModel BuyTravel(int id, string userEmail)
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

                if (isPurchased > 0)
                {
                    throw new UnauthorizedAccessException("This user already owns this travel");
                }

                var purchase = new User_Purchased_Travel()
                {
                    TravelId = travel.Id,
                    UserId = user.Id
                };

                db.User_Purchased_Travels.Add(purchase);
                db.SaveChanges();

                return Mapper.Map<CustomerTravelBindingModel>(travel);
            }
        }

        public CustomerTravelBindingModel BuyTravelAnonymous(int id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Travel travel = db.Travels.Where(x => x.Id == id).FirstOrDefault();

                if (travel == null)
                {
                    throw new ObjectNotFoundException("Travel not found");
                }

                return Mapper.Map<CustomerTravelBindingModel>(travel);
            }
        }

        public CreatorTravelBindingModel Add(CreatorTravelBindingModel item)
        {
            //todo validate user role
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Travel travel = Mapper.Map<Travel>(item);

                db.Travels.Add(travel);
                db.SaveChanges();

                return Mapper.Map<CreatorTravelBindingModel>(travel);
            }
        }

        public CreatorTravelBindingModel Update(int id, SimpleCreatorTravelBindingModel item)
        {
            //todo validate user role
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Travel oldTravel = db.Travels.Where(x => x.Id == id).FirstOrDefault();

                Travel travel = Mapper.Map<SimpleCreatorTravelBindingModel, Travel>(item, oldTravel);

                var entry = db.Entry(travel);
                entry.State = EntityState.Modified;

                entry.Property(e => e.ApproverId).IsModified = false;
                entry.Property(e => e.CreatorId).IsModified = false;
                entry.Property(e => e.ApprovalStatus).IsModified = false;


                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    //todo check if exists
                    throw;
                }

                return Mapper.Map<CreatorTravelBindingModel>(travel);
            }
        }


        public void Approve(int id, string userEmail)
        {
            //todo validate user role
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                User user = userManager.FindByEmail(userEmail);
                Travel travel = db.Travels.Where(x => x.Id == id).FirstOrDefault();

                if (travel == null)
                {
                    throw new ObjectNotFoundException("Travel not found");
                }

                if (travel.CreatorId != user.Id && !userManager.IsInRole(user.Id, "Approver"))
                {
                    throw new UnauthorizedAccessException("This user is not eligible to this action");
                }

                travel.ApprovalStatus = true;
                db.SaveChanges();
            }
        }
    }
}
