using PropertyChanged;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace VirtualGuide.Mobile.Model
{
    [Table("Travel")]
    public class Travel
    {
        [PrimaryKey, Unique]
        public int Id { get; set; }

        [Column("Name")]
        [NotNull]
        public string Name { get; set; }

        [Column("Description")]
        public string Description { get; set; }

        [Column("Price")]
        public double Price { get; set; }

        [Column("Author")]
        public virtual String Author { get; set; }

        /// <summary>
        /// Language code in format of pl_PL
        /// </summary>

        [Column("Language")]
        public string Language { get; set; }

        [Ignore]
        public List<Property> Properties { get; set; }

        [Ignore]
        public List<Place> Places { get; set; }

        public DateTime LastUpdated { get; set; }

        [Column("Latitude")]
        public double Latitude { get; set; }
        
        [Column("Longitude")]
        public double Longitude { get; set; }
     
        [Column("ZoomLevel")]
        public double ZoomLevel { get; set; }

    }
}
