using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VIMS.Models
{
    public class CustomerPolicyRecord
    {
        [Key]
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        [Required]
        [Display(Name = "Policy Number")]
        public string PolicyNumber { get; set; }
        [Required]
        [Display(Name = "Vehicle")]
        public int VehicleInformationId { get; set; }
        public virtual VehicleInformation VehicleInformation { get; set; }
        [Required]
        [Display(Name = "Policy")]
        public int PolicyTypeId { get; set; }
        public virtual PolicyType PolicyType { get; set; }
        public DateTime PolicyDate { get; set; }
        [Required]
        [Display(Name = "Tracker for Vehicle")]
        public bool Tracker { get; set; }
        public virtual ICollection<CustomerBillingInformation> CustomerBillingInformation { get; set; }
        public virtual ICollection<ClaimDetail> ClaimDetails { get; set; }

    }
}