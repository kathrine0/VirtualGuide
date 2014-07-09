using PropertyChanged;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobileGuide.Model
{
    [Table("Excursion")]
    //[ImplementPropertyChanged]
    class Excursion
    {

        [Column("Id")]
        //[Column("Id", IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int Id { get; set; }

        [Column("Name")]
        public string Name { get; set; }

        //public IList<Property> Properties { get; set; }

        //public IList<Place_Excursion> Places { get; set; }


        //public DateTime CreatedAt { get; set; }

        //public DateTime UpdatedAt { get; set; }

        public Excursion GetCopy()
        {
            Excursion copy = (Excursion)this.MemberwiseClone();
            return copy;
        }
    }
}
