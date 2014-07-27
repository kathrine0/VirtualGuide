using PropertyChanged;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace VirtualGuide.Mobile.Model
{

    [Table("User")]
    public class User 
    {
        public string Username { get; set; }
    }
}
