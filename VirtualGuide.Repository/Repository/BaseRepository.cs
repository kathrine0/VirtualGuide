using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity.Core;
using VirtualGuide.Models;

namespace VirtualGuide.Repository
{
    public abstract class BaseRepository : IDisposable
    {

        protected BaseVGUserManager userManager = new BaseVGUserManager();

        protected User findUserByEmail(string userEmail)
        {
            User user = userManager.FindByEmail(userEmail);
            if (user == null)
            {
                throw new ObjectNotFoundException("Invalid user");
            }

            return user;
        }



        public void Dispose()
        {
            userManager.Dispose();
        }
    }
}
