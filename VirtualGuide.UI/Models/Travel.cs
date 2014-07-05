using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace VirtualGuide.Models
{
    public class Travel : IAuditable
    {
        public Travel()
        {
            Excursions = new List<Excursion>();
            Places = new List<Place>();
            Properties = new List<Property>();
        }

        public void AddProperty(string PropertyName)
        {
            Properties.Add(new Property()
            {
                Title = PropertyName
            });
        }

        public void AddPlace()
        {
            Places.Add(new Place());
        }

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }


        public double Price { get; set; }

        public string CreatorId { get; set; }

        [ForeignKey("CreatorId")]
        public virtual User Creator { get; set; }

        public string ApproverId { get; set; }

        [ForeignKey("ApproverId")]
        public virtual User Approver { get; set; }


        public bool ApprovalStatus { get; set; }

        [InverseProperty("Travel")]
        public virtual IList<Place> Places { get; set; }


        [InverseProperty("Travel")]
        public virtual IList<Excursion> Excursions { get; set; }

        [InverseProperty("Travel")]

        public virtual IList<Property> Properties { get; set; }

        /// <summary>
        /// Language code in format of pl_PL
        /// </summary>
        [Required]
        //[RegularExpression(@"^[a-z]{2}-[A-Z]{2}$")]
        public string Language { get; set; }


        public DateTime CreatedAt { get; set; }


        public DateTime UpdatedAt { get; set; }
    }
}