using SQLite;

namespace VirtualGuide.Mobile.Model
{

    [Table("User")]
    public class User 
    {
        public string Username { get; set; }
    }
}
