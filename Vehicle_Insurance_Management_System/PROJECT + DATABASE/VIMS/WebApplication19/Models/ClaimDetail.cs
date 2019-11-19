using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VIMS.Models
{
    public class ClaimDetail
    {
        [Key]
        public int ClaimDetailId { get; set; }
        [Required]
        [DisplayName("Claim Number")]
        public string ClaimNumber { get; set; }
        [Required]
        [DisplayName("Policy")]
        [ForeignKey("CustomerPolicyRecord")]
        public int CustomerPolicyRecordId { get; set; }
        public CustomerPolicyRecord CustomerPolicyRecord { get; set; }
        [Required]
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        [Required]
        [DisplayName("Place of Accident")]
        public string PlaceOfAccident { get; set; }
        [Required]
        [DisplayName("Date of Accident")]
        public DateTime DateOfAccident { get; set; }
        [Required]
        [DisplayName("Insured Amount")]
        public int InsuredAmount { get; set; }
        [Required]
        [DisplayName("Claimable Amount")]
        public int ClaimableAmount { get; set; }
    }
}