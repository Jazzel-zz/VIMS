using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using VIMS.Models;

namespace VIMS.Models
{
    public class Estimate
    {
        [Key]
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public string EstimateNumber { get; set; }
        public int VehicleInformationId { get; set; }
        public virtual VehicleInformation VehicleInformation { get; set; }
        public int PolicyTypeId { get; set; }
        public virtual PolicyType PolicyType { get; set; }


    }
}