using PropertyChanged;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace VirtualGuide.Mobile.DBModel
{

    [Table("User")]
    public class User 
    {
        public string Username { get; set; }
    }
}
