using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace VirtualGuide.Mobile.Model
{
    public interface BaseImageModel : BaseModel
    {
        string ImageSrc { get; set; }
    }
}
