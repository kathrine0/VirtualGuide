using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace VirtualGuide.Mobile.DBModel
{
    public interface BaseImageModel : BaseModel
    {
        string ImageSrc { get; set; }
    }
}
