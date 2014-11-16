using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualGuide.Models;

namespace VirtualGuide.Services.Repository
{
    public abstract class BaseRepository
    {

        protected VGUserManager userManager = new VGUserManager();

        protected User findUserByEmail(string userEmail)
        {
            User user = userManager.FindByEmail(userEmail);
            if (user == null)
            {
                throw new ObjectNotFoundException("Invalid user");
            }

            return user;
        }
        

    }
}
