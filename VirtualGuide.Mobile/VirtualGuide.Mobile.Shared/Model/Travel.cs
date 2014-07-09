using PropertyChanged;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace VirtualGuide.Mobile.Model
{
    [ImplementPropertyChanged]
    [Table("Travel")]
    class Travel
    {
        [PrimaryKey, Unique]
        public int Id { get; set; }

        [Column("Name")]
        public string Name { get; set; }

        [Column("Description")]
        public string Description { get; set; }

        [Column("Price")]
        public double Price { get; set; }

        [Column("Author")]
        public virtual String Author { get; set; }

        //public virtual IList<Place> Places { get; set; }

        //public virtual IList<Excursion> Excursions { get; set; }

        //public virtual IList<Property> Properties { get; set; }

        /// <summary>
        /// Language code in format of pl_PL
        /// </summary>

        [Column("Language")]
        public string Language { get; set; }
    }
}
