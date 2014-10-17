using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualGuide.Models;

namespace VirtualGuide.Services.Repository
{
    public class PlaceRepository : BaseRepository
    {

        public IList<PlaceCategoryViewModel> GetPlaceCategories(string language)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                IQueryable<PlaceCategory> categories = db.PlaceCategory.Where(x => x.Language == language);

                var result = new List<PlaceCategoryViewModel>();

                foreach (var category in categories)
                {
                    result.Add(new PlaceCategoryViewModel(category));
                }

                return result;
            }
    
        }
    }
}
