using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using WebApplication19.Models;

namespace VIMS.Models
{
    public class VehicleInformation
    {
        // warranty
        [Key]
        public int VehicleInformationId { get; set; }
        [Required]
        [DisplayName("Vehicle Company")]
        public string VehicleCompany { get; set; }
        [Required]
        [DisplayName("Vehicle Name")]
        public string VehicleName { get; set; }
        [Required]
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        [Required]
        [DisplayName("Color")]
        public string VehicleColor { get; set; }
        [Required]
        [DisplayName("Model")]
        public string VehicleModel { get; set; }
        [Required]
        [DisplayName("Price Now")]
        public int VehicleRate { get; set; }
        [Required]
        [DisplayName("Body Number")]
        public string VehicleBodyNumber { get; set; }
        [Required]
        [DisplayName("Engine Number")]
        public string VehicleEngineNumber { get; set; }
        [Required]
        [DisplayName("Registration Number")]
        public int VehicleNumber { get; set; }
        [Required]
        [DisplayName("Warranty")]
        public string VehicleWarranty { get; set; }
        [Required]
        [DisplayName("Description")]
        public string VehicleDescription { get; set; }
        public DateTime RegistrationDate { get; set; }
        public virtual ICollection<Estimate> Estimates { get; set; }
        public virtual ICollection<VehicleImages> VehicleImages { get; set; }

    }

}