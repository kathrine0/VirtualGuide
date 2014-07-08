using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace VirtualGuide.Models
{
    [DataContract]
    public class Travel : IAuditable
    {
        public Travel()
        {
            Excursions = new List<Excursion>();
            Places = new List<Place>();
            Properties = new List<Property>();
        }

        public Travel(Travel travel)
        {
            Id = travel.Id;
            Name = travel.Name;
            Description = travel.Description;
            Price = travel.Price;
            CreatorId = travel.CreatorId;
            ApproverId = travel.ApproverId;
            ApprovalStatus = travel.ApprovalStatus;
            //TODO - copy contructors 
            //Places = travel.Places;
            //Excursions = travel.Excursions;
            //Properties = travel.Properties;
            Language = travel.Language;
            CreatedAt = travel.CreatedAt;
            UpdatedAt = travel.UpdatedAt;
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

        [DataMember]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [DataMember]
        public string Name { get; set; }

        [Required]
        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public double Price { get; set; }

        public string CreatorId { get; set; }

        [ForeignKey("CreatorId")]
        [DataMember]
        public virtual User Creator { get; set; }

        public string ApproverId { get; set; }

        [ForeignKey("ApproverId")]
        [DataMember]
        public virtual User Approver { get; set; }


        [DataMember]
        public bool ApprovalStatus { get; set; }

        [InverseProperty("Travel")]
        [DataMember]
        public virtual IList<Place> Places { get; set; }


        [InverseProperty("Travel")]
        [DataMember]
        public virtual IList<Excursion> Excursions { get; set; }

        [InverseProperty("Travel")]
        [DataMember]

        public virtual IList<Property> Properties { get; set; }

        /// <summary>
        /// Language code in format of pl_PL
        /// </summary>
        [Required]
        [DataMember]
        public string Language { get; set; }

        [DataMember]
        public DateTime CreatedAt { get; set; }


        [DataMember]
        public DateTime UpdatedAt { get; set; }
    }
}