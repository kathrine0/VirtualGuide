﻿
namespace VirtualGuide.Models
{
    public class Place_Excursion
    {
        public Place_Excursion()
        {

        }

        public int Id { get; set; }

        public int Order { get; set; }

        public string Description { get; set; }

        public Place Place { get; set; }
    }
}