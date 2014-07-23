using PropertyChanged;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace VirtualGuide.Mobile.Model
{
    [ImplementPropertyChanged]
    [Table("Property")]
    public class Property
    {
        [PrimaryKey, Unique]
        public int Id { get; set; }

        [Column("Title")]
        [NotNull]
        public string Title { get; set; }

        [Column("Description")]
        public string Description { get; set; }

        [Column("ItemsOrder")]
        public int Order { get; set; }

        [Column("TravelId"), Indexed]
        public int TravelId { get; set; }
    }
}
