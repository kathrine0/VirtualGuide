using PropertyChanged;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace VirtualGuide.Mobile.Model
{
    [Table("Property")]
    public class Property : BaseModel
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

        [Column("Icon")]
        public string IconSymbol { get; set; }
    }
}
