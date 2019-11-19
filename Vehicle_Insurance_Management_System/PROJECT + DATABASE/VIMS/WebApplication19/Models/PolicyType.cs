using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using VIMS.Models;

namespace VIMS.Models
{
    public class PolicyType
    {
        [Key]
        public int PolicyTypeId { get; set; }
        [Required]
        [DisplayName("Policy Name")]
        public string Type { get; set; }
        [Required]
        [DisplayName("Policy Description")]
        public string Description { get; set; }
        [Required]
        [DisplayName("Rate Percentage")]
        public double Rate { get; set; }
        [DisplayName("Discount applied")]
        public double Offer { get; set; }
        [DisplayName("Tracker Rate")]
        public double Tracker { get; set; }
        [DisplayName("Policy Duration")]
        [Required]
        public string Duration { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public virtual ICollection<Estimate> Estimates { get; set; }

    }
}