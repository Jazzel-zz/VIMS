using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VIMS.Models;

namespace VIMS.Models
{
    public class VehicleImages
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public int VehicleInformationId { get; set; }
        public virtual VehicleInformation VehicleInformation { get; set; }
    }
}