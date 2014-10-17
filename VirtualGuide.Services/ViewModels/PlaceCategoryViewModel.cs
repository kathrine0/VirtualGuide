using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualGuide.Models;

namespace VirtualGuide.Services
{
    public class PlaceCategoryViewModel
    {
        public PlaceCategoryViewModel()
        {

        }

        public PlaceCategoryViewModel(PlaceCategory category)
        {
            Id = category.Id;
            Name = category.Name;
            Description = category.Description;
            Language = category.Language;
            IconName = category.IconName;
        }

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Language { get; set; }

        public string IconName { get; set; }

        public PlaceCategory ToModel()
        {
            return new PlaceCategory()
            {
                Id = this.Id,
                Name = this.Name,
                Description = this.Description,
                Language = this.Language,
                IconName = this.IconName
            };
        }
    }
}
