using SQLite;
using System.Collections.Generic;

namespace VirtualGuide.Mobile.Model
{
    [Table("Place")]
    public class Place : BaseImageModel
    {
        [PrimaryKey, Unique]
        public int Id { get; set; }

        [Column("Name")]
        public string Name { get; set; }

        [Column("Description")]
        public string Description { get; set; }

        [Column("Latitude")]
        public double Latitude { get; set; }

        [Column("Longitude")]
        public double Longitude { get; set; }

        [Column("ParentId")]
        public int? ParentId { get; set; }

        [Column("TravelId")]
        public int TravelId { get; set; }

        [Column("ImageSrc")]
        public string ImageSrc { get; set; }

        [Column("Category")]
        public string Category { get; set; }

        [Column("IconName")]
        public string IconName { get; set; }

        [Ignore]
        public IList<Property> Properties { get; set; }

        [Ignore]
        public virtual IList<Place> Children { get; set; }
    }

}
